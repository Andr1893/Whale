using Newtonsoft.Json;

namespace Whalelib.Entity;

public class Transactions
{
    [JsonProperty("result")]
    public string Result { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("transactions")]
    public List<Transaction> Transaction { get; set; }
}
