namespace Whale.Alert.Errors;

public class WhaleException : Exception
{
    public WhaleException() : base() { }
    public WhaleException(string message) : base(message) { }
}