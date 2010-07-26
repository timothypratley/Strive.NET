using System;

namespace Strive.Rendering
{
	/// <summary>
	/// A Factory of Factories.
	/// Instanciate an instance of a particular implementation
	/// of IFactory, which can everything you need to access in Rendering.
	/// </summary>
	public class Activator
	{
		public static IEngine GetEngine() {
			return (IEngine)(System.Activator.CreateInstance( "Strive.Rendering.TV3D", "Strive.Rendering.TV3D.Engine")).Unwrap();
		}

		public static IEngine GetEngine( string AssemblyName, string ClassName ) {
			return (IEngine)(System.Activator.CreateInstance( AssemblyName, ClassName)).Unwrap();
		}

	}
}
