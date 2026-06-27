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


        public string GetAuthorFullName(string au_id)
        {


            if (string.IsNullOrWhiteSpace(au_id))
            {
                return string.Empty;
            }

          var author = this.Repository.DbConnection
                .Query<Models.Author>(
                    $"SELECT * FROM {this.Repository.GetTableName()} WHERE au_id=@au_id",
                    new { au_id = au_id })
                .FirstOrDefault();

            return author?.au_fullname ?? string.Empty;

            //Models .Author author = this.DbConnection.Query<Models.Author>("SELECT * FROM Authors WHERE au_id=@au_id", new { au_id = au_id }).First();
            ////return $"{author.au_fname.Trim()} {author.au_lname.Trim()}".Trim();
            //return author.au_fullname;
        }

        public IEnumerable<Models.Author> GetAuthors()
        {

            return this.GetAllItems().Value.ToList();

          
        }

        public bool UpdateAuthor(Models.Author author = null)
        {
            return (bool)this.UpdateItem(author).Value;
        }
    }
}