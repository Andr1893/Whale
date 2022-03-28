using RestSharp;
using System;
using Whalelib.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using Whalelib.Errors;

namespace Whalelib;
public class Whale
{
    //Endpoits
    private readonly string _endpoint = "https://api.whale-alert.io";
    private readonly string _status = "/v1/status";
    private readonly string _transaction = "/v1/transaction/{0}/{1}";
    private readonly string _transactions = "/v1/transactions";

    //headers
    private readonly string _header = "X-WA-API-KEY";
    private readonly string _apiKey;

    public Whale(string apiKey) => this._apiKey = apiKey;

    //Connect
    private IRestResponse connect(string url)
    {

        //connect
        var client = new RestClient(this._endpoint);
        var request = new RestRequest(url);

        //headers
        request.AddHeader(this._header, this._apiKey);


        //get response
        var response = client.Get(request);

        return response;

    }

    private IRestResponse connect(string url, Parameters parameters)
    {
        //connect
        var client = new RestClient(this._endpoint);
        var request = new RestRequest(url);

        //headers
        request.AddHeader(this._header, this._apiKey);


        request.AddOrUpdateParameter("start", parameters.start);
        request.AddOrUpdateParameter("end", parameters.end);
        request.AddOrUpdateParameter("cursor", parameters.cursor);
        request.AddOrUpdateParameter("min_value", parameters.min_value);
        request.AddOrUpdateParameter("limit", parameters.limit);
        request.AddOrUpdateParameter("currency", parameters.currency);


        //get response
        var response = client.Get(request);

        return response;
    }

    //Errors
    private T errorReturn<T>(IRestResponse response)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                throw new WhaleExeception("Your request was not valid.");
            case HttpStatusCode.Unauthorized:
                throw new WhaleExeception("No valid API key was provided.");
            case HttpStatusCode.Forbidden:
                throw new WhaleExeception("Access to this resource is restricted for the given caller.");
            case HttpStatusCode.NotFound:
                throw new WhaleExeception("The requested resource does not exist.");
            case HttpStatusCode.MethodNotAllowed:
                throw new WhaleExeception("An invalid method was used to access a resource.");
            case HttpStatusCode.NotAcceptable:
                throw new WhaleExeception("An unsupported format was requested.");
            case HttpStatusCode.TooManyRequests:
                throw new WhaleExeception("You have exceeded the allowed number of calls per minute. Lower call frequency or upgrade your plan for a higher rate limit.");
            case HttpStatusCode.InternalServerError:
                throw new WhaleExeception("There was a problem with the API host server. Try again or later.");
            case HttpStatusCode.ServiceUnavailable:
                throw new WhaleExeception("API is temporarily offline for maintenance. Try again or later.");
            case HttpStatusCode.OK:
                return JsonConvert.DeserializeObject<T>(response.Content);
            default:
                throw new WhaleExeception("Unexpected error. Try again or later.");
                
        }
    }

    //Methods gets


    /// <summary>
    /// Shows the current status of Whale Alert. 
    /// Response lists all currently tracked blockchains, currencies and the current status for each blockchain. 
    /// If Whale Alert is currently receiving data from a blockchain the status will be listed as "connected".
    /// </summary>
    /// <returns></returns>
    public Status GetStatus()
    {
        IRestResponse response = connect(this._status);

        return errorReturn<Status>(response);
    }

    /// <summary>
    /// Returns the transaction from a specific blockchain by hash. 
    /// Blockchain inputs are: bitcoin, ethereum, ripple, neo, eos, tron and stellar. 
    /// If a transaction consists of multiple OUTs, it is split into multiple transactions, provided the corresponding OUT is of high enough value (>=$10 USD).
    /// </summary>
    /// <param name="blockchain">The blockchain to search for the specific hash (lowercase)</param>
    /// <param name="hash">The hash of the transaction to return</param>
    /// <returns></returns>
    public Transactions GetTransactions(string blockchain, string hash)
    {
        string url = string.Format(this._transaction, blockchain, hash);

        IRestResponse response = connect(url);

        return errorReturn<Transactions>(response);
    }

    /// <summary>
    /// Returns transactions with timestamp after a set start time (excluding) in order in which they were added to our database. 
    /// This timestamp is the execution time of the transaction on its respective blockchain. Some transactions might be reported with a small delay.
    /// 
    /// Use the cursor with the same start time when retrieving transactions in multiple or continuous requests 
    /// (when retrieving the newest transactions or when the number of retrieved transactions is higher than the limit per result set).
    ///
    /// Low value transactions(<$10 USD) are periodically grouped per blockchain and per FROM and TO address owner to reduce data size.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Transactions GetTransactions(Parameters parameters)
    {
        IRestResponse response = connect(this._transactions, parameters);

        return errorReturn<Transactions>(response);
    }

    /// <summary>
    /// Returns transactions with timestamp after a set start time (excluding) in order in which they were added to our database.
    /// This timestamp is the execution time of the transaction on its respective blockchain. Some transactions might be reported with a small delay.
    /// </summary>
    /// <returns></returns>
    public Transactions GetTransactions()
    {
        IRestResponse response = connect(this._transactions);

        return errorReturn<Transactions>(response);
    }
}
