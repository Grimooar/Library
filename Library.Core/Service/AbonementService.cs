using AutoMapper;
using ClassLibrary1;
using Kirel.Repositories.Interfaces;
using Kirel.Repositories.Sorts;
using Library.Models;

namespace Library.Core.Service;

public class AbonementService
{
     private readonly IKirelGenericEntityFrameworkRepository<int, Abonement> _repository;
   
    private readonly IMapper _mapper;

    public AbonementService(IKirelGenericEntityFrameworkRepository<int,Abonement> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
       
    }

    public async Task<IEnumerable<AuthorDto>> GetAll()
    {
        // DTO <--> Entity
        var allCount = await _repository.Count();
        var author = await _repository.GetList(null, "Created", SortDirection.Asc, 1, allCount);
        var authorDto = _mapper.Map<List<AuthorDto>>(author);
        return authorDto;
    }

    public async Task<AbonementDto> GetById(int id)
    {
        var abonement = await _repository.GetById(id);
        var abonementDto = _mapper.Map<AbonementDto>(abonement);
        return abonementDto;
    }

    public void Insert(AbonementCreateDto abonementCreateDto)
    {
        var abonement = _mapper.Map<Abonement>(abonementCreateDto);
        _repository.Insert(abonement);
    }

    public async Task<AbonementDto> Update(AbonementDto entity,int id)
    {
       var abonement = await _repository.GetById(id);
    _mapper.Map(entity, abonement);
    await _repository.Update(abonement!);
    return _mapper.Map<AbonementDto>(abonement);
        
    }
    

    public async Task Delete(int id)
    {
        await _repository.Delete(id);
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