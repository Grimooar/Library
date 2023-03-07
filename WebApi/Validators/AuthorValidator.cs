using ClassLibrary1;
using FluentValidation;
using Kirel.Repositories.Interfaces;
using WebApi.Models;

namespace WebApi.Validators;

public class AuthorValidator : AbstractValidator<AuthorCreateDto>
{
    private readonly IKirelGenericEntityFrameworkRepository<int, Author> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInfo> _bookRepository;
    public AuthorValidator(IKirelGenericEntityFrameworkRepository<int, Author> repository, IKirelGenericEntityFrameworkRepository<int, BookInfo> bookRepository)
    {
        _repository = repository;
        _bookRepository = bookRepository;
        RuleFor(author => author.Id).NotNull();
        RuleFor(author => author.Name).NotEmpty().MaximumLength(100);
        
    }



    public async Task<bool> DeleteIsValid(int id)
    {
        var booksByAuthor = await _bookRepository.GetList(x => x.AuthorId == id, x => x.OrderBy(y => y.Created));
    var author = await _repository.GetById(id);
        if (author == null) return false;
    if (booksByAuthor.Any())
    {
        throw new Exception("Author cannot be deleted as it is in use");
    }

    return true;
    }
}
