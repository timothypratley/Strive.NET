using System;
using System.Collections;

using Strive.Rendering.Cameras;

namespace Strive.Rendering.R3D.Cameras
{
	

	public class CameraCollection : ICameraCollection
	{
		protected Hashtable innerHash;
		
		#region "Constructors"
		public  CameraCollection()
		{
			innerHash = new Hashtable();
		}
		public CameraCollection(CameraCollection original)
		{
			innerHash = new Hashtable (original.innerHash);
		}
		public CameraCollection(IDictionary dictionary)
		{
			innerHash = new Hashtable (dictionary);
		}

		public CameraCollection(int capacity)
		{
			innerHash = new Hashtable(capacity);
		}

		public CameraCollection(IDictionary dictionary, float loadFactor)
		{
			innerHash = new Hashtable(dictionary, loadFactor);
		}

		public CameraCollection(IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (codeProvider, comparer);
		}

		public CameraCollection(int capacity, int loadFactor)
		{
			innerHash = new Hashtable(capacity, loadFactor);
		}

		public CameraCollection(IDictionary dictionary, IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (dictionary, codeProvider, comparer);
		}
		
		public CameraCollection(int capacity, IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (capacity, codeProvider, comparer);
		}

		public CameraCollection(IDictionary dictionary, float loadFactor, IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (dictionary, loadFactor, codeProvider, comparer);
		}

		public CameraCollection(int capacity, float loadFactor, IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (capacity, loadFactor, codeProvider, comparer);
		}

		
#endregion

		#region "Camera Factory Loader"
		/// <summary>
		/// Loads a new camera and adds it the collection
		/// </summary>
		/// <param name="cameraKey">The specified Key for the camera</param>
		/// <param name="cameras">The collection to add the camera too</param>
		/// <returns>The newly created camera</returns>
		public ICamera CreateCamera(string cameraKey) {
			Camera c = Camera.CreateCamera( cameraKey );
			this.Add( cameraKey, c );
			return c;
		}

		public ICamera CreateCamera(EnumCommonCameraView view ) {
			return CreateCamera(view.ToString());
		}

		#endregion

		#region Implementation of IDictionary
        public CameraCollectionEnumerator GetEnumerator()
        {
	        return new CameraCollectionEnumerator(this);
        }
        
		System.Collections.IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new CameraCollectionEnumerator(this);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Remove(string key)
		{
			innerHash.Remove (key);
		}
		void IDictionary.Remove(object key)
		{
			Remove ((string)key);
		}

		public bool Contains(string key)
		{
			return innerHash.Contains(key);
		}
		bool IDictionary.Contains(object key)
		{
			return Contains((string)key);
		}

		public void Clear()
		{
			innerHash.Clear();		
		}

		public void Add(string key, Camera value)
		{
			innerHash.Add (key, value);
		}
		void IDictionary.Add(object key, object value)
		{
			Add ((string)key, (Camera)value);
		}

		public bool IsReadOnly
		{
			get
			{
				return innerHash.IsReadOnly;
			}
		}

		public Camera this[string key]
		{
			get
			{
				return (Camera) innerHash[key];
			}
			set
			{
				innerHash[key] = value;
			}
		}
		object IDictionary.this[object key]
		{
			get
			{
				return this[(string)key];
			}
			set
			{
				this[(string)key] = (Camera)value;
			}
		}
        
		public System.Collections.ICollection Values
		{
			get
			{
				return innerHash.Values;
			}
		}

		public System.Collections.ICollection Keys
		{
			get
			{
				return innerHash.Keys;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return innerHash.IsFixedSize;
			}
		}
		#endregion

		#region Implementation of ICollection
		public void CopyTo(System.Array array, int index)
		{
			innerHash.CopyTo (array, index);
		}

		public bool IsSynchronized
		{
			get
			{
				return innerHash.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return innerHash.Count;
			}
		}

		public object SyncRoot
		{
			get
			{
				return innerHash.SyncRoot;
			}
		}
		#endregion

		#region Implementation of ICloneable
		public CameraCollection Clone()
		{
			CameraCollection clone = new CameraCollection();
			clone.innerHash = (Hashtable) innerHash.Clone();
			
			return clone;
		}
		object ICloneable.Clone()
		{
			return Clone();
		}
		#endregion
		
		#region "HashTable Methods"
		public bool ContainsKey (string key)
		{
			return innerHash.ContainsKey(key);
		}
		public bool ContainsValue (Camera value)
		{
			return innerHash.ContainsValue(value);
		}
		public static CameraCollection Synchronized(CameraCollection nonSync)
		{
			CameraCollection sync = new CameraCollection();
			sync.innerHash = Hashtable.Synchronized(nonSync.innerHash);

			return sync;
		}
		#endregion

		internal Hashtable InnerHash
		{
			get
			{
				return innerHash;
			}
		}
	}
	
	public class CameraCollectionEnumerator : IDictionaryEnumerator
	{
		private IDictionaryEnumerator innerEnumerator;
			
		internal CameraCollectionEnumerator (CameraCollection enumerable)
		{
			innerEnumerator = enumerable.InnerHash.GetEnumerator();
		}

		#region Implementation of IDictionaryEnumerator
		public string Key
		{
			get
			{
				return (string)innerEnumerator.Key;
			}
		}
		object IDictionaryEnumerator.Key
		 {
			 get
			 {
				 return Key;
			 }
		 }


		public Camera Value
		{
			get
			{
				return (Camera)innerEnumerator.Value;
			}
		}
		object IDictionaryEnumerator.Value
		{
			get
			{
				return Value;
			}
		}

		public System.Collections.DictionaryEntry Entry
		{
			get
			{
				return innerEnumerator.Entry;
			}
		}

		#endregion

		#region Implementation of IEnumerator
		public void Reset()
		{
			innerEnumerator.Reset();
		}

		public bool MoveNext()
		{
			return innerEnumerator.MoveNext();
		}

		public object Current
		{
			get
			{
				return innerEnumerator.Current;
			}
		}
		#endregion
	}

}