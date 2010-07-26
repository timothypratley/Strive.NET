using System;

namespace Strive.Resources
{
	/// <summary>
	/// Summary description for Exceptions.
	/// </summary>
	public class ResourceNotLoadedException : Exception
	{
		public ResourceNotLoadedException(int resourceID, ResourceType resourceType) : base("Could not locate resource id '" + resourceID + "', of type '" + resourceType.ToString() + "'.")
		{
		}
	}
}
