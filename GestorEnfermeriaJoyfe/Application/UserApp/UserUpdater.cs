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

        public async Task<bool> Run(int id, string name, string lastName, string email, string password)
        {
            var user = new User(new UserId(id), new UserName(name), new UserPassword(password), new UserLastName(lastName), new UserEmail(email));

            user.Password.Value = user.Password.GetHash();

            return await _userRepository.UpdateAsync(user) > 0;
        }
    }
}
