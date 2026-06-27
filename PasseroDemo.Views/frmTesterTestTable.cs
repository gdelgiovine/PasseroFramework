using Dapper;
using FastReport.Data;
using Passero.Framework;
using Passero.Framework.Controls;
using Passero.Framework.QueryEngine;
using PasseroDemo.ViewModels;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Wisej.Core;
using Wisej.Web;


namespace PasseroDemo.Views
{
    public partial class frmTesterTestTable : Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public IDbConnection DbConnection { get; set; }

        private vmTestTable vmTestTable = null;
        private vmAuthor vmAuthor = null;


        public frmTesterTestTable()
        {
            InitializeComponent();
        }

        private void Init()
        {

            this.DbConnection = ConfigurationManager.DBConnections["PasseroDemo"];
            Passero.Framework.Base.ORMType ORMType;
            ORMType = Passero.Framework.Base.ORMType.Dapper;

            vmAuthor = new vmAuthor();
            vmAuthor.Init(this.DbConnection);   

            vmTestTable = new vmTestTable();
            vmTestTable.DataBindingMode = DataBindingMode.None;
            vmTestTable.Init(this.DbConnection);




        }


        private void TestInsert()
        {


            //vmTestTable.DbConnection.Execute("DBCC CHECKIDENT ('dbo.TestTable', RESEED, 0);");
            //vmTestTable.DbConnection.Execute("ALTER TABLE PUBS.TestTable;ALTER COLUMN PK_ID;RESTART WITH 0;");

            for (int i = 0; i < 100; i++)
            {
                var item = new Models.TestTable();
                item.c_string  ="I "+ i.ToString() + " " + DateTime.Now.ToString();
                item.c_date = DateTime.Now.Date;
                //item .c_datetime = DateTime.Now;
                item.c_numeric = i;
                vmTestTable.CreateTestTable(item);
            }

        }   

        private void TestUpdate()
        {
            for (int i = 1; i <= 100; i++)
            {

                var item= vmTestTable.GetTestTable(i);
                if (item != null)
                {
                    item.c_string = "U " + i.ToString() + " " + DateTime.Now.ToString();
                    vmTestTable.UpdateTestTable(item);
                }   
                
            }
        }   

        private void TestDelete()
        {
            for (int i = 1; i <= 100; i++)
            {
                
                var item = vmTestTable.GetTestTable(i);
                if (item != null)   
                    vmTestTable.DeleteTestTable(item);
            }
        }   

        private void frmTesterTestTable_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.TestInsert();
            MessageBox.Show("End Insert");
            this.TestUpdate();
            MessageBox.Show("End Update");
            this.TestDelete();
            MessageBox.Show("End Delete");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var engine = new QueryEngine();

            var request = new SelectQueryRequest<Models.Author >();

            request.SelectColumns.Add(x => x.au_id );
            request.SelectColumns.Add(x => x.au_fname );
            request.SelectColumns.Add(x => x.au_lname );

            request.Conditions.Add(new QueryFilter<Models.Author>
            {
                Column = x => x.au_fname ,
                Operator = QueryOperator.Contains,
                Value = "Smith"
            });

            request.Conditions.Add(new QueryConditionGroup<Models.Author  >
            {
                Condition = QueryLogicalCondition.Or,
                Conditions =
                {
                    new QueryFilter<Models.Author>
                    {
                        Column = x => x.au_fname ,
                        Operator = QueryOperator.GreaterThan,
                        Value = "Gabriele"
                    }
                }
            });

            request.OrderBy.Add(new QueryOrder<Models.Author>
            {
                Column = x => x.au_id  
            });

            var compiled = engine.BuildSelect(vmAuthor, request);


            var compiled2 = engine.BuildSelect(
     vmAuthor,
     new[]
     {
        new QueryConditionGroup<Models.Author>
        {
            Condition = QueryLogicalCondition.And,
            Conditions =
            {
                new QueryFilter<Models.Author>
                {
                    Column = x => x.au_lname,
                    Operator = QueryOperator.Contains,
                    Value = "Smith"
                }
            }
        }
     },
     x => x.au_id,
     x => x.au_lname);


            string sql = compiled.Sql;
            var parameters = compiled.Parameters;

            var items = this.DbConnection.Query<Models.TestTable>(sql, parameters).ToList();
        }
    }

}
