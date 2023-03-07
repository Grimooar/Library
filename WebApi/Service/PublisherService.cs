using AutoMapper;
using ClassLibrary1;
using Kirel.Repositories.Interfaces;
using Kirel.Repositories.Sorts;
using WebApi.Models;

namespace WebApi.Service;

public class PublisherService 
{
    private readonly IKirelGenericEntityFrameworkRepository<int, Publisher> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _bookRepository;
    private readonly IMapper _mapper;

    public PublisherService(IKirelGenericEntityFrameworkRepository<int, Publisher> repository, IMapper mapper, IKirelGenericEntityFrameworkRepository<int, Book> bookRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<PublisherDto>> GetAll()
    {
        // DTO <--> Entity
        var allCount = await _repository.Count();
        var publisher = await _repository.GetList(null, "Created", SortDirection.Asc, 1, allCount);
        var publisherDto = _mapper.Map<List<PublisherDto>>(publisher);
        return publisherDto;
    }

    public async Task<PublisherDto> GetById(int id)
    {
        var publisher = await _repository.GetById(id);
        var publisherDto = _mapper.Map<PublisherDto>(publisher);
        return publisherDto;
    }

    public void Insert(PublisherCreateDto publisherCreateDto)
    {
        var publisher = _mapper.Map<Publisher>(publisherCreateDto);
        _repository.Insert(publisher);
    }

    public async Task<PublisherDto> Update(PublisherDto entity,int id)
    {
        var publisher = await _repository.GetById(id);
        _mapper.Map(entity, publisher);
        await _repository.Update(publisher!);
        return  _mapper.Map<PublisherDto>(publisher);
    }

    public async Task Delete(int id)
    {
        var booksByPublisher = await _bookRepository.GetList(x => x.PublisherId == id, x => x.OrderBy(y => y.Created));
        var publisher = await _repository.GetById(id);
        
        if(publisher == null) return;
        if (booksByPublisher.Any())
        {
            throw new Exception("Publisher cannot be deleted as it is in use");
        }
        
        await  _repository.Delete(publisher.Id);
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
    public async Task DeleteAll()
    {
        var allPublishers = await _repository.GetList(null, "Created", SortDirection.Asc, 1, int.MaxValue);
        if(allPublishers == null) return;
        
        var allPublisherDto = _mapper.Map<List<PublisherDto>>(allPublishers);
        foreach (var publisherDto in allPublisherDto)
        {
            var publisher = _mapper.Map<Publisher>(publisherDto);
            await _repository.Delete(publisher.Id);
        }
    }
   
   
}