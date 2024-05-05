using System;
using System.ComponentModel;

//namespace Passero.Framework.Base
namespace Passero.Framework
{
    public class ModelBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        public ModelBase Clone()
        {
            return Utilities.Clone(this);
        }

    }

    public class ModelStub: ModelBase
    {
        public string String { get; set;}
        public int Integer { get; set;}
        public DateTime DateTime { get; set;}

    }
}