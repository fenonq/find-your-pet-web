using DAL.Model;

namespace BLL.Service;

public interface IUserService : ICrudService<User>
{
    bool LoginUser(string login, string password);
}