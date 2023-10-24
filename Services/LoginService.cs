using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RefactorThis.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    public class LoginService :  BaseService<Login>, ILoginService
    {
        public LoginService(ProductDbContext db) : base(db)
        {
        }

        public async Task<Login> AuthenticateWithUsernamePassword(string username, string password)
        {
            var user = await GetEntityAsync(l => l.Name.Equals(username) && l.Password.Equals(password));
            return user;
        }

        public async Task<Login> AuthenticateWithToken(Guid token)
        {
            var user = await GetEntityAsync(l => l.APIToken.Equals(token));
            return user;
        }

        public bool IsVaildToken(Login user)
        {
            DateTime realExpiryDate;
            DateTime.TryParse(user.APITokenExpiry, out realExpiryDate);
            
            if (DateTime.UtcNow < realExpiryDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<Guid> GenerateToken(Login user)
        {
            var expireTime = DateTime.UtcNow.AddMinutes(120);
            user.APIToken = Guid.NewGuid();
            user.APITokenExpiry = expireTime.ToString();
            user.LastLoggedIn = DateTime.UtcNow;
            await UpdateAsync(user);
            return user.APIToken;
        }
    }
}
