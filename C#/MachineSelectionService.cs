using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;

public class MachineSelectionService
{
    private const string MachinesFileName = "machines.json";

    // Full path to the machines.json that was loaded; changes are saved back here.
    private string _machinesPath = string.Empty;

    // Loads the configured machines and shows a selection window.
    // Returns the selected Machine, or null if the user cancels.
    public Machine? ShowMachineSelector()
    {
        var machines = new BindingList<Machine>(LoadMachines());

        using (var form = new Form())
        {
            form.Text = "Select a System";
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ClientSize = new Size(460, 380);

            var grid = new DataGridView
            {
                Location = new Point(15, 15),
                Size = new Size(430, 240),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                AutoGenerateColumns = false
            };

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "System ID",
                DataPropertyName = nameof(Machine.SystemId),
                FillWeight = 45
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "System IP Address",
                DataPropertyName = nameof(Machine.IpAddress),
                FillWeight = 55
            });

            grid.DataSource = machines;

            var addButton = new Button
            {
                Text = "Add Device",
                Location = new Point(15, 270),
                Size = new Size(100, 30)
            };

            var deleteButton = new Button
            {
                Text = "Delete Device",
                Location = new Point(125, 270),
                Size = new Size(100, 30)
            };

            var okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(275, 330),
                Size = new Size(80, 30)
            };

            var cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(365, 330),
                Size = new Size(80, 30)
            };

            addButton.Click += (s, e) =>
            {
                Machine? newMachine = PromptForDevice(machines);
                if (newMachine != null)
                {
                    machines.Add(newMachine);
                    SaveMachines(machines);
                }
            };

            deleteButton.Click += (s, e) =>
            {
                if (grid.CurrentRow?.DataBoundItem is Machine toRemove)
                {
                    var confirm = MessageBox.Show(
                        $"Remove '{toRemove.SystemId}' ({toRemove.IpAddress})?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        machines.Remove(toRemove);
                        SaveMachines(machines);
                    }
                }
                else
                {
                    PopupService.ShowPopup("Please select a device to delete.", "Warning");
                }
            };

            form.Controls.Add(grid);
            form.Controls.Add(addButton);
            form.Controls.Add(deleteButton);
            form.Controls.Add(okButton);
            form.Controls.Add(cancelButton);
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;

            // Double-clicking a row confirms the selection.
            grid.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                }
            };

            if (form.ShowDialog() == DialogResult.OK && grid.CurrentRow?.DataBoundItem is Machine selected)
            {
                return selected;
            }
        }

        return null;
    }

    // Shows a small dialog asking for a System ID and IP Address.
    // Returns a validated Machine, or null if cancelled.
    private Machine? PromptForDevice(BindingList<Machine> existing)
    {
        using (var dialog = new Form())
        {
            dialog.Text = "Add Device";
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.ClientSize = new Size(320, 160);

            var idLabel = new Label
            {
                Text = "System ID:",
                Location = new Point(15, 20),
                Size = new Size(90, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            var idBox = new TextBox
            {
                Location = new Point(110, 18),
                Size = new Size(190, 23)
            };

            var ipLabel = new Label
            {
                Text = "IP Address:",
                Location = new Point(15, 55),
                Size = new Size(90, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            var ipBox = new TextBox
            {
                Location = new Point(110, 53),
                Size = new Size(190, 23)
            };

            var okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(130, 110),
                Size = new Size(80, 30)
            };
            var cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(220, 110),
                Size = new Size(80, 30)
            };

            dialog.Controls.Add(idLabel);
            dialog.Controls.Add(idBox);
            dialog.Controls.Add(ipLabel);
            dialog.Controls.Add(ipBox);
            dialog.Controls.Add(okButton);
            dialog.Controls.Add(cancelButton);
            dialog.AcceptButton = okButton;
            dialog.CancelButton = cancelButton;

            // Validate before closing when OK is pressed.
            okButton.Click += (s, e) =>
            {
                string id = idBox.Text.Trim();
                string ip = ipBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(id))
                {
                    PopupService.ShowPopup("System ID cannot be empty.", "Warning");
                    dialog.DialogResult = DialogResult.None;
                    return;
                }

                if (!IPAddress.TryParse(ip, out _))
                {
                    PopupService.ShowPopup($"'{ip}' is not a valid IP address.", "Warning");
                    dialog.DialogResult = DialogResult.None;
                    return;
                }

                foreach (var m in existing)
                {
                    if (string.Equals(m.SystemId, id, StringComparison.OrdinalIgnoreCase))
                    {
                        PopupService.ShowPopup($"A device with System ID '{id}' already exists.", "Warning");
                        dialog.DialogResult = DialogResult.None;
                        return;
                    }
                    if (string.Equals(m.IpAddress, ip, StringComparison.OrdinalIgnoreCase))
                    {
                        PopupService.ShowPopup($"A device with IP address '{ip}' already exists.", "Warning");
                        dialog.DialogResult = DialogResult.None;
                        return;
                    }
                }
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return new Machine
                {
                    SystemId = idBox.Text.Trim(),
                    IpAddress = ipBox.Text.Trim()
                };
            }
        }

        return null;
    }

    private List<Machine> LoadMachines()
    {
        _machinesPath = Path.Combine(AppContext.BaseDirectory, MachinesFileName);

        try
        {
            if (!File.Exists(_machinesPath))
            {
                PopupService.ShowPopup(
                    $"Configuration file not found: {_machinesPath}",
                    "Warning");
                return new List<Machine>();
            }

            string json = File.ReadAllText(_machinesPath);
            var config = JsonSerializer.Deserialize<MachineConfig>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return config?.Machines ?? new List<Machine>();
        }
        catch (Exception ex)
        {
            PopupService.ShowPopup(
                $"Failed to read the machine list from {MachinesFileName}: {ex.Message}",
                "Warning");
            return new List<Machine>();
        }
    }

    private void SaveMachines(IEnumerable<Machine> machines)
    {
        try
        {
            var config = new MachineConfig { Machines = new List<Machine>(machines) };
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_machinesPath, json);
        }
        catch (Exception ex)
        {
            PopupService.ShowPopup(
                $"Failed to save the machine list to {MachinesFileName}: {ex.Message}",
                "Warning");
        }
    }
}
