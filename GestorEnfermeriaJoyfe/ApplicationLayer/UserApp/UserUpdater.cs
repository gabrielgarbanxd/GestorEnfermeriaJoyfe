using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.UserApp
{
    public class UserUpdater
    {
        private readonly IUserContract _userRepository;

        public UserUpdater(IUserContract userRepository) => _userRepository = userRepository;

        public async Task<int> Run(User user)
        {
            user.Password = new UserPassword(user.Password.GetHash());

            return await _userRepository.UpdateAsync(user);
        }
    }
}
