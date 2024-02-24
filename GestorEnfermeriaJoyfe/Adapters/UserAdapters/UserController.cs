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
            UserQueryAdapter = new(userRepository);
            UserCommandAdapter = new(userRepository);
        }

        public async Task<Response<List<User>>> GetAll() => await UserQueryAdapter.GetAllUsers();

        public async Task<Response<List<User>>> GetAllPaginated(int perPage, int page) => await UserQueryAdapter.GetAllUsersPaginated(perPage, page);

        public async Task<Response<User>> Get(int id) => await UserQueryAdapter.FindUser(id);

        public async Task<Response<int>> Login(string email, SecureString securePassword) => await UserQueryAdapter.AuthUser(email, securePassword);

        public async Task<Response<int>> Register(User user) => await UserCommandAdapter.RegisterUser(user);

        public async Task<Response<bool>> Update(User user) => await UserCommandAdapter.UpdateUser(user);

        public async Task<Response<bool>> Delete(dynamic data) => await UserCommandAdapter.DeleteUser(data);


    }

}
