using Whale.Alert.Abstractions;

namespace Whale.Alert.DependencyInjection;

public class WhaleAlertOptions
{
    public string BaseAddress { get; set; } = IWhaleAlert.BaseAddress;
    public required string ApiKey { get; set; }
}