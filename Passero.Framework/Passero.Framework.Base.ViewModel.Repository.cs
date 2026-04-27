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
        /// Gets or sets the data binding mode.
        /// </summary>
        /// <value>
        /// The data binding mode.
        /// </value>
        public DataBindingMode DataBindingMode
        {
            get
            {
                return mDataBindingMode;
            }

            set
            {
                mDataBindingMode = value;
                if (mDataBindingMode == DataBindingMode.BindingSource)
                {
                    if (mBindingSource == null)
                    {
                        mBindingSource = new BindingSource();
                        mBindingSource.DataSource = ModelItem;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>
        public IDbConnection DbConnection
        {
            get { return Repository.DbConnection; }
            set { Repository.DbConnection = value; }
        }
        /// <summary>
        /// Gets or sets the data bind controls.
        /// </summary>
        /// <value>
        /// The data bind controls.
        /// </value>
        public Dictionary<string, DataBindControl> DataBindControls { get; set; } = new Dictionary<string, DataBindControl>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets or sets the database transaction.
        /// </summary>
        /// <value>
        /// The database transaction.
        /// </value>
        public IDbTransaction DbTransaction
        {
            get
            {
                return Repository.DbTransaction;
            }
            set
            {
                Repository.DbTransaction = value;
            }
        }
        /// <summary>
        /// Gets or sets the database command timeout.
        /// </summary>
        /// <value>
        /// The database command timeout.
        /// </value>
        public int DbCommandTimeout
        {
            get
            {
                return Repository.DbCommandTimeout;
            }
            set
            {
                Repository.DbCommandTimeout = value;
            }
        }

        /// <summary>
        /// Gets the empty model item.
        /// </summary>
        /// <returns></returns>
        public ModelClass GetEmptyModelItem()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }


        /// <summary>
        /// Gets or sets a value indicating whether [automatic write controls].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic write controls]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoWriteControls { get; set; } = false;
        /// <summary>
        /// Gets or sets a value indicating whether [automatic read controls].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic read controls]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoReadControls { get; set; } = false;
        /// <summary>
        /// The m data bind controls automatic set maximum lenght
        /// </summary>
        private bool mDataBindControlsAutoSetMaxLenght = true;
        /// <summary>
        /// Gets or sets a value indicating whether [data bind controls automatic set maximum lenght].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data bind controls automatic set maximum lenght]; otherwise, <c>false</c>.
        /// </value>
        //public bool DataBindControlsAutoSetMaxLenght
        //{
        //    get
        //    {
        //        return mDataBindControlsAutoSetMaxLenght;
        //    }
        //    set
        //    {
        //        mDataBindControlsAutoSetMaxLenght = value;
        //        if (value == true)
        //        {
        //            if (Repository.DbObject.DbColumns.Count == 0)
        //            {
        //                Repository.DbObject.DbConnection = Repository.DbConnection;
        //                Repository.DbObject.GetSchema();
        //            }
        //        }

        //    }
        //}

        public bool DataBindControlsAutoSetMaxLenght
        {
            get { return mDataBindControlsAutoSetMaxLenght; }
            set
            {
                mDataBindControlsAutoSetMaxLenght = value;
                if (value && DapperRepository != null)
                {
                    if (DapperRepository.DbObject.DbColumns.Count == 0)
                    {
                        DapperRepository.DbObject.DbConnection = Repository.DbConnection;
                        DapperRepository.DbObject.GetSchema();
                    }
                }
            }
        }

        /// <summary>
        /// The m automatic fit columns lenght
        /// </summary>
        private bool mAutoFitColumnsLenght = false;
        /// <summary>
        /// Gets or sets a value indicating whether [automatic fit columns lenght].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic fit columns lenght]; otherwise, <c>false</c>.
        /// </value>
        //public bool AutoFitColumnsLenght
        //{
        //    get
        //    {
        //        return mAutoFitColumnsLenght;
        //    }
        //    set
        //    {
        //        mAutoFitColumnsLenght = value;
        //        if (value == true)
        //        {
        //            if (Repository.DbObject.DbColumns.Count == 0)
        //            {
        //                Repository.DbObject.DbConnection = Repository.DbConnection;
        //                Repository.DbObject.GetSchema();
        //            }
        //        }

        //    }
        //}
        public bool AutoFitColumnsLenght
        {
            get { return mAutoFitColumnsLenght; }
            set
            {
                mAutoFitColumnsLenght = value;
                if (value && DapperRepository != null)
                {
                    if (DapperRepository.DbObject.DbColumns.Count == 0)
                    {
                        DapperRepository.DbObject.DbConnection = Repository.DbConnection;
                        DapperRepository.DbObject.GetSchema();
                    }
                }
            }
        }

        ///// <summary>
        ///// Gets or sets the repository.
        ///// </summary>
        ///// <value>
        ///// The repository.
        ///// </value>
        //public Repository<ModelClass> Repository { get; set; }

        /// <summary>
        /// Repository interno del ViewModel. Può essere un <see cref="Repository{ModelClass}"/>
        /// (Dapper) oppure un <see cref="EfRepository{ModelClass}"/> (EF Core / EF6),
        /// a seconda del costruttore o dell'assegnazione esterna.
        /// </summary>
        public Base.IPasseroRepository<ModelClass> Repository { get; set; }

        /// <summary>
        /// Restituisce il repository interno come <see cref="Repository{ModelClass}"/> (Dapper)
        /// se il repository corrente è di quel tipo, altrimenti null.
        /// Utile per accedere a funzionalità Dapper-specific (DbObject, ecc.).
        /// </summary>
        public Repository<ModelClass> DapperRepository => Repository as Repository<ModelClass>;


        //public ModelClass GetEmptyModel()
        //{
        //    return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        //}

        /// <summary>
        /// The m default SQL query parameters
        /// </summary>
        private DynamicParameters mDefaultSQLQueryParameters;
        /// <summary>
        /// Gets or sets the default SQL query parameters.
        /// </summary>
        /// <value>
        /// The default SQL query parameters.
        /// </value>
        public DynamicParameters DefaultSQLQueryParameters
        {
            get
            {
                mDefaultSQLQueryParameters = Repository.DefaultSQLQueryParameters;
                return mDefaultSQLQueryParameters;
            }
            set
            {
                mDefaultSQLQueryParameters = value;
                Repository.DefaultSQLQueryParameters = value;
            }
        }
        /// <summary>
        /// The m default SQL query
        /// </summary>
        private string mDefaultSQLQuery;
        /// <summary>
        /// Gets or sets the default SQL query.
        /// </summary>
        /// <value>
        /// The default SQL query.
        /// </value>
        public string DefaultSQLQuery
        {
            get
            {
                mDefaultSQLQuery = Repository.DefaultSQLQuery;
                return mDefaultSQLQuery;
            }
            set
            {
                mDefaultSQLQuery = value;
                Repository.DefaultSQLQuery = value;
            }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public DynamicParameters Parameters
        {
            get
            {
                return Repository.Parameters;

            }
            set
            {
                Repository.Parameters = value;
            }
        }

        private bool mUseUpdateEx = false;
        public bool UseUpdateEx 
        { 
            get
            {
                return mUseUpdateEx;
            }
            
            set
            {
                mUseUpdateEx = value;   
                //Repository.UseUpdateEx = value; 
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel{ModelClass}"/> class.
        /// </summary>
        /// <param name="Name">ViewModel Name</param>
        /// <param name="Description">ViewModel Description</param>
        //public ViewModel(string Name = "", string FriendlyName ="", string Description  = "")
        //{
        //    Repository = new Repository<ModelClass>();

        //    DefaultSQLQuery = $"SELECT * FROM {DapperHelper.Utilities.GetTableName<ModelClass>()}";
        //    DefaultSQLQueryParameters = new DynamicParameters();
        //    if (Name != "")
        //        this.Name = Name;
        //    else
        //        this.Name = $"{typeof(ModelClass).Name}_{this.GetHashCode():X}";

        //    if (FriendlyName  != "")
        //        this.FriendlyName = FriendlyName;
        //    else
        //        this.FriendlyName = Name;

        //    if (Description != "")
        //        this.Description = Description;
        //    else
        //        this.Description = Name;

        //    Repository.ViewModel = this;
        //    Repository.Name = $"Repository<{typeof(ModelClass).Name}>";
        //    Repository.ErrorNotificationMessageBox = ErrorNotificationMessageBox;
        //    Repository.ErrorNotificationMode = ErrorNotificationMode;
        //    mModelItemShadow = GetEmptyModelItem();


        //}



        public ViewModel(Base.IPasseroRepository<ModelClass> repository, string Name = "", string FriendlyName = "", string Description = "")
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));

            DefaultSQLQuery = $"SELECT * FROM { Utilities.GetModelTableName<ModelClass>()}";
            DefaultSQLQueryParameters = new DynamicParameters();

            this.Name = Name != "" ? Name : $"{typeof(ModelClass).Name}_{GetHashCode():X}";
            this.FriendlyName = FriendlyName != "" ? FriendlyName : this.Name;
            this.Description = Description != "" ? Description : this.Name;

            // Collega il ViewModel al Repository Dapper se applicabile
            if (Repository is Repository<ModelClass> dapperRepo)
                dapperRepo.ViewModel = this;

            Repository.Name = $"Repository<{typeof(ModelClass).Name}>";
            Repository.ErrorNotificationMessageBox = ErrorNotificationMessageBox;
            Repository.ErrorNotificationMode = ErrorNotificationMode;
            mModelItemShadow = GetEmptyModelItem();
        }

        /// <summary>
        /// Costruttore statico: registra automaticamente <typeparamref name="ModelClass"/>
        /// nei DbContext EF Core ed EF6, eliminando la necessità di passare
        /// <c>entityTypes[]</c> esplicitamente a <see cref="Base.ORMContextFactory.Create"/>.
        /// </summary>
        static ViewModel()
        {
            Passero.Framework.Base.ORMContextFactory.RegisterEntity(typeof(ModelClass));
        }

        /// <summary>
        /// Costruttore con IPasseroDbContext: crea automaticamente il repository
        /// del tipo corretto in base all'<see cref="Base.ORMType"/> esposto dal contesto.
        /// <list type="bullet">
        ///   <item><see cref="Base.ORMType.Dapper"/> → <see cref="Repository{ModelClass}"/></item>
        ///   <item><see cref="Base.ORMType.EntityFrameworkCore"/> → <see cref="EfRepository{ModelClass}"/></item>
        ///   <item><see cref="Base.ORMType.EntityFramework6"/> → <see cref="EfRepository{ModelClass}"/></item>
        ///   <item><see cref="Base.ORMType.EntityFramework"/> → <see cref="EfRepository{ModelClass}"/></item>
        /// </list>
        /// </summary>
        public ViewModel(Base.IPasseroDbContext dbContext, string Name = "", string FriendlyName = "", string Description = "")
            : this(CreateRepositoryFromContext(dbContext), Name, FriendlyName, Description)
        {
            DbContext = dbContext;
            Init(dbContext);
        }

        /// <summary>
        /// Crea il repository appropriato in base all'ORMType del DbContext fornito.
        /// </summary>
        private static Base.IPasseroRepository<ModelClass> CreateRepositoryFromContext(Base.IPasseroDbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            switch (dbContext.ORMType)
            {
                case Base.ORMType.EntityFrameworkCore:
                case Base.ORMType.EntityFramework6:
                case Base.ORMType.EntityFramework:
                    return new EfRepository<ModelClass>(dbContext);

                case Base.ORMType.Dapper:
                default:
                    return new Repository<ModelClass>(dbContext);
            }
        }


               


        public ViewModel(string Name = "", string FriendlyName = "", string Description = "")
        {
            var dapperRepo = new Repository<ModelClass>();
            Repository = dapperRepo;

            DefaultSQLQuery = $"SELECT * FROM { Utilities.GetModelTableName<ModelClass>()}";
            DefaultSQLQueryParameters = new DynamicParameters();

            this.Name = Name != "" ? Name : $"{typeof(ModelClass).Name}_{GetHashCode():X}";
            this.FriendlyName = FriendlyName != "" ? FriendlyName : this.Name;
            this.Description = Description != "" ? Description : this.Name;

            // Solo Repository Dapper ha la property ViewModel
            dapperRepo.ViewModel = this;
            Repository.Name = $"Repository<{typeof(ModelClass).Name}>";
            Repository.ErrorNotificationMessageBox = ErrorNotificationMessageBox;
            Repository.ErrorNotificationMode = ErrorNotificationMode;
            mModelItemShadow = GetEmptyModelItem();
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel{ModelClass}"/> class.
        /// </summary>
        /// <param name="Repository">The repository.</param>
        /// <param name="Name">The name.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        public ViewModel(ref Repository<ModelClass> Repository, string Name = "", string FriendlyName="", string Description = "")
        {

            if (Name != "")
                this.Name = Name;
            else
                this.Name = nameof(ModelClass);
            if (FriendlyName != "")
                this.FriendlyName = FriendlyName;
            else
                this.FriendlyName = Name;

            if (Description != "")
                this.Description = Description;
            else
                this.Description = Name;

            mModelItemShadow = GetEmptyModelItem();
            this.Repository = Repository;
        }



        /// <summary>
        /// Initializes the specified database context.
        /// Reinstanzia il repository interno in base all'<see cref="Base.ORMType"/>
        /// esposto dal <paramref name="DbContext"/> fornito.
        /// </summary>
        /// <param name="DbContext">The database context.</param>
        /// <param name="DataBindingMode">The data binding mode.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Description">The description.</param>
        public virtual void Init(Base.IPasseroDbContext DbContext, DataBindingMode DataBindingMode = DataBindingMode.Passero, string Name = "", string Description = "")
        {
            if (DbContext == null)
                throw new ArgumentNullException(nameof(DbContext));

            if (DbContext.ORMType != Base.ORMType.Dapper)
            {
                Base.ORMContextFactory.RegisterEntity(typeof(ModelClass));
            }

            mDataBindingMode = DataBindingMode;
            this.DbContext = DbContext;
            this.DbContext.EnsureConnectionOpen();
            this.Name = Name;
            this.Description = Description;

            Repository = CreateRepositoryFromContext(DbContext);

            Repository.Name = $"Repository<{typeof(ModelClass).Name}>";
            Repository.ErrorNotificationMessageBox = ErrorNotificationMessageBox;
            Repository.ErrorNotificationMode = ErrorNotificationMode;
            Repository.DefaultSQLQueryParameters = new DynamicParameters();
            Repository.Parameters = new DynamicParameters();
            Repository.ViewModel = this;

            // Per Dapper: imposta la query di default qui perché Repository<T>
            // non dispone di BuildSelectQuery con alias.
            // Per EfRepository: DefaultSQLQuery e SQLQuery sono già impostati
            // correttamente dal costruttore tramite BuildSelectQuery — non vanno sovrascritti.
            if (Repository is Repository<ModelClass> dapperRepo)
            {
                dapperRepo.ViewModel = this;
                dapperRepo.DbConnection = DbContext.DbConnection;

                string defaultQuery = $"SELECT * FROM {Utilities.GetModelTableName<ModelClass>()}";
                dapperRepo.DefaultSQLQuery = defaultQuery;
                dapperRepo.SQLQuery = defaultQuery;
            }
        }

        /// <summary>
        /// Initializes the specified database connection.
        /// </summary>
        /// <param name="DbConnection">The database connection.</param>
        /// <param name="DataBindingMode">The data binding mode.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Description">The description.</param>
        public virtual void Init(IDbConnection DbConnection, DataBindingMode DataBindingMode = DataBindingMode.Passero, string Name="", string Description="")
        {
            mDataBindingMode = DataBindingMode;
            this.DbConnection = DbConnection;
            this.Name = Name;   
            this.Description = Description; 
            Repository.DbConnection = DbConnection;
       
        }




        /// <summary>
        /// Initializes the specified database connection.
        /// </summary>
        /// <param name="DbConnection">The database connection.</param>
        /// <param name="DataBindingMode">The data binding mode.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Description">The description.</param>
        public virtual void Init(DataBindingMode DataBindingMode = DataBindingMode.Passero, string Name = "", string Description = "")
        {
            mDataBindingMode = DataBindingMode;
            this.DbConnection = this.DbContext.DbConnection;
            this.DbContext.EnsureConnectionOpen();
            this.Name = Name;
            this.Description = Description;
            Repository.DbConnection = this.DbConnection;

        }
    }
}
