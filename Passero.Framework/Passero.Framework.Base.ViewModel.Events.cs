using System.ComponentModel;

namespace Passero.Framework
{
    public partial class ViewModel<ModelClass> : INotifyPropertyChanged, INotifyPropertyChanging where ModelClass : class
    {
        // ─── AddNew ───────────────────────────────────────────────────────────────────

        /// <summary>Raised before AddNew. Set cancel = true to abort the operation.</summary>
        public event AddNewRequestEventHandler AddNewRequest;
        public delegate void AddNewRequestEventHandler(ref bool cancel);

        /// <summary>Raised after a successful AddNew. AddNewState is now true.</summary>
        public event AddNewCompletedEventHandler AddNewCompleted;
        public delegate void AddNewCompletedEventHandler();

        // ─── Save (Insert or Update) ──────────────────────────────────────────────────

        /// <summary>Raised before InsertItem or UpdateItem. Set cancel = true to abort.</summary>
        public event SaveRequestEventHandler SaveRequest;
        public delegate void SaveRequestEventHandler(ref bool cancel);

        /// <summary>Raised after a successful save.</summary>
        public event SaveCompletedEventHandler SaveCompleted;
        public delegate void SaveCompletedEventHandler();

        // ─── Delete ───────────────────────────────────────────────────────────────────

        /// <summary>Raised before DeleteItem. Set cancel = true to abort.</summary>
        public event DeleteRequestEventHandler DeleteRequest;
        public delegate void DeleteRequestEventHandler(ref bool cancel);

        /// <summary>Raised after a successful delete.</summary>
        public event DeleteCompletedEventHandler DeleteCompleted;
        public delegate void DeleteCompletedEventHandler();

        // ─── Undo ─────────────────────────────────────────────────────────────────────

        /// <summary>Raised before UndoChanges. Set cancel = true to abort.</summary>
        public event UndoRequestEventHandler UndoRequest;
        public delegate void UndoRequestEventHandler(ref bool cancel);

        /// <summary>Raised after a successful undo.</summary>
        public event UndoCompletedEventHandler UndoCompleted;
        public delegate void UndoCompletedEventHandler();

        // ─── Helper ───────────────────────────────────────────────────────────────────

        /// <summary>
        /// Invokes a cancellable request delegate.
        /// Returns true if the operation should proceed, false if a handler cancelled it.
        /// </summary>
        private bool RaiseRequest(UndoRequestEventHandler handler) => Raise(handler);
        private bool RaiseRequest(AddNewRequestEventHandler handler) => Raise(handler);
        private bool RaiseRequest(SaveRequestEventHandler handler) => Raise(handler);
        private bool RaiseRequest(DeleteRequestEventHandler handler) => Raise(handler);

        private static bool Raise<TDelegate>(TDelegate handler) where TDelegate : class
        {
            bool cancel = false;
            switch (handler)
            {
                case AddNewRequestEventHandler h: h.Invoke(ref cancel); break;
                case SaveRequestEventHandler h: h.Invoke(ref cancel); break;
                case DeleteRequestEventHandler h: h.Invoke(ref cancel); break;
                case UndoRequestEventHandler h: h.Invoke(ref cancel); break;
            }
            return !cancel;
        }
    }
}