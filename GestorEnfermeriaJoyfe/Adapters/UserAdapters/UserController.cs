using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Infraestructure.UserPersistence;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public class UserController
    {
        private readonly UserMapper userMapper;
        private readonly MySqlUserRepository userRepository;

        private readonly UserQueryAdapter UserQueryAdapter;
        private readonly UserCommandAdapter UserCommandAdapter;

        public UserController() 
        {
            userMapper = new();
            userRepository = new(userMapper);
            UserQueryAdapter = new(userRepository, new());
            UserCommandAdapter = new(userRepository);
        }

        // ================== QUERYS ==================

        public async Task<UserResponse> GetAll() => await UserQueryAdapter.GetAllUsers();
        public async Task<UserResponse> GetAllPaginated(int perPage, int page) => await UserQueryAdapter.GetAllUsersPaginated(perPage, page);
        public async Task<UserResponse> Get(int id) => await UserQueryAdapter.FindUser(id);
        public async Task<UserResponse> Login(string email, SecureString securePassword) => await UserQueryAdapter.AuthUser(email, securePassword);

        // ================== COMMANDS ==================
        public async Task<CommandResponse> Register(User user) => await UserCommandAdapter.Register(user);
        public async Task<CommandResponse> Update(User user) => await UserCommandAdapter.Update(user);
        public async Task<CommandResponse> Delete(int id) => await UserCommandAdapter.Delete(id);
    }

}
