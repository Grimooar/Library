using AutoMapper;
using ClassLibrary1;
using Kirel.Repositories.Interfaces;
using Kirel.Repositories.Sorts;
using Library.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Library.Core.Service;

public class UserService
{
    private readonly IKirelGenericEntityFrameworkRepository<int, User> _repository;

    private readonly IKirelGenericEntityFrameworkRepository<int, BookInAbonement> _abonementRepository;

    private readonly IMapper _mapper;

    public UserService(IKirelGenericEntityFrameworkRepository<int, User> repository, IMapper mapper,
        IKirelGenericEntityFrameworkRepository<int, BookInAbonement> abonementRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _abonementRepository = abonementRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAll()
    {
        // DTO <--> Entity
        var allCount = await _repository.Count();
        var User = await _repository.GetList(null, "Created", SortDirection.Asc, 1, allCount);
        var userDtos = _mapper.Map<List<UserDto>>(User);
        return userDtos;
    }

    public async Task<UserDto> GetById(int id)
    {
        var bookAbonement = await _repository.GetById(id);
        var bookAbonementDto = _mapper.Map<UserDto>(bookAbonement);
        return bookAbonementDto;
    }

    public void Insert(UserCreateDto userDto)
    {
        var user = new User
        {
            UserName = userDto.UserName,
            Name = userDto.Name,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Password = userDto.Password
        };
        _repository.Insert(user);
    }


    public async Task<BookInAbonementDto> Update(UserDto entity, int id)
    {
        var bookAbonement = await _repository.GetById(id);
        _mapper.Map(entity, bookAbonement);
        await _repository.Update(bookAbonement!);
        return _mapper.Map<BookInAbonementDto>(bookAbonement);
    }

    public async Task Delete(int id)
    {
        var UserinAbonement = await _abonementRepository.GetList(x => x.Id == id, x => x.OrderBy(y => y.Created));
        var user = await _repository.GetById(id);

        if (user == null) return;
        if (UserinAbonement.Any())
        {
            throw new Exception("User cannot be deleted as it is in use");
        }

        await _repository.Delete(user.Id);
    }

    public async Task DeleteAll()
    {
        var allPublishers = await _repository.GetList(null, "Created", SortDirection.Asc, 1, int.MaxValue);
        if (allPublishers == null) return;

        var allPublisherDto = _mapper.Map<List<PublisherDto>>(allPublishers);
        foreach (var publisherDto in allPublisherDto)
        {
            var publisher = _mapper.Map<Publisher>(publisherDto);
            await _repository.Delete(publisher.Id);
        }
    }

    public async Task RegisterUserAsync(UserDto userDto)
    {
        // Check if user with same email already exists
        var existingUser = await GetUserByEmailAsync(userDto.Email);
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists.");
        }

        // Map userDto to User model and save to database
        var user = _mapper.Map<User>(userDto);
        user.Created = DateTime.UtcNow;
        await _repository.Insert(user);
    }

    public async Task<IdentityUser> GetUserByEmailAsync(string email)
    {
        var users = await _repository.GetList(u => u.Email == email);
        if (users == null || !users.Any())
        {
            return null;
        }

        var user = users.FirstOrDefault();
        return new IdentityUser
        {
            Id = user.Id.ToString(),
            UserName = user.UserName,

            Email = user.Email,
            PasswordHash = user.Password
        };
    }
}