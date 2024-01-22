using System.Collections.Generic;
using System.Net;

namespace GestorEnfermeriaJoyfe.Domain
{
    // Interfaz IUserRepository: Define operaciones CRUD para la gestión de usuarios.
    public interface IUserRepository
    {
        // Autentica a un usuario utilizando las credenciales proporcionadas.
        bool AuthenticateUser(NetworkCredential credential);

        // Agrega un nuevo UserModel a la fuente de datos.
        void Add(UserModel userModel);

        // Edita un UserModel existente en la fuente de datos.
        void Edit(UserModel userModel);

        // Elimina un UserModel de la fuente de datos por su identificador.
        void Remove(int id);

        // Obtiene un UserModel de la fuente de datos por su identificador.
        UserModel GetById(int id);

        // Obtiene un UserModel de la fuente de datos por su nombre de usuario.
        UserModel GetByUsername(string username);

        // Obtiene todos los UserModel de la fuente de datos.
        IEnumerable<UserModel> GetAll();

        // Otros métodos relacionados con la gestión de usuarios...
    }
}
