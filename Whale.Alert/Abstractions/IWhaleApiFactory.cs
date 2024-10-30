namespace Whale.Alert.Abstractions;

public interface IWhaleApiFactory
{
    IWhaleAlert Create(string apiKey);

    IWhaleAlert Create(HttpClient httpClient, string apiKey);
}