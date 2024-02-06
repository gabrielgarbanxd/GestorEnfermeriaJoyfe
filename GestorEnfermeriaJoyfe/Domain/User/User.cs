using GestorEnfermeriaJoyfe.Domain.User.ValueObjects;
using System;

namespace GestorEnfermeriaJoyfe.Domain.User
{
    public class User
    {
        public UserId? Id { get; private set; }
        public UserName UserName { get; private set; }
        public UserPassword Password { get; private set; }
        public UserLastName LastName { get; private set; }
        public UserEmail Email { get; private set; }

        public User(UserName userName, UserPassword password, UserLastName lastName, UserEmail email, UserId? userId = null)
        {
            Id = userId;
            UserName = userName;
            Password = password;
            LastName = lastName;
            Email = email;
        }

        public static User Create(UserName userName, UserPassword password, UserLastName lastName, UserEmail email, UserId? userId = null)
        {
            return new User(userName, password, lastName, email, userId);
        }

        public static User FromPrimitives(int userId, string userName, string password, string lastName, string email)
        {
            return new User(new UserName(userName), new UserPassword(password), new UserLastName(lastName), new UserEmail(email), new UserId(userId));
        }

        public void SetId(UserId id)
        {
            if (Id != null)
            {
                throw new InvalidOperationException("El ID ya ha sido establecido.");
            }
            Id = id;
        }

    }
}
