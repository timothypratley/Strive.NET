using System;

namespace Strive.Rendering
{
	/// <summary>
	/// Base strive rendering exceptino
	/// </summary>
	public abstract class StriveRenderingExceptionBase : Exception
	{

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception</param>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
		public StriveRenderingExceptionBase(string message, Exception innerException) : base("[System:Strive.Rendering.TV3D]" + message, innerException)
		{
			// TODO: Log this
			System.Diagnostics.Debug.WriteLine("** StriveRenderingExceptionBase");
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
		public StriveRenderingExceptionBase(string message) : this(message, null)
		{
		}

		/// <summary>
		/// Shortcut constructor (null message, null innerException
		/// </summary>
		public StriveRenderingExceptionBase() : this("Unknown cause", null)
		{
		}

	}

	/// <summary>
	/// Thrown when the engine could not be initialised
	/// </summary>
	public class EngineInitialisationException : StriveRenderingExceptionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
		public EngineInitialisationException(Exception innerException) : base("Engine could not be initialised", innerException)
		{

		}
		/// <summary>
		/// Shortcut constructor (null innerException)
		/// </summary>
		public EngineInitialisationException() : this(null)
		{
		}
	}

	/// <summary>
	/// Thrown when an attempt is made to change the Scene when it is not properly initialised
	/// </summary>
	public class SceneNotInitialisedException : StriveRenderingExceptionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SceneNotInitialisedException() : base("You must initialise the scene before attempting to modify it")
		{
		}
	}

	/// <summary>
	/// Thrown when an attempt is made to create more than 1 scene
	/// </summary>
	public class SceneAlreadyExistsException : StriveRenderingExceptionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SceneAlreadyExistsException() : base("You can only initialise one Scene")
		{
		}
	}

	/// <summary>
	/// Thrown when a scene operation fails
	/// </summary>
	public class SceneException : StriveRenderingExceptionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception</param>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
		public SceneException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	/// <summary>
	/// Thrown when a redering operation fails
	/// </summary>
	public class RenderingException : StriveRenderingExceptionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception</param>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
		public RenderingException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

}
