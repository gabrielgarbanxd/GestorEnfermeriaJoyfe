using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public class UserRequester : RequesterBase
    {
        public UserRequester() : base(new Dictionary<string, Func<IDictionary<string, object>, Task<Response>>>
        {
            {"login", UserQueryAdapter.AuthUser},
            {"register", UserCommandAdapter.RegisterUser},
            {"index", UserQueryAdapter.GetAllUsers},
            {"show", UserQueryAdapter.FindUser},
            {"update", UserCommandAdapter.UpdateUser},
            {"destroy", UserCommandAdapter.DeleteUser}
            // Agrega más solicitudes aquí
        })
        {
        }
    }

}
