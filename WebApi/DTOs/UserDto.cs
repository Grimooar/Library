namespace WebApi.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string UserNames { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Created { get; set; }
    public string Password { get; set; }

}