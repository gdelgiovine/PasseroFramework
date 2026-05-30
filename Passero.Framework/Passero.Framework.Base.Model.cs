using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Passero.Framework
{
    /// <summary>
    /// Base class for models that supports property change notifications and cloning
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanging" />
    public abstract class ModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Generato quando il valore di una proprietà cambia.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Generato quando il valore di una proprietà sta per cambiare.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">The propertyName.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies that a property is about to change.
        /// </summary>
        /// <param name="propertyName">The propertyName.</param>
        protected virtual void NotifyPropertyChanging([CallerMemberName] string propertyName = "")
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of the current instance</returns>
        public ModelBase Clone()
        {
            return Utilities.Clone(this);
        }

        /// <summary>
        /// Sets the property value and raises the appropriate change notifications.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="field">Reference to the backing field</param>
        /// <param name="value">The new value</param>
        /// <param name="propertyName">Name of the property (automatically set by the compiler)</param>
        /// <returns>True if the value was changed, false if the value was the same</returns>
        protected bool SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return false;

            NotifyPropertyChanging(propertyName);
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }
    }

    /// <summary>
    /// Stub model class for testing and demonstration purposes
    /// </summary>
    /// <seealso cref="Passero.Framework.ModelBase" />
    public class ModelStub : ModelBase
    {
        private string _string;
        private int _integer;
        private DateTime _dateTime;

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>
        /// The string.
        /// </value>
        public string String
        {
            get => _string;
            set => SetProperty(ref _string, value);
        }

        /// <summary>
        /// Gets or sets the integer.
        /// </summary>
        /// <value>
        /// The integer.
        /// </value>
        public int Integer
        {
            get => _integer;
            set => SetProperty(ref _integer, value);
        }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public DateTime DateTime
        {
            get => _dateTime;
            set => SetProperty(ref _dateTime, value);
        }
    }
}