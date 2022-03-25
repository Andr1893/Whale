using Newtonsoft.Json;

namespace Whalelib.Entity;

public class To
{
    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("owner_type")]
    public string OwnerType { get; set; }
}
