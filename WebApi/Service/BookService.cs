using AutoMapper;
using Kirel.Repositories.Interfaces;
using Kirel.Repositories.Sorts;
using Microsoft.EntityFrameworkCore;
using WebApi.DbContext;
using WebApi.DTOs;
using WebApi.Models;


namespace WebApi.Service;

public class BookService
{
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInfo> _bookInfoRepository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Publisher> _publisherRepository;
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInStock> _bookInStockRepository;
    
    private readonly IMapper _mapper;

    public BookService(IKirelGenericEntityFrameworkRepository<int, Book> repository, IMapper mapper, IKirelGenericEntityFrameworkRepository<int, Publisher> publisherRepository, IKirelGenericEntityFrameworkRepository<int, BookInfo> bookInfoRepository, IKirelGenericEntityFrameworkRepository<int, BookInStock> bookInStockRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _publisherRepository = publisherRepository;
        _bookInfoRepository = bookInfoRepository;
        _bookInStockRepository = bookInStockRepository;
    }

    public async Task<IEnumerable<BookDto>> GetAll()
    {
        // DTO <--> Entity
        var allCount = await _repository.Count();
        var books = await _repository.GetList(null, "Created", SortDirection.Asc, 1, allCount);
        var booksDto = _mapper.Map<List<BookDto>>(books);
        return booksDto;
    }
    
    public async Task<BookDto> GetById(int id)
    {
        var book = await _repository.GetById(id);
        var bookDto = _mapper.Map<BookDto>(book);
        return bookDto;
    }
    
    public async Task Insert(BookCreateDto bookCreateDto)
    {
        try
        {
            var book = _mapper.Map<Book>(bookCreateDto);
            var publisher = await _publisherRepository.GetById(book.PublisherId);
            var bookInfo = await _bookInfoRepository.GetById(book.BookInfoId);
            if (publisher == null || bookInfo == null)
            {
                throw new Exception("Something does not exist");
            }

            await _repository.Insert(book);
        }
        catch (Exception ex)
        {
            // ignored
        }
    }
    
    public async Task<BookDto> Update(BookDto entity,int id)
    {
        var book = await _repository.GetById(id);
        _mapper.Map(entity, book);
        await _repository.Update(book!);
        return await _mapper.Map<Task<BookDto>>(book);
    }
    
    public async Task Delete(int id)
    {
        var book = await _repository.GetById(id);
        var bookinBooksInStock =
            await _bookInStockRepository.GetList(x => x.BookId == id, x => x.OrderBy(y => y.Created));
        if(book == null) return;
        if (bookinBooksInStock.Any())
        {
            throw new Exception("Book cannot be deleted as it is in use ");
        }
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
        
        await  _repository.Delete(book.Id);
    }
    
    public async Task DeleteAll()
    {
        var allBooks = await _repository.GetList(null, "Created", SortDirection.Asc, 1, int.MaxValue);
        if(allBooks == null) return;
        
        var allBooksDtos = _mapper.Map<List<BookDto>>(allBooks);
        foreach (var bookDto in allBooksDtos)
        {
            var book = _mapper.Map<Models.BookInfo>(bookDto);
            await _repository.Delete(book.Id);
        }
    }
}