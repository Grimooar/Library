using ClassLibrary1;
using FluentValidation;
using Kirel.Repositories.Interfaces;
using WebApi.Models;

namespace WebApi.Validators;

public class PublisherValidator : AbstractValidator<PublisherCreateDto>
{
    private readonly IKirelGenericEntityFrameworkRepository<int, Publisher> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _bookRepository;
    public PublisherValidator(IKirelGenericEntityFrameworkRepository<int, Publisher> repository, IKirelGenericEntityFrameworkRepository<int, Book> bookRepository)
    {
        _repository = repository;
        _bookRepository = bookRepository;
        RuleFor(publisher => publisher.Id).NotNull();
        RuleFor(publisher => publisher.Name).NotEmpty().MaximumLength(100);
        
    }


    public async Task<bool> DeleteIsValid(int id)
    {
        
        var booksByPublisher = await _bookRepository.GetList(x => x.PublisherId == id, x => x.OrderBy(y => y.Created));
        var publisher = await _repository.GetById(id);
        
        if(publisher == null) return false;
        if (booksByPublisher.Any())
        {
            throw new Exception("Publisher cannot be deleted as it is in use");
        }

        return true;


    }
}
