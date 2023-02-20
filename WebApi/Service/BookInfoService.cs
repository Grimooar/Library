using AutoMapper;
using Kirel.Repositories.Interfaces;
using Kirel.Repositories.Sorts;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Service;

public class BookInfoService
{
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInfo> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Author> _authorRepository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _bookRepository;
    private readonly IMapper _mapper;

    public BookInfoService(IKirelGenericEntityFrameworkRepository<int, BookInfo> repository, IMapper mapper,IKirelGenericEntityFrameworkRepository<int, Author> authorRepository, IKirelGenericEntityFrameworkRepository<int, Book> bookRepository)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
        _repository = repository;
        _mapper = mapper;
      
    }

    public async Task<IEnumerable<BookInfoDto>> GetAll()
    {
        // DTO <--> Entity
        var allCount = await _repository.Count();
        var books = await _repository.GetList(null, "Created", SortDirection.Asc, 1, allCount);
        var booksDto = _mapper.Map<List<BookInfoDto>>(books);
        return booksDto;
    }
    
    public async Task<BookInfoDto> GetById(int id)
    {
        var book = await _repository.GetById(id);
        var bookDto = _mapper.Map<BookInfoDto>(book);
        return bookDto;
        
    }
    
    public async void  Insert(BookInfoCreateDto bookCreateDto)
    {
        try
        {


            var book = _mapper.Map<BookInfo>(bookCreateDto);
            var author = await _authorRepository.GetById(book.AuthorId);
            if (author == null)
            {
                throw new Exception("Author does not exist");
            }

            book.Author = author;
            await _repository.Insert(book);
        }


        catch (Exception ex)
        {
            // ignored
        }
    }

  


    public async Task<BookInfoDto> Update(BookInfoDto entity,int id)
    {
        var book = await _repository.GetById(id);
        _mapper.Map(entity, book);
        await _repository.Update(book!);
        return await _mapper.Map<Task<BookInfoDto>>(book);
    }
    
    public async Task Delete(int id)
    {
        
        var bookInfoByBooks = await _bookRepository.GetList(x => x.BookInfoId == id, x => x.OrderBy(y => y.Created));
        var bookInfo = await _repository.GetById(id);
        if (bookInfoByBooks.Any())
        {
            throw new Exception("BookInfo cannot be deleted as it is in use");
        }
        if(bookInfo == null) return;
        await  _repository.Delete(bookInfo.Id);
        
    }
    
    public async Task DeleteAll()
    {
        var allBooks = await _repository.GetList(null, "Created", SortDirection.Asc, 1, int.MaxValue);
        if(allBooks == null) return;
        
        var allBooksDtos = _mapper.Map<List<BookInfoDto>>(allBooks);
        foreach (var bookDto in allBooksDtos)
        {
            var book = _mapper.Map<BookInfo>(bookDto);
            await _repository.Delete(book.Id);
        }
    }
}