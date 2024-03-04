using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using System.Collections.Generic;

namespace GestorEnfermeriaJoyfe.Adapters.VisitTemplateAdapters
{
    public class VisitTemplateResponse : ResponseBase<IEnumerable<VisitTemplate>>
    {
        public VisitTemplateResponse(bool success, string? message, IEnumerable<VisitTemplate>? data) : base(success, message, data)
        {
        }

        public VisitTemplateResponse(bool success, string? message) : base(success, message)
        {
        }

        public VisitTemplateResponse()
        {
        }

        public override ResponseBase<IEnumerable<VisitTemplate>> Ok(string? message = null, IEnumerable<VisitTemplate>? data = null)
        {
            return new VisitTemplateResponse(true, message, data);
        }

        public override ResponseBase<IEnumerable<VisitTemplate>> Fail(string? message = null)
        {
            return new VisitTemplateResponse(false, message);
        }
    }
}
