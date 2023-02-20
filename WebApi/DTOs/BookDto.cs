namespace WebApi.DTOs;

public class BookDto
{
    public int Id { get; set; }
   
    public int BookInfoId { get; set; }
   
    public int PublisherId { get; set; }
    
    public int Year { get; set;  }
    
    public DateTime Created  { get; set; }
}