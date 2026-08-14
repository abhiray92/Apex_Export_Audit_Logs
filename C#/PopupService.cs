using System.Windows.Forms;

public static class PopupService
{
    public static void ShowPopup(string message, string messageType)
    {
        MessageBoxIcon icon = messageType == "Notification" ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
        if(messageType != "Notification" )
        {
             message += "\n\n For further assistance contact BioNX IT Team at BIONXMRLIT@msd.com";
        }
        MessageBox.Show(message, messageType, MessageBoxButtons.OK, icon);

        //Contact BioNX IT Team at BioNXMRLIT@msd.com //If error/warning
    }
}