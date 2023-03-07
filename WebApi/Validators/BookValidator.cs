using ClassLibrary1;
using FluentValidation;
using Kirel.Repositories.Interfaces;
using WebApi.Models;

namespace WebApi.Validators;

public class BookValidator : AbstractValidator<BookCreateDto>
{
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInfo> _bookInfoRepository;
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInStock> _bookInStockRepository;
    public BookValidator(IKirelGenericEntityFrameworkRepository<int, Book> repository, IKirelGenericEntityFrameworkRepository<int, BookInfo> bookInfoRepository, IKirelGenericEntityFrameworkRepository<int, BookInStock> bookInStockRepository)
    {
        _repository = repository;
        _bookInfoRepository = bookInfoRepository;
        _bookInStockRepository = bookInStockRepository;
       
        RuleFor(book => book.BookInfoId).NotNull();
        RuleFor(book => book.PublisherId).NotNull();
        RuleFor(book => book.Year).NotNull();
        
    }


    public async Task<bool> DeleteIsValid(int id)
    {
        var book = await _repository.GetById(id);
        var bookinBooksInStock =
            await _bookInStockRepository.GetList(x => x.BookId == id, x => x.OrderBy(y => y.Created));
        if(book == null) return false;
        if (bookinBooksInStock.Any())
        {
            throw new Exception("Book cannot be deleted as it is in use ");
        }

        return true;
    }
}
