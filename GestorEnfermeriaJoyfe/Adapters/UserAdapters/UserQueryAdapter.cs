using GestorEnfermeriaJoyfe.ApplicationLayer.UserApp;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Infraestructure.UserPersistence;
using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using System.Windows;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public class UserQueryAdapter : QueryAdapterBase<UserResponse, IEnumerable<User>>
    {
        private readonly IUserContract userRepository;

        public UserQueryAdapter(IUserContract userRepository, UserResponse response) : base(response)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserResponse> AuthUser(string email, SecureString securePassword)
        {
            return await RunQuery(async () =>
            {
                UserAuthenticator userAuthenticator = new(userRepository);

                return new List<User> { await userAuthenticator.Run(email, securePassword) };
            });
        }

        public async Task<UserResponse> FindUser(int id)
        {
            return await RunQuery(async () =>
            {
                return new List<User> { await new UserFinder(userRepository).Run(id) };
            });
        }

        public async Task<UserResponse> GetAllUsers()
        {
            return await RunQuery(async () =>
            {
                return await new UserLister(userRepository).Run();
            });
        }

        public async Task<UserResponse> GetAllUsersPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new UserLister(userRepository).Run(true, perPage, page);
            });
        }
    }
}
