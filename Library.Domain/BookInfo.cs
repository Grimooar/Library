using Kirel.Repositories.Interfaces;

namespace Library.Models
{
    public class BookInfo : ICreatedAtTrackedEntity, IKeyEntity<int>
    {
        

        public int Id { get; set; }

        public Author Author { get; set; }
        
        public string Name { get; set; }

        public int AuthorId { get; set; }
        
        public DateTime Created { get; set; }

        
    }
}
