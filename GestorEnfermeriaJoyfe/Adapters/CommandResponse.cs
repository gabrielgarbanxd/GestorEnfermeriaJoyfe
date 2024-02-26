namespace GestorEnfermeriaJoyfe.Adapters
{
    public class CommandResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? Id { get; set; }

        public CommandResponse(bool success, string? message, int? id)
        {
            Success = success;
            Message = message;
            Id = id;
        }

        public static CommandResponse Ok(string? message = null, int? id = default)
        {
            return new CommandResponse(true, message, id);
        }

        public static CommandResponse Fail(string? message = null)
        {
            return new CommandResponse(false, message, null);
        }
    }
}
