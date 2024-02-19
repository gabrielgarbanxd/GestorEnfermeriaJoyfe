using GestorEnfermeriaJoyfe.Application.UserApp;
using GestorEnfermeriaJoyfe.Infraestructure.UserPersistence;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public static class UserQueryAdapter
    {
        private static readonly UserMapper userMapper = new();
        private static readonly MySqlUserRepository userRepository = new(userMapper);

        public static async Task<Response> AuthUser(dynamic data)
        {
            try
            {
                string email = data.Email;
                string password = data.Password;

                UserAuthenticator userAuthenticator = new(userRepository);

                bool authenticated = await userAuthenticator.Run(email, password);
                
                return authenticated ? Response.Ok("Usuario autenticado") : Response.Fail("Usuario no autenticado");
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
           
        }

        public static async Task<Response> FindUser(dynamic data)
        {
            try
            {
                int id = data.Id;
                UserFinder userFinder = new(userRepository);

                var user = await userFinder.Run(id);

                return user != null ? Response.Ok("Usuario encontrado", user) : Response.Fail("Usuario no encontrado");
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
        }

        public static async Task<Response> GetAllUsers(dynamic data)
        {
            try
            {
                UserLister userLister = new(userRepository);

                var users = await userLister.Run();

                return users.Count > 0 ? Response.Ok("Usuarios encontrados", users) : Response.Fail("No se encontraron usuarios");
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
        }
    }
}
