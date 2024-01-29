namespace ClassLibrary1;

public class BookInStockDto
{
    public int Id { get; set; }
    
    public int BookId { get; set; }
    
    public int Amount { get; set; }
    
    public DateTime Created  { get; set; }
}