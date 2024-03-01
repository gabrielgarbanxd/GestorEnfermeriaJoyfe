using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;

namespace GestorEnfermeriaJoyfe.Adapters.CiteAdapters
{
    public class CiteResponse : ResponseBase<IEnumerable<Cite>>
    {
        public CiteResponse(bool success, string? message, IEnumerable<Cite>? data) : base(success, message, data)
        {
        }

        public CiteResponse(bool success, string? message) : base(success, message)
        {
        }

        public CiteResponse()
        {
        }

        public override CiteResponse Ok(string? message = null, IEnumerable<Cite>? data = default)
        {
            return new CiteResponse(true, message, data);
        }

        public override CiteResponse Fail(string? message = null)
        {
            return new CiteResponse(false, message);
        }
    }
}
