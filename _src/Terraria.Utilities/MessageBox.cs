using System.Windows.Forms;

namespace Terraria.Utilities;

public static class MessageBox
{
	public static DialogResult Show(string message, string title, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
	{
		return (DialogResult)System.Windows.Forms.MessageBox.Show(message, title, (System.Windows.Forms.MessageBoxButtons)buttons, (System.Windows.Forms.MessageBoxIcon)icon);
	}
}
