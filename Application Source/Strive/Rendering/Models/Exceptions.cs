using System;

namespace Strive.Rendering.Models
{
	/// <summary>
	/// Thrown when the model format could not be determined
	/// </summary>
	public class ModelFormatUnknownException : StriveRenderingExceptionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="location">The path of other location of the model that could not be loaded</param>
		/// <param name="format">The format the model was attempted to be loaded as</param>
		/// <param name="innerException">The exception that is the cause of the ModelFormatUnknownException</param>
		public ModelFormatUnknownException(string location, ModelFormat format, Exception innerException) : base("Unknown format '" + format.ToString()  +"'" +
			"for model '" + location + "'.", innerException)
		{
		}

		/// <summary>
		/// Shortcut constructor (no innerException)
		/// </summary>
		/// <param name="location">The path of other location of the model that could not be loaded</param>
		/// <param name="format">The format the model was attempted to be loaded as</param>
		public ModelFormatUnknownException(string location, ModelFormat format) : this(location, format, null)
		{
		}
	}

	/// <summary>
	/// Thrown when a model can not be loaded
	/// </summary>
	public class ModelNotLoadedException : StriveRenderingExceptionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="location">The path of other location of the model that could not be loaded</param>
		/// <param name="format">The format the model was attempted to be loaded as</param>
		/// <param name="innerException">The exception that is the cause of the ModelFormatUnknownException</param>
		public ModelNotLoadedException(string location, ModelFormat format, Exception innerException) : base("Unknown format '" + format.ToString()  +"'" +
			"for model '" + location + "'.", innerException)
		{
		}

		/// <summary>
		/// Shortcut constructor (no innerException)
		/// </summary>
		/// <param name="location">The path of other location of the model that could not be loaded</param>
		/// <param name="format">The format the model was attempted to be loaded as</param>
		public ModelNotLoadedException(string location, ModelFormat format) : this(location, format, null)
		{
		}
	}

	/// <summary>
	/// Thrown during a model operation exception
	/// </summary>
	public class ModelException : StriveRenderingExceptionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception</param>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
		public ModelException(string message, Exception innerException) : base(message, innerException)
	{
	}
	}
	

}
