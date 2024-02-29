using GestorEnfermeriaJoyfe.Domain.Cites;
using System;
using System.Collections.Generic;

namespace GestorEnfermeriaJoyfe.Adapters.CitesAdapters
{
    public class CitesResponse : ResponseBase<IEnumerable<Cite>>
    {
        public CitesResponse(bool success, string? message, IEnumerable<Cite>? data) : base(success, message, data)
        {
        }

        public CitesResponse(bool success, string? message) : base(success, message)
        {
        }

        public CitesResponse()
        {
        }

        public override CitesResponse Ok(string? message = null, IEnumerable<Cite>? data = default)
        {
            return new CitesResponse(true, message, data);
        }

        public override CitesResponse Fail(string? message = null)
        {
            return new CitesResponse(false, message);
        }
    }
}
