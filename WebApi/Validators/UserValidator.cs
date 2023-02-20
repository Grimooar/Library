using FluentValidation;
using Kirel.Repositories.Interfaces;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Validators;

public class UserValidator: AbstractValidator<UserCreateDto>
{
    private readonly IKirelGenericEntityFrameworkRepository<int, User> _repository;
    private readonly IKirelGenericEntityFrameworkRepository<int, BookInAbonement> _abonementRepository;
    public UserValidator(IKirelGenericEntityFrameworkRepository<int, User> repository, IKirelGenericEntityFrameworkRepository<int, BookInAbonement> abonementRepository)
    {
        _repository = repository;
        _abonementRepository = abonementRepository;
        RuleFor(user => user.UserName).NotEmpty();
    }


    public async Task<bool> DeleteIsValid(int id)
    {
        var userInAbonement = await _abonementRepository.GetList(x => x.abonemnetId == id, x => x.OrderBy(y => y.Created));
        var user = await _repository.GetById(id);
        if (userInAbonement.Any())
        {
            throw new Exception("User cannot be deleted as it is in use");
        }
        if(user == null) return false;
        return true;
    }
}