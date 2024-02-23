using GestorEnfermeriaJoyfe.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public class UserController
    {
        public UserController() { }

        public static async Task<Response<List<User>>> GetAll()
        {
            return await UserQueryAdapter.GetAllUsers();
        }

        public static async Task<Response<User>> Get(int id)
        {
            return await UserQueryAdapter.FindUser(id);
        }

        public static async Task<Response<bool>> Login(string email, string password)
        {
            return await UserQueryAdapter.AuthUser(email, password);
        }

        public static async Task<Response<int>> Register(string name, string lastName, string email, string password)
        {
            return await UserCommandAdapter.RegisterUser(name, lastName, email, password);
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
