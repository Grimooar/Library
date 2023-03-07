using Kirel.Repositories.Interfaces;

namespace WebApi.Models;

public class Publisher : ICreatedAtTrackedEntity, IKeyEntity<int>
{
    public int Id { get; set; }
    private string Name { get; set; }
    public DateTime Created { get; set; }
}