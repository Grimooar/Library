namespace ClassLibrary1;

public class BookInAbonementDto
{
    public int Id { get; set; }
    public int abonemnetId { get; set; }
    public int BookId { get; set; }
    public DateTime AbonementDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public DateTime Created { get; set; }
    public bool IsReturned { get; set; }
    public DateTime factReturnDate { get; set; }
}