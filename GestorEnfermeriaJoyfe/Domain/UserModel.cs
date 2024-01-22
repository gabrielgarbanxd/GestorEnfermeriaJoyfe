
namespace GestorEnfermeriaJoyfe.Domain
{
    // Clase UserModel: Representa un modelo de datos para la información de un usuario.
    public class UserModel
    {
        // Propiedad Id: Almacena el identificador único del usuario.
        public string Id { get; set; }

        // Propiedad UserName: Almacena el nombre de usuario del usuario.
        public string UserName { get; set; }

        // Propiedad Password: Almacena la contraseña asociada al usuario (se recomienda utilizar SecureString para contraseñas).
        public string Password { get; set; }

        // Propiedad Name: Almacena el nombre del usuario.
        public string Name { get; set; }

        // Propiedad LastName: Almacena el apellido del usuario.
        public string LastName { get; set; }

        // Propiedad Email: Almacena la dirección de correo electrónico asociada al usuario.
        public string Email { get; set; }
    }
}
