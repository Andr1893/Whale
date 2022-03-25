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
        request.AddOrUpdateParameter("end", parameters.start);
        request.AddOrUpdateParameter("cursor", parameters.start);
        request.AddOrUpdateParameter("min_value", parameters.start);
        request.AddOrUpdateParameter("limit", parameters.start);
        request.AddOrUpdateParameter("currency", parameters.start);

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
                throw new WhaleExeception("There was a problem with the API host server. Try again later.");
            case HttpStatusCode.ServiceUnavailable:
                throw new WhaleExeception("API is temporarily offline for maintenance. Try again later.");
            case HttpStatusCode.OK:
            default:
                return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }

    //Methods gets
    public Status getStatus()
    {
        IRestResponse response = connect(this._status);

        return errorReturn<Status>(response);
    }
   
    public Transactions GetTransactions(string blockchain, string hash)
    {
        string url = string.Format(this._transaction, blockchain, hash);

        IRestResponse response = connect(url);

        return errorReturn<Transactions>(response);
    }
    
    public Transactions GetTransactions(Parameters parameters)
    {
        IRestResponse response = connect(this._transactions, parameters);

        return errorReturn<Transactions>(response);
    }
   
    public Transactions GetTransactions()
    {
        IRestResponse response = connect(this._transactions);

        return errorReturn<Transactions>(response);
    }
}
