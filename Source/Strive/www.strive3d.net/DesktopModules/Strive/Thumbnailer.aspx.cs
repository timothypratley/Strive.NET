using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace www.strive3d.net
{
	/// <summary>
	/// Summary description for Thumbnailer.
	/// </summary>
	public class Thumbnailer : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!thisterminal.Web.QueryString.ContainsVariable("i"))
			{
				throw new thisterminal.Web.QueryStringException("You must specify an image.");
			}
			if(!thisterminal.Web.QueryString.ContainsVariable("w"))
			{
				throw new thisterminal.Web.QueryStringException("You must specify an image width.");
			}
			if(!thisterminal.Web.QueryString.ContainsVariable("h"))
			{
				throw new thisterminal.Web.QueryStringException("You must specify an image height.");
			}
			int rotation = 0;
			if(thisterminal.Web.QueryString.ContainsVariable("r"))
			{
				rotation = thisterminal.Web.QueryString.GetVariableInt32Value("r");
			}
			string filename = Server.MapPath(thisterminal.Web.QueryString.GetVariableStringValue("i"));
			
			if(!filename.ToLower().EndsWith(".png") &&
				!filename.ToLower().EndsWith(".jpg") &&
				!filename.ToLower().EndsWith(".jpeg") &&
				!filename.ToLower().EndsWith(".gif") &&
				!filename.ToLower().EndsWith(".bmp"))
			{
				throw new thisterminal.Web.QueryStringException("You must specify an image.");
			}

			if(!System.IO.File.Exists(Server.MapPath(thisterminal.Web.QueryString.GetVariableStringValue("i"))))
			{
				throw new thisterminal.Web.QueryStringException("The image '" + thisterminal.Web.QueryString.GetVariableStringValue("i") + "' does not exist.");
			}


			System.IO.FileStream stream = System.IO.File.OpenRead(filename);
			byte[] originalImage = new byte[stream.Length];
			stream.Read(originalImage, 0, (int)stream.Length);

			Response.Clear();
			Response.ContentType = "image/" + System.IO.Path.GetExtension(filename).Replace(".", "");
			Response.AddHeader("Content-Disposition", "filename=\"" + System.IO.Path.GetFileName(filename) + "\";");
			Response.BinaryWrite(GetImage(originalImage, 
				thisterminal.Web.QueryString.GetVariableInt32Value("h"), 
				thisterminal.Web.QueryString.GetVariableInt32Value("w"),
				rotation)  );
			Response.End();

		}

		/// <summary>
		/// Retrieves the specified image bytes as a resized image
		/// </summary>
		/// <param name="imageContents">The image bytes</param>
		/// <param name="imageMimeType">The mime type of the specified image</param>
		/// <param name="imageHeight">The desired height of the returned image</param>
		/// <param name="imageWidth">The desired width of the returned image</param>
		/// <returns>The bytes of a resized image</returns>
		public static byte[] GetImage(byte[] imageContents, int imageHeight, int imageWidth, int rotation)
		{
			if(imageContents == null)
			{
				throw new ArgumentNullException("imageContents");
			}
			if(imageHeight <= 0 || imageWidth <= 0 )
			{
				return imageContents;
			}
			else
			{
				// resize it:
				System.IO.MemoryStream originalImage = new System.IO.MemoryStream(imageContents, 0, imageContents.Length, false, false);
				Bitmap originalBitmap = new Bitmap(originalImage);
				if(imageHeight > originalBitmap.Height ||
					imageWidth > originalBitmap.Width)
				{
					throw new Exception("You cannot request an image larger than its native size.");
				}
				Bitmap newBitmap = new Bitmap(originalBitmap, imageWidth, imageHeight);
				System.Drawing.RotateFlipType rotateType = RotateFlipType.RotateNoneFlipNone;
				switch(rotation)
				{
					case 0:
						break;
					case 90:
					{
						rotateType = RotateFlipType.Rotate90FlipNone;
						break;
					}
					case 180:
					{
						rotateType = RotateFlipType.Rotate180FlipNone;
						break;
					}
					case 270:
					{
						rotateType = RotateFlipType.Rotate270FlipNone;
						break;
					}
					default:
						throw new Exception( "bugger" );
				}
				newBitmap.RotateFlip(rotateType);
				System.IO.MemoryStream newImage = new System.IO.MemoryStream();
				newBitmap.Save(newImage, originalBitmap.RawFormat);
				return newImage.GetBuffer();
			}
		}

		/// <summary>
		/// Measures the specified image
		/// </summary>
		/// <param name="imageContents">THe bytes of the specified image</param>
		/// <param name="imageHeight">Will be set to the height of the specified image</param>
		/// <param name="imageWidth">Will be set to the width of the specified image</param>
		public static void GetImageDimensions(byte[] imageContents, out int imageHeight, out int imageWidth)
		{
			System.IO.MemoryStream originalImage = new System.IO.MemoryStream(imageContents, 0, imageContents.Length, false, false);
			Bitmap originalBitmap = new Bitmap(originalImage);
			imageHeight = originalBitmap.Height;
			imageWidth = originalBitmap.Width;
			originalBitmap.Dispose();
		}


		public bool ThumbnailCallback()
		{
			return false;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
