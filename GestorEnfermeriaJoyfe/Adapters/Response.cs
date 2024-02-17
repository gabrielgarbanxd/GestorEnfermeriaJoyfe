using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters
{
    public class Response
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public Response(bool success, string? message, object? data)
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

        public static Response Ok(string? message = null, object ? data = null)
        {
            return new Response(true, message, data);
        }

        public static Response Fail(string? message = null, object? data = null)
        {
            return new Response(false, message, data);
        }
    }
}
