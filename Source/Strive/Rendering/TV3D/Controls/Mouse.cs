using System;
using TrueVision3D;

using Strive.Rendering.Controls;

namespace Strive.Rendering.TV3D.Controls
{
	/// <summary>
	/// Summary description for Mouse.
	/// </summary>
	public class Mouse : IMouse
	{
		int x=0, y=0;
		short button1down=0, button2down=0, button3down=0;
		int intellimouseroll=0;
		
		public void GetState() 
		{
			Engine.Input.GetMouseState(ref x, ref y, ref button1down, ref button2down, ref button3down, ref intellimouseroll );
		}

		public void GetAbsState() 
		{
			Engine.Input.GetAbsMouseState( ref x, ref y, ref button1down, ref button2down, ref button3down );
			//Logging.Log.LogMessage( "x: " + x + ", y: " + y );
		}

		public void AccumulateState() {
			int tmpx=0, tmpy=0;
			short tmpb1=0, tmpb2=0, tmpb3=0;
			int tmpimr=0;
			Engine.Input.GetMouseState( ref tmpx, ref tmpy, ref tmpb1, ref tmpb2, ref tmpb3, ref tmpimr );
			x+=tmpx; y+=tmpy;
			if ( tmpb1 != 0 ) button1down = 1;
			if ( tmpb2 != 0 ) button2down = 1;
			if ( tmpb3 != 0 ) button3down = 1;
			if ( tmpimr != 0 ) intellimouseroll = 1;
		}

		public void ShowCursor( bool showCursor ) 
		{
			Engine.TV3DEngine.ShowWinCursor( showCursor );
		}

		public int X 
		{
			get { return x; }
		}
		public int Y 
		{
			get { return y; }
		}
		public bool Button1down 
		{
			get { return button1down!=0; }
		}
		public bool Button2down 
		{
			get { return button2down!=0; }
		}
		public bool Button3down 
		{
			get { return button3down!=0; }
		}
	}
}
