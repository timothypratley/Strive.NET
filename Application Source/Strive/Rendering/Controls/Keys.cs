using System;
using DxVBLibA;
using Revolution3D8087b;

namespace Strive.Rendering.Controls
{
	/// <summary>
	/// Represents a single keyboard key
	/// </summary>
	public struct Key
	{
		int Code;

		/// <summary>
		/// Supports the following: R3DKey r = (R3DKey)Key;
		/// </summary>
		/// <param name="k">The Key to cast from</param>
		/// <returns>The R3DKey result of the cast</returns>
		public static implicit operator Revolution3D8087b.R3DKey(Key k)
		{
			return (Revolution3D8087b.R3DKey)Enum.Parse(typeof(Revolution3D8087b.R3DKey), k.Code.ToString(), true);
		}
		
		/// <summary>
		/// Supports the following: Key k = (Key)R3DKey;
		/// </summary>
		/// <param name="k">The R3DKey to cast from</param>
		/// <returns>The Key result of the cast</returns>
		public static implicit operator Key(Revolution3D8087b.R3DKey k)
		{
			Key kReturn;
			kReturn.Code = (int)k;
			return kReturn;
		}
	}

	/// <summary>
	/// Represents the set of entire keyboard keys
	/// </summary>
	public class Keys
	{
		#region Alphanumeric

		/// <summary>
		/// Keyboard
		/// </summary>
		public static Key key_0 = R3DKey.R3DKEY_0;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_1 = R3DKey.R3DKEY_1;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_2 = R3DKey.R3DKEY_2;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_3 = R3DKey.R3DKEY_3;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_4 = R3DKey.R3DKEY_4;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_5 = R3DKey.R3DKEY_5;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_6 = R3DKey.R3DKEY_6;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_7 = R3DKey.R3DKEY_7;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_8 = R3DKey.R3DKEY_8;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_9 = R3DKey.R3DKEY_9;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_A = R3DKey.R3DKEY_A;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_B = R3DKey.R3DKEY_B;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_C = R3DKey.R3DKEY_C;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_D = R3DKey.R3DKEY_D;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_E = R3DKey.R3DKEY_E;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F = R3DKey.R3DKEY_F;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_G = R3DKey.R3DKEY_G;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_H = R3DKey.R3DKEY_H;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_I = R3DKey.R3DKEY_I;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_J = R3DKey.R3DKEY_J;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_K = R3DKey.R3DKEY_K;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_L = R3DKey.R3DKEY_L;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_M = R3DKey.R3DKEY_M;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_N = R3DKey.R3DKEY_N;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_O = R3DKey.R3DKEY_O;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_P = R3DKey.R3DKEY_P;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_Q = R3DKey.R3DKEY_Q;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_R = R3DKey.R3DKEY_R;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_S = R3DKey.R3DKEY_S;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_T = R3DKey.R3DKEY_T;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_U = R3DKey.R3DKEY_U;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_V = R3DKey.R3DKEY_V;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_W = R3DKey.R3DKEY_W;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_X = R3DKey.R3DKEY_X;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_Y = R3DKey.R3DKEY_Y;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_Z = R3DKey.R3DKEY_Z;

		#endregion

		#region Numeric Keypad
		
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD0 = R3DKey.R3DKEY_NUMPAD0;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD1 = R3DKey.R3DKEY_NUMPAD1;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD2 = R3DKey.R3DKEY_NUMPAD2;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD3 = R3DKey.R3DKEY_NUMPAD3;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD4 = R3DKey.R3DKEY_NUMPAD4;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD5 = R3DKey.R3DKEY_NUMPAD5;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD6 = R3DKey.R3DKEY_NUMPAD6;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD7 = R3DKey.R3DKEY_NUMPAD7;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD8 = R3DKey.R3DKEY_NUMPAD8;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPAD9 = R3DKey.R3DKEY_NUMPAD9;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPADCOMMA = R3DKey.R3DKEY_NUMPADCOMMA;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMPADENTER = R3DKey.R3DKEY_NUMPADENTER;
		/// <summary>
		/// Keyboard
		/// </summary>		
		#endregion

		#region Function keys
		public static Key key_F1 = R3DKey.R3DKEY_F1;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F2 = R3DKey.R3DKEY_F2;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F3 = R3DKey.R3DKEY_F3;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F4 = R3DKey.R3DKEY_F4;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F5 = R3DKey.R3DKEY_F5;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F6 = R3DKey.R3DKEY_F6;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F7 = R3DKey.R3DKEY_F7;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F8 = R3DKey.R3DKEY_F8;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F9 = R3DKey.R3DKEY_F9;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F10 = R3DKey.R3DKEY_F10;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F11 = R3DKey.R3DKEY_F11;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F12 = R3DKey.R3DKEY_F12;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F13 = R3DKey.R3DKEY_F13;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F14 = R3DKey.R3DKEY_F14;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_F15 = R3DKey.R3DKEY_F15;
		#endregion

		#region Other keys
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_ADD = R3DKey.R3DKEY_ADD;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_APOSTROPHE = R3DKey.R3DKEY_APOSTROPHE;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_APPLICATION = R3DKey.R3DKEY_APPLICATION;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_BACKSLASH = R3DKey.R3DKEY_BACKSLASH;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_BACKSPACE = R3DKey.R3DKEY_BACKSPACE;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_CAPITAL = R3DKey.R3DKEY_CAPITAL;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_COMMA = R3DKey.R3DKEY_COMMA;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_DECIMAL = R3DKey.R3DKEY_DECIMAL;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_DELETE = R3DKey.R3DKEY_DELETE;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_DIVIDE = R3DKey.R3DKEY_DIVIDE;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_DOWN = R3DKey.R3DKEY_DOWN;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_END = R3DKey.R3DKEY_END;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_EQUALS = R3DKey.R3DKEY_EQUALS;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_ESCAPE = R3DKey.R3DKEY_ESCAPE;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_GRAVE = R3DKey.R3DKEY_GRAVE;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_HOME = R3DKey.R3DKEY_HOME;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_INSERT = R3DKey.R3DKEY_INSERT;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_LEFT = R3DKey.R3DKEY_LEFT;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_LEFTALT = R3DKey.R3DKEY_LEFTALT;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_LEFTBRACKET = R3DKey.R3DKEY_LEFTBRACKET;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_LEFTCONTROL = R3DKey.R3DKEY_LEFTCONTROL;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_LEFTSHIFT = R3DKey.R3DKEY_LEFTSHIFT;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_LEFTWIN = R3DKey.R3DKEY_LEFTWIN;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_MINUS = R3DKey.R3DKEY_MINUS;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_MULTIPLY = R3DKey.R3DKEY_MULTIPLY;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_NUMLOCK = R3DKey.R3DKEY_NUMLOCK;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_PAGEDOWN = R3DKey.R3DKEY_PAGEDOWN;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_PAGEUP = R3DKey.R3DKEY_PAGEUP;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_PAUSE = R3DKey.R3DKEY_PAUSE;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_PERIOD = R3DKey.R3DKEY_PERIOD;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_RETURN = R3DKey.R3DKEY_RETURN;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_RIGHT = R3DKey.R3DKEY_RIGHT;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_RIGHTALT = R3DKey.R3DKEY_RIGHTALT;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_RIGHTBRACKET = R3DKey.R3DKEY_RIGHTBRACKET;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_RIGHTCONTROL = R3DKey.R3DKEY_RIGHTCONTROL;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_RIGHTSHIFT = R3DKey.R3DKEY_RIGHTSHIFT;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_RIGHTWIN = R3DKey.R3DKEY_RIGHTWIN;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_SCROLL = R3DKey.R3DKEY_SCROLL;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_SEMICOLON = R3DKey.R3DKEY_SEMICOLON;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_SLASH = R3DKey.R3DKEY_SLASH;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_SPACE = R3DKey.R3DKEY_SPACE;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_SUBTRACT = R3DKey.R3DKEY_SUBTRACT;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_SYSRQ = R3DKey.R3DKEY_SYSRQ;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_TAB = R3DKey.R3DKEY_TAB;
		/// <summary>
		/// Keyboard
		/// </summary>		
		public static Key key_UP = R3DKey.R3DKEY_UP;
		#endregion
	}
}


// Partial DirectX keys:
//	public enum Keys
//	{
//		K_0 = CONST_DIKEYFLAGS.DIK_0,
//		K_1 = CONST_DIKEYFLAGS.DIK_1,
//		K_2 = CONST_DIKEYFLAGS.DIK_2,
//		K_3 = CONST_DIKEYFLAGS.DIK_3,
//		K_4 = CONST_DIKEYFLAGS.DIK_4,
//		K_5 = CONST_DIKEYFLAGS.DIK_5,
//		K_6 = CONST_DIKEYFLAGS.DIK_6,
//		K_7 = CONST_DIKEYFLAGS.DIK_7,
//		K_8 = CONST_DIKEYFLAGS.DIK_8,
//		K_9 = CONST_DIKEYFLAGS.DIK_9,
//		K_A = CONST_DIKEYFLAGS.DIK_A,
//		K_B = CONST_DIKEYFLAGS.DIK_B,
//		K_C = CONST_DIKEYFLAGS.DIK_C,
//		K_D = CONST_DIKEYFLAGS.DIK_D,
//		K_E = CONST_DIKEYFLAGS.DIK_E,
//		K_F = CONST_DIKEYFLAGS.DIK_F,
//		K_G = CONST_DIKEYFLAGS.DIK_G,
//		K_H = CONST_DIKEYFLAGS.DIK_H,
//		K_I = CONST_DIKEYFLAGS.DIK_I,
//		K_J = CONST_DIKEYFLAGS.DIK_J,
//		K_K = CONST_DIKEYFLAGS.DIK_K,
//		K_L = CONST_DIKEYFLAGS.DIK_L,
//		K_M = CONST_DIKEYFLAGS.DIK_M,
//		K_N = CONST_DIKEYFLAGS.DIK_N,
//		K_O = CONST_DIKEYFLAGS.DIK_O,
//		K_P = CONST_DIKEYFLAGS.DIK_P,
//		K_Q = CONST_DIKEYFLAGS.DIK_Q,
//		K_R = CONST_DIKEYFLAGS.DIK_R,
//		K_S = CONST_DIKEYFLAGS.DIK_S,
//		K_T = CONST_DIKEYFLAGS.DIK_T,
//		K_U = CONST_DIKEYFLAGS.DIK_U,
//		K_V = CONST_DIKEYFLAGS.DIK_V,
//		K_W = CONST_DIKEYFLAGS.DIK_W,
//		K_X = CONST_DIKEYFLAGS.DIK_X,
//		K_Y = CONST_DIKEYFLAGS.DIK_Y,
//		K_Z = CONST_DIKEYFLAGS.DIK_Z,
//		K_ABTN_C1 = CONST_DIKEYFLAGS.DIK_ABNT_C1,
//		K_ABTN_C2 = CONST_DIKEYFLAGS.DIK_ABNT_C2,
//		K_ADD = CONST_DIKEYFLAGS.DIK_ADD,
//		K_APOSTROPHE = CONST_DIKEYFLAGS.DIK_APOSTROPHE,
//		K_APPS = CONST_DIKEYFLAGS.DIK_APPS,
//		K_AT = CONST_DIKEYFLAGS.DIK_AT,
//		K_AX = CONST_DIKEYFLAGS.DIK_AX,
//		K_BACK = CONST_DIKEYFLAGS.DIK_BACK,
//		K_BACKSLASH = CONST_DIKEYFLAGS.DIK_BACKSLASH,
//		K_BACKSPACE = CONST_DIKEYFLAGS.DIK_BACKSPACE,
//		K_CALCULATOR = CONST_DIKEYFLAGS.DIK_CALCULATOR,
//		K_CAPITAL = CONST_DIKEYFLAGS.DIK_CAPITAL,
//		K_CAPSLOCK = CONST_DIKEYFLAGS.DIK_CAPSLOCK,
//		K_CIRCUMFLEX = CONST_DIKEYFLAGS.DIK_CIRCUMFLEX,
//		K_COLON = CONST_DIKEYFLAGS.DIK_COLON,
//		K_COMMA = CONST_DIKEYFLAGS.DIK_COMMA,
//		K_CONVERT = CONST_DIKEYFLAGS.DIK_CONVERT,
//		K_DECIMAL = CONST_DIKEYFLAGS.DIK_DECIMAL,
//		K_DELETE = CONST_DIKEYFLAGS.DIK_DELETE,
//		K_DIVIDE = CONST_DIKEYFLAGS.DIK_DIVIDE,
//		K_DOWN = CONST_DIKEYFLAGS.DIK_DOWN
//		
//
//
//
//	}