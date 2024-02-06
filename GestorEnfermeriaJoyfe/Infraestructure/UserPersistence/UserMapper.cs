using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System;
using System.Data;
using GestorEnfermeriaJoyfe.Domain.User;

namespace GestorEnfermeriaJoyfe.Infraestructure.UserPersistence
{
    public class UserMapper : IObjectMapper
    {
        public T Map<T>(IDataReader reader)
        {
            if (typeof(T) == typeof(User))
            {
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string password = reader.GetString(reader.GetOrdinal("password"));
                string lastName = reader.GetString(reader.GetOrdinal("last_name"));
                string email = reader.GetString(reader.GetOrdinal("email"));

                User user = new User(
                   new UserId(id),
                   new UserName(name),
                   new UserPassword(password),
                   new UserLastName(lastName),
                   new UserEmail(email)
               );

                return (T)(object)user;
            }

            throw new ArgumentException($"No mapper available for type {typeof(T)}");
        }
    }

}
