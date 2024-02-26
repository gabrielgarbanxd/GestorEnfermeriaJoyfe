using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.UserApp
{
    public class UserDeleter
    {
        private readonly IUserContract _userRepository;

        public UserDeleter(IUserContract userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Run(int id)
        {
            return await _userRepository.DeleteAsync(new UserId(id));
        }
    }
}
