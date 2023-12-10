using Kirel.Repositories.Interfaces;

namespace Library.Models;

public class Abonement : ICreatedAtTrackedEntity, IKeyEntity<int>
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public string Name { get; set; }
    
    public int Number { get; set; }
    
    public DateTime Created { get; set; }
}