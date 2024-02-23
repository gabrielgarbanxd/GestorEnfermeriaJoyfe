using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System;

namespace GestorEnfermeriaJoyfe.Domain.User
{
    public class User
    {
        public UserId Id { get; private set; }
        public UserName UserName { get; set; }
        public UserPassword Password { get; set; }
        public UserLastName LastName { get; set; }
        public UserEmail Email { get; set; }

        public User(UserId userId, UserName userName, UserPassword password, UserLastName lastName, UserEmail email)
        {
            Id = userId;
            UserName = userName;
            Password = password;
            LastName = lastName;
            Email = email;
        }

        public static User Create(UserId userId, UserName userName, UserPassword password, UserLastName lastName, UserEmail email)
        {
            return new User(userId, userName, password, lastName, email);
        }

        public static User FromPrimitives(int userId, string userName, string password, string lastName, string email)
        {
            return new User(new UserId(userId), new UserName(userName), new UserPassword(password), new UserLastName(lastName), new UserEmail(email));
        }

        public void SetId(UserId id)
        {
            if (Id.Value != 0)
            {
                throw new InvalidOperationException("El ID ya ha sido establecido.");
            }
            Id = id;
        }
    }
}
