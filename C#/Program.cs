using System;
using System.Threading.Tasks;
using System.Windows.Forms; // Make sure to include this for MessageBox

class Program
{
    // The port is fixed for every machine.
    private const int Port = 4000;

    [STAThread] // Required for Windows Forms applications
    static async Task Main(string[] args)
    {
        Application.EnableVisualStyles();

        // 1. Let the user pick which machine to connect to.
        var machineSelectionService = new MachineSelectionService();
        Machine? selectedMachine = machineSelectionService.ShowMachineSelector();

        if (selectedMachine == null)
        {
            PopupService.ShowPopup("No system selected. Exiting...", "Warning");
            Application.Exit();
            return;
        }

        string uri = $"ws://{selectedMachine.IpAddress}:{Port}";
        var instrumentService = new InstrumentService();

        try
        {
            // Fetch instrument details
            InstrumentInfo instrumentInfo = await instrumentService.GetInstrumentDetailsAsync(uri);

#if ADMIN_VERSION
            // Admin version: allow the user to choose the date.
            var datePickerService = new DatePickerService();
            DateTime? selectedDate = datePickerService.ShowDatePicker();

            if (!selectedDate.HasValue)
            {
                PopupService.ShowPopup("No date selected. Exiting...", "Warning");
                Application.Exit();
                return;
            }

            DateTime exportDate = selectedDate.Value;
#else
            // User version: always export the current date.
            DateTime exportDate = DateTime.Now;
#endif

            string dateStamp = exportDate.ToString("dd-MM-yyyy");

            // Create a log file path using the export date
            string logFilePath = $"C:\\software\\auditlogs_{instrumentInfo.InstrumentName}_{dateStamp}.LOG";

            // Create an instance of LogService to export logs
            var logService = new LogService();
            await logService.ExportAuditLogsAsync(instrumentInfo.InstrumentName, instrumentInfo.SerialNumber, logFilePath, uri, exportDate);

            PopupService.ShowPopup($"Logs exported successfully to {logFilePath}", "Notification");
        }
        catch (Exception ex)
        {
            PopupService.ShowPopup($"An error occurred: {ex.Message}", "Warning");
        }

        // Optionally, you can close the application after the user acknowledges the message
        Application.Exit();
    }
}
