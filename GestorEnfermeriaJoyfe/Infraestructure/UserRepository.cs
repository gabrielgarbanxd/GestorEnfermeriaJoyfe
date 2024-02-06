using System;
using System.Collections.Generic;
using System.Net;
using MySql.Data.MySqlClient;
using GestorEnfermeriaJoyfe.Domain;
using System.Windows;

namespace GestorEnfermeriaJoyfe.Infraestructure
{
    // Clase UserRepository: Implementa la interfaz IUserRepository y gestiona la manipulación de datos de usuarios.
    public class UserRepository : RepositoryBase, IUserRepository
    {
        // Método Add: Agrega un nuevo UserModel (usuario) al repositorio (no implementado actualmente).
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        // Método AuthenticateUser: Autentica a un usuario utilizando las credenciales proporcionadas.
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser = false;

            // Usando la conexión a la base de datos proporcionada por RepositoryBase.
            using (var connection = GetSqlConnection())
            {
                connection.Open();

                // Utilizando un objeto MySqlCommand para ejecutar comandos SQL.
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM User WHERE username = @username AND password = @password";
                    command.Parameters.AddWithValue("@username", credential.UserName);
                    command.Parameters.AddWithValue("@password", credential.Password);

                    // Utilizando un lector de datos para leer el resultado de la consulta.
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            validUser = true;
                        }
                    }
                }
            }

            return validUser;
        }

        // Método Edit: Edita la información de un UserModel (no implementado actualmente).
        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        // Método GetAll: Obtiene todos los usuarios (no implementado actualmente).
        public IEnumerable<UserModel> GetAll()
        {
            throw new NotImplementedException();
        }

        // Método GetById: Obtiene un UserModel por su identificador (no implementado actualmente).
        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        // Método GetByUsername: Obtiene un UserModel por su nombre de usuario.
        public UserModel GetByUsername(string username)
        {
            UserModel validUser = null;

            // Usando la conexión a la base de datos proporcionada por RepositoryBase.
            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {

                    // mostramos la excepcion
                    MessageBox.Show(e.ToString());
                }

                // Utilizando un objeto MySqlCommand para ejecutar comandos SQL.
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT Id, Username, Password, Name, LastName, Email FROM User WHERE username = @username";
                    command.Parameters.Add(new MySqlParameter("@username", MySqlDbType.VarChar) { Value = username });

                    // Utilizando un lector de datos para leer el resultado de la consulta.
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Construyendo un UserModel a partir de los datos leídos.
                            validUser = new UserModel()
                            {
                                Id = reader[0].ToString(),
                                UserName = reader[1].ToString(),
                                Password = reader[2].ToString(),
                                Name = reader[3].ToString(),
                                LastName = reader[4].ToString(),
                                Email = reader[5].ToString()
                            };
                        }
                    }
                }

                return validUser;
            }
        }

        // Método Remove: Elimina un UserModel por su identificador (no implementado actualmente).
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
