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

	public class TerrainChunk : ITerrainChunk {
		TVLandscape _mesh;
		private string _key;
		private int _id;
		private string _label;
		private Vector3D _position = new Vector3D( 0, 0, 0 );
		private Vector3D _rotation = new Vector3D( 0, 0, 0 );
		private float _RadiusSquared;
		private bool _show = true;
		private float _height = 0;
		private float _width = 0;
		private float _depth = 0;
		private float _gap_size = 1;
		private float _heights;
		private ITexture _texture = null;

		public static ITerrainChunk CreateTerrainChunk( float x, float z, float gap_size, int heights ) {
            TerrainChunk t = new TerrainChunk();
			t._gap_size = gap_size;
			t._heights = heights;
			t.Position.X = x; t.Position.Z = z;
			t._mesh = new TVLandscape();
			t._width = gap_size*heights;
			t._height = gap_size*heights;
			t._mesh.CreateEmptyTerrain( (CONST_TV_LANDSCAPE_PRECISION)(256/heights),
				1, 1,
				x, z );
			t._mesh.SetTerrainScale( gap_size*heights/256F, 1, gap_size*heights/256F, true );
			return t;
		}

		~TerrainChunk() {
			Delete();
		}

		public void SetHeight( float x, float z, float altitude ) {
			// TODO: remove +1 ... bug in TV3D landscape scaling, z+1

			/** this wont work, relative is only height relative
			x = x % (_heights*_gap_size);
			z = z % (_heights*_gap_size);
			if ( x < 0 ) x = _height*_gap_size + x;
			if ( x < 0 ) z = _height*_gap_size + z;
			*/

			_mesh.ChangePointAltitude( x+0.01f, z+0.01f, altitude, true, false, false );
		}

		public float GetHeight( float x, float z ) {
			return _mesh.GetHeight( x, z );
		}

		public void SetTexture( ITexture t ) {
			_mesh.SetTexture( t.ID, -1 );
			_texture = t;
		}

		public void SetDetailTexture( ITexture t ) {
			_mesh.SetDetailTexture( t.ID );
			_mesh.SetDetailTextureMode( CONST_TV_DETAILMAP_MODE.TV_DETAILMAP_ADDSIGNED );
		}

		public void DrawTexture( ITexture t, float x, float z, float rotation ) {
			float tx = (x - Position.X)/_width*Constants.terrainPieceTextureWidth*Constants.terrainHeightsPerChunk;
			float tz = (z - Position.Z)/_height*Constants.terrainPieceTextureWidth*Constants.terrainHeightsPerChunk;
			_texture.Draw( t, tx, tz, rotation, 1);
		}

		public void Clear( float x, float z ) {
			float tx = (x - Position.X)/_width*Constants.terrainPieceTextureWidth*Constants.terrainHeightsPerChunk;
			float tz = (z - Position.Z)/_height*Constants.terrainPieceTextureWidth*Constants.terrainHeightsPerChunk;
			_texture.Clear( tx, tz, Constants.terrainPieceTextureWidth, Constants.terrainPieceTextureWidth );
		}

		public void SetClouds( ITexture texture ) {
			// TODO: make clouds always above your head
			_mesh.InitClouds( texture.ID, CONST_TV_LAND_CLOUDMODE.TV_CLOUD_MOVE, 500f, 1, 1, 1f, 1f, 1f);
			_mesh.SetCloudVelocity(1, 0.01f, 0.01f);
		}

		public void Update() {
			_mesh.FlushHeightChanges( true, true );
		}

		public void Render() {
			if ( _mesh != null ) {
				_mesh.Render( false, false );
			}
		}

		public void Delete() {
			if ( _mesh != null ) {
				_mesh.DeleteAll();
				_mesh = null;
			}
		}


		public IModel Duplicate( string name, float height ) {
			return null;
		}

		public void applyTexture( ITexture texture ) {
		}

		public void GetBoundingBox( Vector3D minbox, Vector3D maxbox ) {
		
		}

		public void SetLOD( int vertices ) {

		}

		#region "Implementation of IManeuverable"
		/// <summary>
		/// Moves the model
		/// </summary>
		/// <param name="movement">The amount to move (relative)</param>
		/// <returns>Indicates whether the move was successful</returns>
		public bool Move(Vector3D movement) {
			// Calculate new absolute vector:
			Vector3D newPosition = _position + movement;
			_position = newPosition;
			_mesh.SetLandscapePosition( newPosition.X, newPosition.Y, newPosition.Z );
			// TODO: Implement success 
			return true;
		}


		/// <summary>
		/// Rotates the model
		/// </summary>
		/// <param name="rotation">The amount to rotate</param>
		/// <returns>Indicates whether the rotation was successful</returns>
		public bool Rotate(Vector3D rotation) {
			// Calculate absolute rotation
			Vector3D newRotation = _rotation + rotation;
			_rotation = newRotation;
			_mesh.SetLandscapeRotation( newRotation.X, newRotation.Y, newRotation.Z );
			// TODO: Implement success
			return true;
		}

		/// <summary>
		/// The position of the model 
		/// </summary>
		public Vector3D Position {
			get {
				return _position;
			}
			set {
				_position = value;
				_mesh.SetLandscapePosition( value.X, value.Y, value.Z );
			}
		}

		/// <summary>
		/// The rotation of the model
		/// </summary>
		public Vector3D Rotation {
			get {
				return _rotation;
			}
			set {
				_rotation = value;
				_mesh.SetLandscapeRotation( value.X, value.Y, value.Z );
			}
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
			set { _show = value; /* _mesh.Enable( value ); */ }
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

	}

}
