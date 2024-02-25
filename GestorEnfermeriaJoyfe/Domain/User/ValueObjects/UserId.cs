using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.User.ValueObjects
{
    public class UserId : IdValueObject
    {
        public UserId(int value) : base(value) { }
    }
}
