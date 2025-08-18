using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Passero.Framework.Base
{
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

        private IDbConnection CreateDbConnection(string connectionString)
        {
            // Puoi sostituire SqlConnection con un altro provider, se necessario
            return new System.Data.SqlClient.SqlConnection(connectionString);
        }
    }
}