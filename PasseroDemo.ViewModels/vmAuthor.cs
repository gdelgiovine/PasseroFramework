using Dapper;


namespace PasseroDemo.ViewModels
{
    public class vmAuthor : Passero.Framework.ViewModel<Models.Author>
    {
        public vmAuthor()
        {
        }
        public Models.Author GetAuthor(string au_id)
        {
            
            return this.Repository.DbConnection.Query<Models.Author>($"Select * FROM {this.Repository.GetTableName()} Where au_id=@ID", new { au_id = au_id }).Single();
        }
        public IEnumerable<Models.Author> GetAuthors()
        {

            if (this.UseModelData == Passero.Framework.UseModelData.External)
            {
                if (this.Repository.SQLQuery == "")
                {
                    this.ModelItems = this.Repository.DbConnection.Query<Models.Author>($"Select * FROM {this.Repository.GetTableName()}").ToList();
                }
                else
                {
                    this.ModelItems = this.Repository.DbConnection.Query<Models.Author>(this.Repository .SQLQuery ,this.Repository .Parameters).ToList();
                }

                this.ModelItem = this.ModelItems.FirstOrDefault();
                this.MoveFirstItem();
                this.WriteControls(); // è superfluo quando è .AutoWriteControls = true;
                return this.ModelItems;
                
            }
            else
            {
                //return this.Repository.GetItems();
                return this.GetAllItems().Value.ToList();
                
            }
        }
        public bool UpdateAuthor(Models.Author author = null) 
        {
            
            return (bool)this.UpdateItem(author).Value ;
            //return this.Repository.UpdateItem (author);
        }
    }
}