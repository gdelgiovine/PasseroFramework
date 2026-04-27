using Dapper;
using FastDeepCloner;
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Wisej.Web;
using Wisej.Web.Data;

namespace Passero.Framework
{
    public partial class ViewModel<ModelClass> : INotifyPropertyChanged, INotifyPropertyChanging where ModelClass : class
    {
        public ViewModelTypes ViewModelType = ViewModelTypes.Base;
        /// <summary>
        /// The m class name
        /// </summary>
        private const string mClassName = "Passero.Framework.Base.ViewModel<T>";

        private Dictionary<string, dynamic> _ViewModels = new(StringComparer.InvariantCultureIgnoreCase);


        #region INotifyPropertyChanged and INotifyPropertyChanging  
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
        public ViewModel<ModelClass> Clone()
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

        #endregion


        #region Gestion ViewModels aggiuntivi
        public void AddViewModel<ModelClass>(string Key, ViewModel<ModelClass> viewModel) where ModelClass : class
        {
            string methodName = "AddViewModel<ModelClass>";
            try
            {
                if (viewModel == null)
                {
                    throw new ArgumentNullException(nameof(viewModel), "The view model instance cannot be null.");
                }
               
                if(Key.IsNullOrWhiteSpace())
                {
                    throw new ArgumentNullException(Key, "The ViewModel Key be empty or null.");
                }   
                if (_ViewModels.ContainsKey(Key))
                {
                    throw new ArgumentException($"A view model with the key '{Key}' already exists in the collection.");
                }
                _ViewModels[Key] = viewModel;
                
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in {mClassName}.{methodName}: {ex.Message}";
                throw new Exception(errorMessage, ex);
            }
        }
        public dynamic GetViewModel(string Key)
        {
            string methodName = "GetViewModel";
            try
            {
                if (Key.IsNullOrWhiteSpace())
                {
                    throw new ArgumentNullException(Key, "The ViewModel Key be empty or null.");
                }
                if (_ViewModels.ContainsKey(Key))
                {
                    return _ViewModels[Key];
                }
                else
                {
                    throw new ArgumentException($"A ViewModel with the Key '{Key}' did not exists in the ViewModels collection.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in {mClassName}.{methodName}: {ex.Message}";
                throw new Exception(errorMessage, ex);
            }
        }







        public void UpdateViewModel<ModelType>(string Key, ViewModel<ModelType> viewModel) where ModelType : class
        {
            string methodName = "UpdateViewModel<ModelClass>";
            try
            {
                if (viewModel == null)
                {
                    throw new ArgumentNullException(nameof(viewModel), "The ViewModel instance cannot be null.");
                }
                
                if (_ViewModels.ContainsKey(Key))
                {
                    _ViewModels[Key] = viewModel;
                }
                else
                {
                    throw new ArgumentException($"A ViewModel with the Key '{Key}' did not exists in the ViewModels collection.", nameof(viewModel));

                }

            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in {mClassName}.{methodName}: {ex.Message}";
                throw new Exception(errorMessage, ex);
            }
        }

        public void RemoveViewModel<ModelType>(string Key) where ModelType : class
        {
            string methodName = "RemoveViewModel<ModelType>";
            try
            {
                if (Key == null)
                {
                    throw new ArgumentNullException("The ViewModel Key cannot be null.");
                }
               
                if (_ViewModels.ContainsKey(Key))
                {
                    _ViewModels.Remove(Key);
                }
                else
                {
                    throw new ArgumentException($"A ViewModel with the Key '{Key}' did not exists in the ViewModels collection.");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in {mClassName}.{methodName}: {ex.Message}";
                throw new Exception(errorMessage, ex);
            }
        }



        #endregion
    }
}
