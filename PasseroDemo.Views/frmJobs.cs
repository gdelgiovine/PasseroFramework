using Passero.Framework;
using Passero.Framework.Base;
using Passero.Framework.Controls;
using Passero.Framework.Controls.Models;
using PasseroDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Wisej.Web;

namespace PasseroDemo.Views
{
    public partial class frmJobs : Form
    {

        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public Passero.Framework.ViewModel<Models.Job> vmJobs2 = new Passero.Framework.ViewModel<Models.Job>();
        //public vmTEST vmJobs = new vmTEST();
        public ViewModels.vmJobs vmJobs = new vmJobs();
        private System.Data.SqlClient.SqlConnection DbConnection;
        private string json;

        public frmJobs()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];

            vmJobs.Init(this.DbConnection);
            vmJobs.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmJobs.BindingSource = this.bsJobs;

            vmJobs.Jobs.Init(this.DbConnection);
            vmJobs.Jobs.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmJobs.Jobs.BindingSource = this.bsJobs;

            vmJobs2.Init(this.DbConnection);
            vmJobs2.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
            vmJobs2.BindingSource = this.bsJobs;

            vmJobs.AddViewModel<Models.Job>("vmJobs2",vmJobs2);

            
            ViewModel<Models.Job> vm = vmJobs.GetViewModel("vmJobs2");

            //this.dataNavigator1.ViewModels["Jobs"] = new Passero.Framework.Controls.DataNavigatorViewModel(this.vmJobs.Jobs , "Jobs");

            //this.dataNavigator1.ViewModels["Jobs"] = new Passero.Framework.Controls.DataNavigatorViewModel(vm, "Jobs");
            //this.dataNavigator1.SetActiveViewModel("Jobs");


            this.dataNavigator1.AddViewModel (this.vmJobs, "Jobs");
            this.dataNavigator1.SetActiveViewModel(this.vmJobs );


            this.dataNavigator1.ManageNavigation = true;
            this.dataNavigator1.ManageChanges = true;
            this.dataNavigator1.Init(true);
            
            queryBuilderControl1.SetViewModel(new ViewModel<Models .Job >(),this.DbConnection );
            queryBuilderControl1.QBEColumns.Add(nameof(Models.Job.job_id), "Job Id", "", "", true, true, 20);
            queryBuilderControl1.QBEColumns.Add(nameof(Models.Job.job_desc ), "Description", "", "", true, true, 20);
            //queryBuilderControl1.EnsureQueryBuilder();

            queryBuilderControl1.LoadColumnsAndRulesFromQBE();
            //queryBuilderControl1.SetColumns(CreateDemoColumns());
            //queryBuilderControl1.LoadRules(CreateInitialRules());




            //DbObject<Models.Job> DbObjectJob = new DbObject<Models.Job>(this.DbConnection);  
        }


        private static IEnumerable<QueryBuilderColumn> CreateDemoColumns()
        {
            return new[]
            {
            new QueryBuilderColumn
            {
                Field = "CustomerName",
                Label = "Cliente",
                Type = QueryGridFieldType.String ,
                SqlFieldName = "Customers.CustomerName"
            },
            new QueryBuilderColumn
            {
                Field = "City",
                Label = "Città",
                Type = QueryGridFieldType.String,
                SqlFieldName = "Customers.City"
            },
            new QueryBuilderColumn
            {
                Field = "Amount",
                Label = "Importo",
                Type = QueryGridFieldType.Number,
                SqlFieldName = "Orders.Amount"
            },
            new QueryBuilderColumn
            {
                Field = "OrderDate",
                Label = "Data ordine",
                Type = QueryGridFieldType.Date,
                SqlFieldName = "Orders.OrderDate"
            },
            new QueryBuilderColumn
            {
                Field = "IsActive",
                Label = "Attivo",
                Type = QueryGridFieldType.Boolean,
                SqlFieldName = "Customers.IsActive"
            },
            new QueryBuilderColumn
            {
                Field = "CustomerType",
                Label = "Tipo cliente",
                Type = QueryGridFieldType.Enum,
                SqlFieldName = "Customers.CustomerType",
                Values =
                {
                    new QueryBuilderLookupItem { Text = "Retail", Value = "RETAIL" },
                    new QueryBuilderLookupItem { Text = "Business", Value = "BUSINESS" },
                    new QueryBuilderLookupItem { Text = "Pubblica Amministrazione", Value = "PA" }
                }
            }
        };
        }

        private static QueryBuilderRuleSet CreateInitialRules()
        {
            return new QueryBuilderRuleSet
            {
                Condition = "and",
                Rules =
                {
                    new QueryBuilderRuleNode
                    {
                        Field = "CustomerName",
                        Label = "Cliente",
                        Type = "string",
                        Operator = "contains",
                        Value = "rossi"
                    },
                    new QueryBuilderRuleNode
                    {
                        Condition = "or",
                        Rules = new List<QueryBuilderRuleNode>
                        {
                            new QueryBuilderRuleNode
                            {
                                Field = "Amount",
                                Label = "Importo",
                                Type = "number",
                                Operator = "greaterthan",
                                Value = 100
                            },
                            new QueryBuilderRuleNode
                            {
                                Field = "CustomerType",
                                Label = "Tipo cliente",
                                Type = "enum",
                                Operator = "equal",
                                Value = "BUSINESS"
                            }
                        }
                    }
                }
            };
        }

        private void QueryBuilderControl_RulesChanged(object? sender, QueryBuilderChangedEventArgs e)
        {
            RefreshPreview();
        }

        private void ButtonGenerateSql_Click(object? sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            textBoxJson.Text = queryBuilderControl1 .GetRulesJson();

            var sql = queryBuilderControl1.GetParameterizedSqlWhere();
            var builder = new StringBuilder();

            builder.AppendLine("WHERE " + sql.WhereClause);
            builder.AppendLine();
            builder.AppendLine("PARAMETERS:");

            foreach (var parameter in sql.Parameters)
            {
                builder.AppendLine($"{parameter.Key} = {parameter.Value}");
            }

            textBoxSql.Text = builder.ToString();
        }


        public void Reload()
        {
            this.dataNavigator1.Init(true);
        }
        private void frmJobs_Load(object sender, EventArgs e)
        {
            Init();
        }

     
        private void QBEJobs()
        {
            QBEForm<Models.Job > QBE = new QBEForm<Models.Job>(this.DbConnection);

            QBE.QBEColumns.Add(nameof(Models.Job.job_id), "Job Id", "", "", true, true,  20);
            //QBE.QBEColumns.Add(nameof(Models.Author.au_id), "Author Id", "", "", true, true, 20);
            //QBE.QBEColumns.Add(nameof(Models.Author.au_fname), "First Name", "", "", true, true, 20);
            //QBE.QBEColumns.Add(nameof(Models.Author.au_lname), "Last Name", "", "", true, true, 20);
            //QBE.QBEColumns.Add(nameof(Models.Author.contract), "Have contract", "", "", true, true, Passero.Framework.Controls.QBEColumnsTypes.CheckBox, 20);

            //QBE.QBEColumns["au_id"].FontStyle = System.Drawing.FontStyle.Bold;
            ////QBE.QBEColumns["au_id"].FontSize = 10;
            ////QBE.QBEColumns[nameof(Models.Author.contract)].Aligment = DataGridViewContentAlignment.MiddleCenter ;

            //QBE.SetupQBEForm();
            //QBE.ResultGrid.Columns["au_id"].DefaultCellStyle .BackColor = System.Drawing.Color.Magenta ;

            //QBE.QBEResultMode = QBEResultMode.BoundControls;
            //QBE.QBEResultMode = QBEResultMode.SingleRowSQLQuery;
            //QBE.QBEResultMode = QBEResultMode.AllRowsItems;
            //QBE.QBEResultMode = QBEResultMode.MultipleRowsItems;
            QBE.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery;
            QBE.Owner = this;
            QBE.SetFocusControlAfterClose = this.txt_job_id ;
            //xQBEForm_Author.CallBackAction = () => { this.Reload(); };
            //QBE.SetTargetRepository(this.vmJobs.DefaultViewModel.Repository, () => { this.Reload(); });
            QBE.SetTargetRepository((Repository<Models.Job>)this.vmJobs.Repository, () => { this.Reload(); });
            //xQBEForm_Author.SetTargetRepository(this.vmAuthor.Repository);

         
            //xQBEForm_Author.QBEBoundControls.Add(nameof(Models.Author.au_id), this.txt_au_id, "text");
            QBE.QBEModelPropertiesMapping.Add(nameof(Models.Job .job_id), nameof(Models.Job.job_id));
            //xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));
            QBE.AutoLoadData = true;

            QBE.ShowQBE();
        }

        private void dataNavigator1_eMoveNextCompleted()
        {
            MessageBox.Show("a");

        }

        private void dataNavigator1_eFind()
        {
            this.QBEJobs ();

        }

        private void queryBuilderControl1_RulesChanged(object sender, QueryBuilderChangedEventArgs e)
        {
            RefreshPreview();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sql = this.queryBuilderControl1 .GetParameterizedSqlWhere();  
            var parameters = sql.Parameters;    
            var whereClause = sql.WhereClause;

            this.vmJobs .SQLQuery =this.queryBuilderControl1 .SQLQuery;
            this.vmJobs.Parameters = this.queryBuilderControl1.Parameters;
            this.vmJobs.ReloadItems();


            //var result = this.vmJobs.GetItems(this.queryBuilderControl1.SQLQuery, this.queryBuilderControl1.Parameters);
            

        }

        private void queryBuilderControl1_LoadQueryRequest(object sender, QueryBuilderRequestEventArgs e)
        {
            this.queryBuilderControl1.LoadRulesJson(json);
        }

        private void queryBuilderControl1_SaveQueryRequest(object sender, QueryBuilderRequestEventArgs e)
        {
            json = this.queryBuilderControl1.GetRulesJson();
           


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.queryBuilderControl1.ClearRules();

        }
    }
}
