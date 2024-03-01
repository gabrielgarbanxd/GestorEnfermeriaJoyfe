using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects
{
    public class CiteDate : DateTimeValueObject
    {
        public CiteDate(DateTime value) : base(value) { }
    }
}

