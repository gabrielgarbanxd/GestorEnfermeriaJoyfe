using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.User
{
    public interface IUserContract
    {
        Task<IEnumerable<User>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1);
        Task<User> FindAsync(UserId userId);
        Task<User> GetByEmailAsync(UserEmail email);
        Task<int> AddAsync(User user);
        Task<int> UpdateAsync(User user);
        Task<int> DeleteAsync(UserId userId);
    }
}
