using AutoMapper;
using ClassLibrary1;
using Kirel.Repositories.Interfaces;
using Kirel.Repositories.Sorts;
using Library.Models;

namespace Library.Core.Service;

public class BookInAbonementService
{
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInAbonement> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInStock> _bookInStockrepository;
    private readonly IKirelGenericEntityFrameworkRepository<int, User> _userRepository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _bookRepository;
    
    private readonly IMapper _mapper;

    public BookInAbonementService(IKirelGenericEntityFrameworkRepository<int, BookInAbonement> repository, IMapper mapper, IKirelGenericEntityFrameworkRepository<int, User> userRepository, IKirelGenericEntityFrameworkRepository<int, Book> bookRepository, IKirelGenericEntityFrameworkRepository<int, BookInStock> bookInStockrepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userRepository = userRepository;
        _bookInStockrepository = bookInStockrepository;
    }

    public async Task<IEnumerable<BookInAbonementDto>> GetAll()
    {
        // DTO <--> Entity
        var allCount = await _repository.Count();
        var bookAbonement = await _repository.GetList(null, "Created", SortDirection.Asc, 1, allCount);
        var bookAbonementDto = _mapper.Map<List<BookInAbonementDto>>(bookAbonement);
        return bookAbonementDto;
    }

    public async Task<BookInAbonementDto> GetById(int id)
    {
        var bookAbonement = await _repository.GetById(id);
        var bookAbonementDto = _mapper.Map<BookInAbonementDto>(bookAbonement);
        return bookAbonementDto;
    }

    public void Insert(BookInAbonementCreateDto bookAbonementCreateDto)
    {
        var bookAbonement = _mapper.Map<BookInAbonement>(bookAbonementCreateDto);
        _repository.Insert(bookAbonement);
    }

    public async Task<BookInAbonementDto> Update(BookInAbonementDto entity,int id)
    {
        var bookAbonement = await _repository.GetById(id);
        _mapper.Map(entity, bookAbonement);
        await _repository.Update(bookAbonement!);
        return  _mapper.Map<BookInAbonementDto>(bookAbonement);
    }

    public async Task Delete(int id)
    {
        var booksByPublisher = await _userRepository.GetList(x => x.Id == id, x => x.OrderBy(y => y.Created));
        var bookInAbonement = await _repository.GetById(id);
        
        if(bookInAbonement == null) return;
        if (booksByPublisher.Any())
        {
            throw new Exception("BookAbonement cannot be deleted as it is in use");
        }
        
        await  _repository.Delete(bookInAbonement.Id);
        /*var
         booksByAuthor = await _bookRepository.GetList(x => x.AuthorId == id, x => x.OrderBy(y => y.Created));
        var author = await _repository.GetById(id);
        if (author == null) return;
        if (booksByAuthor.Any())
        {
            throw new Exception("Author cannot be deleted as it is in use");
        }

        await _repository.Delete(author.Id);
        */
    }
    public async Task<BookInAbonementCreateDto> AbonateBook(BookInAbonementCreateDto bookAbonementCreateDto)
    {
        var bookAbonementCreate = _mapper.Map<BookInAbonement>(bookAbonementCreateDto);
        var userId = bookAbonementCreate.abonemnetId;
       
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var book = await _bookRepository.GetById(bookAbonementCreate.BookId);
        if (book == null) 
            throw new Exception("Book not found");
        bookAbonementCreate.AbonementDate = DateTime.Now;
        bookAbonementCreate.ReturnDate = bookAbonementCreate.AbonementDate.AddMonths(1);
        var bookid = book.Id;
        var bookInStock = await _bookInStockrepository.GetById(bookid);
        if (bookInStock.Amount == 0) 
            throw new Exception("Sorry no books");
        
            bookInStock.Amount--;
        await _repository.Insert(bookAbonementCreate);
        return _mapper.Map<BookInAbonementCreateDto>(bookAbonementCreate);
    }
    public async Task GiveBookBack(int abonementId,int bookId)
    {
        var abonement = await _repository.GetById(abonementId);
        var bookInStock = await _bookInStockrepository.GetById(bookId);
        if (abonement == null)
            throw new KeyNotFoundException($"Abonement with id {abonementId} was not found.");
            
        abonement.factReturnDate = DateTime.Now;
        abonement.IsReturned = true;
        bookInStock.Amount++;

        await _repository.Update(abonement);
    }
    public async Task<BookInAbonementDto> ReturnBook(int id)
    {
        var bookInStock = await _bookInStockrepository.GetById(id);
        var bookAbonement = await _repository.GetById(id);
        if (bookAbonement == null)
            throw new Exception("BookAbonement not found");
        bookAbonement.ReturnDate = DateTime.Now;
        await _repository.Update(bookAbonement);
       
        bookInStock.Amount++;
        return _mapper.Map<BookInAbonementDto>(bookAbonement);
    }
}
