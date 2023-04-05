using System.Text.Json.Nodes;
using RestSharp;

namespace CheapConnect.NET;

public class CheapConnectApi
{
    private const string BaseUrl = "https://account.cheapconnect.net/API/v1";
    private readonly RestClient _client;
    private readonly string _apiKey;
    
    public CheapConnectApi(string apiKey)
    {
        _client = new RestClient(BaseUrl);
        _apiKey = apiKey.Trim();
    }

    /// <summary>
    /// Send an SMS message asynchronously.
    /// </summary>
    /// <param name="sender">Sender's phone number in E164 format</param>
    /// <param name="recipient">Recipient's phone number in E164 format</param>
    /// <param name="message">Message for recipient</param>
    /// <exception cref="CheapConnectException"></exception>
    public async Task SendSmsMessageAsync(string sender, string recipient, string message)
    {
        var request = new RestRequest("/sms/SendSMS", Method.Post);
        
        request.AddParameter("apikey", _apiKey);
        request.AddParameter("from", sender);
        request.AddParameter("to", recipient);
        request.AddParameter("msg", message);
        
        var response = await _client.ExecuteAsync(request);
        if (response.Content == null) 
            throw new CheapConnectException("The server returned an empty response");

        var error = JsonNode.Parse(response.Content)?["error"]?.GetValue<string>();
        if (error == null)
            throw new CheapConnectException($"The server gave an invalid response: {response.Content}");

        var split = error.Split(':', 1);
        throw new CheapConnectException(split[1], int.Parse(split[0]));
    }

    /// <summary>
    /// Send an SMS message.
    /// </summary>
    /// <param name="sender">Sender's phone number in E164 format</param>
    /// <param name="recipient">Recipient's phone number in E164 format</param>
    /// <param name="message">Message for recipient</param>
    /// <exception cref="CheapConnectException"></exception>
    public void SendSmsMessage(string sender, string recipient, string message) =>
        SendSmsMessageAsync(sender, recipient, message).GetAwaiter().GetResult();

    /// <summary>
    /// Try to send an SMS message asynchronously.
    /// </summary>
    /// <param name="sender">Sender's phone number in E164 format</param>
    /// <param name="recipient">Recipient's phone number in E164 format</param>
    /// <param name="message">Message for recipient</param>
    /// <returns>Returns true if sent successfully</returns>
    public async Task<bool> TrySendSmsMessageAsync(string sender, string recipient, string message)
    {
        try
        {
            await SendSmsMessageAsync(sender, recipient, message);
            return true;
        }
        catch (CheapConnectException)
        {
            return false;
        }
    }
    
    /// <summary>
    /// Try to send an SMS message.
    /// </summary>
    /// <param name="sender">Sender's phone number in E164 format</param>
    /// <param name="recipient">Recipient's phone number in E164 format</param>
    /// <param name="message">Message for recipient</param>
    /// <returns>Returns true if sent successfully</returns>
    public bool TrySendSmsMessage(string sender, string recipient, string message) =>
        TrySendSmsMessageAsync(sender, recipient, message).GetAwaiter().GetResult();
}