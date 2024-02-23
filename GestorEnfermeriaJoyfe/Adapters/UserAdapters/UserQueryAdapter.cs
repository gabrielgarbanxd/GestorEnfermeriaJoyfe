using GestorEnfermeriaJoyfe.ApplicationLayer.UserApp;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Infraestructure.UserPersistence;
using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public static class UserQueryAdapter
    {
        private static readonly UserMapper userMapper = new();
        private static readonly MySqlUserRepository userRepository = new(userMapper);

        public static async Task<Response<int>> AuthUser(string email, SecureString securePassword)
        {
            try
            {
                UserAuthenticator userAuthenticator = new(userRepository);

                int authenticatedId = await userAuthenticator.Run(email, securePassword);

                return authenticatedId > 0 ? Response<int>.Ok("Usuario autenticado", authenticatedId) : Response<int>.Fail("Usuario no autenticado");
            }
            catch (Exception e)
            {
                return Response<int>.Fail(e.Message);
            }
        }


        public static async Task<Response<User>> FindUser(int id)
        {
            try
            {
                UserFinder userFinder = new(userRepository);

                var user = await userFinder.Run(id);

                return user != null ? Response<User>.Ok("Usuario encontrado", user) : Response<User>.Fail("Usuario no encontrado");
            }
            catch (Exception e)
            {
                return Response<User>.Fail(e.Message);
            }
        }


        public static async Task<Response<List<User>>> GetAllUsers()
        {
            try
            {
                UserLister userLister = new(userRepository);

                var users = await userLister.Run();

                return users.Count > 0 ? Response<List<User>>.Ok("Usuarios encontrados", users) : Response<List<User>>.Fail("No se encontraron usuarios");
            }
            catch (Exception e)
            {
                return Response<List<User>>.Fail(e.Message);
            }
        }

        public static async Task<Response<List<User>>> GetAllUsersPaginated(int perPage, int page)
        {
            try
            {
                UserLister userLister = new(userRepository);

                var users = await userLister.Run(true, perPage, page);

                return users.Count > 0 ? Response<List<User>>.Ok("Usuarios encontrados", users) : Response<List<User>>.Fail("No se encontraron usuarios");
            }
            catch (Exception e)
            {
                return Response<List<User>>.Fail(e.Message);
            }
        }

    }
}
