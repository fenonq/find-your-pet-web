using DAL.Model;
using DAL.Repository;

namespace BLL.Service.impl;

public class UserService : IUserService
{
    private readonly IEntityRepository<User> _userRepository;

    public UserService(IEntityRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public List<User> FindAll()
    {
        return _userRepository.FindAll().ToList();
    }

    public User? FindById(int id)
    {
        return _userRepository.FindById(id);
    }

    public void RegisterUser(string name, string surname, string login, string password)
    {
        var user = new User
        {
            Name = name,
            Surname = surname,
            Login = login,
            Password = password
        };
        _userRepository.Add(user);
    }

    public bool LoginUser(string login, string password)
    {
        var user = _userRepository.FindAll().FirstOrDefault(u => u.Login == login);

        if (user == null)
        {
            return false;
        }

        return password == user.Password;
    }

    public void Remove(int id)
    {
        _userRepository.Remove(id);
    }
}