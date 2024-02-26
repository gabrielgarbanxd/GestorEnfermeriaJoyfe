using GestorEnfermeriaJoyfe.ApplicationLayer.UserApp;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Infraestructure.UserPersistence;
using System;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public class UserCommandAdapter : CommandAdapterBase
    {
        private readonly IUserContract userRepository;

        public UserCommandAdapter(IUserContract userRepository) => this.userRepository = userRepository;


        public async Task<CommandResponse> Register(User user)
        {
            return await RunCommand(async () =>
            {
                return await new UserRegister(userRepository).Run(user);
            });
        }

        public async Task<CommandResponse> Update(User user)
        {
            return await RunCommand(async () =>
            {
                return await new UserUpdater(userRepository).Run(user);
            });
        }

        public async Task<CommandResponse> Delete(int id)
        {
            return await RunCommand(async () =>
            {
                return await new UserDeleter(userRepository).Run(id);
            });
        }
    }
}
