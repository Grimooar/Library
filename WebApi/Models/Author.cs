using Kirel.Repositories.Interfaces;

namespace WebApi.Models;

public class Author : ICreatedAtTrackedEntity, IKeyEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
}