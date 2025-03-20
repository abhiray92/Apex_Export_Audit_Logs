
$uri = "ws://192.168.1.2:4000"
$client = New-Object Net.WebSockets.ClientWebSocket
$client.ConnectAsync($uri, [System.Threading.CancellationToken]::None).Wait()

$request = @{
  "type" = "get"
  "group" = "thirdPartyAPI"
  "data" = @{
   "field" = "information"
   "value" = $true
  }
  "messageID" = "a0a3fd19-965c-45ea-ba17-9a4d399ed769"
} | ConvertTo-Json

$buffer = [System.Text.Encoding]::UTF8.GetBytes($request)
$arraySegment = New-Object System.ArraySegment[byte]($buffer, 0, $buffer.Length)
$client.SendAsync($arraySegment, [Net.WebSockets.WebSocketMessageType]::Text, $True, [System.Threading.CancellationToken]::None).Wait()

$receiveBuffer = New-Object byte[] 2048
$result = $client.ReceiveAsync($receiveBuffer, [SystemThreading.CancellationToken]::None).Result
$response = [System.Text.Encoding]::UTF8GetString($receiveBuffer, 0, $result.Count)

Write-Output $response
$client.Dispose()

