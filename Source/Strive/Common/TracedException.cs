using System;

namespace Strive.Common
{
	/// <summary>
	/// Summary description for TracedException.
	/// </summary>
	public class TracedException : System.Exception
	{

		public TracedException() : this(null, null)
		{
		}

		public TracedException(string message) : this(message, null)
		{
		}

		public TracedException(string message, System.Exception innerException) : base(message, innerException)
		{
			string traceMessage = "";
			Exception e = this;
			while(e != null)
			{
				traceMessage += "    " + e.Message + Environment.NewLine;
				traceMessage += "    " + e.Source + Environment.NewLine;
				traceMessage += "    " + e.ToString() + Environment.NewLine + Environment.NewLine;
				e = e.InnerException;
			}

			System.Diagnostics.EventLog.WriteEntry("Strive.Common.TracedException", traceMessage, System.Diagnostics.EventLogEntryType.Error);
		}
	}
}
