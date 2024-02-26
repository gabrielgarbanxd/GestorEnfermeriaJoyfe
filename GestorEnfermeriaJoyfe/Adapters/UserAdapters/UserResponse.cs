using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Domain.Visit;
using System.Collections.Generic;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public class UserResponse : ResponseBase<IEnumerable<User>>
    {
        public UserResponse(bool success, string? message, IEnumerable<User>? data) : base(success, message, data)
        {
        }

        public UserResponse(bool success, string? message) : base(success, message)
        {
        }

        public UserResponse()
        {
        }

        public override ResponseBase<IEnumerable<User>> Ok(string? message = null, IEnumerable<User>? data = null)
        {
            return new UserResponse(true, message, data);
        }

        public override ResponseBase<IEnumerable<User>> Fail(string? message = null)
        {
            return new UserResponse(false, message);
        }
    }
}
