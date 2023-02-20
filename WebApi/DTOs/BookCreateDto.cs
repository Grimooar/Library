namespace WebApi.DTOs;

public class BookCreateDto
{
    public int BookInfoId { get; set; }
   
    public int PublisherId { get; set; }
    
    public int Year { get; set;  }
}