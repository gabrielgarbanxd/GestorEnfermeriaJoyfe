﻿using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.User
{
    public interface IUserContract
    {
        Task<User> FindAsync(UserId userId);
        Task<User> GetByEmailAsync(UserEmail email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task RemoveAsync(User user);
        Task DeleteAsync(User user);
        Task<List<User>> GetAllAsync();
    }
}
