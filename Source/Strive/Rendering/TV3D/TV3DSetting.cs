using System;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using TrueVision3D;

namespace Strive.Rendering.TV3D
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	[Serializable]
	public struct TV3DSetting
	{
		public TV3DSetting( Option option, Type type, object value ) 
		{
			this.option = option;
			this.type = type;
			this.value = value;
		}
		public Option option;
		public Type type;
		public object value;

		static TV3DSetting [] _settings = new TV3DSetting[] {
			new TV3DSetting(Option.EnableAntialising, typeof(bool), false),
			new TV3DSetting(Option.DisplayFPS, typeof(bool), false),
			new TV3DSetting(Option.EnableHardwareTL, typeof(bool), false),
			new TV3DSetting(Option.EnableShaders, typeof(bool), false),
			new TV3DSetting(Option.SetVSync, typeof(bool), false),
			new TV3DSetting(Option.SetAutoTransColor, typeof(TrueVision3D.CONST_TV_COLORKEY), TrueVision3D.CONST_TV_COLORKEY.TV_COLORKEY_USE_ALPHA_CHANNEL),
			new TV3DSetting(Option.AutoTransColorMultiBlending, typeof(bool), true),
			new TV3DSetting(Option.DepthBufffer, typeof(TrueVision3D.CONST_TV_DEPTHBUFFER), TrueVision3D.CONST_TV_DEPTHBUFFER.TV_WBUFFER),
			new TV3DSetting(Option.Dithering, typeof(bool), false),
			new TV3DSetting(Option.RenderMode, typeof(CONST_TV_RENDERMODE), CONST_TV_RENDERMODE.TV_SOLID),
			new TV3DSetting(Option.SetShadeMode, typeof(CONST_TV_SHADEMODE), CONST_TV_SHADEMODE.TV_SHADEMODE_FLAT),
			new TV3DSetting(Option.SetSpecularLighting, typeof(bool), false),
			new TV3DSetting(Option.SetTextureFilter, typeof(CONST_TV_TEXTUREFILTER), CONST_TV_TEXTUREFILTER.TV_FILTER_NONE)
		};

		public static void ApplySettings()
		{
			TV3DSetting[] settings = GetSettings();
			Engine.TV3DEngine.EnableAntialising((bool)settings[0].value);
			Engine.TV3DEngine.DisplayFPS = (bool)settings[1].value;
			Engine.TV3DEngine.EnableHardwareTL((bool)settings[2].value);
			Engine.TV3DEngine.EnableShaders((bool)settings[3].value);
			Engine.TV3DEngine.SetVSync((bool)settings[4].value);
			Engine.TV3DScene.SetAutoTransColor((TrueVision3D.CONST_TV_COLORKEY)settings[5].value, (bool)settings[6].value);
			Engine.TV3DScene.SetDepthBuffer((CONST_TV_DEPTHBUFFER)settings[7].value);
			Engine.TV3DScene.SetDithering((bool)settings[8].value);
			Engine.TV3DScene.SetRenderMode((CONST_TV_RENDERMODE)settings[9].value);
			Engine.TV3DScene.SetShadeMode((CONST_TV_SHADEMODE)settings[10].value);
			Engine.TV3DScene.SetSpecularLightning((bool)settings[11].value);
			Engine.TV3DScene.SetTextureFilter((CONST_TV_TEXTUREFILTER)settings[12].value);
		}

		public enum Option
		{
			EnableAntialising,
			DisplayFPS,
			EnableHardwareTL,
			EnableShaders,
			SetVSync,
			SetAutoTransColor,
			AutoTransColorMultiBlending,
			DepthBufffer,
			Dithering,
			RenderMode,
			SetShadeMode,
			SetSpecularLighting,
			SetTextureFilter
		}

		public static TV3DSetting[] GetSettings()
		{
			if(System.Configuration.ConfigurationSettings.AppSettings["Strive.Rendering.TV3D.TV3DSetting.PersistedFileName"] != null &&
				System.Configuration.ConfigurationSettings.AppSettings["Strive.Rendering.TV3D.TV3DSetting.PersistedFileName"] != "" &&
				System.IO.File.Exists(System.Configuration.ConfigurationSettings.AppSettings["Strive.Rendering.TV3D.TV3DSetting.PersistedFileName"].ToString()))
			{
				System.IO.FileStream fileContents = System.IO.File.OpenRead(System.Configuration.ConfigurationSettings.AppSettings["Strive.Rendering.TV3D.TV3DSetting.PersistedFileName"].ToString());
				try
				{
					BinaryFormatter bf = new BinaryFormatter();
					TV3DSetting[] localCopy = (TV3DSetting[])bf.Deserialize(fileContents);
					for(int i = 0; i < localCopy.Length; i++)
					{
						_settings[i] = localCopy[i];
					}
					return _settings;
				}
				catch(Exception e)
				{
					throw e;
				}
				finally
				{
					fileContents.Close();
				}
			}
			else
			{
				return _settings;
			}
		}

		public static void SaveSettings(TV3DSetting[] settings)
		{
			if(System.Configuration.ConfigurationSettings.AppSettings["Strive.Rendering.TV3D.TV3DSetting.PersistedFileName"] != null &&
				System.Configuration.ConfigurationSettings.AppSettings["Strive.Rendering.TV3D.TV3DSetting.PersistedFileName"] != "" )
			{
				System.IO.FileStream fileContents = System.IO.File.OpenWrite(System.Configuration.ConfigurationSettings.AppSettings["Strive.Rendering.TV3D.TV3DSetting.PersistedFileName"].ToString());
				try
				{
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(fileContents, settings);

				}
				catch(Exception e)
				{
					throw e;
				}
				finally
				{
					fileContents.Close();					
					
				}
				ApplySettings();
			}
		}
	} 
}
