# Apex Instrument Logger Suite

## Overview

The Apex Instrument Logger Suite is a multi-language application designed to interact with Apex instruments, specifically focusing on exporting audit logs and retrieving instrument details. This suite includes implementations in C#, Python, and PowerShell, catering to various environments and user preferences.

## Features

- **C# Implementation**: A robust .NET application that connects to Apex instruments via WebSockets, retrieves instrument details, and exports audit logs.
- **Python Implementation**: A Python-based solution that mirrors the functionality of the C# application, providing an alternative for users familiar with Python.
- **PowerShell Implementation**: Scripts that allow users to fetch instrument details and export audit logs directly from PowerShell, enhancing accessibility for Windows users.


## Prerequisites

### For C#

- [.NET SDK](https://dotnet.microsoft.com/download) (Version 9.0 or higher)
- Visual Studio or any compatible IDE

### For Python

- Python 3.x
- Required packages (install via `pip`):
  - `websocket-client`
  - `tkinter` (usually included with Python)

### For PowerShell

- PowerShell 5.1 or higher (available on Windows by default)

## Installation

### C# Implementation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/abhiray92/Apex_Export_Audit_Logs.git
2. Open the solution in Visual Studio and restore the NuGet packages.
3. Build the project and configure any necessary settings in appsettings.json.


###Python Implementation
1. Clone the repository:
   git clone https://github.com/abhiray92/Apex_Export_Audit_Logs.git
2 Navigate to the Python folder:
   cd Apex_Export_Audit_Logs/Python
3. Install required packages:
    pip install websocket-client

###PowerShell Implementation
1. Clone the repository:
    git clone https://github.com/abhiray92/Apex_Export_Audit_Logs.git
2. Navigate to the PowerShell folder:
    cd Apex_Export_Audit_Logs/PowerShell


##Usage
###C# Usage
1. Navigate to the project folder and open the solution in Visual Studio.
2. Update the appsettings.json with the WebSocket URI of your instrument.
3. Run the Program.cs to start fetching instrument details and exporting logs.
###Python Usage
1. Update the uri variable in main.py with the WebSocket URL of your instrument.
2. Execute the script:
    '''python main.py'''
###PowerShell Usage
1. Execute the PowerShell scripts in the terminal, update the URI variable if necessary:
    '''.\Export-AuditLogs.ps1'''

##Contributing
Contributions are welcome! Feel free to submit pull requests or open issues for enhancements, bug fixes, or documentation improvements.
