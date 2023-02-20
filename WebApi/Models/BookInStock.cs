using Kirel.Repositories.Interfaces;

namespace WebApi.Models;

public class BookInStock : ICreatedAtTrackedEntity, IKeyEntity<int>
{
    public int Id { get; set; }
    
    public int BookId { get; set; }
    
    public int Amount { get; set; }
    
    public DateTime Created { get; set; }
}