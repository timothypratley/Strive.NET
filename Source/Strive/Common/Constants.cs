using System;

namespace Strive.Common
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public class Constants
	{
		public const int terrainPieceSize = 10;
		public const int terrainHeightsPerChunk = 8;
		public const int worldBuilderTerrainPieceSize = 75;
		public const int worldBuilderTerrainSquareSize = 10;
		public const int terrainPieceTextureWidth = 32;

		// TODO: these should be settable per client
		public const int terrainZoomOrder = 2;
		public const int terrainXOrder = 4;
		public const int terrainZOrder = 4;
		public const int objectScopeRadius = terrainPieceSize*terrainHeightsPerChunk/2;

		public static int[] scale = new int[terrainZoomOrder];
		public static int[] xRadius = new int[terrainZoomOrder];
		public static int[] zRadius = new int[terrainZoomOrder];

		public const float furthestLOD = objectScopeRadius/2;
		public const int mostDetailedLOD = 5000;

		static Constants() {
			// set up terrain radius values
			int k;
			for ( k=0; k<terrainZoomOrder; k++ ) {
				scale[k] = (int)Math.Pow(terrainHeightsPerChunk,k);
				xRadius[k] = scale[k] * terrainHeightsPerChunk * terrainXOrder / 2 + 2*scale[k];
				zRadius[k] = scale[k] * terrainHeightsPerChunk * terrainZOrder / 2 + 2*scale[k];
			}
		}
	}
}
