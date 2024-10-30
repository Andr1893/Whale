namespace Whale.Alert.Entities;

public class Transactions
{
    public string Result { get; set; } = string.Empty;
    
    public int Count { get; set; }

    public List<Transaction> Transaction { get; set; } = [];
}
