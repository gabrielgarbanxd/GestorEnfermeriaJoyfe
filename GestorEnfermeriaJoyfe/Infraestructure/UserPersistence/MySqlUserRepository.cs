using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;

namespace GestorEnfermeriaJoyfe.Infraestructure.UserPersistence
{
    public class MySqlUserRepository : MySqlRepositoryBase<User>, IUserContract
    {

        public MySqlUserRepository(UserMapper mapper) : base(mapper)
        {
        }

        public MySqlUserRepository()
        {
        }

        public async Task<List<User>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@PerPage", perPage},
                    {"@Page", page}
                };

                return await ExecuteQueryAsync("GetAllUsersPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetAllUsersProcedure");
        }

        public async Task<User> FindAsync(UserId userId)
        {
            var result = await ExecuteQueryAsync("GetUserByIdProcedure", new Dictionary<string, object> { { "@Id", userId.Value } });

            if (result.Count == 0)
            {
                throw new Exception("No se ha encontrado el usuario.");
            }

            return result[0];
        }

        public async Task<User> GetByEmailAsync(UserEmail email)
        {
            var parameters = new Dictionary<string, object>
            {
                {"@Email", email.Value}
            };

            var result = await ExecuteQueryAsync("GetUserByEmailProcedure", parameters);

            if (result.Count == 0)
            {
                throw new Exception("No se ha encontrado el usuario.");
            }

            return result[0];
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

        public async Task<int> UpdateAsync(User user)
        {
            var parameters = new Dictionary<string, object>
            {
                {"@Id", user.Id},
                {"@Name", user.UserName},
                {"@Password", user.Password},
                {"@LastName", user.LastName},
                {"@Email", user.Email}
            };

            var result = await ExecuteNonQueryAsync("UpdateUserProcedure", parameters);

            if (result <= 0)
            {
                throw new Exception("No se ha podido actualizar el usuario.");
            }

            return result;
        }

        public async Task<int> DeleteAsync(UserId userId)
        {
            var result = await ExecuteNonQueryAsync("DeleteUserProcedure", new Dictionary<string, object> { { "@Id", userId.Value } });

            if (result <= 0)
            {
                throw new Exception("No se ha podido eliminar el usuario.");
            }
            return result;
        }
    }


}
