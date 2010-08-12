using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

// TODO: this is actually a model and should implement notification via update controls

namespace Strive.Client.ViewModel
{
    public class ViewEntity
    {
        public string Name;
        public string ModelID;
        public bool IsSelected;
        public double X;
        public double Y;
        public double Z;
        public double DirX;
        public double DirY;
        public double DirZ;

        public ViewEntity(string name, string modelID, double x, double y, double z, double dirX, double dirY, double dirZ)
        {
            Name = name; ModelID = modelID; X = x; Y = y; Z = z; DirX = dirX; DirY = dirY; DirZ = dirZ;
            IsSelected = false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
