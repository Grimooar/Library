using ClassLibrary1;
using FluentValidation;
using Kirel.Repositories.Interfaces;
using Library.Models;

namespace Library.Core.Validators;

public class BookInAbonementValidator : AbstractValidator<BookInAbonementCreateDto>
{
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInAbonement> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, Book> _bookRepository;
    public BookInAbonementValidator(IKirelGenericEntityFrameworkRepository<int, BookInAbonement> repository,IKirelGenericEntityFrameworkRepository<int, Book> bookRepository)
    {
        _repository = repository;
        _bookRepository = bookRepository;
        RuleFor(bookInAbonement => bookInAbonement.AbonemnetId).NotNull();
        RuleFor(bookInAbonement => bookInAbonement.BookId).NotNull();
      
    }
    public async Task<bool> DeleteIsValid(int id)
    {
        var bookInAbonements = await _repository.GetList(x => x.abonemnetId == id, x => x.OrderBy(y => y.Created));
        var abonementInBook = await _repository.GetById(id);
        if (bookInAbonements.Any())
        {
            throw new Exception("BookInfo cannot be deleted as it is in use");
        }
        if(abonementInBook == null) return false;
        return true;
    }
    
}
