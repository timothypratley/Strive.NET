using System;

namespace Strive.UI.Resources
{
	/// <summary>
	/// Summary description for Exceptions.
	/// </summary>
	public class ResourceNotLoadedException : StriveUIExceptionBase
	{
		public ResourceNotLoadedException(int resourceID, ResourceType resourceType) : base("Could not locate resource id '" + resourceID + "', of type '" + resourceType.ToString() + "'.")
		{
		}
	}
}
