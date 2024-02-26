using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public Response(bool success, string? message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public Response(bool success, string? message)
        {
            Success = success;
            Message = message;
        }

        public Response()
        {
        }

        // Método Ok para cuando la operación es exitosa
        public static Response<T> Ok(string? message = null, T? data = default)
        {
            return new Response<T>(true, message, data);
        }

        // Método Fail para cuando la operación falla
        public static Response<T> Fail(string? message = null)
        {
            return new Response<T>(false, message);
        }

        public virtual Response<T> Ok2(string? message = null, T? data = default)
        {
            return new Response<T>(true, message, data);
        }

        // Método Fail para cuando la operación falla
        public virtual Response<T> Fail2(string? message = null)
        {
            return new Response<T>(false, message);
        }
    }

}
