using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Strive.UI.Icons
{
	/// <summary>
	/// Summary description for IconManager.
	/// </summary>
	public class IconManager
	{

		public static Bitmap GetAsBitmap(AvailableIcons icon)
		{
			// Get the assembly we are built into
			Assembly myAssembly = 
				Assembly.GetAssembly(Type.GetType("Strive.UI.Icons.IconManager"));
 
			// Get the resource stream containing the embedded resource
			Stream imageStream = 
				myAssembly.GetManifestResourceStream("Strive.UI.Icons." + icon.ToString() + ".bmp");

			// Load the bitmap from the stream
			Bitmap pics = new Bitmap(imageStream);
			imageStream.Close();

			return pics;
		}

		public static ImageList GetAsImageList(AvailableIcons icon)
		{
			ImageList returnList = new ImageList();
			returnList.Images.Add(GetAsBitmap(icon));
			return returnList;
		}

	}

	public enum AvailableIcons
	{
		Connection,
		StartedServer,
		StoppedServer
	}
}
