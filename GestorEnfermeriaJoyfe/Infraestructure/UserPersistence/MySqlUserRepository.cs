using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public async Task<IEnumerable<User>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"p_per_page", perPage},
                    {"p_page", page}
                };

                return await ExecuteQueryAsync("GetAllUsersPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetAllUsersProcedure");
        }

        public async Task<User> FindAsync(UserId userId)
        {
            var result = await ExecuteQueryAsync("GetUserByIdProcedure", new Dictionary<string, object> { { "p_id", userId.Value } });

            if (!result.Any())
            {
                throw new Exception("No se ha encontrado el usuario.");
            }

            return result.First();
        }

        public async Task<User> GetByEmailAsync(UserEmail email)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_email", email.Value}
            };

            var result = await ExecuteQueryAsync("GetUserByEmailProcedure", parameters);

            if (!result.Any())
            {
                throw new Exception("No se ha encontrado el usuario.");
            }

            return result.First();
        }

        public async Task<int> AddAsync(User user)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_name", user.UserName},
                {"p_password", user.Password},
                {"p_last_name", user.LastName},
                {"p_email", user.Email}
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
                {"p_id", user.Id},
                {"p_name", user.UserName},
                {"p_password", user.Password},
                {"p_last_name", user.LastName},
                {"p_email", user.Email}
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
            var result = await ExecuteNonQueryAsync("DeleteUserProcedure", new Dictionary<string, object> { { "p_id", userId.Value } });

            if (result <= 0)
            {
                throw new Exception("No se ha podido eliminar el usuario.");
            }
            return result;
        }
    }


}
