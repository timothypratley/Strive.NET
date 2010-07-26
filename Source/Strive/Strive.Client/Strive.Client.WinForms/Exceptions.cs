using System;

namespace Strive.UI
{
	/// <summary>
	/// Base strive UI exceptino
	/// </summary>
	public abstract class StriveUIExceptionBase : Exception
	{

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception</param>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
		public StriveUIExceptionBase(string message, Exception innerException) : base("[System:Strive.UI]" + message, innerException)
		{
			// TODO: Log this
			System.Diagnostics.Debug.WriteLine("** StriveUIExceptionBase");
			System.Diagnostics.Debug.WriteLine("**** " + message);
			System.Diagnostics.Debug.WriteLine("*******************************");
			System.Diagnostics.Debug.WriteLine(this.ToString());
			System.Diagnostics.Debug.WriteLine("*******************************");
			Exception e = innerException;
			while(e != null)
			{
				System.Diagnostics.Debug.WriteLine(e.ToString());
				System.Diagnostics.Debug.WriteLine("*******************************");
				e = e.InnerException;
			}
		}

		/// <summary>
		/// Shortcut constructor (null innerException)
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception</param>
		public StriveUIExceptionBase(string message) : this(message, null)
		{
		}

		/// <summary>
		/// Shortcut constructor (null message, null innerException
		/// </summary>
		public StriveUIExceptionBase() : this("Unknown cause", null)
		{
		}

	}
}
