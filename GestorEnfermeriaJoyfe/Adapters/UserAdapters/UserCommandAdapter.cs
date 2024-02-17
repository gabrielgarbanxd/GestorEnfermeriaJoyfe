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

        public static async Task<Response> RegisterUser(IDictionary<string, object> data)
        {
            try
            {
                string name = (string)data["name"];
                string lastName = (string)data["lastName"];
                string email = (string)data["email"];
                string password = (string)data["password"];

                UserRegister userRegister = new(userRepository);

                int newUserId = await userRegister.RegisterUser(name, lastName, email, password);

                return Response.Ok("Usuario registrado", newUserId);
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
            
        }

        public static Task<Response> DeleteUser(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

        public static Task<Response> UpdateUser(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }
    }
}
