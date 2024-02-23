using GestorEnfermeriaJoyfe.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.UserApp
{
    public class UserLister
    {
        private readonly IUserContract _userRepository;

        public UserLister(IUserContract userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> Run(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _userRepository.GetAllAsync(paginated, perPage, page);
            }

            return await _userRepository.GetAllAsync();
        }
    }
}
