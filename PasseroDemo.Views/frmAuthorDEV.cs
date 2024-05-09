using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using Dapper;
using Passero.Framework;
using Passero.Framework;
using Passero.Framework.Controls;
using Passero.Framework.SSRSReports;
using Wisej.Web;


namespace PasseroDemo.Views
{
    public partial class frmAuthorDEV : Form
    {
        public Passero.Framework.ConfigurationManager ConfigurationManager = new Passero.Framework.ConfigurationManager();
        public IDbConnection DbConnection { get; set; }
        // Cosi di usa il viewmodel esteso
        //public PasseroDemo.ViewModels.Author vmAuthor = new ViewModels.Author();
        
        // cosi quello base
        public ViewModel <Models .Author> vmAuthor =  new ViewModel <Models .Author> ();

        QBEForm<Models.Author> xQBEForm_Author = new QBEForm<Models.Author>();
        QBEReport xQBEReport = new  QBEReport ();
        Passero.Framework.DbLookUp<Models.Author> dblAuthor = new DbLookUp<Models.Author>();

        public frmAuthorDEV()
        {
            InitializeComponent();
            
           
            
        }
        public void Init()
        {
            this.DbConnection = (System.Data.SqlClient.SqlConnection)ConfigurationManager.DBConnections["PasseroDemo"];
            vmAuthor.Init(this.DbConnection );
            vmAuthor.DataBindControlsAutoSetMaxLenght = true;
            vmAuthor.AutoWriteControls= true;
            vmAuthor.AutoReadControls = true;
            vmAuthor.AutoFitColumnsLenght = true;
            //vmAuthor.UseModelData = Passero.Framework.Base.UseModelData.InternalRepository  ;

            vmAuthor.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource   ;
            vmAuthor.BindingSource = this.bsAuthors;


            //vmAuthor.AddControl(this.txt_au_id, "Text", nameof(Models.Author.au_id), Passero.Framework.Base.BindingBehaviour.SelectInsertUpdate );
            //vmAuthor.AddControl(this.txt_au_fname , "Text", "au_fname");
            //vmAuthor.AddControl(this.txt_au_lname, "Text", "au_lname");
            //this.vmAuthor.CreatePasseroBindingFromBindingSource(this);

          
            this.comboBox1.ValueMember = "au_id";
            this.comboBox1.DisplayMember = "au_fullname";

        
            dblAuthor.DbConnection = this.DbConnection;


            dblAuthor.SQLQuery = "SELECT * FROM Authors Where au_id=@au_id";
            dblAuthor.DataBindingMode = DataBindingMode.Passero   ;
            //dblAuthor.BindingSource = this.bindingSource1;
           
            dblAuthor.AddControl(this.textBox1, "text", nameof(dblAuthor.Model.address));
            dblAuthor.AddControl(this.textBox3, "text", nameof(dblAuthor.Model.city ));

            
            // Questo Metodo sta nel ViewModel Customizzato
            //this.vmAuthor.GetAuthors();
            // Questo Metodo invece sta nella classe base ViewModel
            this.vmAuthor.GetAllItems();
            this.comboBox1 .DataSource = Passero.Framework.Utilities.Clone(this.vmAuthor.ModelItems);
            //this.dbLookUpTextBox1.DbConnection = this.DbConnection;
            //this.dbLookUpTextBox1.ModelClass = typeof(Models.Author);
            //this.dbLookUpTextBox1.DisplayMember = "au_fullname";
            //this.dbLookUpTextBox1.ValueMember = "au_id";
            //this.dbLookUpTextBox1.SelectClause = "SELECT *, TRIM(au_fname)+' '+TRIM(au_lname) as au_fullname";

            this.dataNavigator1.ViewModels["Author"] = new DataNavigatorViewModel(this.vmAuthor, "Author");
            this.dataNavigator1.SetActiveViewModel("Author");
          
            
        }

        public void Reload()
        {
            this.vmAuthor.GetAllItems();

        }
        private void frmAuthor_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void dataNavigator1_eMoveFirst()
        {
            this.vmAuthor.MoveFirstItem();
                       
            
        }

        private void dataNavigator1_eMoveNext()
        {
            this.vmAuthor.MoveNextItem();
            

        }

        private void dataNavigator1_eMovePrevious()
        {
            this.vmAuthor .MovePreviousItem ();
            //this.vmAuthor.WriteControls();
        }

        private void dataNavigator1_eMoveLast()
        {
            this.vmAuthor.MoveLastItem();
            //this.vmAuthor.WriteControls();
        }

        private void dataNavigator1_eSave()
        {
            if (this.vmAuthor.AddNewState)
            {
                this.vmAuthor.InsertItem();
            }
            else
            {
                //this.vmAuthor.UpdateAuthor();
                this.vmAuthor.UpdateItem();
            }
        }

        private void dataNavigator1_eAddNew()
        {
            this.vmAuthor.AddNew();
        }

        private void dataNavigator1_eUndo()
        {
            this.vmAuthor.UndoChanges();
        }
     
        private void dataNavigator1_Click(object sender, EventArgs e)
        {
            int x = 0;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.vmAuthor.BindingSource.MovePrevious();
            //Action action = () => { this.Reload(); };
            //action.Invoke();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.vmAuthor .BindingSource .MoveNext();
           
        }

        private void dataNavigator1_eDelete()
        {
            this.vmAuthor.DeleteItem();
        }

        private void bsAuthors_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataNavigator1_eFind()
        {
            
            xQBEForm_Author = new QBEForm<Models.Author>(this.DbConnection);

            xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_id), "Author Id","","",true,true,20);
            xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_fname ), "First Name", "", "", true, true,20);
            xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.au_lname), "Last Name", "", "", true, true,20);
            xQBEForm_Author.QBEColumns.Add(nameof(Models.Author.contract ), "Have contract", "", "", true, true,Passero.Framework.Controls.QBEColumnsTypes.CheckBox , 20);
            //xQBEForm_Author.QBEColumns["au_id"].ForeColor = System.Drawing.Color.Red;
            xQBEForm_Author.QBEColumns["au_id"].FontStyle = System.Drawing.FontStyle.Bold;
            //xQBEForm_Author.QBEColumns["au_id"].FontSize = 10;
            //xQBEForm_Author.QBEColumns[nameof(Models.Author.contract)].Aligment = DataGridViewContentAlignment.MiddleCenter ;

            xQBEForm_Author.SetupQBEForm();
//            xQBEForm_Author .ResultGrid.Columns["au_id"].DefaultCellStyle .BackColor = System.Drawing.Color.Magenta ;

            //xQBEForm_Author.QBEResultMode = QBEResultMode.BoundControls;
            xQBEForm_Author.QBEResultMode = QBEResultMode.SingleRowSQLQuery;
            //xQBEForm_Author.QBEResultMode = QBEResultMode.AllRowsItems;
            //xQBEForm_Author.QBEResultMode = QBEResultMode.MultipleRowsItems;
            //xQBEForm_Author.Owner = this;
            xQBEForm_Author.SetFocusControlAfterClose = this.txt_au_id;
            xQBEForm_Author.CallBackAction = () => { this.Reload(); };
            
            //xQBEForm_Author.SetTargetRepository(this.vmAuthor.Repository,() => { this.Reload(); });
            xQBEForm_Author.SetTargetRepository(this.vmAuthor.Repository);
            

            xQBEForm_Author.QBEBoundControls.Add(nameof(Models .Author .au_id ), this.txt_au_id, "text");
            xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_id), nameof(Models.Author.au_id));
            //xQBEForm_Author.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_fname ), nameof(Models.Author.au_fname ));

           
            xQBEForm_Author.ShowQBE();
         
        }

        private void dataNavigator1_eRefresh()
        {
            //this.vmAuthor.GetAuthors();
            this.vmAuthor.GetAllItems();
        }

        private void dataNavigator1_ePrint()
        {
            xQBEReport = new QBEReport();
            //xQBEReport.ReportRenderRequest -= XQBEReport_ReportRenderRequest;
            //xQBEReport.ReportRenderRequest += XQBEReport_ReportRenderRequest;

            xQBEReport.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.RDL", "REPORT UNO");
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet1", this.vmAuthor.Repository.DbConnection);
            xQBEReport.QBEReports["REPORT1"].AddDataSet<Models.Author>("DataSet2", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].SQLQuery="SELECT * FROM Authors Where au_id=@au_id";
            //xQBEReport.QBEReports["REPORT1"].DataSources["DataSet2"].Parameters.Add("@au_id", "1123");
            
            
            xQBEReport.QBEReports.Add("REPORT2", @"C:\Reports\REPORT1.RDL", "REPORT DUE");
            xQBEReport.QBEReports["REPORT2"].AddDataSet<Models.Author>("DataSet1", this.vmAuthor.Repository.DbConnection, "SELECT * FROM Authors");

            xQBEReport.DefaultReport = xQBEReport.QBEReports["REPORT1"];
            
            xQBEReport.QBEColumns.AddForReport("REPORT1",nameof(Models.Author.au_id), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1",nameof(Models.Author.au_fname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_lname), "", "");
            xQBEReport.QBEColumns.AddForReport("REPORT2", nameof(Models.Author.au_id), "CODICE","");
            
            xQBEReport.SetFocusControlAfterClose = this.txt_au_id;
            xQBEReport.CallBackAction = () => { this.Reload(); };
            xQBEReport.ShowQBEReport();
        }

        private void XQBEReport_ReportRenderRequest(object sender, EventArgs e)
        {
            ReportRenderRequestEventArgs _e = (ReportRenderRequestEventArgs)e;

            _e.DataSets["DataSet1"].Data = this.vmAuthor.Repository.ModelItems;

            //_e.Cancel = true;   
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {


            //dblAuthor.DbParameters = new { au_id = this.textBox2.Text }; //parameters;    
            //dblAuthor.Lookup();


        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            //dblAuthor.DbParameters = new { au_id = this.txt_au_id .Text }; //parameters;    
            //this.dblAuthor .Lookup();
        }

        private void txt_au_id_TextChanged(object sender, EventArgs e)
        {
            //dblAuthor.SQLQuery = "SELECT * FROM Authors Where au_id=@au_id";
            //dblAuthor.DbParameters = new DynamicParameters(new { au_id = this.txt_au_id.Text }); 
            ////dblAuthor.DbParameters.AddDynamicParams(new {au_id=this.txt_au_id.Text});
            //this.dblAuthor.Lookup();

        }

        private void txt_au_id_Validated(object sender, EventArgs e)
        {
            //dblAuthor.DbParameters = new { au_id = this.txt_au_id.Text }; //parameters;    
            //this.dblAuthor.Lookup();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //this.dbLookUpTextBox1 .DbParameters = new DynamicParameters ( new { au_id = this.txt_au_id.Text } );
            //this.dbLookUpTextBox1.LookUp();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Models.Author author = (Models.Author)this.comboBox1.SelectedItem;
            author.address = "STO CAZZO";
        }
    }
}
