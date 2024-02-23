using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System;
using System.Data;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;

namespace GestorEnfermeriaJoyfe.Infraestructure.UserPersistence
{
    public class UserMapper : IObjectMapper<User>
    {
        public User Map(IDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("id"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            string password = reader.GetString(reader.GetOrdinal("password"));
            string lastName = reader.GetString(reader.GetOrdinal("last_name"));
            string email = reader.GetString(reader.GetOrdinal("email"));

            User user = new(
               new UserId(id),
               new UserName(name),
               new UserPassword(password),
               new UserLastName(lastName),
               new UserEmail(email)
           );

            return user;
        }
    }

}
