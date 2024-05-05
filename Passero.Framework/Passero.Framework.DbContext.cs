using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Passero.Framework
{

    public class DbContext
    {

        private const string mClassName = "Passero.Framework.DbContext";
        private SqlConnection mSqlConnection { get; set; }
        private SqlTransaction mSqlTransaction { get; set; }

        public Dictionary<string, ViewModel<object>> ViewModels { get; set; } = new Dictionary<string, ViewModel<object>>();

        public string ConnectionString { get; set; }


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

        public void Dispose()
        {
            mSqlTransaction.Dispose();
            mSqlConnection.Close();
            mSqlConnection.Dispose();
        }


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
                    _ViewModel.Repository.SqlTransaction = SqlTransaction;

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

        public bool AddViewModel(ViewModel<object> ViewModel)
        {
            if (ViewModels.ContainsKey(ViewModel.Name) == false)
            {
                ViewModels.Add(ViewModel.Name, ViewModel);
                ViewModel.Repository.DbConnection = SqlConnection;
                ViewModel.Repository.SqlTransaction = SqlTransaction;
                return true;
            }
            else
            {
                return false;
            }
        }

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