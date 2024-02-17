using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Application.UserApp
{
    public class UserRegister
    {
        private readonly IUserContract _userRepository;

        public UserRegister(IUserContract userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> RegisterUser(string name, string lastName, string email, string password)
        {
            var user = User.Create(new UserId(0), new UserName(name), new UserPassword(password), new UserLastName(lastName), new UserEmail(email));

            var newUserId = await _userRepository.AddAsync(user);

            user.SetId(new UserId(newUserId));

            return user.Id.Value;
        }

    }
}
