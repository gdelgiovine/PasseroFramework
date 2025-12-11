using Microsoft .Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Passero.Framework.Base
{


    // <Serializable>
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ModelClass">The type of the Model class.</typeparam>
    public class DbObject<ModelClass> where ModelClass : class
    {

        /// <summary>
        /// The m class name
        /// </summary>
        private const string mClassName = "Passero.Framework.Base.DbObject";
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the name e.
        /// </summary>
        /// <value>
        /// The name e.
        /// </value>
        public string NameE { get; set; }
        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the type of the database object.
        /// </summary>
        /// <value>
        /// The type of the database object.
        /// </value>
        public DbObjectType DbObjectType { get; set; } = DbObjectType.Table;
        /// <summary>
        /// Gets or sets the schema.
        /// </summary>
        /// <value>
        /// The schema.
        /// </value>
        public string Schema { get; set; }
        /// <summary>
        /// Gets or sets the database columns.
        /// </summary>
        /// <value>
        /// The database columns.
        /// </value>
        public Dictionary<string, DbColumn> DbColumns { get; set; } = new Dictionary<string, DbColumn>(StringComparer.InvariantCultureIgnoreCase);
        /// <summary>
        /// Gets or sets the primary keys.
        /// </summary>
        /// <value>
        /// The primary keys.
        /// </value>
        public DataColumn[] PrimaryKeys { get; set; }

        private string mProviderName = "";
        public string ProviderName
        {
            get
            { 
                return this.mProviderName;
            }
        }

        private IDbConnection mDbConnection;
        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>
        public IDbConnection DbConnection
        {
            get
            {
                return mDbConnection;
            }
            set
            {
                mDbConnection = value;
            }
        }
        // Public Property DbConnection As System.Data.SqlClient.SqlConnection
        /// <summary>
        /// Gets or sets the SQL schema query.
        /// </summary>
        /// <value>
        /// The SQL schema query.
        /// </value>
        public string SQLSchemaQuery { get; set; } 

        public SqlCommands SqlCommands { get; set; } = new SqlCommands();

        /// <summary>
        /// The model
        /// </summary>
        private ModelClass Model;
        /// <summary>
        /// Initializes a new instance of the <see cref="DbObject{ModelClass}"/> class.
        /// </summary>
        /// <param name="DbConnection">The database connection.</param>
        public DbObject(IDbConnection DbConnection)
        {
            Model = (ModelClass)Activator.CreateInstance(typeof(ModelClass));
            this.mDbConnection = DbConnection;
            this.SQLSchemaQuery = $"SELECT * FROM {Passero.Framework.DapperHelper .Utilities .GetTableName(Model)} WHERE 1=0";
            this.GetSchema();
        }
        /// <summary>
        /// Assigns the value.
        /// </summary>
        /// <param name="Obj">The object.</param>
        /// <param name="Value">The value.</param>
        private void AssignValue(ref object Obj, object Value)
        {

            if (!ReferenceEquals(Value, DBNull.Value))
            {
                Obj = Value;
            }
        }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <param name="SQLSchemaQuery">The SQL schema query.</param>
        /// <returns></returns>
        /// 
        public ExecutionResult GetSchema(string SQLSchemaQuery = "")
        {
            var ER = new ExecutionResult($"{mClassName}.Schema(Of {nameof(ModelClass)}");

            if (this.mDbConnection is null)
            {
                ER.ErrorCode = 2;
                ER.ResultMessage = "DbConnection is null.";
                return ER;
            }

            if (string.IsNullOrEmpty(SQLSchemaQuery.Trim()))
            {
                SQLSchemaQuery = this.SQLSchemaQuery;
            }

            System.Data.Common.DbDataAdapter DataAdapter;

            try
            {
         
                var dbConnection = this.mDbConnection as System.Data.Common.DbConnection;
                if (dbConnection is null)
                {
                    ER.ErrorCode = 3;
                    ER.ResultMessage = "DbConnection must be a DbConnection type.";
                    return ER;
                }

                var providerFactory = System.Data.Common.DbProviderFactories.GetFactory(dbConnection);
                this.mProviderName = providerFactory.GetType().Namespace;

              
                DataAdapter = providerFactory.CreateDataAdapter();
                var command = providerFactory.CreateCommand();
                command.CommandText = SQLSchemaQuery;
                command.Connection = dbConnection;
                DataAdapter.SelectCommand = command;

                DataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                var SchemaColumns = new DataTable();
                DataAdapter.FillSchema(SchemaColumns, SchemaType.Mapped);
                PrimaryKeys = SchemaColumns.PrimaryKey;
                Name = SchemaColumns.TableName;
                DbColumns.Clear();
                foreach (DataColumn DataColumn in SchemaColumns.Columns)
                {
                    var DbColumn = new DbColumn();
                    DbColumn.ColumnName = DataColumn.ColumnName;
                    DbColumn.IsKey = SchemaColumns.PrimaryKey.Contains(DataColumn);
                    DbColumn.DataColumn = DataColumn;
                    DbColumns[DbColumn.ColumnName] = DbColumn;
                }
                GetSqlCommands();



            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
            }

            return ER;
        }

        /// <summary>
        /// Gets the SQL INSERT, UPDATE, DELETE commands using DbCommandBuilder.
        /// </summary>
        /// <returns>ExecutionResult with generated commands.</returns>
        public ExecutionResult<SqlCommands>  GetSqlCommands()
        {
            var ER = new ExecutionResult<SqlCommands>($"{mClassName}.GetSqlCommands()");
            ER.Value = new SqlCommands();   
            if (mDbConnection is null)
            {
                ER.ErrorCode = 2;
                ER.ResultMessage = "DbConnection is null.";
                ER.ResultCode = ExecutionResultCodes.Failed;
                return ER;
            }

            try
            {
                var dbConnection = mDbConnection as System.Data.Common.DbConnection;
                if (dbConnection is null)
                {
                    ER.ErrorCode = 3;
                    ER.ResultMessage = "DbConnection must be a DbConnection type.";
                    ER.ResultCode = ExecutionResultCodes.Failed;
                    return ER;
                }

                var providerFactory = System.Data.Common.DbProviderFactories.GetFactory(dbConnection);

                // Crea DataAdapter e CommandBuilder
                var dataAdapter = providerFactory.CreateDataAdapter();
                var selectCommand = providerFactory.CreateCommand();
                selectCommand.CommandText = SQLSchemaQuery;
                selectCommand.Connection = dbConnection;
                dataAdapter.SelectCommand = selectCommand;

                // Usa CommandBuilder per generare automaticamente i comandi
                var commandBuilder = providerFactory.CreateCommandBuilder();
                commandBuilder.DataAdapter = dataAdapter;

                // Recupera i comandi generati automaticamente
                ER.Value.InsertCommand = commandBuilder.GetInsertCommand().CommandText;
                ER.Value.UpdateCommand = commandBuilder.GetUpdateCommand().CommandText;
                ER.Value.DeleteCommand = commandBuilder.GetDeleteCommand().CommandText;
                ER.Value.SelectCommand = SQLSchemaQuery;
                this.SqlCommands = ER.Value;    
                ER.ResultCode = ExecutionResultCodes.Success ;
            }
            catch (Exception ex)
            {
                ER.Exception = ex;
                ER.ErrorCode = 1;
                ER.ResultMessage = ex.Message;
                ER.ResultCode = ExecutionResultCodes.Failed;
            }

            return ER;
        }

        /// <summary>
        /// Gets the SQL INSERT command using DbCommandBuilder.
        /// </summary>
        /// <returns>The INSERT SQL command.</returns>
        public string GetInsertCommand()
        {
            var result = GetSqlCommands();
            return result.ResultCode == ExecutionResultCodes.Success  ? result.Value.InsertCommand : string.Empty;
        }

        /// <summary>
        /// Gets the SQL UPDATE command using DbCommandBuilder.
        /// </summary>
        /// <returns>The UPDATE SQL command.</returns>
        public string GetUpdateCommand()
        {
            var result = GetSqlCommands();
            return result.ResultCode == ExecutionResultCodes.Success ? result.Value.UpdateCommand : string.Empty;
        }

        /// <summary>
        /// Gets the SQL DELETE command using DbCommandBuilder.
        /// </summary>
        /// <returns>The DELETE SQL command.</returns>
        public string GetDeleteCommand()
        {
            var result = GetSqlCommands();
            return result.ResultCode == ExecutionResultCodes.Success ? result.Value.DeleteCommand : string.Empty;
        }

        //public ExecutionResult GetSchemaSQLClient(string SQLSchemaQuery = "")
        //{


        //    var ER = new ExecutionResult($"{mClassName}.Schema(Of {nameof(ModelClass)}");

        //    if (this.mDbConnection is null)
        //    {
        //        ER.ErrorCode = 2;
        //        ER.ResultMessage = "DbConnection is null.";
        //        return ER;
        //    }

        //    if (string.IsNullOrEmpty(SQLSchemaQuery.Trim()))
        //    {
        //        SQLSchemaQuery = this.SQLSchemaQuery;
        //    }

        //    SqlDataAdapter DataAdapter;

        //    try
        //    {
        //        DataAdapter = new SqlDataAdapter(SQLSchemaQuery, (SqlConnection)this.mDbConnection);
        //        DataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
        //        var SchemaColumns = new DataTable();
        //        DataAdapter.FillSchema(SchemaColumns, SchemaType.Mapped);
        //        PrimaryKeys = SchemaColumns.PrimaryKey;
        //        Name = SchemaColumns.TableName;
        //        DbColumns.Clear();
        //        foreach (DataColumn DataColumn in SchemaColumns.Columns)
        //        {
        //            var DbColumn = new DbColumn();
        //            DbColumn.ColumnName = DataColumn.ColumnName;
        //            DbColumn.IsKey = SchemaColumns.PrimaryKey.Contains(DataColumn);
        //            DbColumn.DataColumn = DataColumn;
        //            DbColumns[DbColumn.ColumnName] = DbColumn;
        //        }
        //    }


        //    catch (Exception ex)
        //    {
        //        ER.Exception = ex;
        //        ER.ErrorCode = 1;
        //        ER.ResultMessage = ex.Message;

        //    }

        //    return ER;
        //}

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <returns></returns>
        private string GetTableName()
        {
            string tableName = "";
            var type = typeof(ModelClass);
            var tableAttr = type.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();

            if (tableAttr is not null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name; // & "s"
        }

    }


    /// <summary>
    /// Container for SQL commands (INSERT, UPDATE, DELETE, SELECT).
    /// </summary>
    public class SqlCommands
    {
        /// <summary>
        /// Gets or sets the SELECT command.
        /// </summary>
        public string SelectCommand { get; set; }

        /// <summary>
        /// Gets or sets the INSERT command.
        /// </summary>
        public string InsertCommand { get; set; }

        /// <summary>
        /// Gets or sets the UPDATE command.
        /// </summary>
        public string UpdateCommand { get; set; }

        /// <summary>
        /// Gets or sets the DELETE command.
        /// </summary>
        public string DeleteCommand { get; set; }
    }
}
