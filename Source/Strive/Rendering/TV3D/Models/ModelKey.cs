using System;

namespace Strive.Rendering.TV3D.Models
{
	/// <summary>
	/// Used to unique identify a model within the model collection.
	/// </summary>
	/// <remarks>Implements IComparable so it can be the key of Collection classes</remarks>
	public class ModelKey : IComparable
	{
		#region "Fields"
		private string _key;
		#endregion

		#region "Constructors"
		/// <summary>
		/// Create a new ModelKey
		/// </summary>
		/// <param name="key">The atomic representation of the key</param>
		public ModelKey(string key)
		{
			_key = key;
		}
		#endregion

		#region "Operators"
		/// <summary>
		/// Implicit cast to string
		/// </summary>
		/// <param name="key">The ModelKey to cast</param>
		/// <returns>The string representation</returns>
		public static implicit operator string(ModelKey key) 
		{
			return key._key;
		}
		/// <summary>
		/// Implementation of IComparable
		/// </summary>
		/// <param name="modelKey">The ModelKey object to compare to</param>
		/// <returns>And integer value representing the result of the comparison</returns>
		public int CompareTo(object modelKey)
		{
			return _key.CompareTo(modelKey);
		}

		/// <summary>
		/// ToString() implementation
		/// </summary>
		/// <returns>The string representation of this object (key in text)</returns>
		public override string ToString()
		{
			return _key;
		}
		#endregion
	}
}
