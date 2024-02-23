using GestorEnfermeriaJoyfe.Domain.User;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public class UserController
    {
        public UserController() { }

        public static async Task<Response<List<User>>> GetAll()
        {
            return await UserQueryAdapter.GetAllUsers();
        }

        public static async Task<Response<List<User>>> GetAllPaginated(int perPage, int page)
        {
            return await UserQueryAdapter.GetAllUsersPaginated(perPage, page);
        }

        public static async Task<Response<User>> Get(int id)
        {
            return await UserQueryAdapter.FindUser(id);
        }

        public static async Task<Response<int>> Login(string email, SecureString securePassword)
        {
            return await UserQueryAdapter.AuthUser(email, securePassword);
        }

        public static async Task<Response<int>> Register(User user)
        {
            return await UserCommandAdapter.RegisterUser(user);
        }

        public static async Task<Response<bool>> Update(User user)
        {
            return await UserCommandAdapter.UpdateUser(user);
        }

        public static async Task<Response<bool>> Delete(dynamic data)
        {
            return await UserCommandAdapter.DeleteUser(data);
        }

    }

}
