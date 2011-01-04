using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Strive.WPF.Model;

namespace Strive.WPF.ViewModel
{
    public class UnitViewModel
    {
        public UnitModel UnitModel { get; set; }

        public UnitViewModel()
        {
            UnitModel = new UnitModel();
        }
    }
}
