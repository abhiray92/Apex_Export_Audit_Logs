using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.WebSockets;

public class InstrumentService
{
    public async Task<InstrumentInfo> GetInstrumentDetailsAsync(string uri)
    {
        try
        {
            using (var webSocketClient = new WebSocketClient())
            {
                await webSocketClient.ConnectAsync(uri);
                Console.WriteLine("Connected to WebSocket for instrument name.");

                var request = new
                {
                    type = "get",
                    group = "thirdPartyAPI",
                    data = new
                    {
                        field = "information",
                        value = true
                    },
                    messageID = Guid.NewGuid().ToString()
                };

                string jsonRequest = JsonSerializer.Serialize(request);
                await webSocketClient.SendAsync(jsonRequest);

                string response = await webSocketClient.ReceiveAsync();
                Console.WriteLine("Raw JSON response: " + response);

                var jsonResponse = JsonSerializer.Deserialize<JsonResponse>(response);
                var instrumentValue = jsonResponse?.data?.value;
                
                var instrumentInfo = new InstrumentInfo             // Create an instance of InstrumentInfo
                {
                    InstrumentName = instrumentValue?.instrumentName ?? "Unknown Instrument",
                    SerialNumber = instrumentValue?.serialNumber
                };

            return instrumentInfo; // Now return the InstrumentInfo object
            }
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON error occurred: {jsonEx.Message}");
            PopupService.ShowPopup("Invalid Login. The ID/Password is incorrect or the user does not have sufficient privileges", "Warning");
            Environment.Exit(1);
            return null;
        }
        catch (WebSocketException webSocketEx)
        {
            Console.WriteLine($"WebSocket error occurred: {webSocketEx.Message}");
            PopupService.ShowPopup("WebSocket connection failed.", "Warning");
            Environment.Exit(1);
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            PopupService.ShowPopup("An unexpected error occurred. Please try again.", "Warning");
            Environment.Exit(1);
            return null;
        }
    }
}