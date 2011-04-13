using System;
using System.Net;

namespace Strive.Common
{
   public class Constants
    {
        public const int DefaultPort = 8888;

        public const int TerrainPieceSize = 10;
        public const int TerrainHeightsPerChunk = 8;
        public const int WorldBuilderTerrainPieceSize = 75;
        public const int WorldBuilderTerrainSquareSize = 10;
        public const int TerrainPieceTextureWidth = 32;

        // TODO: these should be settable per client
        public const int TerrainZoomOrder = 2;
        public const int TerrainXOrder = 4;
        public const int TerrainZOrder = 4;
        public const int ObjectScopeRadius = TerrainPieceSize * TerrainHeightsPerChunk / 2;

        public static int[] scale = new int[TerrainZoomOrder];
        public static int[] xRadius = new int[TerrainZoomOrder];
        public static int[] zRadius = new int[TerrainZoomOrder];

        public const float FurthestLod = ObjectScopeRadius / 2;
        public const int MostDetailedLod = 5000;

        static Constants()
        {
            // set up terrain radius values
            int k;
            for (k = 0; k < TerrainZoomOrder; k++)
            {
                scale[k] = (int)Math.Pow(TerrainHeightsPerChunk, k);
                xRadius[k] = scale[k] * TerrainHeightsPerChunk * TerrainXOrder / 2 + 2 * scale[k];
                zRadius[k] = scale[k] * TerrainHeightsPerChunk * TerrainZOrder / 2 + 2 * scale[k];
            }
        }
    }
}
