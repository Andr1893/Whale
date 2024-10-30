namespace Whale.Alert.Entities;

public class Transaction
{
    public string Blockchain { get; set; } = string.Empty;
    
    public string Symbol { get; set; } = string.Empty;
    
    public string TransactionType { get; set; } = string.Empty;
    
    public string Hash { get; set; } = string.Empty;

    public From From { get; set; } = null!;
    
    public To To { get; set; }= null!;
    
    public int Timestamp { get; set; }
    
    public double Amount { get; set; }
    
    public double AmountUsd { get; set; }
    
    public int TransactionCount { get; set; }
}
