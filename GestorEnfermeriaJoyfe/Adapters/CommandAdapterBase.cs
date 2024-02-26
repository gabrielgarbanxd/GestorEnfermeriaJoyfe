using System;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters
{
    public class CommandAdapterBase
    {
        public async Task<CommandResponse> RunCommand(Func<Task<int>> command, string? message = null)
        {
            try
            {
                return CommandResponse.Ok(message, await command());
            }
            catch (Exception e)
            {
                return CommandResponse.Fail(e.Message);
            }
        }
    }
}
