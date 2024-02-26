namespace GestorEnfermeriaJoyfe.Adapters
{
    public class ResponseBase<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ResponseBase(bool success, string? message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public ResponseBase(bool success, string? message)
        {
            Success = success;
            Message = message;
        }

        public ResponseBase()
        {
        }

        public virtual ResponseBase<T> Ok(string? message = null, T? data = default)
        {
            return new ResponseBase<T>(true, message, data);
        }

        // Método Fail para cuando la operación falla
        public virtual ResponseBase<T> Fail(string? message = null)
        {
            return new ResponseBase<T>(false, message);
        }
    }
}
