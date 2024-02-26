using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters
{
    public interface IResponse<T>
    {
        bool Success { get; set; }
        string? Message { get; set; }
        T? Data { get; set; }

        // Método Ok para cuando la operación es exitosa
        IResponse<T> Ok(string? message = null, T? data = default);

        // Método Fail para cuando la operación falla
        IResponse<T> Fail(string? message = null);


    }

}
