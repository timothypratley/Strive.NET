using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Strive.Logging {
	/// <summary>
	/// Summary description for Log.
	/// TODO: need to keep the log string regardless of if there is a display,
	/// so that if a log window is created, we can populate it with old log
	/// messages... alternatively write/read from a log file.
	/// </summary>
	public class Log {
		static private TextBoxBase textBoxOutput = null;
		static private TextWriter logFileWriter = null;
		static private StatusBar statusWriter = null;

		public static void SetLogOutput( TextBoxBase output ) {
			textBoxOutput = output;
		}

		public static void SetLogOutput( string filename ) {
			FileStream fs = File.Open( filename, FileMode.Create, FileAccess.Write , FileShare.Read );
			logFileWriter = new StreamWriter( fs );
		}

		public static void SetStatusOutput(StatusBar output)
		{
			statusWriter = output;
		}

		public static void LogStatus(string message)
		{
			
		}

		public static void LogMessage( string message ) {
			message = "["
				+ DateTime.Now.ToShortDateString() + " - "
				+ DateTime.Now.ToLongTimeString()
				+ "] " + message;
			Trace.WriteLine( message );
			if ( logFileWriter != null ) {
				logFileWriter.WriteLine( message );	
				logFileWriter.Flush();
			}
			StringAppendFinite( message );
			Console.WriteLine( message );
		}

		public static void ErrorMessage( Exception e ) {
			ErrorMessage( e.ToString() );
		}

		public static void ErrorMessage( string message ) {
			LogMessage( "ERROR: "+message );
		}

		public static void WarningMessage( string message ) {
			LogMessage( "WARNING: "+message );
		}

		public static void DebugMessage( string message ) {
			Debug.WriteLine( message );
			StringAppendFinite( message );
		}

		private static void StringAppendFinite( string message ) {
			if ( textBoxOutput != null ) {
				textBoxOutput.Text = message + Environment.NewLine + textBoxOutput.Text;
				if ( textBoxOutput.Text.Length > 1000 ) {
					//textBoxOutput.Text = textBoxOutput.Text.Remove(1000, textBoxOutput.Text.Length - 1000);
				}
			}
		}
	}
}
