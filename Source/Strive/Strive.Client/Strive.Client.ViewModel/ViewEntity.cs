using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Strive.Client.ViewModel
{
    public class ViewEntity
    {
        private string name; public string Name { get { return name; } }
        private int modelID; public int ModelID { get { return modelID; } }
        private float x; public float X { get { return x; } }
        private float y; public float Y { get { return y; } }
        private float z; public float Z { get { return z; } }
        private float dirX; public float DirX { get { return dirX; } }
        private float dirY; public float DirY { get { return dirY; } }
        private float dirZ; public float DirZ { get { return dirZ; } }

        public ViewEntity(string name, int modelID, float x, float y, float z, float dirX, float dirY, float dirZ)
        {
            this.name = name; this.modelID = modelID; this.x = x; this.y = y; this.z = z; this.dirX = dirX; this.dirY = dirY; this.dirZ = dirZ;
        }
    }
}
