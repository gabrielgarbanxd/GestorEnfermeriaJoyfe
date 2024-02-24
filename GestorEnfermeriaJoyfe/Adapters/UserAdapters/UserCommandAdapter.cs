using GestorEnfermeriaJoyfe.ApplicationLayer.UserApp;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Infraestructure.UserPersistence;
using System;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public class UserCommandAdapter
    {
        private readonly IUserContract userRepository;

        public UserCommandAdapter(IUserContract userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Response<int>> RegisterUser(User user)
        {
            try
            {
                UserRegister userRegister = new(userRepository);

                int newUserId = await userRegister.Run(user);

                return Response<int>.Ok("Usuario registrado", newUserId);
            }
            catch (Exception e)
            {
                return Response<int>.Fail(e.Message);
            }
        }

        public async Task<Response<bool>> DeleteUser(int id)
        {
            try
            {
                UserDeleter userDeleter = new(userRepository);

                bool deleted = await userDeleter.Run(id);

                return deleted ? Response<bool>.Ok("Usuario eliminado") : Response<bool>.Fail("Usuario no eliminado");
            }
            catch (Exception e)
            {
                return Response<bool>.Fail(e.Message);
            }
        }

        public async Task<Response<bool>> UpdateUser(User user)
        {
            try
            {
                UserUpdater userUpdater = new(userRepository);
                bool updated = await userUpdater.Run(user);

                return updated ? Response<bool>.Ok("Usuario actualizado") : Response<bool>.Fail("Usuario no actualizado");
            }
            catch (Exception e)
            {
                return Response<bool>.Fail(e.Message);
            }
        }

    }
}
