namespace Whale.Alert.Entities;

public class Blockchain
{
    public string Name { get; set; } = string.Empty;

    public List<string> Symbols { get; set; } = [];

    public string Status { get; set; } = string.Empty;
}