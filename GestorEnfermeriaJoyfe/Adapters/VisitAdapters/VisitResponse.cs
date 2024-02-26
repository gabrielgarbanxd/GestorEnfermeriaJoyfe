using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.VisitAdapters
{
    public class VisitResponse : Response<IEnumerable<Visit>>
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

        public static new VisitResponse Ok(string? message = null, IEnumerable<Visit>? data = default)
        {
            return new VisitResponse(true, message, data);
        }

        public static new VisitResponse Fail(string? message = null)
        {
            return new VisitResponse(false, message);
        }
    }
}
