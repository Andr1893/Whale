using Whale.Alert.Domain.Entities;
using Whale.Alert.Entities;
using Whale.Alert.Errors;
using Whale.Alert.Parameters;

namespace Whale.Alert.Abstractions;

public interface IWhaleAlert
{
    const string BaseAddress = "https://api.whale-alert.io";
    
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
    Task<Status> GetStatus();

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
    Task<Transactions> GetTransaction(string blockchain, string hash);

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
    Task<Transactions> GetTransactions(Parameter parameters);

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
    Task<Transactions> GetTransactions();
}