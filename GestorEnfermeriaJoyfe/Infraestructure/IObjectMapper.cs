using System.Data;

namespace GestorEnfermeriaJoyfe.Infraestructure
{
    public interface IObjectMapper
    {
        T Map<T>(IDataReader reader);
    }
}
