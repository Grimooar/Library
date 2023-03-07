using ClassLibrary1;
using FluentValidation;
using Kirel.Repositories.Interfaces;
using WebApi.Models;

namespace WebApi.Validators;

public class BookInStockValidator : AbstractValidator<BookInStockCreateDto>
{
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInStock> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _bookRepository;
    public BookInStockValidator(IKirelGenericEntityFrameworkRepository<int, BookInStock> repository, IKirelGenericEntityFrameworkRepository<int, Book> bookRepository)
    {
        _repository = repository;
        _bookRepository = bookRepository;
        RuleFor(bookInStock => bookInStock.BookId).NotNull();
        RuleFor(bookInStock => bookInStock.Amount).NotNull();
    }

    public async Task<bool> DeleteIsValid(int id)
    {
        var bookInSockBooks = await _bookRepository.GetList(x => x.BookInfoId == id, x => x.OrderBy(y => y.Created));
        var bookInStock = await _repository.GetById(id);
        if (bookInSockBooks.Any())
        {
            throw new Exception("BookInStock cannot be deleted as it is in use");
        }
        if(bookInStock == null) return false;
        return true;
    }
    public async Task<bool> ValidateBookInStockAmount(int bookId)
    {
        var bookInStock = await _repository.GetById(bookId);
        if (bookInStock == null)
        {
            throw new Exception("Book in stock not found");
        }
        if (bookInStock.Amount == 0)
        {
            throw new Exception("Sorry, the book is out of stock");
        }
        return true;
    }
}
