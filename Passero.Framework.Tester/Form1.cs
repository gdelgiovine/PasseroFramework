using PasseroDemo.Models;
using System.Data.SqlClient;

namespace Passero.Framework.Tester
{
    public partial class Form1 : Form
    {
        private System.Data.SqlClient.SqlConnection SqlConnection;
        private string cs = "Data Source=(local);Initial Catalog=Pubs;Integrated Security=SSPI;MultipleActiveResultSets=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mAuthor = new PasseroDemo.Models.Author (); 
            var rAuthor = new Repository<Author>(SqlConnection);
            var DbObject = new DbObject<Author>(SqlConnection);
            mAuthor = rAuthor.GetItem("SELECT * FROM AUTHORS WHERE AU_ID=@AU_ID", new { AU_ID = "172-32-1176"}).Value ;
            DbObject.GetSchema();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection = new System.Data.SqlClient.SqlConnection(cs);
            SqlConnection.Open();
        }
    }
}