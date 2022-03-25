using Newtonsoft.Json;

namespace Whalelib.Entity;


public class From
{
    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("owner")]
    public string Owner { get; set; }

    [JsonProperty("owner_type")]
    public string OwnerType { get; set; }
}
