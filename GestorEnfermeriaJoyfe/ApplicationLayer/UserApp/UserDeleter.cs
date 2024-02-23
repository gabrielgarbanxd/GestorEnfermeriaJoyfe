using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<bool> Run(int id)
        {
            return await _userRepository.DeleteAsync(new UserId(id)) > 0;
        }
    }
}
