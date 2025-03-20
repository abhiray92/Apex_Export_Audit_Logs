using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string uri = "ws://192.168.1.2:4000";
        var instrumentService = new InstrumentService();

        InstrumentInfo instrumentInfo = await instrumentService.GetInstrumentDetailsAsync(uri);
        string logFilePath = $"C:\\software\\auditlogs_{instrumentInfo.InstrumentName}_{DateTime.UtcNow:dd-MMM-yyyy}.LOG";
        
        var logService = new LogService();
        await logService.ExportAuditLogsAsync(instrumentInfo.InstrumentName, instrumentInfo.SerialNumber, logFilePath, uri);
    }
}