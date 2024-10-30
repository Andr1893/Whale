namespace Whale.Alert.Parameters;

public class Parameter
{
    public required int Start { get; set; }
    public int End { get; set; }
    public string Cursor { get; set; } = string.Empty;
    public int MinValue { get; set; }
    public int Limit { get; set; }
    public string Currency { get; set; } = string.Empty;
}