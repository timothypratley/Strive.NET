using System;

namespace Strive.Rendering
{
	/// <summary>
	/// Represents a video resolution
	/// </summary>
	public class Resolution
	{

		#region Private Fields
		private short  _width;
		private short  _height;
		private byte  _colourdepth;
		private static readonly Resolution _automatic = new Resolution();
		#endregion

		#region Constructors

		/// <summary>
		/// Construct a new Resolution
		/// </summary>
		/// <param name="width">Screen width</param>
		/// <param name="height">Screen height</param>
		/// <param name="colourDepth">Colour depth (32bit,16bit)</param>
		public Resolution(short width, short height, byte colourDepth)
		{
			_width = width;
			_height = height;
			_colourdepth = colourDepth;
		}

		private Resolution()
		{
		}

		#endregion
		

		#region Properties

		/// <summary>
		/// The width of the screen
		/// </summary>
		public short Width
		{
			get
			{
				return _width;
			}
		}

		/// <summary>
		/// Height
		/// </summary>
		public short Height
		{
			get
			{
				return _height;
			}
		}

		/// <summary>
		/// ColourDepth
		/// </summary>
		public byte ColourDepth
		{
			get
			{
				return _colourdepth;
			}
		}

		/// <summary>
		/// Automatic resolution
		/// </summary>
		public static Resolution Automatic
		{
			get
			{
				return _automatic;
			}
		}

		#endregion
	}
}
