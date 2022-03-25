using Newtonsoft.Json;

namespace Whalelib.Entity;


public class Blockchain
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("symbols")]
    public List<string> Symbols { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
}
