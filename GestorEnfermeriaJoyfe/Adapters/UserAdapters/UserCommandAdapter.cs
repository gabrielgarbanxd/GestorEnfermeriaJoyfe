using GestorEnfermeriaJoyfe.Application.UserApp;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Infraestructure.UserPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public static class UserCommandAdapter
    {
        private static readonly IUserContract userRepository = new MySqlUserRepository();

        public static async Task<Response> RegisterUser(dynamic data)
        {
            try
            {
                string name = data.Name;
                string lastName = data.LastName;
                string email = data.Email;
                string password = data.Password;

                UserRegister userRegister = new(userRepository);

                int newUserId = await userRegister.RegisterUser(name, lastName, email, password);

                return Response.Ok("Usuario registrado", newUserId);
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
            
        }

        public static Task<Response> DeleteUser(dynamic data)
        {
            throw new NotImplementedException();
        }

        public static Task<Response> UpdateUser(dynamic data)
        {
            throw new NotImplementedException();
        }
    }
}
