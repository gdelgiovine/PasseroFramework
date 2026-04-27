using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
// Aggiungi riferimenti per altri provider se necessari
// using MySql.Data.MySqlClient;
// using Npgsql;

namespace Passero.Framework.Base
{
    public enum DatabaseProvider
    {
        SqlServer,
        MySql,
        PostgreSql,
        Oracle,
        Sqlite
    }

    public enum EntityState
    {
        Added,
        Modified,
        Deleted
    }
    /// <summary>
    /// 
    /// </summary>
    public class DbContext
    {
        private readonly ConcurrentDictionary<object, EntityState> _changeTracker = new();
        private readonly Dictionary<Type, object> _dbSets = new();

        private const string mClassName = "Passero.Framework.DbContext";

        private IDbConnection mDbConnection { get; set; }
        private IDbTransaction mDbTransaction { get; set; }

        public Dictionary<string, ViewModel<ModelBase >> ViewModels { get; set; } = new Dictionary<string, ViewModel<ModelBase >>();
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database provider.
        /// </summary>
        /// <value>
        /// The database provider.
        /// </value>
        public DatabaseProvider Provider { get; set; } = DatabaseProvider.SqlServer;

        protected virtual void OnConfiguring()
        {
            
            ConnectionString = "DefaultConnectionString";
        }

        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public DbContext()
        {
            OnConfiguring();
        }
        public DbContext(DbContextOptionsBuilder optionsBuilder)
        {
            ConnectionString = optionsBuilder.GetConnectionString();
        }

        protected virtual void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseConnectionString("DefaultConnectionString");
        }

        public ExecutionResult Init(string ConnectionString = "")
        {
            var ER = new ExecutionResult($"{mClassName}.Init()");
            ViewModels.Clear();
            try
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    ConnectionString = this.ConnectionString;
                }
                else
                {
                    this.ConnectionString = ConnectionString;
                }

                mDbConnection = CreateDbConnection(this.ConnectionString);
                mDbConnection.Open();
            }
            catch (Exception ex)
            {
                ER.ErrorCode = 1;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
            }
            return ER;
        }

        public void Dispose()
        {
            mDbTransaction?.Dispose();
            mDbConnection?.Close();
            mDbConnection?.Dispose();
        }

        public DbSet<T> Set<T>() where T : class
        {
            if (!_dbSets.ContainsKey(typeof(T)))
            {
                _dbSets[typeof(T)] = new DbSet<T>();
            }

            return (DbSet<T>)_dbSets[typeof(T)];
        }

        public IDbConnection DbConnection
        {
            get { return mDbConnection; }
            set
            {
                mDbConnection = value;
                foreach (ViewModel<ModelBase > _ViewModel in ViewModels.Values)
                    _ViewModel.Repository.DbConnection = DbConnection;
            }
        }

        public IDbTransaction DbTransaction
        {
            get { return mDbTransaction; }
            set
            {
                mDbTransaction = value;
                foreach (ViewModel<ModelBase > _ViewModel in ViewModels.Values)
                    _ViewModel.Repository.DbTransaction = DbTransaction;
            }
        }

        public ExecutionResult Ping(string ConnectionString = "")
        {
            var ER = new ExecutionResult($"{mClassName}.Ping()");

            if (string.IsNullOrEmpty(ConnectionString.Trim()))
            {
                ConnectionString = this.ConnectionString;
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
                ER.ResultMessage = "Empty ConnectionString!";
                ER.ErrorCode = 1;
                return ER;
            }

            try
            {
                using (var dbConnection = CreateDbConnection(ConnectionString))
                {
                    dbConnection.Open();
                }
            }
            catch (Exception ex)
            {
                ER.ErrorCode = 2;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
            }

            return ER;
        }

        public bool AddViewModel(ViewModel<ModelBase > ViewModel)
        {
            if (!ViewModels.ContainsKey(ViewModel.Name))
            {
                ViewModels.Add(ViewModel.Name, ViewModel);
                ViewModel.Repository.DbConnection = DbConnection;
                ViewModel.Repository.DbTransaction = DbTransaction;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveViewModel(ViewModel<ModelBase > ViewModel)
        {
            if (ViewModels.ContainsKey(ViewModel.Name))
            {
                ViewModels.Remove(ViewModel.Name);
                return true;
            }
            else
            {
                return false;
            }
        }

        public ExecutionResult BeginTransaction(IsolationLevel IsolationLevel = IsolationLevel.ReadCommitted, string TransactionName = "")
        {
            var ER = new ExecutionResult($"{mClassName}.BeginTransaction()");

            try
            {
                mDbTransaction = mDbConnection.BeginTransaction(IsolationLevel);
            }
            catch (Exception ex)
            {
                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
                ER.Exception = ex;
            }

            return ER;
        }

        public ExecutionResult CommitTransaction()
        {
            var ER = new ExecutionResult($"{mClassName}.CommitTransaction()");

            try
            {
                mDbTransaction.Commit();
                mDbTransaction.Dispose();
            }
            catch (Exception ex)
            {
                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
                ER.Exception = ex;
                mDbTransaction?.Dispose();
            }

            return ER;
        }

        public ExecutionResult RollBackTransaction()
        {
            var ER = new ExecutionResult($"{mClassName}.RollBackTransaction()");

            try
            {
                mDbTransaction.Rollback();
                mDbTransaction.Dispose();
            }
            catch (Exception ex)
            {
                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
                ER.Exception = ex;
                mDbTransaction?.Dispose();
            }

            return ER;
        }

        /// <summary>
        /// Creates the database connection based on the provider.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>An instance of <see cref="IDbConnection"/>.</returns>
        private IDbConnection CreateDbConnection(string connectionString)
        {
            switch (Provider)
            {
                case DatabaseProvider.SqlServer:
                    return new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                case DatabaseProvider.MySql:
                    // Assumi che MySql.Data sia installato
                    // return new MySqlConnection(connectionString);
                    throw new NotImplementedException("MySQL provider not implemented. Install MySql.Data package.");
                case DatabaseProvider.PostgreSql:
                    // Assumi che Npgsql sia installato
                    // return new NpgsqlConnection(connectionString);
                    throw new NotImplementedException("PostgreSQL provider not implemented. Install Npgsql package.");
                case DatabaseProvider.Oracle:
                    // Assumi che Oracle.ManagedDataAccess sia installato
                    // return new OracleConnection(connectionString);
                    throw new NotImplementedException("Oracle provider not implemented. Install Oracle.ManagedDataAccess package.");
                case DatabaseProvider.Sqlite:
                    // Assumi che System.Data.SQLite sia installato
                    // return new SQLiteConnection(connectionString);
                    throw new NotImplementedException("SQLite provider not implemented. Install System.Data.SQLite package.");
                default:
                    throw new ArgumentException("Unsupported database provider.");
            }
        }
    }
}