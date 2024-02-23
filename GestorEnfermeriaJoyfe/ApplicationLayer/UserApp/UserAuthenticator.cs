using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System.Runtime.InteropServices;
using System.Security;
using System;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.UserApp
{
    public class UserAuthenticator
    {
        private readonly IUserContract _userRepository;

        public UserAuthenticator(IUserContract userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Run(string email, SecureString securePassword)
        {
            var user = await _userRepository.GetByEmailAsync(new UserEmail(email));

            // Convertir SecureString a String para la verificación
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                var password = Marshal.PtrToStringUni(unmanagedString) ?? string.Empty;


                // Verificar la contraseña
                if (user.Password.VerifyPassword(password))
                {
                    return user.Id.Value;
                }
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }

            return -1;
        }
    }

}
