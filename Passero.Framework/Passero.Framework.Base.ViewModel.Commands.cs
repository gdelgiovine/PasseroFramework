using FastDeepCloner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Wisej.Web;

namespace Passero.Framework
{
    public partial class ViewModel<ModelClass> : INotifyPropertyChanged, INotifyPropertyChanging where ModelClass : class
    {
        private bool HasRepository()
        {
            return Repository != null;
        }

        private ModelClass ResolveCommandItem(ModelClass item, bool readControls = false)
        {
            if (item == null)
            {
                item = ModelItem;
            }

            if (mDataBindingMode == DataBindingMode.BindingSource && BindingSource != null)
            {
                BindingSource.EndEdit();
                if (BindingSource.Current != null)
                {
                    item = (ModelClass)BindingSource.Current;
                    ModelItem = item;
                }
            }
            else if (mDataBindingMode == DataBindingMode.Passero && readControls && AutoReadControls)
            {
                ReadControls();
            }

            return item;
        }

        private void ApplyBindingEndState(bool writeControls = false, bool resetCurrentItem = false)
        {
            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (writeControls && AutoWriteControls)
                    {
                        WriteControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    if (resetCurrentItem && BindingSource != null)
                    {
                        BindingSource.ResetCurrentItem();
                    }
                    break;
            }
        }

        /// <summary>
        /// Undoes the changes.
        /// </summary>
        /// <param name="AllItems">if set to <c>true</c> [all items].</param>
        /// <returns></returns>
        public ExecutionResult UndoChanges(bool AllItems = false)
        {
            var ERContenxt = $"{mClassName}.UndoChanges()";
            var ER = new ExecutionResult(ERContenxt);


            if (!RaiseRequest(UndoRequest))
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ResultMessage = "UndoChanges cancelled by UndoRequest handler.";
                return ER;
            }

            try
            {
                var currentItems = ModelItems ?? new List<ModelClass>();

                if (AddNewState == false)
                {
                    ModelItem = Utilities.Clone<ModelClass>(ModelItemShadow);
                    if (currentItems.Count >= CurrentModelItemIndex)
                    {
                        currentItems[CurrentModelItemIndex] = Utilities.Clone<ModelClass>(ModelItemShadow);
                    }
                }
                else
                {
                    if (currentItems.Count > 0)
                    {
                        currentItems.RemoveAt(currentItems.Count - 1);
                    }
                }

                if (ModelItemsShadow != null)
                {
                    var restoredItems = new List<ModelClass>(ModelItemsShadow.Count);
                    foreach (var shadowItem in ModelItemsShadow)
                    {
                        restoredItems.Add(shadowItem == null ? null : Utilities.Clone<ModelClass>(shadowItem));
                    }

                    ModelItems = restoredItems;
                }
                else
                {
                    ModelItems = new List<ModelClass>();
                }

                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero:
                        if (AutoWriteControls)
                        {
                            WriteControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        if (AddNewState)
                        {
                            if (BindingSource != null)
                            {
                                BindingSource.Position = AddNewCurrentModelItemIndex;
                            }
                            MoveAtItem(AddNewCurrentModelItemIndex);
                            if (mDataNavigator != null)
                            {
                                mDataNavigator.UpdateRecordLabel();
                            }
                        }
                        else if (BindingSource != null)
                        {
                            BindingSource.CancelEdit();
                        }
                        break;

                    default:
                        break;
                }
                if (mAddNewState == true)
                {
                    SetAddNewState(false, true);
                }

                if (ER.Success)
                    UndoCompleted?.Invoke();
            }
            catch (Exception ex)
            {

                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                ER.ResultCode = ExecutionResultCodes.Failed;

            }

            return ER;

        }

        /// <summary>
        /// Inserts the item asynchronously.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult> InsertItemAsync(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContext = $"{mClassName}.InsertItemAsync()";
            var ER = new ExecutionResult(ERContext);

            try
            {
                if (Item == null)
                {
                    Item = ModelItem;
                }

                if (DbTransaction == null)
                {
                    DbTransaction = this.DbTransaction;
                }

                if (DbCommandTimeout == null)
                {
                    DbCommandTimeout = this.DbCommandTimeout;
                }

                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero:
                        if (AutoReadControls == true)
                        {
                            ReadControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        BindingSource.EndEdit();
                        Item = (ModelClass)BindingSource.Current;
                        ModelItem = Item;
                        break;
                    default:
                        break;
                }

                ER = await Repository.InsertItemAsync(Item, DbTransaction, DbCommandTimeout);
                ER.Context = ERContext;

                if (ER.Success)
                {
                    SetAddNewState(false, true);

                    switch (mDataBindingMode)
                    {
                        case DataBindingMode.None:
                            break;
                        case DataBindingMode.Passero:
                            if (AutoWriteControls == true)
                            {
                                WriteControls();
                            }
                            break;
                        case DataBindingMode.BindingSource:
                            this.BindingSource.ResetCurrentItem();
                            break;
                        default:
                            break;
                    }
                }


                LastExecutionResult = ER;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                //HandleExeception(ER);
            }

            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;
        }


        /// <summary>
        /// Inserts the items asynchronously.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult> InsertItemsAsync(List<ModelClass> Items = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContext = $"{mClassName}.InsertItemsAsync()";
            var ER = new ExecutionResult(ERContext);

            try
            {
                if (Items == null)
                {
                    Items = ModelItems.ToList();
                }

                if (DbTransaction == null)
                {
                    DbTransaction = this.DbTransaction;
                }

                if (DbCommandTimeout == null)
                {
                    DbCommandTimeout = this.DbCommandTimeout;
                }

                ER = await Repository.InsertItemsAsync(Items, DbTransaction, DbCommandTimeout);
                ER.Context = ERContext;
                if (!ER.Success)
                    HandleExeception(ER);

                long x = Convert.ToInt64(ER.Value);
                if (x > 0)
                {
                    ModelItem = Items.ElementAt(0);
                    ModelItems = Items;
                    CurrentModelItemIndex = 0;
                }

                LastExecutionResult = ER;
                SetAddNewState(false, true);
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                //HandleExeception(ER);
            }

            if (!ER.Success)
                HandleExeception(ER);
            return ER;
        }




        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult InsertItem(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContext = $"{mClassName}.InsertItem()";
            var ER = new ExecutionResult(ERContext);


            if (Item == null) Item = ModelItem;
            if (DbTransaction == null) DbTransaction = this.DbTransaction;
            if (DbCommandTimeout == null) DbCommandTimeout = this.DbCommandTimeout;


            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoReadControls == true)
                    {
                        ReadControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    //BindingSource.CurrencyManager.EndCurrentEdit()
                    BindingSource.EndEdit();
                    Item = (ModelClass)BindingSource.Current;
                    ModelItem = Item;
                    break;
                //Item = CType(mBindingSource.Current, ModelClass)

                default:
                    break;
            }

            ER = Repository.InsertItem(Item, DbTransaction, DbCommandTimeout);
            ER.Context = ERContext;



            if (ER.Success)
            {
                SetAddNewState(false, true);
                // carica il nuovo item
                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero:
                        if (AutoWriteControls == true)
                        {
                            WriteControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        this.BindingSource.ResetCurrentItem();
                        break;
                    default:
                        break;
                }
                SaveCompleted?.Invoke();
            }


            LastExecutionResult = ER;

            if (!ER.Success)
            {
                HandleExeception(ER);
            }

            return ER;

        }

        /// <summary>
        /// Inserts the items.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult InsertItems(List<ModelClass> Items = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContenxt = $"{mClassName}.InsertItems()";
            var ER = new ExecutionResult(ERContenxt);
            if (ModelItem == null)
            {
                ModelItem = ModelItem;
            }
            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }

            long x = 0;
            ER = Repository.InsertItems(Items, DbTransaction, DbCommandTimeout);


            ER.Context = ERContenxt;
            x = Convert.ToInt64(ER.Value);
            if (x > 0)
            {
                ModelItem = Items.ElementAt(0);
                ModelItems = Items;
                CurrentModelItemIndex = 0;
            }
            LastExecutionResult = ER;
            SetAddNewState(false, true);


            if (!ER.Success)
                HandleExeception(ER);

            return ER;

        }



        /// <summary>
        /// Updates the item asynchronously.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult> UpdateItemAsync(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContext = $"{mClassName}.UpdateItemAsync()";
            var ER = new ExecutionResult(ERContext);

            try
            {
                if (mAddNewState == true)
                {
                    ER = await InsertItemAsync(Item, DbTransaction, DbCommandTimeout);
                    ER.Context = ERContext;
                    LastExecutionResult = ER;
                    if (!ER.Success)
                    {
                        HandleExeception(ER);
                    }
                    return ER;
                }

                if (Item == null)
                {
                    Item = ModelItem;
                }

                if (DbTransaction == null)
                {
                    DbTransaction = this.DbTransaction;
                }

                if (DbCommandTimeout == null)
                {
                    DbCommandTimeout = this.DbCommandTimeout;
                }

                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero:
                        if (AutoReadControls == true)
                        {
                            ReadControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        mBindingSource.EndEdit();
                        ModelItem = Item;
                        break;
                    default:
                        break;
                }

                ER = await Repository.UpdateItemAsync(Item, DbTransaction, DbCommandTimeout);

                if (Convert.ToBoolean(ER.Value))
                {
                    ModelItem = Item;
                }

                ER.Context = ERContext;

                LastExecutionResult = ER;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                //HandleExeception(ER);
            }
            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;
        }


        /// <summary>
        /// Updates the item ex asynchronously.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="ItemShadow">The item shadow.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult> UpdateItemExAsync(ModelClass Item = null, ModelClass ItemShadow = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContext = $"{mClassName}.UpdateItemExAsync()";
            var ER = new ExecutionResult(ERContext);

            try
            {
                if (mAddNewState == true)
                {
                    ER = await InsertItemAsync(Item, DbTransaction, DbCommandTimeout);
                    ER.Context = ERContext;
                    LastExecutionResult = ER;
                    if (!ER.Success)
                    {
                        HandleExeception(ER);
                    }
                    return ER;
                }

                if (Item == null)
                {
                    Item = ModelItem;
                }
                if (ItemShadow == null)
                {
                    ItemShadow = ModelItemShadow;
                }

                if (DbTransaction == null)
                {
                    DbTransaction = this.DbTransaction;
                }

                if (DbCommandTimeout == null)
                {
                    DbCommandTimeout = this.DbCommandTimeout;
                }

                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero:
                        if (AutoReadControls == true)
                        {
                            ReadControls();
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        mBindingSource.EndEdit();
                        ModelItem = Item;
                        break;
                    default:
                        break;
                }

                ER = await Repository.UpdateItemExAsync(Item, ItemShadow, DbTransaction, DbCommandTimeout);

                if (Convert.ToBoolean(ER.Value))
                {
                    ModelItem = Item;
                }

                ER.Context = ERContext;
                LastExecutionResult = ER;

            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                //HandleExeception(ER);
            }
            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;
        }


        /// <summary>
        /// Updates the items asynchronously.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult> UpdateItemsAsync(IList<ModelClass> Items = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContext = $"{mClassName}.UpdateItemsAsync()";
            var ER = new ExecutionResult(ERContext);

            try
            {
                if (Items == null)
                {
                    Items = ModelItems;
                }

                if (DbTransaction == null)
                {
                    DbTransaction = this.DbTransaction;
                }

                if (DbCommandTimeout == null)
                {
                    DbCommandTimeout = this.DbCommandTimeout;
                }

                ER = await Repository.UpdateItemsAsync(Items, DbTransaction, DbCommandTimeout);
                ER.Context = ERContext;

                LastExecutionResult = ER;

                if (Convert.ToBoolean(ER.Value))
                {
                    ModelItem = Items.ElementAt(0);
                    ModelItems = Items;
                    ApplyViewModelStateAfterRepositorySync(true);
                }

            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;
                //HandleExeception(ER);
            }
            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;
        }


        /// <summary>
        /// Updates the items ex asynchronously.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="ItemsShadow">The items shadow.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult> UpdateItemsExAsync(IList<ModelClass> Items = null, IList<ModelClass> ItemsShadow = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERContext = $"{mClassName}.UpdateItemsExAsync()";
            var ER = new ExecutionResult(ERContext);

            try
            {
                if (Items == null)
                {
                    Items = ModelItems.Clone();
                }
                if (ItemsShadow == null)
                {
                    ItemsShadow = ModelItemsShadow.Clone();
                }

                if (DbTransaction == null)
                {
                    DbTransaction = this.DbTransaction;
                }

                if (DbCommandTimeout == null)
                {
                    DbCommandTimeout = this.DbCommandTimeout;
                }

                ER = await Repository.UpdateItemsExAsync(Items, ItemsShadow, DbTransaction, DbCommandTimeout);
                ER.Context = ERContext;

                LastExecutionResult = ER;

                if (Convert.ToBoolean(ER.Value))
                {
                    ModelItem = Items.ElementAt(0);
                    ModelItems = Items;
                    ApplyViewModelStateAfterRepositorySync(true);
                }

            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;

            }
            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;
        }



        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItem(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERcontext = $"{mClassName}.UpdateItem()";
            var ER = new ExecutionResult(ERcontext);

            if (!RaiseRequest(SaveRequest))
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ResultMessage = "Save cancelled by SaveRequest handler.";
                return ER;
            }

            if (mAddNewState == true)
            {
                ER = InsertItem(Item, DbTransaction, DbCommandTimeout);
                ER.Context = ERcontext;
                LastExecutionResult = ER;
                if (!ER.Success)
                {
                    HandleExeception(ER);
                }
                return ER;
            }


            if (Item == null)
            {
                Item = ModelItem;
            }

            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }

            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoReadControls == true)
                    {
                        ReadControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    mBindingSource.EndEdit();
                    ModelItem = Item;
                    break;
                default:
                    break;
            }

            ER = Repository.UpdateItem(Item, DbTransaction, DbCommandTimeout);

            if (Convert.ToBoolean(ER.Value))
            {
                ModelItem = Item;
                ApplyViewModelStateAfterRepositorySync(true);
            }

            ER.Context = ERcontext;

            LastExecutionResult = ER;

            if (ER.Success)
                SaveCompleted?.Invoke();

            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;

        }



        /// <summary>
        /// Updates the item ex.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="ItemShadow">The item shadow.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItemEx(ModelClass Item = null, ModelClass ItemShadow = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ERcontext = $"{mClassName}.UpdateItemEx()";
            var ER = new ExecutionResult();


            if (mAddNewState == true)
            {
                ER = InsertItem(Item);
                ER.Context = ERcontext;
                if (!ER.Success)
                {
                    HandleExeception(ER);
                }
                LastExecutionResult = ER;
                return ER;
            }


            if (Item == null)
            {
                Item = ModelItem;
            }
            if (ItemShadow == null)
            {
                ItemShadow = ModelItemShadow;
            }

            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }

            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    if (AutoReadControls == true)
                    {
                        ReadControls();
                    }
                    break;
                case DataBindingMode.BindingSource:
                    mBindingSource.EndEdit();
                    ModelItem = Item;
                    break;
                default:
                    break;
            }

            ER = Repository.UpdateItemEx(Item, ItemShadow, DbTransaction, DbCommandTimeout);

            if (Convert.ToBoolean(ER.Value))
            {
                ModelItem = Item;
                ApplyViewModelStateAfterRepositorySync(true);
            }


            ER.Context = ERcontext;
            LastExecutionResult = ER;
            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;

        }
        /// <summary>
        /// Updates the items.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItems(IList<ModelClass> Items = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {

            var ER = new ExecutionResult($"{mClassName}.UpdateItems()");
            string Context = ER.Context;

            if (Items == null)
            {
                Items = ModelItems;
            }

            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }


            ER = Repository.UpdateItems(Items, DbTransaction, DbCommandTimeout);
            LastExecutionResult = Repository.LastExecutionResult;
            if (Convert.ToBoolean(ER.Value))
            {
                ModelItem = Items.ElementAt(0);
                ModelItems = Items;
                ApplyViewModelStateAfterRepositorySync(true);
            }
            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;

        }


        /// <summary>
        /// Updates the items ex.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="ItemsShadow">The items shadow.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult UpdateItemsEx(IList<ModelClass> Items = null, IList<ModelClass> ItemsShadow = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {

            var ER = new ExecutionResult($"{mClassName}.UpdateItemsEx()");
            string Context = ER.Context;

            if (Items == null)
            {
                Items = ModelItems.Clone();
            }
            if (ItemsShadow == null)
            {
                ItemsShadow = ModelItemsShadow.Clone();
            }


            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }


            ER = Repository.UpdateItemsEx(Items, ItemsShadow, DbTransaction, DbCommandTimeout);
            LastExecutionResult = Repository.LastExecutionResult;
            if (Convert.ToBoolean(ER.Value))
            {
                ModelItem = Items.ElementAt(0);
                ModelItems = Items;
                ApplyViewModelStateAfterRepositorySync(true);
            }
            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;

        }


        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult DeleteItem(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItem()");
            string Context = ER.Context;

            if (!RaiseRequest(DeleteRequest))
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ResultMessage = "DeleteItem cancelled by DeleteRequest handler.";
                return ER;
            }

            if (Item == null)
            {
                Item = ModelItem;
            }
            if (DbTransaction == null)
            {
                DbTransaction = this.DbTransaction;
            }

            if (DbCommandTimeout == null)
            {
                DbCommandTimeout = this.DbCommandTimeout;
            }

            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    ER = Repository.DeleteItem(Item, DbTransaction, DbCommandTimeout);
                    break;
                case DataBindingMode.Passero:
                    ER = Repository.DeleteItem(Item, DbTransaction, DbCommandTimeout);

                    if (Convert.ToBoolean(ER.Value))
                    {
                        if (ModelItems.Count > 0)
                        {
                            ModelItem = ModelItems.ElementAt(0);
                            if (AutoReadControls == true)
                            {
                                ReadControls();
                            }
                        }
                        ApplyViewModelStateAfterRepositorySync(true);
                    }
                    break;
                case DataBindingMode.BindingSource:

                    mBindingSource.Remove(Item);
                    mBindingSource.EndEdit();
                    ER = Repository.DeleteItem(Item, DbTransaction, DbCommandTimeout);
                    CurrentModelItemIndex = mBindingSource.CurrencyManager.Position;
                    break;
                default:
                    break;
            }

            ER.Context = Context;
            LastExecutionResult = ER;


            if (ER.Success)
                DeleteCompleted?.Invoke();


            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;

        }


        /// <summary>
        /// Deletes the item asynchronously.
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult> DeleteItemAsync(ModelClass Item = null, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItemAsync()");
            string Context = ER.Context;

            try
            {
                if (Item == null)
                {
                    Item = ModelItem;
                }
                if (DbTransaction == null)
                {
                    DbTransaction = this.DbTransaction;
                }

                if (DbCommandTimeout == null)
                {
                    DbCommandTimeout = this.DbCommandTimeout;
                }

                switch (mDataBindingMode)
                {
                    case DataBindingMode.None:
                        ER = await Repository.DeleteItemAsync(Item, DbTransaction, DbCommandTimeout);
                        break;
                    case DataBindingMode.Passero:
                        ER = await Repository.DeleteItemAsync(Item, DbTransaction, DbCommandTimeout);

                        if (Convert.ToBoolean(ER.Value))
                        {
                            ModelItem = ModelItems.ElementAt(0);
                            if (AutoReadControls == true)
                            {
                                ReadControls();
                            }
                            ApplyViewModelStateAfterRepositorySync(true);
                        }
                        break;
                    case DataBindingMode.BindingSource:
                        mBindingSource.Remove(Item);
                        mBindingSource.EndEdit();
                        ER = await Repository.DeleteItemAsync(Item, DbTransaction, DbCommandTimeout);
                        CurrentModelItemIndex = mBindingSource.CurrencyManager.Position;
                        break;
                    default:
                        break;
                }

                ER.Context = Context;
                LastExecutionResult = ER;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;

            }
            if (!ER.Success)
            {
                HandleExeception(ER);
            }
            return ER;
        }



        /// <summary>
        /// Deletes the items .
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public ExecutionResult  DeleteItems(List<ModelClass> Items, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItems()");
            string Context = ER.Context;

            try
            {
                if (DbTransaction == null)
                {
                    DbTransaction = this.DbTransaction;
                }

                if (DbCommandTimeout == null)
                {
                    DbCommandTimeout = this.DbCommandTimeout;
                }

                ER = Repository.DeleteItems(Items, DbTransaction, DbCommandTimeout);

                if (Convert.ToBoolean(ER.Value))
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        ModelItems.Remove(Items[i]);
                    }

                    ModelItem = ModelItems.ElementAt(0);
                }

                ER.Context = Context;
                LastExecutionResult = ER;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;

            }

            if (!ER.Success)
            {
                HandleExeception(ER);
            }

            return ER;
        }


        /// <summary>
        /// Deletes the items asynchronously.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="DbTransaction">The database transaction.</param>
        /// <param name="DbCommandTimeout">The database command timeout.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ExecutionResult> DeleteItemsAsync(List<ModelClass> Items, IDbTransaction DbTransaction = null, int? DbCommandTimeout = null)
        {
            var ER = new ExecutionResult($"{mClassName}.DeleteItemsAsync()");
            string Context = ER.Context;

            try
            {
                if (DbTransaction == null)
                {
                    DbTransaction = this.DbTransaction;
                }

                if (DbCommandTimeout == null)
                {
                    DbCommandTimeout = this.DbCommandTimeout;
                }

                ER = await Repository.DeleteItemsAsync(Items, DbTransaction, DbCommandTimeout);

                if (Convert.ToBoolean(ER.Value))
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        ModelItems.Remove(Items[i]);
                    }

                    ModelItem = ModelItems.ElementAt(0);
                }

                ER.Context = Context;
                LastExecutionResult = ER;
            }
            catch (Exception ex)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
                ER.ErrorCode = 1;

            }

            if (!ER.Success)
            {
                HandleExeception(ER);
            }

            return ER;
        }

    }
}
