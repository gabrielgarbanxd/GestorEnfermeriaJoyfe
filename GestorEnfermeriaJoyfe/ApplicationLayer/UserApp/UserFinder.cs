using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.UserApp
{
    public class UserFinder
    {
        private readonly IUserContract _userRepository;

        public UserFinder(IUserContract userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Run(int id)
        {
            return await _userRepository.FindAsync(new UserId(id));
        }
    }
}
