using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Application.UserApp
{
    public class UserAuthenticator
    {
        private readonly IUserContract _userRepository;

        public UserAuthenticator(IUserContract userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AuthenticateUser(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(new UserEmail(email));

            if (user.Password.VerifyPassword(password))
            {
                return true;
            }

            return false;
        }
    }
}
