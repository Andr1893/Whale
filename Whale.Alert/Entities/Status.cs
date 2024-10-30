using Whale.Alert.Entities;

namespace Whale.Alert.Domain.Entities;

public class Status
{
    public string Result { get; set; } = string.Empty;

    public int BlockchainCount { get; set; }

    public List<Blockchain> Blockchains { get; set; } = [];
}