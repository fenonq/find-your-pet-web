using DAL.Model;

namespace BLL.Service;

public interface IUserService
{
    List<User> FindAll();

    User? FindById(int id);

    void RegisterUser(string name, string surname, string login, string password);

    bool LoginUser(string login, string password);

    void Remove(int id);
}