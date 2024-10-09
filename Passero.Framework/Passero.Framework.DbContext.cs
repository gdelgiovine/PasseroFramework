using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Passero.Framework
{

    /// <summary>
    /// 
    /// </summary>
    public class DbContext
    {

        /// <summary>
        /// The m class name
        /// </summary>
        private const string mClassName = "Passero.Framework.DbContext";
        /// <summary>
        /// Gets or sets the m SQL connection.
        /// </summary>
        /// <value>
        /// The m SQL connection.
        /// </value>
        private SqlConnection mSqlConnection { get; set; }
        /// <summary>
        /// Gets or sets the m SQL transaction.
        /// </summary>
        /// <value>
        /// The m SQL transaction.
        /// </value>
        private SqlTransaction mSqlTransaction { get; set; }

        /// <summary>
        /// Gets or sets the view models.
        /// </summary>
        /// <value>
        /// The view models.
        /// </value>
        public Dictionary<string, ViewModel<object>> ViewModels { get; set; } = new Dictionary<string, ViewModel<object>>();

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }


        /// <summary>
        /// Initializes the specified connection string.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns></returns>
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
                mSqlConnection = new SqlConnection(this.ConnectionString);
                mSqlConnection.Open();
            }

            catch (Exception ex)
            {
                ER.ErrorCode = 1;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
            }
            return ER;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            mSqlTransaction.Dispose();
            mSqlConnection.Close();
            mSqlConnection.Dispose();
        }


        /// <summary>
        /// Gets or sets the SQL connection.
        /// </summary>
        /// <value>
        /// The SQL connection.
        /// </value>
        public SqlConnection SqlConnection
        {
            get
            {
                return mSqlConnection;
            }
            set
            {
                mSqlConnection = value;
                foreach (ViewModel<object> _ViewModel in ViewModels.Values)
                    _ViewModel.Repository.DbConnection = SqlConnection;

            }
        }

        /// <summary>
        /// Gets or sets the SQL transaction.
        /// </summary>
        /// <value>
        /// The SQL transaction.
        /// </value>
        public SqlTransaction SqlTransaction
        {
            get
            {
                return mSqlTransaction;
            }
            set
            {
                mSqlTransaction = value;
                foreach (ViewModel<object> _ViewModel in ViewModels.Values)
                    _ViewModel.Repository.DbTransaction = SqlTransaction;

            }
        }

        /// <summary>
        /// Pings the specified connection string.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns></returns>
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
                var sqlcon = new SqlConnection(ConnectionString);
                sqlcon.Open();
            }

            catch (Exception ex)
            {
                ER.ErrorCode = 2;
                ER.Exception = ex;
                ER.ResultMessage = ex.Message;
            }

            return ER;
        }

        /// <summary>
        /// Adds the view model.
        /// </summary>
        /// <param name="ViewModel">The view model.</param>
        /// <returns></returns>
        public bool AddViewModel(ViewModel<object> ViewModel)
        {
            if (ViewModels.ContainsKey(ViewModel.Name) == false)
            {
                ViewModels.Add(ViewModel.Name, ViewModel);
                ViewModel.Repository.DbConnection = SqlConnection;
                ViewModel.Repository.DbTransaction = SqlTransaction;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the view model.
        /// </summary>
        /// <param name="ViewModel">The view model.</param>
        /// <returns></returns>
        public bool RemoveViewModel(ViewModel<object> ViewModel)
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

        // Public Function BeginTransaction(ByVal TransactionName As String, Optional ByVal IsolationLevel As IsolationLevel = IsolationLevel.ReadCommitted) As ExecutionResult
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="TransactionName">Name of the transaction.</param>
        /// <returns></returns>
        public ExecutionResult BeginTransaction(string TransactionName)
        {


            var ER = new ExecutionResult($"{mClassName}.BeginTransaction()");

            try
            {
                mSqlTransaction = mSqlConnection.BeginTransaction(TransactionName);
            }

            catch (Exception ex)
            {

                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
                ER.Exception = ex;
            }

            return ER;

        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult CommitTransaction()
        {

            var ER = new ExecutionResult($"{mClassName}.CommitTransaction()");

            try
            {
                mSqlTransaction.Commit();
                mSqlTransaction.Dispose();
            }
            catch (Exception ex)
            {

                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
                ER.Exception = ex;
                if (mSqlTransaction is not null)
                {
                    mSqlTransaction.Dispose();
                }

            }

            return ER;

        }
        /// <summary>
        /// Rolls the back transaction.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult RollBackTransaction()
        {

            var ER = new ExecutionResult($"{mClassName}.RollBackTransaction()");

            try
            {
                mSqlTransaction.Rollback();
                mSqlTransaction.Dispose();
            }
            catch (Exception ex)
            {

                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
                ER.Exception = ex;
                if (mSqlTransaction is not null)
                {
                    mSqlTransaction.Dispose();
                }

            }

            return ER;

        }
    }
}