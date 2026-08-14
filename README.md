# Apex Instrument Logger Suite

A multi-language application for interacting with Apex instruments, focusing on retrieving instrument details and exporting audit trail logs.

## Overview

This suite provides implementations in:

- **C#** — a Windows desktop application for ApexZ50 instruments using a WebSocket API
- **Python** — a script-based alternative with similar export and retrieval functionality
- **PowerShell** — scripts for Windows users who prefer command-line workflows

The C# implementation is the most feature-rich and is the primary focus of this repository.

## Features

### C# Implementation
- Connects to Apex instruments over WebSockets
- Retrieves instrument name and serial number
- Exports audit trail logs for a selected date
- Supports configurable device selection via `machines.json`
- Allows runtime add/delete of devices from the selection window
- Builds into **User** and **Admin** editions from the same code base
- Writes formatted `.LOG` files to a local directory

### Python Implementation
- Mirrors the core functionality of the C# application
- Provides a Python-based alternative for users who prefer Python
- Supports WebSocket-based instrument communication

### PowerShell Implementation
- Fetches instrument details and exports audit logs
- Provides a lightweight option for Windows users
- Can be executed directly from PowerShell

## Requirements

### For C#
- Windows
- .NET 8 SDK
- Windows Forms support

### For Python
- Python 3.x
- `websocket-client`
- `tkinter` (usually included with Python)

### For PowerShell
- PowerShell 5.1 or higher

## Building the C# Application

From the project folder:

### User edition
Exports the current date only.

```bash
dotnet build -c User
```

### Admin edition
Includes a date picker so any date can be exported.

```bash
dotnet build -c Admin
```

### Output executables
- User: `bin/User/net8.0-windows/ApexZ50_Export_Audit_Logs.exe`
- Admin: `bin/Admin/net8.0-windows/ApexZ50_Export_Audit_Logs.exe`

When deploying, copy the entire `net8.0-windows` folder so the executable retains its runtime files and the `machines.json` device list.

## Configuring Devices

The list of selectable systems lives in `machines.json`, which is copied next to the executable on build:

```json
{
  "Machines": [
    { "SystemId": "LE-L434-01", "IpAddress": "10.245.240.192" },
    { "SystemId": "LE-L434-02", "IpAddress": "10.245.240.193" }
  ]
}
```

Devices can also be managed at runtime from the selection window:

- **Add Device** — prompts for a System ID and IP Address
  - Validates non-empty System ID
  - Validates a valid IP address
  - Prevents duplicates
- **Delete Device** — removes the selected device after confirmation

Changes are saved back to the `machines.json` file next to the executable.

> Note: rebuilding from source can overwrite the deployed copy with the source `machines.json`, so for permanent changes during development, update the source file as well.

## Installation

### C# Implementation
1. Clone the repository:

   ```bash
   git clone https://github.com/abhiray92/Apex_Export_Audit_Logs.git
   ```

2. Open the solution in Visual Studio or your preferred IDE.
3. Restore NuGet packages.
4. Build the project and configure any required settings in `appsettings.json`.

### Python Implementation
1. Clone the repository:

   ```bash
   git clone https://github.com/abhiray92/Apex_Export_Audit_Logs.git
   ```

2. Navigate to the Python folder:

   ```bash
   cd Apex_Export_Audit_Logs/Python
   ```

3. Install the required package:

   ```bash
   pip install websocket-client
   ```

### PowerShell Implementation
1. Clone the repository:

   ```bash
   git clone https://github.com/abhiray92/Apex_Export_Audit_Logs.git
   ```

2. Navigate to the PowerShell folder:

   ```bash
   cd Apex_Export_Audit_Logs/PowerShell
   ```

## Usage

### C# Usage
1. Navigate to the project folder and open the solution.
2. Update `machines.json` with the WebSocket IP address(es) of your instrument(s).
3. Run the application.
4. Use the selection window to choose a device.
5. In **Admin** edition, select the date to export.
6. The audit log will be written to:

   ```text
   C:\software\auditlogs_<InstrumentName>_<dd-MM-yyyy>.LOG
   ```

### Python Usage
1. Update the `uri` variable in `main.py` with the WebSocket URL of your instrument.
2. Run the script:

   ```bash
   python main.py
   ```

### PowerShell Usage
1. Execute the PowerShell script in a terminal.
2. Update the URI variable if necessary.
3. Run:

   ```powershell
   .\Export-AuditLogs.ps1
   ```

## Project Structure

### C# Project
- `Program.cs` — Entry point; orchestrates device selection, date logic, and export
- `MachineSelectionService.cs` — Device selection window with add/delete support
- `DatePickerService.cs` — Date picker dialog (Admin edition)
- `InstrumentService.cs` — Retrieves instrument name and serial number over WebSocket
- `LogService.cs` — Requests audit trails and writes the formatted `.LOG` file
- `WebSocketClient.cs` — Thin WebSocket connect/send/receive wrapper
- `PopupService.cs` — Standardized notification/warning message boxes
- `Models/` — Data models for machines, instrument info, and audit log response types
- `machines.json` — Configurable list of selectable devices

### Python Project
- `main.py` — Python entry point for instrument connection and audit log export
- Supporting modules — Implement the WebSocket and export workflow

### PowerShell Project
- `Export-AuditLogs.ps1` — PowerShell script to fetch instrument details and export audit logs

## Output

Exported log files are written to:

```text
C:\software\auditlogs_<InstrumentName>_<dd-MM-yyyy>.LOG
```

Each file includes:
- Instrument name
- Serial number
- A fixed-width table of timestamp, user, and message entries for the exported day

## Contributing

Contributions are welcome. Feel free to submit pull requests or open issues for enhancements, bug fixes, or documentation improvements.

## Support

If you need help, open an issue in this repository or contact the project maintainer.