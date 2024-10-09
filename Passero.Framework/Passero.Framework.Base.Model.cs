using System;
using System.ComponentModel;

//namespace Passero.Framework.Base
namespace Passero.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class ModelBase : INotifyPropertyChanged
    {

        /// <summary>
        /// Generato quando il valore di una proprietà cambia.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="info">The information.</param>
        public void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public ModelBase Clone()
        {
            return Utilities.Clone(this);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Passero.Framework.ModelBase" />
    public class ModelStub : ModelBase
    {
        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>
        /// The string.
        /// </value>
        public string String { get; set; }
        /// <summary>
        /// Gets or sets the integer.
        /// </summary>
        /// <value>
        /// The integer.
        /// </value>
        public int Integer { get; set; }
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public DateTime DateTime { get; set; }

    }
}