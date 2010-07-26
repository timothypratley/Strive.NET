using System;
using System.Windows.Forms;

namespace Strive.Client.WinForms.Cursors
{
	/// <summary>
	/// Summary description for CursorManager.
	/// </summary>
	public class CursorManager
	{
		public static Cursor Default = new Cursor( typeof(CursorManager).Assembly.GetManifestResourceStream("Strive.UI.Cursors.Default.cur") );
		public static Cursor Kill = new Cursor( typeof(CursorManager).Assembly.GetManifestResourceStream("Strive.UI.Cursors.Kill.cur") );
		public CursorManager()
		{
		}
	}
}
