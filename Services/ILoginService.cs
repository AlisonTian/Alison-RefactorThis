using RefactorThis.Models;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    public interface ILoginService : IBaseService<Login>
    {
        Task<Login> AuthenticateWithUsernamePassword(string username, string password);
        bool IsVaildToken(Login user);
        Task<Guid> GenerateToken(Login user);
        Task<Login> AuthenticateWithToken(Guid token);
    }
}
