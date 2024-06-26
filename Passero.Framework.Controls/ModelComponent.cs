﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework.Controls
{
    [ComplexBindingProperties("DataSource")]
    public class Model: Component
    {
        public string Name { get; set; }
        [Category("Data")]
        [Description("Indicates the source of data for the control.")]
        //[RefreshProperties(RefreshProperties.Repaint)]
        //[AttributeProvider("PasseroDemo.Models.Publisher")]
        [AttributeProvider(typeof(IListSource ))]
        public object  DataSource { get; set; }    
    }
}
