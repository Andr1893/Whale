using Newtonsoft.Json;

namespace Whalelib.Entity;

public class Transaction
{
    [JsonProperty("blockchain")]
    public string Blockchain { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("transaction_type")]
    public string TransactionType { get; set; }

    [JsonProperty("hash")]
    public string Hash { get; set; }

    [JsonProperty("from")]
    public From From { get; set; }

    [JsonProperty("to")]
    public To To { get; set; }

    [JsonProperty("timestamp")]
    public int Timestamp { get; set; }

    [JsonProperty("amount")]
    public double Amount { get; set; }

    [JsonProperty("amount_usd")]
    public double AmountUsd { get; set; }

    [JsonProperty("transaction_count")]
    public int TransactionCount { get; set; }
}
