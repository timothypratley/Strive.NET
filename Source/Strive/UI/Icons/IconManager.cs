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
		private static ImageList _globalImageList;

		private static Bitmap GetAsBitmap(AvailableIcons icon)
		{
			return GetAsBitmap(icon.ToString());
		}

		private static Bitmap GetAsBitmap(string iconname)
		{
			// Get the assembly we are built into
			Assembly myAssembly = 
				Assembly.GetAssembly(Type.GetType("Strive.UI.Icons.IconManager"));
 
			// Get the resource stream containing the embedded resource
			Stream imageStream = 
				myAssembly.GetManifestResourceStream("Strive.UI.Icons." + iconname + ".bmp");

			// Load the bitmap from the stream
			Bitmap pics = new Bitmap(imageStream);
			imageStream.Close();

			return pics;
		}

		private static ImageList GetAsImageList(AvailableIcons icon)
		{
			ImageList returnList = new ImageList();
			returnList.Images.Add(GetAsBitmap(icon));
			return returnList;
		}

		public static ImageList GlobalImageList
		{
			get
			{
				if(_globalImageList == null)
				{
					_globalImageList = new ImageList();
					System.Array values = Enum.GetValues(typeof(AvailableIcons));
					foreach(object o in values)
					{
                        string name = Enum.GetName(typeof(AvailableIcons), o);
						_globalImageList.Images.Add(GetAsBitmap(name));
					}
				}
				return _globalImageList;
			}

		}

	}

	public enum AvailableIcons
	{
		Connection = 0,
		StartedServer = 1,
		StoppedServer = 2,
		Player = 3,
		Mobile = 4,
		MobilePossessed = 5,
		Refresh = 6,
		Log = 7,
		Chat = 8,
		Command = 9
	}
}
