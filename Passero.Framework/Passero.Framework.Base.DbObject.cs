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
        // Public Property DbConnection As System.Data.Common.DbConnection
        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>
        public IDbConnection DbConnection { get; set; }
        // Public Property DbConnection As System.Data.SqlClient.SqlConnection
        /// <summary>
        /// Gets or sets the SQL schema query.
        /// </summary>
        /// <value>
        /// The SQL schema query.
        /// </value>
        public string SQLSchemaQuery { get; set; }
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
            this.DbConnection = DbConnection;
            SQLSchemaQuery = $"SELECT * FROM {GetTableName()} WHERE 1=0";
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
        public ExecutionResult GetSchema(string SQLSchemaQuery = "")
        {


            var ER = new ExecutionResult($"{mClassName}.Schema(Of {nameof(ModelClass)}");

            if (DbConnection is null)
            {
                ER.ErrorCode = 2;
                ER.ResultMessage = "DbConnection is null.";
                return ER;
            }

            if (string.IsNullOrEmpty(SQLSchemaQuery.Trim()))
            {
                SQLSchemaQuery = this.SQLSchemaQuery;
            }
            SqlDataAdapter DataAdapter;

            try
            {
                DataAdapter = new SqlDataAdapter(SQLSchemaQuery, (SqlConnection)DbConnection);
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
}
