using System;
using System.Collections;

using TrueVision3D;
using Strive.Math3D;
using Strive.Common;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.TV3D;
using Strive.Rendering.TV3D.Textures;

namespace Strive.Rendering.TV3D.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Terrain : ITerrain {
		public Hashtable terrainHeights = new Hashtable();
		public Hashtable terrainChunks = new Hashtable();

		public void SetHeight( float x, float z, float altitude ) {
			int tx = (int)( x/Constants.terrainPieceSize );
			int tz = (int)( z/Constants.terrainPieceSize );
			Vector2D loc = new Vector2D( tx, tz );

			if ( terrainHeights.Contains( loc ) ) {
				//Strive.Logging.Log.WarningMessage( "Replacing terrain peice " + tpexists.instance_id + " with " + tp.instance_id );
				terrainHeights.Remove( loc );
			}
			terrainHeights.Add( loc, altitude );

			int cx = tx/Constants.terrainHeightsPerChunk;
			if ( tx < 0 && tx%Constants.terrainHeightsPerChunk != 0 ) cx--;
			int cz = tz/Constants.terrainHeightsPerChunk;
			if ( tz < 0 && tz%Constants.terrainHeightsPerChunk != 0 ) cz--;

			loc.Set( cx, cz );
			ITerrainChunk tc = terrainChunks[loc] as ITerrainChunk;
			if ( tc == null ) {
				tc = TerrainChunk.CreateTerrainChunk( cx*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, cz*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, Constants.terrainPieceSize, Constants.terrainHeightsPerChunk );
                terrainChunks.Add( loc, tc );
			}
			tc.SetHeight( x, z, altitude );
			if ( tx%Constants.terrainHeightsPerChunk == 0 ) {
				loc.Set( cx-1, cz );
				tc = terrainChunks[loc] as ITerrainChunk;
				if ( tc == null ) {
					tc = TerrainChunk.CreateTerrainChunk( (cx-1)*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, cz*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, Constants.terrainPieceSize, Constants.terrainHeightsPerChunk );
					terrainChunks.Add( loc, tc );
				}
				tc.SetHeight( x, z, altitude );
			}
			if ( tz%Constants.terrainHeightsPerChunk == 0 ) {
				loc.Set( cx, cz-1 );
				tc = terrainChunks[loc] as ITerrainChunk;
				if ( tc == null ) {
					tc = TerrainChunk.CreateTerrainChunk( cx*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, (cz-1)*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, Constants.terrainPieceSize, Constants.terrainHeightsPerChunk );
					terrainChunks.Add( loc, tc );
				}
				tc.SetHeight( x, z, altitude );
			}
			if ( tx%Constants.terrainHeightsPerChunk == 0 && tz%Constants.terrainHeightsPerChunk == 0 ) {
				loc.Set( cx-1, cz-1 );
				tc = terrainChunks[loc] as ITerrainChunk;
				if ( tc == null ) {
					tc = TerrainChunk.CreateTerrainChunk( (cx-1)*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, (cz-1)*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, Constants.terrainPieceSize, Constants.terrainHeightsPerChunk );
					terrainChunks.Add( loc, tc );
				}
				tc.SetHeight( x, z, altitude );
			}
		}

		public float HeightLookup( float x, float z ) {
			int tx = (int)(x/Constants.terrainPieceSize);
			int tz = (int)(z/Constants.terrainPieceSize);
			Vector2D loc = new Vector2D( tx, tz );

			return (float)terrainHeights[ loc ];
		}

		public void SetTexture( float x, float z, ITexture texture, float rotation ) {
			int tx = (int)(x/Constants.terrainPieceSize);
			int tz = (int)(z/Constants.terrainPieceSize);
			int cx = tx/Constants.terrainHeightsPerChunk;
			if ( tx < 0 && tx%Constants.terrainHeightsPerChunk != 0 ) cx--;
			int cz = tz/Constants.terrainHeightsPerChunk;
			if ( tz < 0 && tz%Constants.terrainHeightsPerChunk != 0 ) cz--;

			Vector2D loc = new Vector2D( cx, cz );
			ITerrainChunk tc = terrainChunks[loc] as ITerrainChunk;
			if ( tc == null ) {
				tc = TerrainChunk.CreateTerrainChunk( cx*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, cz*Constants.terrainHeightsPerChunk*Constants.terrainPieceSize, Constants.terrainPieceSize, Constants.terrainHeightsPerChunk );
				terrainChunks.Add( loc, tc );
			}
			tc.DrawTexture( texture, x, z, rotation );
		}

		public void Clear() {
			foreach ( TerrainChunk tc in terrainChunks.Values ) {
				tc.Delete();
			}
			terrainChunks.Clear();
			terrainHeights.Clear();
		}

		public void Render() {
			foreach ( TerrainChunk tc in terrainChunks.Values ) {
				tc.Render();
			}
		}

		public void Cull( float x, float z ) {
			int tx = (int)(x/Constants.terrainHeightsPerChunk);
			int tz = (int)(z/Constants.terrainHeightsPerChunk);
			foreach ( Vector2D loc in terrainChunks.Keys ) {
				if ( Math.Abs( loc.X - tx ) > 2 || Math.Abs( loc.Y - tz ) > 2 ) {
					TerrainChunk tc = terrainChunks[loc] as TerrainChunk;
					if ( tc != null ) {
						tc.Delete();
						terrainChunks.Remove( loc );
					}
					Vector2D height_loc = new Vector2D(0,0);
					for ( int i = 0; i<Constants.terrainHeightsPerChunk; i++ ) {
						for ( int j = 0; j<Constants.terrainHeightsPerChunk; j++ ) {
							height_loc.Set( loc.X*Constants.terrainHeightsPerChunk+i, loc.Y*Constants.terrainHeightsPerChunk+j );
							terrainHeights.Remove( height_loc );
						}
					}
				}
			}
		}

		public void SetLOD( int vertices ) {
		}


		#region "Fields"

		private string _key;
		private int _id;
		private string _label;
		private Vector3D _position = new Vector3D( 0, 0, 0 );
		private Vector3D _rotation = new Vector3D( 0, 0, 0 );
		private float _RadiusSquared;
		private TVMesh _mesh;
		private bool _show = true;
		private float _height = 0;
		private float _width = 0;
		private float _depth = 0;
		#endregion

		#region "Constructors"

		#endregion

		#region "Factory Initialisers"
		public static ITerrain CreateTerrain( string name, ITexture texture, float texture_rotation, float y, float xy, float zy, float xzy ) {
			Terrain t = new Terrain();
			t._mesh = Engine.TV3DScene.CreateMeshBuilder( name );
			//TODO: use the 1337 texturemod stuffs umg

			switch ( (int)texture_rotation ) {
				case 0:
					t._mesh.AddTriangle( texture.ID, 0, zy, 10, 10, xzy, 10, 0, y, 0, -1, 1, false, false );
					t._mesh.AddTriangle( texture.ID, 10, xy, 0, 0, y, 0, 10, xzy, 10, 1, -1, false, false );
					break;
				case 180:
					t._mesh.AddTriangle( texture.ID, 0, zy, 10, 10, xzy, 10, 0, y, 0, 1, -1, false, false );
					t._mesh.AddTriangle( texture.ID, 10, xy, 0, 0, y, 0, 10, xzy, 10, -1, 1, false, false );
					break;

				case 90:
					t._mesh.AddTriangle( texture.ID, 0, zy, 10, 0, y, 0, 10, xzy, 10, 1, 1, false, false );
					t._mesh.AddTriangle( texture.ID, 10, xy, 0, 10, xzy, 10, 0, y, 0, -1, -1, false, false );
					break;
				case 270:
					t._mesh.AddTriangle( texture.ID, 0, zy, 10, 0, y, 0, 10, xzy, 10, -1, -1, false, false );
					t._mesh.AddTriangle( texture.ID, 10, xy, 0, 10, xzy, 10, 0, y, 0, 1, 1, false, false );
					break;

				default:
					throw new Exception( "We aren't 1337 yet umg, use the buff texturemod stuffs etc" );
			}


			//t._mesh.Optimize();
			//t._mesh.ComputeBoundingVolumes();
			t._mesh.ComputeNormals();

			// TODO: bumpmapping would be cool :)
			//t._mesh.SetBumpMapping( true, texture.ID, -1, -1, 10 );
			t._key = name;
			t._id = t._mesh.GetMeshIndex();
			t._RadiusSquared = 0;
			t._height = (y + zy + xy + xzy)/2;
			return t;
		}
		#endregion

		#region "Methods"

		public void Delete() {
			Engine.TV3DScene.DestroyMesh( ref _mesh );
			_mesh = null;
		}

		// TODO:
		// umg duplicate is a dibarcal!
		public IModel Duplicate( string name, float height ) {
			return null;
		}

		public void applyTexture( ITexture texture ) {
		}

		public void GetBoundingBox( Vector3D minbox, Vector3D maxbox ) {
		
		}


		#endregion

		#region "Properties"

		/// <summary>
		/// The key of the underlying model
		/// </summary>
		public string Name {
			get {
				return _key;
			}
		}

		public int ID {
			get {
				return _id;
			}
		}

		public float RadiusSquared {
			get {
				return _RadiusSquared;
			}
		}

		public string Label {
			get { return _label; }
			set { _label = value; }
		}

		public bool Visible {
			get { return _show; }
			set { _show = value; _mesh.Enable( value ); }
		}

		public float Height {
			get { return _height; }
		}
		public float Width {
			get { return _width; }
		}
		public float Depth {
			get { return _depth; }
		}

		#endregion

		#region "Implementation of IManeuverable"
		/// <summary>
		/// Moves the model
		/// </summary>
		/// <param name="movement">The amount to move (relative)</param>
		/// <returns>Indicates whether the move was successful</returns>
		public bool Move(Vector3D movement)
		{
			// Calculate new absolute vector:
			Vector3D newPosition = _position + movement;
			_position = newPosition;
			_mesh.SetPosition( newPosition.X, newPosition.Y, newPosition.Z );
			// TODO: Implement success 
			return true;
		}


		/// <summary>
		/// Rotates the model
		/// </summary>
		/// <param name="rotation">The amount to rotate</param>
		/// <returns>Indicates whether the rotation was successful</returns>
		public bool Rotate(Vector3D rotation)
		{
			// Calculate absolute rotation
			Vector3D newRotation = _rotation + rotation;
			_rotation = newRotation;
			_mesh.SetRotation( newRotation.X, newRotation.Y, newRotation.Z );
			// TODO: Implement success
			return true;
		}

		/// <summary>
		/// The position of the model 
		/// </summary>
		public Vector3D Position
		{
			get
			{
				return _position;
			}
			set {
				_position = value;
				_mesh.SetPosition( value.X, value.Y, value.Z );
			}
		}

		/// <summary>
		/// The rotation of the model
		/// </summary>
		public Vector3D Rotation
		{
			get
			{
				return _rotation;
			}
			set {
				_rotation = value;
				_mesh.SetRotation( value.X, value.Y, value.Z );
			}
		}
		#endregion
	}

}
