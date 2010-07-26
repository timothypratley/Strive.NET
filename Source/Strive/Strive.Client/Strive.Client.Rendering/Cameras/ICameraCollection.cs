using System;
using System.Collections;

namespace Strive.Rendering.Cameras
{
	public interface ICameraCollection : IDictionary, ICollection, IEnumerable, ICloneable {
		ICamera CreateCamera(EnumCommonCameraView view);
	}
}