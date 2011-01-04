using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Strive.WPF.Model;

namespace Strive.WPF.ViewModel
{
    public class FactoryViewModel
    {
        public FactoryModel FactoryModel { get; set; }

        public FactoryViewModel()
        {
            FactoryModel = new FactoryModel();
        }
    }
}
