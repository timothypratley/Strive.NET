using System;

using Strive.Rendering.Controls;
using TrueVision3D;

namespace Strive.Rendering.TV3D.Controls {
	
	/// Represents a single keyboard key
	
	public class Keys {
		
		/// Supports the following: CONST_TV_KEY r: return (CONST_TV_KEY)Key;
		
		/// <param name="k">The Key to cast from</param>
		/// <returns>The CONST_TV_KEY result of the cast</returns>
		public static CONST_TV_KEY getTVKeyFromKey(Key k) {
			switch ( k ) {
				default: throw new Exception( "No such key!" );
					#region Alphanumeric
				case Key.key_0: return CONST_TV_KEY.TV_KEY_0;
				case Key.key_1: return CONST_TV_KEY.TV_KEY_1;
		
		
				
				case Key.key_2: return CONST_TV_KEY.TV_KEY_2;
		
		
				
				case Key.key_3: return CONST_TV_KEY.TV_KEY_3;
		
		
				
				case Key.key_4: return CONST_TV_KEY.TV_KEY_4;
		
		
				
				case Key.key_5: return CONST_TV_KEY.TV_KEY_5;
		
		
				
				case Key.key_6: return CONST_TV_KEY.TV_KEY_6;
		
		
				
				case Key.key_7: return CONST_TV_KEY.TV_KEY_7;
		
		
				
				case Key.key_8: return CONST_TV_KEY.TV_KEY_8;
		
		
				
				case Key.key_9: return CONST_TV_KEY.TV_KEY_9;
		
		
				
				case Key.key_A: return CONST_TV_KEY.TV_KEY_A;
		
		
				
				case Key.key_B: return CONST_TV_KEY.TV_KEY_B;
		
		
				
				case Key.key_C: return CONST_TV_KEY.TV_KEY_C;
		
		
				
				case Key.key_D: return CONST_TV_KEY.TV_KEY_D;
		
		
				
				case Key.key_E: return CONST_TV_KEY.TV_KEY_E;
		
		
				
				case Key.key_F: return CONST_TV_KEY.TV_KEY_F;
		
		
				
				case Key.key_G: return CONST_TV_KEY.TV_KEY_G;
		
		
				
				case Key.key_H: return CONST_TV_KEY.TV_KEY_H;
		
		
				
				case Key.key_I: return CONST_TV_KEY.TV_KEY_I;
		
		
				
				case Key.key_J: return CONST_TV_KEY.TV_KEY_J;
		
		
				
				case Key.key_K: return CONST_TV_KEY.TV_KEY_K;
		
		
				
				case Key.key_L: return CONST_TV_KEY.TV_KEY_L;
		
		
				
				case Key.key_M: return CONST_TV_KEY.TV_KEY_M;
		
		
				
				case Key.key_N: return CONST_TV_KEY.TV_KEY_N;
		
		
				
				case Key.key_O: return CONST_TV_KEY.TV_KEY_O;
		
		
				
				case Key.key_P: return CONST_TV_KEY.TV_KEY_P;
		
		
				
				case Key.key_Q: return CONST_TV_KEY.TV_KEY_Q;
		
		
				
				case Key.key_R: return CONST_TV_KEY.TV_KEY_R;
		
		
				
				case Key.key_S: return CONST_TV_KEY.TV_KEY_S;
		
		
				
				case Key.key_T: return CONST_TV_KEY.TV_KEY_T;
		
		
				
				case Key.key_U: return CONST_TV_KEY.TV_KEY_U;
		
		
				
				case Key.key_V: return CONST_TV_KEY.TV_KEY_V;
		
		
				
				case Key.key_W: return CONST_TV_KEY.TV_KEY_W;
		
		
				
				case Key.key_X: return CONST_TV_KEY.TV_KEY_X;
		
		
				
				case Key.key_Y: return CONST_TV_KEY.TV_KEY_Y;
		
		
				
				case Key.key_Z: return CONST_TV_KEY.TV_KEY_Z;

		#endregion

		#region Numeric Keypad
		
		
		
				
				case Key.key_NUMPAD0: return CONST_TV_KEY.TV_KEY_NUMPAD0;
		
		
				
				case Key.key_NUMPAD1: return CONST_TV_KEY.TV_KEY_NUMPAD1;
		
		
				
				case Key.key_NUMPAD2: return CONST_TV_KEY.TV_KEY_NUMPAD2;
		
		
				
				case Key.key_NUMPAD3: return CONST_TV_KEY.TV_KEY_NUMPAD3;
		
		
				
				case Key.key_NUMPAD4: return CONST_TV_KEY.TV_KEY_NUMPAD4;
		
		
				
				case Key.key_NUMPAD5: return CONST_TV_KEY.TV_KEY_NUMPAD5;
		
		
				
				case Key.key_NUMPAD6: return CONST_TV_KEY.TV_KEY_NUMPAD6;
		
		
				
				case Key.key_NUMPAD7: return CONST_TV_KEY.TV_KEY_NUMPAD7;
		
		
				
				case Key.key_NUMPAD8: return CONST_TV_KEY.TV_KEY_NUMPAD8;
		
		
				
				case Key.key_NUMPAD9: return CONST_TV_KEY.TV_KEY_NUMPAD9;
		
		
				
				case Key.key_NUMPADCOMMA: return CONST_TV_KEY.TV_KEY_NUMPADCOMMA;
		
		
				
				case Key.key_NUMPADENTER: return CONST_TV_KEY.TV_KEY_NUMPADENTER;
		
		
				
		#endregion

		#region Function keys
				case Key.key_F1: return CONST_TV_KEY.TV_KEY_F1;
		
		
				
				case Key.key_F2: return CONST_TV_KEY.TV_KEY_F2;
		
		
				
				case Key.key_F3: return CONST_TV_KEY.TV_KEY_F3;
		
		
				
				case Key.key_F4: return CONST_TV_KEY.TV_KEY_F4;
		
		
				
				case Key.key_F5: return CONST_TV_KEY.TV_KEY_F5;
		
		
				
				case Key.key_F6: return CONST_TV_KEY.TV_KEY_F6;
		
		
				
				case Key.key_F7: return CONST_TV_KEY.TV_KEY_F7;
		
		
				
				case Key.key_F8: return CONST_TV_KEY.TV_KEY_F8;
		
		
				
				case Key.key_F9: return CONST_TV_KEY.TV_KEY_F9;
		
		
				
				case Key.key_F10: return CONST_TV_KEY.TV_KEY_F10;
		
		
				
				case Key.key_F11: return CONST_TV_KEY.TV_KEY_F11;
		
		
				
				case Key.key_F12: return CONST_TV_KEY.TV_KEY_F12;
		
		
				
				case Key.key_F13: return CONST_TV_KEY.TV_KEY_F13;
		
		
				
				case Key.key_F14: return CONST_TV_KEY.TV_KEY_F14;
		
		
				
				case Key.key_F15: return CONST_TV_KEY.TV_KEY_F15;
		#endregion

		#region Other keys
		
		
				
				case Key.key_ADD: return CONST_TV_KEY.TV_KEY_ADD;
		
		
				
				case Key.key_APOSTROPHE: return CONST_TV_KEY.TV_KEY_APOSTROPHE;
		
		
				
				case Key.key_APPLICATION: return CONST_TV_KEY.TV_KEY_APPS;
		
		
				
				case Key.key_BACKSLASH: return CONST_TV_KEY.TV_KEY_BACKSLASH;
		
		
				
				case Key.key_BACKSPACE: return CONST_TV_KEY.TV_KEY_BACKSPACE;
		
		
				
				case Key.key_CAPITAL: return CONST_TV_KEY.TV_KEY_CAPITAL;
		
		
				
				case Key.key_COMMA: return CONST_TV_KEY.TV_KEY_COMMA;
		
		
				
				case Key.key_DECIMAL: return CONST_TV_KEY.TV_KEY_DECIMAL;
		
		
				
				case Key.key_DELETE: return CONST_TV_KEY.TV_KEY_DELETE;
		
		
				
				case Key.key_DIVIDE: return CONST_TV_KEY.TV_KEY_DIVIDE;
		
		
				
				case Key.key_DOWN: return CONST_TV_KEY.TV_KEY_DOWN;
		
		
				
				case Key.key_END: return CONST_TV_KEY.TV_KEY_END;
		
		
				
				case Key.key_EQUALS: return CONST_TV_KEY.TV_KEY_EQUALS;
		
		
				
				case Key.key_ESCAPE: return CONST_TV_KEY.TV_KEY_ESCAPE;
		
		
				
				case Key.key_GRAVE: return CONST_TV_KEY.TV_KEY_GRAVE;
		
		
				
				case Key.key_HOME: return CONST_TV_KEY.TV_KEY_HOME;
		
		
				
				case Key.key_INSERT: return CONST_TV_KEY.TV_KEY_INSERT;
		
		
				
				case Key.key_LEFT: return CONST_TV_KEY.TV_KEY_LEFT;
		
		
				
				case Key.key_LEFTALT: return CONST_TV_KEY.TV_KEY_ALT_LEFT;
		
		
				
				case Key.key_LEFTBRACKET: return CONST_TV_KEY.TV_KEY_LEFTBRACKET;
		
		
				
				case Key.key_LEFTCONTROL: return CONST_TV_KEY.TV_KEY_LEFTCONTROL;
		
		
				
				case Key.key_LEFTSHIFT: return CONST_TV_KEY.TV_KEY_LEFTSHIFT;
		
		
				
				case Key.key_LEFTWIN: return CONST_TV_KEY.TV_KEY_LEFTWINDOWS;
		
		
				
				case Key.key_MINUS: return CONST_TV_KEY.TV_KEY_MINUS;
		
		
				
				case Key.key_MULTIPLY: return CONST_TV_KEY.TV_KEY_MULTIPLY;
		
		
				
				case Key.key_NUMLOCK: return CONST_TV_KEY.TV_KEY_NUMLOCK;
		
		
				
				case Key.key_PAGEDOWN: return CONST_TV_KEY.TV_KEY_PAGEDOWN;
		
		
				
				case Key.key_PAGEUP: return CONST_TV_KEY.TV_KEY_PAGEUP;
		
		
				
				case Key.key_PAUSE: return CONST_TV_KEY.TV_KEY_PAUSE;
		
		
				
				case Key.key_PERIOD: return CONST_TV_KEY.TV_KEY_PERIOD;
		
		
				
				case Key.key_RETURN: return CONST_TV_KEY.TV_KEY_RETURN;
		
		
				
				case Key.key_RIGHT: return CONST_TV_KEY.TV_KEY_RIGHT;
		
		
				
				case Key.key_RIGHTALT: return CONST_TV_KEY.TV_KEY_ALT_RIGHT;
		
		
				
				case Key.key_RIGHTBRACKET: return CONST_TV_KEY.TV_KEY_RIGHTBRACKET;
		
		
				
				case Key.key_RIGHTCONTROL: return CONST_TV_KEY.TV_KEY_RCONTROL;
		
		
				
				case Key.key_RIGHTSHIFT: return CONST_TV_KEY.TV_KEY_RIGHTSHIFT;
		
		
				
				case Key.key_RIGHTWIN: return CONST_TV_KEY.TV_KEY_RWIN;
		
		
				
				case Key.key_SCROLL: return CONST_TV_KEY.TV_KEY_SCROLL;
		
		
				
				case Key.key_SEMICOLON: return CONST_TV_KEY.TV_KEY_SEMICOLON;
		
		
				
				case Key.key_SLASH: return CONST_TV_KEY.TV_KEY_SLASH;
		
		
				
				case Key.key_SPACE: return CONST_TV_KEY.TV_KEY_SPACE;
		
		
				
				case Key.key_SUBTRACT: return CONST_TV_KEY.TV_KEY_SUBTRACT;
		
		
				
				case Key.key_SYSRQ: return CONST_TV_KEY.TV_KEY_SYSRQ;
		
		
				
				case Key.key_TAB: return CONST_TV_KEY.TV_KEY_TAB;
		
		
				
				case Key.key_UP: return CONST_TV_KEY.TV_KEY_UP;
		#endregion
			}
		}
	}
}