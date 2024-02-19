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

                int newUserId = await userRegister.Run(name, lastName, email, password);

                return Response.Ok("Usuario registrado", newUserId);
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
            
        }

        public static async Task<Response> DeleteUser(dynamic data)
        {
            try
            {
                int id = data.Id;

                UserDeleter userDeleter = new(userRepository);

                bool deleted = await userDeleter.Run(id);

                return deleted ? Response.Ok("Usuario eliminado") : Response.Fail("Usuario no eliminado");
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
        }

        public static async Task<Response> UpdateUser(dynamic data)
        {
            try
            {
                int id = data.Id;
                string name = data.Name;
                string lastName = data.LastName;
                string email = data.Email;
                string password = data.Password;

                UserUpdater userUpdater = new(userRepository);

                bool updated = await userUpdater.Run(id, name, lastName, email, password);

                return updated ? Response.Ok("Usuario actualizado") : Response.Fail("Usuario no actualizado");
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
        }
    }
}
