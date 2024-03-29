﻿using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.UserApp
{
    public class UserRegister
    {
        private readonly IUserContract _userRepository;

        public UserRegister(IUserContract userRepository) => _userRepository = userRepository;

        public async Task<int> Run(User user)
        {
            user.Password = new UserPassword(user.Password.GetHash());

            return await _userRepository.AddAsync(user);
        }

    }
}
