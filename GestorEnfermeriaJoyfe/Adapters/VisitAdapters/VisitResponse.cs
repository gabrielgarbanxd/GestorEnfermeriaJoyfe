using GestorEnfermeriaJoyfe.Domain.Visit;
using System.Collections.Generic;

namespace GestorEnfermeriaJoyfe.Adapters.VisitAdapters
{
    public class VisitResponse : ResponseBase<IEnumerable<Visit>>
    {
        public VisitResponse(bool success, string? message, IEnumerable<Visit>? data) : base(success, message, data)
        {
        }

        public VisitResponse(bool success, string? message) : base(success, message)
        {
        }

        public VisitResponse()
        {
        }

        public override ResponseBase<IEnumerable<Visit>> Ok(string? message = null, IEnumerable<Visit>? data = null)
        {
            return new VisitResponse(true, message, data);
        }

        public override ResponseBase<IEnumerable<Visit>> Fail(string? message = null)
        {
            return new VisitResponse(false, message);
        }
    }
}
