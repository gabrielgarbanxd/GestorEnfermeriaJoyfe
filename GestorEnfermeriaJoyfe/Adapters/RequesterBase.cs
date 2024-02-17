using GestorEnfermeriaJoyfe.Adapters.UserAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters
{
    public abstract class RequesterBase
    {
        protected readonly Dictionary<string, Func<IDictionary<string, object>, Task<Response>>> RequestTo;

        protected RequesterBase(Dictionary<string, Func<IDictionary<string, object>, Task<Response>>> requestTo)
        {
            RequestTo = requestTo;
        }

        public async Task<Response> Request(string request, IDictionary<string, object> data)
        {
            try
            {
                return await RequestTo[request](data);
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
        }

        public Task<Response> Request(string request)
        {
            return Request(request, new Dictionary<string, object>());
        }
    }

}
