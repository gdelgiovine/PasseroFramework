using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{


    // <Serializable>
    public class DbObject<ModelClass> where ModelClass : class
    {

        private const string mClassName = "Passero.Framework.Base.DbObject";
        public string Name { get; set; }
        public string NameE { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public DbObjectType DbObjectType { get; set; } = DbObjectType.Table;
        public string Schema { get; set; }
        public Dictionary<string, DbColumn> DbColumns { get; set; } = new Dictionary<string, DbColumn>(StringComparer.InvariantCultureIgnoreCase);
        public DataColumn[] PrimaryKeys { get; set; }
        // Public Property DbConnection As System.Data.Common.DbConnection
        public IDbConnection DbConnection { get; set; }
        // Public Property DbConnection As System.Data.SqlClient.SqlConnection
        public string SQLSchemaQuery { get; set; }
        private ModelClass Model;
        public DbObject(IDbConnection DbConnection)
        {
            Model = (ModelClass)Activator.CreateInstance(typeof(ModelClass));
            this.DbConnection = DbConnection;
            this.SQLSchemaQuery = $"SELECT * FROM {GetTableName()} WHERE 1=0";
        }
        private void AssignValue(ref object Obj, object Value)
        {

            if (!ReferenceEquals(Value, DBNull.Value))
            {
                Obj = Value;
            }
        }

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
