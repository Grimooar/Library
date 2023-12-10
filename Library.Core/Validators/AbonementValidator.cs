using ClassLibrary1;
using FluentValidation;
using Kirel.Repositories.Interfaces;
using Library.Models;

namespace Library.Core.Validators;
public class AbonementValidator : AbstractValidator<AbonementCreateDto>
{
private readonly IKirelGenericEntityFrameworkRepository<int, Abonement> _repository;
private readonly IKirelGenericEntityFrameworkRepository<int, BookInAbonement> _abonementInRepository;
        
public AbonementValidator(IKirelGenericEntityFrameworkRepository<int, Abonement> repository, IKirelGenericEntityFrameworkRepository<int, BookInAbonement> abonementInRepository)
{
    _repository = repository;
    _abonementInRepository = abonementInRepository;
    RuleFor(abonement => abonement.Name).NotEmpty();
    RuleFor(abonement => abonement.Number).NotEmpty();
    RuleFor(abonement => abonement.UserId).NotNull();
}

public async Task<bool> DeleteIsValid(int id)
{
    var bookInfoByBooks = await _abonementInRepository.GetList(x => x.abonemnetId == id, x => x.OrderBy(y => y.Created));
    var bookInfo = await _repository.GetById(id);
    if (bookInfoByBooks.Any())
    {
        throw new Exception("Abonement cannot be deleted as it is in use");
    }
    return true;
    

    
    
}
}

