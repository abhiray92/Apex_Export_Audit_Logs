Import-Module C:\software\Get-InstrumentName.psm1

$instrumentName = Get-Instrument

$uri = "ws://192.168.1.2:4000"

# Get today's date and convert it to DateTime at midnight
$utcDateTimeAtMidnight = [System.DateTime]::UtcNow.Date
$utcDateTimeAtEndOfDay = [System.DateTime]::UtcNow.Date.AddHours(23).AddMinutes(59)
$utctime_string = $utcDateTimeAtMidnight.ToString("dd-MM-yyyy")

$logFilePath = "C:\software\auditlogs_$instrumentName_$utctime_string.LOG"
$client = New-Object Net.WebSockets.ClientWebSocket
$client.ConnectAsync($uri, [System.Threading.CancellationToken]::None).Wait()

# Get today's date and convert it to DateTime at midnight
$utcDateTimeAtMidnight = [System.DateTime]::UtcNow.Date
$utcDateTimeAtEndOfDay = [System.DateTime]::UtcNow.Date.AddHours(23).AddMinutes(59)

# Format to string in the desired format
$utctodaystart = $utcDateTimeAtMidnight.ToString("yyyy-MM-ddTHH:mm:ss.000Z")
$utctodayend = $utcDateTimeAtEndOfDay.ToString("yyyy-MM-ddTHH:mm:ss.000Z")

$request = @{
  "type" = "get"
  "group" = "thirdPartyAPI"
  "data" = @{
    "field" = "auditTrails"
    "value" = @{
            "startDateTime" = $utctodaystart
            "endDateTime" = $utctodayend
        }
  }
  "messageID" = "a0a3fd19-965c-45ea-ba17-9a4d399ed769"
} | ConvertTo-Json


$buffer = [System.Text.Encoding]::UTF8.GetBytes($request)
$arraySegment = New-Object System.ArraySegment[byte]($buffer, 0, $buffer.Length)
$client.SendAsync($arraySegment, [Net.WebSockets.WebSocketMessageType]::Text, $True, [System.Threading.CancellationToken]::None).Wait()

$receiveBuffer = New-Object byte[] 2048
$resultText = ""

do {
    $result = $client.ReceiveAsync($receiveBuffer, [System.Threading.CancellationToken]::None).Result
    $resultText += [System.Text.Encoding]::UTF8.GetString($receiveBuffer, 0, $result.Count)
} while (-not $result.EndOfMessage)  # Continue until the end of the message

$response = $resultText | ConvertFrom-Json  # Parse the complete JSON response

# Extracting the values and converting to a tabular format with localized timestamps
$tabularData = $response.data.value | Select-Object @{Name='timestamp'; Expression={[System.DateTime]::Parse($_.timestamp).ToLocalTime()}}, user, message

foreach ($item in $tabularData)
{
    $item | Add-Member -MemberType NoteProperty -Name "Instrument_Name" -Value $instrumentName
}
$tabularData | Out-File -FilePath "c:\software\$logFilePath"
$tabularData

$client.Dispose()