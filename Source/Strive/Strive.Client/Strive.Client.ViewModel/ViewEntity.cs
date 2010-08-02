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
        public string Name;
        public int ModelID;
        public double X;
        public double Y;
        public double Z;
        public double DirX;
        public double DirY;
        public double DirZ;

        public ViewEntity(string name, int modelID, double x, double y, double z, double dirX, double dirY, double dirZ)
        {
            Name = name; ModelID = modelID; X = x; Y = y; Z = z; DirX = dirX; DirY = dirY; DirZ = dirZ;
        }
    }
}
