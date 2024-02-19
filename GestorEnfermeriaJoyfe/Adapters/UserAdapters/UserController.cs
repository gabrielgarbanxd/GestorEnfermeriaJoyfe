using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public class UserController
    {
        /// <summary>
        /// Intenta autenticar a un usuario con las credenciales proporcionadas.
        /// </summary>
        /// <param name="data">
        ///     Un objeto dinámico que contiene los datos necesarios para autenticar al usuario.
        ///     Se espera que tenga las siguientes propiedades:
        ///     - "Email": El correo electrónico del usuario.
        ///     - "Password": La contraseña del usuario.
        /// </param>
        /// <returns>Una respuesta que indica si la autenticación fue exitosa o no.</returns>
        public static async Task<Response> Login(dynamic data)
        {
            return await UserQueryAdapter.AuthUser(data);
        }

        /// <summary>
        /// Registra un nuevo usuario con la información proporcionada.
        /// </summary>
        /// <param name="data">
        ///     Un objeto dinámico que contiene los datos necesarios para registrar al usuario.
        ///     Se espera que tenga las siguientes propiedades:
        ///     - "Name": El nombre del usuario.
        ///     - "LastName": El apellido del usuario.
        ///     - "Email": El correo electrónico del usuario.
        ///     - "Password": La contraseña del usuario.
        /// </param>
        /// <returns>Una respuesta que indica si el registro fue exitoso o no.</returns>
        public static async Task<Response> Register(dynamic data)
        {
            return await UserCommandAdapter.RegisterUser(data);
        }

    }

}
