using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Whale.Alert.Abstractions;
using Whale.Alert.Domain.Entities;
using Whale.Alert.Entities;
using Whale.Alert.Errors;
using Whale.Alert.Parameters;

namespace Whale.Alert.Behaviors;

internal sealed class WhaleAlert : IWhaleAlert
{
    public const string BaseAddress = "https://api.whale-alert.io";
    private const string StatusEndpoint = "/v1/status";
    private const string TransactionEndpointWithHash = "/v1/transaction/{0}/{1}";
    private const string TransactionEndpoint = "/v1/transaction";

    //headers
    private const string Header = "X-WA-API-KEY";
    private readonly HttpClient _httpClient;

    // Private constructor to prevent direct instantiation
    private WhaleAlert(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add(Header, apiKey);
    }

    // Factory class has access to create instances
    internal static IWhaleAlert Create(HttpClient httpClient, string apiKey) => new WhaleAlert(httpClient, apiKey);

    private async Task<TResponse?> HandleResponseAsync<TResponse>(HttpResponseMessage response)
    {
        if (response == null)
        {
            throw new WhaleException("No response received from the server.");
        }

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        var errorMessage = response.StatusCode switch
        {
            HttpStatusCode.BadRequest => "Your request was not valid.",
            HttpStatusCode.Unauthorized => "No valid API key was provided.",
            HttpStatusCode.Forbidden => "Access to this resource is restricted for the given caller.",
            HttpStatusCode.NotFound => "The requested resource does not exist.",
            HttpStatusCode.MethodNotAllowed => "An invalid method was used to access a resource.",
            HttpStatusCode.NotAcceptable => "An unsupported format was requested.",
            HttpStatusCode.TooManyRequests => "You have exceeded the allowed number of calls per minute.",
            HttpStatusCode.InternalServerError => "There was a problem with the API host server.",
            HttpStatusCode.ServiceUnavailable => "API is temporarily offline for maintenance.",
            _ => "Unexpected error. Try again later."
        };

        throw new WhaleException(errorMessage);
    }

    /// <summary>
    /// Retrieves the current status of Whale Alert.
    /// </summary>
    /// <remarks>
    /// This method sends a GET request to the /v1/status endpoint, which lists all currently tracked blockchains,
    /// currencies, and the current status for each blockchain. If Whale Alert is currently receiving data from a
    /// blockchain, the status will be listed as "connected".
    /// </remarks>
    /// <returns>
    /// A <see cref="Status"/> object containing the status information of tracked blockchains and currencies.
    /// </returns>
    /// <exception cref="WhaleException">
    /// Thrown when there is no response received from the server or if the response is not successful.
    /// </exception>
    public async Task<Status> GetStatus()
    {
        var response = await _httpClient.GetAsync(StatusEndpoint);

        return await HandleResponseAsync<Status>(response) ??
               throw new WhaleException("No response received from the server.");
    }

    /// <summary>
    /// Retrieves a specific transaction from a specified blockchain using its hash.
    /// </summary>
    /// <remarks>
    /// This method sends a GET request to the /v1/transaction/{blockchain}/{hash} endpoint. 
    /// Supported blockchains include: bitcoin, ethereum, ripple, neo, eos, tron, and stellar. 
    /// If a transaction consists of multiple OUTs, it is split into multiple transactions 
    /// provided the corresponding OUT is of high enough value (≥ $10 USD).
    /// </remarks>
    /// <param name="blockchain">The blockchain to search for the specific hash (lowercase).</param>
    /// <param name="hash">The hash of the transaction to return.</param>
    /// <returns>
    /// A <see cref="Transactions"/> object representing the requested transaction details.
    /// </returns>
    /// <exception cref="WhaleException">
    /// Thrown when there is no response received from the server or if the response is not successful.
    /// </exception>
    public async Task<Transactions> GetTransaction(string blockchain, string hash)
    {
        var response = await _httpClient.GetAsync(string.Format(TransactionEndpointWithHash, blockchain, hash));

        return await HandleResponseAsync<Transactions>(response) ??
               throw new WhaleException("No response received from the server.");
    }

    /// <summary>
    /// Retrieves a list of transactions that occurred after a specified start time.
    /// </summary>
    /// <remarks>
    /// This method sends a GET request to the /v1/transactions endpoint. 
    /// The transactions are retrieved based on their execution time on the respective blockchain,
    /// and they are returned in the order they were added to the database. 
    /// Some transactions may be reported with a slight delay. 
    /// Low-value transactions (less than $10 USD) are periodically grouped per blockchain 
    /// and per FROM and TO address owner to minimize data size.
    /// </remarks>
    /// <returns>
    /// A list of <see cref="Transactions"/> representing the retrieved transactions.
    /// </returns>
    /// <exception cref="WhaleException">
    /// Thrown when there is no response received from the server or if the response is not successful.
    /// </exception>
    public async Task<Transactions> GetTransactions()
    {
        var response = await _httpClient.GetAsync(TransactionEndpoint);

        return await HandleResponseAsync<Transactions>(response) ??
               throw new WhaleException("No response received from the server.");
    }

    /// <summary>
    /// Retrieves a list of transactions that occurred after a specified start time, excluding the start time itself.
    /// </summary>
    /// <remarks>
    /// This method sends a GET request to the /v1/transactions endpoint. 
    /// The transactions are retrieved based on their execution time on the respective blockchain,
    /// and they are returned in the order they were added to the database. 
    /// Some transactions may be reported with a slight delay. 
    /// Use the cursor from the previous response for pagination when making multiple or continuous requests.
    /// Low-value transactions (less than $10 USD) are periodically grouped per blockchain 
    /// and per FROM and TO address owner to minimize data size.
    /// </remarks>
    /// <param name="parameters">The parameters for the transaction query, including start time, end time, 
    /// cursor for pagination, minimum value, limit, and currency.</param>
    /// <returns>
    /// A list of <see cref="Transactions"/> representing the retrieved transactions.
    /// </returns>
    /// <exception cref="WhaleException">
    /// Thrown when there is no response received from the server or if the response is not successful.
    /// </exception>
    public async Task<Transactions> GetTransactions(Parameter parameters)
    {
        var url = BuildUrl(TransactionEndpoint, parameters);
        var response = await _httpClient.GetAsync(url);

        return await HandleResponseAsync<Transactions>(response) ??
               throw new WhaleException("No response received from the server.");
    }

    private static string BuildUrl(string baseUrl, Parameter parameters)
    {
        var builder = new UriBuilder(baseUrl);
        var query = HttpUtility.ParseQueryString(builder.Query);

        query["start"] = parameters.Start.ToString();
        if (parameters.End != 0) query["end"] = parameters.End.ToString();
        if (!string.IsNullOrEmpty(parameters.Cursor)) query["cursor"] = parameters.Cursor;
        if (parameters.MinValue != 0) query["min_value"] = parameters.MinValue.ToString();
        if (parameters.Limit != 0) query["limit"] = parameters.Limit.ToString();
        if (!string.IsNullOrEmpty(parameters.Currency)) query["currency"] = parameters.Currency;

        builder.Query = query.ToString();
        return builder.ToString();
    }
}