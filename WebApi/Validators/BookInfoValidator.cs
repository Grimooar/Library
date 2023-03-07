using ClassLibrary1;
using FluentValidation;
using Kirel.Repositories.Interfaces;
using WebApi.Models;

namespace WebApi.Validators;

public class BookInfoValidator : AbstractValidator<BookInfoCreateDto>
{
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInfo> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Author> _authorRepository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _bookRepository;
    public BookInfoValidator(IKirelGenericEntityFrameworkRepository<int, BookInfo> repository, IKirelGenericEntityFrameworkRepository<int, Author> authorRepository, IKirelGenericEntityFrameworkRepository<int, Book> bookRepository)
    {
        _repository = repository;
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
      
        RuleFor(bookInfo => bookInfo.Name).NotEmpty().MaximumLength(100);
        RuleFor(bookInfo => bookInfo.AuthorId).NotNull();
        
    }

    public async Task<bool> DeleteIsValid(int id)
    {
        var bookInfoByBooks = await _bookRepository.GetList(x => x.BookInfoId == id, x => x.OrderBy(y => y.Created));
        var bookInfo = await _repository.GetById(id);
        if (bookInfoByBooks.Any())
        {
            throw new Exception("BookInfo cannot be deleted as it is in use");
        }
        if(bookInfo == null) return false;
        return true;


    }
}
