using Whale.Alert.Abstractions;
using Whale.Alert.Behaviors;

namespace Whale.Alert;

public sealed class WhaleAlertFactory : IWhaleApiFactory
{
    private readonly HttpClient _httpClient;

    public WhaleAlertFactory(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public IWhaleAlert Create(string apiKey)
    {
        return WhaleAlert.Create(_httpClient, apiKey);
    }

    public IWhaleAlert Create(HttpClient httpClient, string apiKey)
    {
        return WhaleAlert.Create(httpClient, apiKey);
    }
}