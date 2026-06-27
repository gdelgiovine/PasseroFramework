using Dapper;
using Passero.Framework;
using Passero.Framework.Base;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace PasseroDemo.ViewModels
{

    public class vmTestTable : Passero.Framework.ViewModel<Models.TestTable>
    {
        /// <summary>Costruttore default — repository Dapper, connessione da Init().</summary>
        public vmTestTable() { }

        /// <summary>
        /// Costruttore con DbContext: il repository (Dapper o EF) viene scelto
        /// automaticamente in base all'<see cref="ORMType"/> del contesto.
        /// </summary>
        public vmTestTable(IPasseroDbContext dbContext)
            : base(dbContext)
        {
        }

        // -------------------------
        // READ
        // -------------------------

        public Models.TestTable GetTestTable(int pk_id)
        {


            
            DynamicParameters parameters = new DynamicParameters();
            parameters .Add("@pk_id", pk_id, DbType.Int32, ParameterDirection.Input);   

            var item = this.GetItem($"SELECT * FROM {this.Repository.GetTableName()} WHERE pk_id=@pk_id", parameters ).Value;
            this.ResolvedSQLQuery();
            return item;

        }

        public List<Models.TestTable> GetTestTables()
        {
            return this.GetAllItems().Value?.ToList() ?? new List<Models.TestTable>();
        }

    

        // -------------------------
        // CREATE
        // -------------------------

        public ExecutionResult CreateTestTable(Models.TestTable item , IDbTransaction dbTransaction = null, int? dbCommandTimeout = null)
        {
            return base.InsertItem(item, dbTransaction, dbCommandTimeout);
        }

        public ExecutionResult CreateTestTables(List<Models.TestTable> items = null, IDbTransaction dbTransaction = null, int? dbCommandTimeout = null)
        {
            return base.InsertItems(items, dbTransaction, dbCommandTimeout);
        }

        // -------------------------
        // UPDATE
        // -------------------------

        public ExecutionResult UpdateTestTable(Models.TestTable item , IDbTransaction dbTransaction = null, int? dbCommandTimeout = null)
        {
            return base.UpdateItem(item, dbTransaction, dbCommandTimeout);
        }

        public ExecutionResult UpdateTestTables(IList<Models.TestTable> items , IDbTransaction dbTransaction = null, int? dbCommandTimeout = null)
        {
            return base.UpdateItems(items, dbTransaction, dbCommandTimeout);
        }

        // -------------------------
        // DELETE
        // -------------------------

        public ExecutionResult DeleteTestTable(Models.TestTable item, IDbTransaction dbTransaction = null, int? dbCommandTimeout = null)
        {
            return base.DeleteItem(item, dbTransaction, dbCommandTimeout);
        }

        public ExecutionResult DeleteTestTables(List<Models.TestTable> items , IDbTransaction dbTransaction = null, int? dbCommandTimeout = null)
        {
            return base.DeleteItems(items, dbTransaction, dbCommandTimeout);
        }
    }
}