using DAL.Model;
using DAL.Repository;

namespace BLL.Service.impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
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

    public int Add(User user)
    {
        _userRepository.Add(user);
        return user.Id;
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