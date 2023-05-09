using DAL.Model;

namespace BLL.Service;

public interface IUserService
{
    Task<bool> LoginUser(string login, string password, bool rememberMe);

    Task<bool> RegistrateUser(User user, string userPassword);

    Task<bool> ConfirmEmail(string email, string token);

    Task<bool> ForgotPassword(string email);

    Task<bool> ResetPassword(string email, string token, string password);
}