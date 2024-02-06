using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;

namespace GestorEnfermeriaJoyfe.Infraestructure.UserPersistence
{
    public class UserRepository : MySqlRepositoryBase, IUserContract
    {

        public UserRepository(UserMapper mapper) : base(mapper)
        {
        }

        public UserRepository()
        {
        }

        public Task AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindAsync(UserId userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmailAsync(UserEmail email)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }


}
