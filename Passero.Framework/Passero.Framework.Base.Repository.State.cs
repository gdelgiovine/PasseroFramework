using Dapper;
using Dapper.ColumnMapper;
using Dapper.Contrib.Extensions;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Passero.Framework.Extensions;
using Wisej.Web;
//using Passero.Framework.Base;
#nullable enable

namespace Passero.Framework
{
    public partial class Repository<ModelClass>
        where ModelClass : class
    {
        public void ResetModelItem(bool ResetModelItems = true)
        {
            ModelItem = GetEmptyModelItem();
            if (ResetModelItems)
                ModelItems = new List<ModelClass>();
        }
        /// <summary>
        /// Resets the model items.
        /// </summary>
        public void ResetModelItems()
        {
            ModelItems = new List<ModelClass>();
        }

        /// <summary>
        /// Raises the <see cref="E:ModelEvents" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnModelEvents(EventArgs e)
        {
            ModelEvents?.Invoke(this, e);
        }

        /// <summary>
        /// The add new current model item index
        /// </summary>
        private int _AddNewCurrentModelItemIndex = -1;

        /// <summary>
        /// Gets or sets the index of the add new current model item.
        /// </summary>
        /// <value>
        /// The index of the add new current model item.
        /// </value>
        public int AddNewCurrentModelItemIndex
        {
            get
            {
                return _AddNewCurrentModelItemIndex;
            }
            set
            {
                _AddNewCurrentModelItemIndex = value;
            }
        }


        /// <summary>
        /// The current model item index
        /// </summary>
        private int _CurrentModelItemIndex = -1;

        /// <summary>
        /// Gets or sets the index of the current model item.
        /// </summary>
        /// <value>
        /// The index of the current model item.
        /// </value>
        public int CurrentModelItemIndex
        {
            get
            {
                return _CurrentModelItemIndex;
            }
            set
            {
                if (value < -1)
                {
                    value = -1;
                }
                _CurrentModelItemIndex = value;
                if (value > -1)
                {
                    if (_ModelItems.Count < value)
                    {
                        _ModelItem = _ModelItems.ElementAt(_CurrentModelItemIndex);
                    }
                }
            }
        }


        /// <summary>
        /// Gets the model items at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ExecutionResult<ModelClass> GetModelItemsAt(int index)
        {
            var ERContext = $"{mClassName}.GetModelItemsAt()";
            ExecutionResult<ModelClass> ER = new ExecutionResult<ModelClass>(ERContext);
            if (_ModelItems == null)
            {
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ResultMessage = "Invalid Index!";
                ER.ErrorCode = 0;
            }
            if (index > -1 && index < _ModelItems.Count())
            {
                ER.Value = _ModelItems.ElementAt(index);
            }
            LastExecutionResult = ER.ToExecutionResult();
            return ER;

        }
     
        private ExecutionResult MoveToIndex(int index)
        {
            var ERContext = $"{mClassName}.MoveToIndex()";
            ExecutionResult ER = new ExecutionResult(ERContext);

            if (_ModelItems != null && _ModelItems.Count > 0)
            {
                if (index >= 0 && index < _ModelItems.Count)
                {
                    _CurrentModelItemIndex = index;
                    _ModelItem = _ModelItems.ElementAt(index);
                }
                else
                {
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    ER.ErrorCode = 1;
                    ER.ResultMessage = "Invalid Index Position.";
                }
            }
            else
            {
                _ModelItem = null;
                _CurrentModelItemIndex = -1;
                ER.ResultCode = ExecutionResultCodes.Failed;
                ER.ErrorCode = 1;
                ER.ResultMessage = "Model items collection is empty.";
            }

            return ER;
        }


       

        /// <summary>
        /// Moves to the first item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MoveFirstItem()
        {
            return MoveToIndex(0);
        }
        /// <summary>
        /// Moves to the last item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MoveLastItem()
        {
            return MoveToIndex(_ModelItems.Count - 1);
        }

        /// <summary>
        /// Moves to the previous item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MovePreviousItem()
        {
            return MoveToIndex(_CurrentModelItemIndex - 1);
        }

        /// <summary>
        /// Moves to the next item.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult MoveNextItem()
        {
            return MoveToIndex(_CurrentModelItemIndex + 1);
        }

        /// <summary>
        /// Moves at item.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <returns></returns>
        public ExecutionResult MoveAtItem(int index)
        {
            return MoveToIndex(index);
        }



        /// <summary>
        /// Gets or sets the model items.
        /// </summary>
        /// <value>
        /// The model items.
        /// </value>
        //private List<ModelClass>? _ModelItems { get; set; } = new List<ModelClass>();
        private IList<ModelClass>? _ModelItems { get; set; } = new List<ModelClass>();

        /// <summary>
        /// Gets or sets the model items.
        /// </summary>
        /// <value>
        /// The model items.
        /// </value>
        public IList<ModelClass>? ModelItems
        {
            get
            {
                return _ModelItems;
            }
            set
            {
                _ModelItems = value;
            }
        }


        /// <summary>
        /// Gets or sets the modeltem.
        /// </summary>
        /// <value>
        /// The modeltem.
        /// </value>
        private ModelClass? _ModelItem { get; set; }
        /// <summary>
        /// Gets or sets the model item.
        /// </summary>
        /// <value>
        /// The model item.
        /// </value>
        public ModelClass? ModelItem
        {
            get
            {
                return _ModelItem;
            }
            set
            {
                _ModelItem = value;
            }
        }

#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        /// <summary>
        /// Gets or sets the model item shadow.
        /// </summary>
        /// <value>
        /// The model item shadow.
        /// </value>
        private ModelClass? _ModelItemShadow { get; set; }
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        /// <summary>
        /// Gets or sets the model item shadow.
        /// </summary>
        /// <value>
        /// The model item shadow.
        /// </value>
        public ModelClass? ModelItemShadow
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
        {
            get
            {
                return _ModelItemShadow;
            }
            set
            {
                _ModelItemShadow = value;
            }
        }

        /// <summary>
        /// Gets or sets the model items shadow.
        /// </summary>
        /// <value>
        /// The model items shadow.
        /// </value>
        private IList<ModelClass> _ModelItemsShadow { get; set; } = new List<ModelClass>();
        /// <summary>
        /// Gets or sets the model items shadow.
        /// </summary>
        /// <value>
        /// The model items shadow.
        /// </value>
        public IList<ModelClass> ModelItemsShadow
        {
            get
            {
                return _ModelItemsShadow;
            }
            set
            {
                _ModelItemsShadow = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [m add new state].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [m add new state]; otherwise, <c>false</c>.
        /// </value>
        private bool mAddNewState { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [add new state].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [add new state]; otherwise, <c>false</c>.
        /// </value>
        public bool AddNewState
        {
            get
            {
                return mAddNewState;
            }
            set
            {

                mAddNewState = value;
            }
        }
        /// <summary>
        /// Adds the new.
        /// </summary>
        public void AddNew()
        {
            if (AddNewState == false)
            {
                AddNewState = true;
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        //public SqlConnection SqlConnection { get; set; }
        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>

        private IDbConnection mDbConnection;
        public IDbConnection DbConnection
        {
            get
            {
                return mDbConnection;
            }
            set
            {
                mDbConnection = value;
                DbObject = new Base.DbObject<ModelClass>(mDbConnection);
                SqlDialect =DetectSqlDialect();
                EnsureTypeMapRegistered();
            }
        }



        private static readonly ConcurrentDictionary<Type, bool> _typeMapRegistered = new();

        /// <summary>
        /// Registra il Dapper ColumnTypeMapper per <typeparamref name="ModelClass"/>
        /// al primo utilizzo di questo repository, garantendo il mapping corretto
        /// delle colonne DB anche quando i nomi differiscono dalle proprietà del modello.
        /// </summary>
        static Repository()
        {
            Dapper.SqlMapper.SetTypeMap(
                typeof(ModelClass),
                new Dapper.ColumnMapper.ColumnTypeMapper(typeof(ModelClass)));
        }

        /// <summary>
        /// Ensures that the Dapper column mapper is registered for <typeparamref name="ModelClass"/>
        /// exactly once per type, transparently and without requiring any code in the model class.
        /// Called when <see cref="DbConnection"/> is set, guaranteeing registration before any query.
        /// </summary>
        private static void EnsureTypeMapRegistered()
        {
            _typeMapRegistered.GetOrAdd(typeof(ModelClass), t =>
            {
                Dapper.SqlMapper.SetTypeMap(t, new Dapper.ColumnMapper.ColumnTypeMapper(t));
                return true;
            });
        }

        //public SqlTransaction SqlTransaction { get; set; }
        /// <summary>
        /// Gets or sets the database transaction.
        /// </summary>
        /// <value>
        /// The database transaction.
        /// </value>
        public IDbTransaction DbTransaction { get; set; }
        /// <summary>
        /// Gets or sets the database command timeout.
        /// </summary>
        /// <value>
        /// The database command timeout.
        /// </value>
        public int DbCommandTimeout { get; set; } = 30;

        ///// <summary>
        ///// Gets or sets the database context.
        ///// </summary>
        ///// <value>
        ///// The database context.
        ///// </value>
        //public Base.DbContext DbContext { get; set; }
        
        

        /// <summary>
        /// Gets or sets the database object.
        /// </summary>
        /// <value>
        /// The database object.
        /// </value>
        public Base.DbObject<ModelClass> DbObject { get; set; }

        /// <summary>
        /// Gets the model item clone.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetModelItemClone()
        {
            return Utilities.Clone(_ModelItem);
        }

        /// <summary>
        /// Gets the model items clone.
        /// </summary>
        /// <returns></returns>
        public IList<ModelClass> GetModelItemsClone()
        {
            return Utilities.Clone(_ModelItems);
        }


        /// <summary>
        /// Sets the model item shadow.
        /// </summary>
        public void SetModelItemShadow()
        {
            _ModelItemShadow = Utilities.Clone(_ModelItem);
            //TBD: verifica se è superfluo
            if (ViewModel != null)
            {
                ViewModel.ModelItemShadow = _ModelItemShadow;
            }
        }

        /// <summary>
        /// Sets the model items shadow.
        /// </summary>
        public void SetModelItemsShadow()
        {
            _ModelItemsShadow = Utilities.Clone(_ModelItems);
            if (ViewModel != null)
            {
                ViewModel.ModelItemShadow = _ModelItemShadow;
            }
        }

        /// <summary>
        /// Gets the empty model.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetEmptyModel()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }


        /// <summary>
        /// Creates the database object.
        /// </summary>
        private void CreateDbObject()
        {
            DbObject = new Base.DbObject<ModelClass>(DbConnection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{ModelClass}"/> class.
        /// </summary>
        /// <param name="SqlConnection">The SQL connection.</param>
        /// <param name="SqlTransaction">The SQL transaction.</param>
        public Repository(IDbConnection SqlConnection, IDbTransaction SqlTransaction = null)
        {

            _ModelItem = GetEmptyModel();
            SetModelItemShadow();
            DbTransaction = SqlTransaction;
            DbConnection = SqlConnection;
            DbObject = new Base.DbObject<ModelClass>(DbConnection);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{ModelClass}"/> class.
        /// </summary>
        /// <param name="Model">The model.</param>
        public Repository(ModelClass Model)
        {
            //ValidateModelClass();
            _ModelItem = GetEmptyModel();
            SetModelItemShadow();
            SetModelItemsShadow();
            //DbObject = new Base.DbObject<ModelClass>(DbConnection);


        }

        public bool UseUpdateEx { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{ModelClass}"/> class.
        /// </summary>
        public Repository()
        {
            //ValidateModelClass();
            _ModelItem = GetEmptyModel();
            SetModelItemShadow();
            SetModelItemsShadow();
            //DbObject = new Base.DbObject<ModelClass>(DbConnection);


        }


        /// <summary>
        /// Inizializza il repository Dapper dalla connessione esposta da un <see cref="IPasseroDbContext"/>.
        /// Permette di condividere lo stesso DbContext tra EfRepository (LINQ) e Repository (Dapper).
        /// La connessione viene aperta se non lo è già.
        /// </summary>
        /// <param name="dbContext">DbContext che implementa <see cref="IPasseroDbContext"/>.</param>
        /// <param name="sqlTransaction">Transazione ADO.NET opzionale.</param>
        public Repository(Passero.Framework .Base.IPasseroDbContext dbContext, IDbTransaction sqlTransaction = null)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            dbContext.EnsureConnectionOpen();
            _ModelItem = GetEmptyModel();
            SetModelItemShadow();
            SetModelItemsShadow();
            DbTransaction = sqlTransaction;
            DbConnection = dbContext.DbConnection;
            DbObject = new Base.DbObject<ModelClass>(DbConnection);
        }

        /// <summary>
        /// Resolveds the SQL query.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
    }
}
