using System;
using System.Windows.Forms;

public class DatePickerService
{
    public DateTime? ShowDatePicker()
    {
        using (var datePickerForm = new Form())
        {
            var dateTimePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Location = new System.Drawing.Point(20, 20),
                Width = 200
            };

            var okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(20, 60)
            };

            datePickerForm.Controls.Add(dateTimePicker);
            datePickerForm.Controls.Add(okButton);
            datePickerForm.AcceptButton = okButton;

            if (datePickerForm.ShowDialog() == DialogResult.OK)
            {
                return dateTimePicker.Value;
            }
        }

        return null; // Return null if the dialog is canceled
    }
}
