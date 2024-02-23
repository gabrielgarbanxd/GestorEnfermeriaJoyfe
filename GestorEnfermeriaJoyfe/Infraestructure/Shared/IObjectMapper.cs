using System.Data;

namespace GestorEnfermeriaJoyfe.Infraestructure.Shared
{
    public interface IObjectMapper<T> where T : class
    {
        T Map(IDataReader reader);
    }
}
