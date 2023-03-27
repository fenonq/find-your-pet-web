namespace BLL.Service;

using DAL.Model;

public interface IUserService : ICrudService<User>
{
    bool LoginUser(string login, string password);
}