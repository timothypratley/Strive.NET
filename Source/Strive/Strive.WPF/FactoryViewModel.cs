using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace Strive.WPF
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
