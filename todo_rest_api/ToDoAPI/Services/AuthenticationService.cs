using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoAPI.Data;
using TodoAPI.Dtos;
using TodoAPI.Helpers;

namespace TodoAPI.Services
{
    public interface IAuthenticationService
    {
        Task<string> Authenticate(UserLoginDto userLoginParams);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly TodoContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticationService(TodoContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> Authenticate(UserLoginDto userLoginParams)
        {
            var user = await _context.Users.SingleOrDefaultAsync(
                u => u.Email == userLoginParams.Email && PasswordHelper.Validate(userLoginParams.Password, u.Password, u.Salt)
            );

            if (user == null)
            {
                return null;
            }

            var tokenString = _jwtTokenService.Create(user);

            return tokenString;
        }
    }
}
