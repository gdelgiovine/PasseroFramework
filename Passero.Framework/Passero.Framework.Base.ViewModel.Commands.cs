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
        /// <summary>
        /// Undoes the changes.
        /// </summary>
        /// <param name="AllItems">if set to <c>true</c> [all items].</param>
        /// <returns></returns>
        public ExecutionResult UndoChanges(bool AllItems = false)
        {
            var ERContenxt = $"{mClassName}.UndoChanges()";
            var ER = new ExecutionResult(ERContenxt);
            try
            {
                if (AddNewState == false)
                {
                    ModelItem = Utilities.Clone<ModelClass>(ModelItemShadow);
                    if (ModelItems.Count >= CurrentModelItemIndex)
                    {
                        ModelItems[CurrentModelItemIndex] = Utilities.Clone<ModelClass>(ModelItemShadow);
                    }
                }
                else
                {
                    if (ModelItemsCount > 0)
                    {
                        ModelItems.RemoveAt(ModelItemsCount - 1);
                    }
                }


                //If AllItems And AutoUpdateModelItemsShadows = True Then
                ModelItems = Utilities.Clone<IList<ModelClass>>(ModelItemsShadow);
                //End If

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
                            mBindingSource.Position = AddNewCurrentModelItemIndex;
                            MoveAtItem(AddNewCurrentModelItemIndex);
                            if (mDataNavigator != null)
                            {
                                //ReflectionHelper.InvokeMethodByName(ref mDataNavigator, "UpdateRecordLabel");
                                mDataNavigator.UpdateRecordLabel();
                            }
                        }
                        else
                        {
                            mBindingSource.CancelEdit();
                        }
                        break;

                    default:
                        break;
                }
                if (mAddNewState == true)
                {
                    mAddNewState = false;
                }

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
                    mAddNewState = false;

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
                AddNewState = false;
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
                mAddNewState = false;
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
            AddNewState = false;


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
            if (mAddNewState == true)
            {
                //long r= this.Repository.InsertItem(Item);
                //Dim r = InsertItem(Item)
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
                //long r= this.Repository.InsertItem(Item);
                //Dim r = InsertItem(Item)
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
                        ModelItem = ModelItems.ElementAt(0);
                        if (AutoReadControls == true)
                        {
                            ReadControls();
                        }
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
