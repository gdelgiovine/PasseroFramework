using System.ComponentModel;

namespace Passero.Framework.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ComponentModel.Component" />
    [ComplexBindingProperties("DataSource")]
    public class Model : Component
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        [Category("Data")]
        [Description("Indicates the source of data for the control.")]
        //[RefreshProperties(RefreshProperties.Repaint)]
        //[AttributeProvider("PasseroDemo.Models.Publisher")]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource { get; set; }
    }
}
