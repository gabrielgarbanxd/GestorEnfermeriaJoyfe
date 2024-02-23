using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Application.UserApp
{
    public class UserUpdater
    {
        private readonly IUserContract _userRepository;

        public UserUpdater(IUserContract userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Run(User user)
        {
            user.Password = new UserPassword(user.Password.GetHash());

            return await _userRepository.UpdateAsync(user) > 0;
        }
    }
}
