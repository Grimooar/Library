using Kirel.Repositories.Interfaces;

namespace WebApi.Models;

public class Book : ICreatedAtTrackedEntity, IKeyEntity<int>
{
    public int Id { get; set; }
   
    public int BookInfoId { get; set; }
   
    public int PublisherId { get; set; }
    
    public int Year { get; set;  }
    
    public DateTime Created  { get; set; }
}