using Dapper;
using Passero.Framework.Base;


namespace PasseroDemo.ViewModels
{
    public class vmAuthor : Passero.Framework.ViewModel<Models.Author>
    {
        /// <summary>Costruttore default — repository Dapper, connessione da Init().</summary>
        public vmAuthor() { }

        /// <summary>
        /// Costruttore con DbContext: il repository (Dapper o EF) viene scelto
        /// automaticamente in base all'<see cref="ORMType"/> del contesto.
        /// </summary>
        public vmAuthor(IPasseroDbContext dbContext)
            : base(dbContext) { }

        public Models.Author GetAuthor(string au_id)
        {
            return this.Repository.DbConnection.Query<Models.Author>(
                $"Select * FROM {this.Repository.GetTableName()} Where au_id=@au_id",
                new { au_id }).Single();
        }

        public IEnumerable<Models.Author> GetAuthors()
        {
            if (this.UseModelData == Passero.Framework.UseModelData.External)
            {
                if (this.Repository.SQLQuery == "")
                {
                    this.ModelItems = this.Repository.DbConnection
                        .Query<Models.Author>($"Select * FROM {this.Repository.GetTableName()}").ToList();
                }
                else
                {
                    this.ModelItems = this.Repository.DbConnection
                        .Query<Models.Author>(this.Repository.SQLQuery, this.Repository.Parameters).ToList();
                }

                this.ModelItem = this.ModelItems.FirstOrDefault();
                this.MoveFirstItem();
                this.WriteControls();
                return this.ModelItems;
            }
            else
            {
                return this.GetAllItems().Value.ToList();
            }
        }

        public bool UpdateAuthor(Models.Author author = null)
        {
            return (bool)this.UpdateItem(author).Value;
        }
    }
}