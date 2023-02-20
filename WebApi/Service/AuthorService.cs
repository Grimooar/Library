using AutoMapper;
using Kirel.Repositories.Interfaces;
using Kirel.Repositories.Sorts;
using Microsoft.EntityFrameworkCore;
using WebApi.DbContext;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Service;

public class AuthorService
{
    private readonly IKirelGenericEntityFrameworkRepository<int, Author> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInfo> _bookRepository;
    private readonly IMapper _mapper;

    public AuthorService(IKirelGenericEntityFrameworkRepository<int, Author> repository, IMapper mapper, IKirelGenericEntityFrameworkRepository<int, BookInfo> bookRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<AuthorDto>> GetAll()
    {
        // DTO <--> Entity
        var allCount = await _repository.Count();
        var author = await _repository.GetList(null, "Created", SortDirection.Asc, 1, allCount);
        var authorDto = _mapper.Map<List<AuthorDto>>(author);
        return authorDto;
    }

    public async Task<AuthorDto> GetById(int id)
    {
        var author = await _repository.GetById(id);
        var authorDto = _mapper.Map<AuthorDto>(author);
        return authorDto;
    }

    public void Insert(AuthorCreateDto authorCreateDto)
    {
        var author = _mapper.Map<Author>(authorCreateDto);
        _repository.Insert(author);
    }

    public async Task<AuthorDto> Update(AuthorDto entity,int id)
    {
       var author = await _repository.GetById(id);
    _mapper.Map(entity, author);
    await _repository.Update(author!);
    return _mapper.Map<AuthorDto>(author);
        
    }
    

    public async Task Delete(int id)
    {
        var booksByAuthor = await _bookRepository.GetList(x => x.AuthorId == id, x => x.OrderBy(y => y.Created));
        var author = await _repository.GetById(id);
        if (author == null) return;
        if (booksByAuthor.Any())
        {
            throw new Exception("Author cannot be deleted as it is in use");
        }

        await _repository.Delete(author.Id);
    }
    
    public async Task DeleteAll()
    {
        var allAuthors = await _repository.GetList(null, "Created", SortDirection.Asc, 1, int.MaxValue);
        if(allAuthors == null) return;
        
        var allAuthorDtos = _mapper.Map<List<AuthorDto>>(allAuthors);
        foreach (var authorDto in allAuthorDtos)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _repository.Delete(author.Id);
        }
    }

   
}
