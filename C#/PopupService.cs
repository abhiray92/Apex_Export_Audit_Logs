using System.Windows.Forms;

public static class PopupService
{
    public static void ShowPopup(string message, string messageType)
    {
        MessageBoxIcon icon = messageType == "Notification" ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
        if(messageType != "Notification" )
        {
             message += "\n\n For further assistance contact IT Team at *******@.com";
        }
        MessageBox.Show(message, messageType, MessageBoxButtons.OK, icon);
    }
}