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

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> FindAsync(UserId userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmailAsync(UserEmail email)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(User user)
        {
            var parameters = new Dictionary<string, object>
            {
                {"@Name", user.UserName},
                {"@Password", user.Password},
                {"@LastName", user.LastName},
                {"@Email", user.Email}
            };
            var result = await ExecuteNonQueryAsync("CreateUserProcedure", parameters);

            if (result <= 0)
            {
                throw new Exception("No se ha podido insertar el usuario.");
            }

            return result;
        }

        public Task<int> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }
    }


}
