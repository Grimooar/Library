using Kirel.Repositories.Interfaces;
using MessagePack;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
namespace WebApi.Models;

public class Publisher : ICreatedAtTrackedEntity, IKeyEntity<int>
{
    public int Id { get; set; }
    private string Name { get; set; }
    public DateTime Created { get; set; }
}