using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class WebSocketClient : IDisposable
{
    private ClientWebSocket _client;

    public WebSocketClient()
    {
        _client = new ClientWebSocket();
    }

    public async Task ConnectAsync(string uri)
    {
        await _client.ConnectAsync(new Uri(uri), CancellationToken.None);
    }

    public async Task SendAsync(string jsonRequest)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(jsonRequest);
        await _client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task<string> ReceiveAsync()
    {
        var receiveBuffer = new byte[2048];
        var resultText = new StringBuilder();
        WebSocketReceiveResult result;

        do
        {
            result = await _client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
            resultText.Append(Encoding.UTF8.GetString(receiveBuffer, 0, result.Count));
        } while (!result.EndOfMessage);

        return resultText.ToString();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}