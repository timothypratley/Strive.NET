using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Strive.Logging {
	/// <summary>
	/// Summary description for Log.
	/// </summary>
	public class Log {
		private TextBoxBase output = null;
		private TextWriter logFileWriter = null;

		public Log() {
		}

		public Log( TextBoxBase output ) {
			SetLogOutput( output );
		}

		public Log( string filename ) {
			SetLogOutput( filename );
		}

		public void SetLogOutput( TextBoxBase output ) {
			this.output = output;
		}

		public void SetLogOutput( string filename ) {
			FileStream fs = File.Open( filename, FileMode.Create, FileAccess.Write , FileShare.Read );
			logFileWriter = new StreamWriter( fs );
		}

		public void LogMessage( string message ) {
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
		}

		public void ErrorMessage( string message ) {
			LogMessage( "ERROR: "+message );
		}

		public void WarningMessage( string message ) {
			LogMessage( "WARNING: "+message );
		}

		public void DebugMessage( string message ) {
			Debug.WriteLine( message );
			StringAppendFinite( message );
		}

		private void StringAppendFinite( string message ) {
			if ( output != null ) {
				output.Text = message + Environment.NewLine + output.Text;
				if ( output.Text.Length > 1000 ) {
					output.Text = output.Text.Remove(1000, output.Text.Length - 1000);
				}
			}
		}
	}
}
