using System;
using System.Collections;

using Strive.Rendering;

namespace Strive.Rendering.Models
{
	/// <summary>
	/// A strongly typed collection of models
	/// </summary>
	public class ModelCollection : Hashtable, IEnumerable
	{
		#region "Constructors"
		/// <summary>
		/// Creates a new ModelCollection
		/// </summary>
		public ModelCollection() : base()
		{

		}
		#endregion

		#region "Operators"

		#region "Collection Support"
		/// <summary>
		/// Adds a model to the collection
		/// </summary>
		/// <param name="model">The model to add</param>
		public void Add(Model model)
		{
			base.Add(model.Key, model);
		}

		/// <summary>
		/// Model indexer
		/// </summary>
		public Model this[string key]
		{
			get
			{
				Model indexedModel = (Model)base[key];
				if(indexedModel != null)
				{
					try
					{
						switch(indexedModel.ModelFormat)
						{
							case ModelFormat.MDL:
							{
								// TODO: Investigate if this will lead to double (potentially slow) MDL_SetPointer calls
								Interop._instance.MdlSystem.MDL_SetPointer(key);
								break;
							}
							case ModelFormat._3DS:
							{
								Interop._instance.Meshbuilder.Mesh_SetPointer(key);
								break;
							}

						}
					}
					catch(Exception e)
					{
						throw new ModelException("Could not set pointer to '" + key + "'.", e);
					}
					return indexedModel;
				}
				else
				{
					throw new ModelException("Could not locate model '" + key + "'.", new NullReferenceException());
				}
			}
			set
			{
				base[key] = value;
			}
		}

		/// <summary>
		/// Returns the model
		/// </summary>
		public Model this[int key]
		{
			get
			{
				return this[key.ToString()];
			}
		}

		#endregion

		#region "Implementation of IEnumerable"

		/// <summary>
		/// Gets an enumerator over the ModelCollection
		/// </summary>
		/// <returns>An enumerator over the ModelCollection</returns>
		public new ModelCollectionEnumerator GetEnumerator()
		{
			return new ModelCollectionEnumerator(this);
		}

		/// <summary>
		/// A custom enumerator over the model collection class
		/// </summary>
		public class ModelCollectionEnumerator : IEnumerator
		{
			private IEnumerator _data;

			/// <summary>
			/// Creates a new ModelCollectionEnumerator
			/// </summary>
			/// <param name="collection">The ModelCollection to enumerate</param>
			public ModelCollectionEnumerator(ModelCollection collection)
			{
				_data = ((Hashtable)collection).GetEnumerator();
			}

			/// <summary>
			/// Reset the enumerator
			/// </summary>
			public void Reset()
			{
				_data.Reset();
			}

			/// <summary>
			/// Move the enumerator to the next value in the collection
			/// </summary>
			/// <returns>Indicates whether the move was successful (more value)</returns>
			public bool MoveNext()
			{
				bool bReturn = _data.MoveNext();
				if(bReturn)
				{
					// TODO: Investigate if this will lead to double (potentially slow) MDL_SetPointer calls
					Interop._instance.MdlSystem.MDL_SetPointer(this.Current.Key);
				}
				return bReturn;
			}
			/// <summary>
			/// Returns the current value
			/// </summary>
			public Model Current
			{
				get
				{
					return (Model)_data.Current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return (object)this.Current;
				}
			}
		}

		#endregion

		#endregion
	}
}
