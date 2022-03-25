using Newtonsoft.Json;

namespace Whalelib.Entity;


public class Status
{
    [JsonProperty("result")]
    public string Result { get; set; }

    [JsonProperty("blockchain_count")]
    public int BlockchainCount { get; set; }

    [JsonProperty("blockchains")]
    public List<Blockchain> Blockchains { get; set; }
}
