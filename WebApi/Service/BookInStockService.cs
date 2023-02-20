using AutoMapper;
using Kirel.Repositories.Interfaces;
using Kirel.Repositories.Sorts;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Service;

public class BookInStockService
{
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInStock> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _bookRepository;
    private readonly IMapper _mapper;

    public BookInStockService(IKirelGenericEntityFrameworkRepository<int, BookInStock> repository, IMapper mapper, IKirelGenericEntityFrameworkRepository<int, Book> bookRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookInStockDto>> GetAll()
    {
        // DTO <--> Entity
        var allCount = await _repository.Count();
        var books = await _repository.GetList(null, "Created", SortDirection.Asc, 1, allCount);
        var booksDto = _mapper.Map<List<BookInStockDto>>(books);
        return booksDto;
    }
    
    public async Task<BookInStockDto> GetById(int id)
    {
        var book = await _repository.GetById(id);
        var bookDto = _mapper.Map<BookInStockDto>(book);
        return bookDto;
    }
    
    public async void Insert(BookInStockCreateDto bookCreateDto)
    {
        try
        {



            var bookInStock = _mapper.Map<Models.BookInStock>(bookCreateDto);
            var book = await _bookRepository.GetById(bookInStock.BookId);
            
            if (book == null)
            {
                throw new Exception("Book does not exist");
            }

           await _repository.Insert(bookInStock);
        }
        catch (Exception ex)
        {
            //ignored
        }
    }
    
    public async Task<BookInStockDto> Update(BookInStockDto entity,int id)
    {
        var book = await _repository.GetById(id);
        _mapper.Map(entity, book);
        await _repository.Update(book!);
        return await _mapper.Map<Task<BookInStockDto>>(book);
    }
    
    public async Task Delete(int id)
    {
        var book = await _repository.GetById(id);
        if(book == null) return;
        
        
        await  _repository.Delete(book.Id);
    }
    
    public async Task DeleteAll()
    {
        var allBooks = await _repository.GetList(null, "Created", SortDirection.Asc, 1, int.MaxValue);
        if(allBooks == null) return;
        
        var allBooksDtos = _mapper.Map<List<BookInStockDto>>(allBooks);
        foreach (var bookDto in allBooksDtos)
        {
            var book = _mapper.Map<BookInStock>(bookDto);
            await _repository.Delete(book.Id);
        }
    }
}