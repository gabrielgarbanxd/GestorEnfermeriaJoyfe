using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure
{
    public abstract class MySqlRepositoryBase
    {
        private readonly MySqlConnection _connection;

        public MySqlRepositoryBase()
        {
            string connectionString = "Server=localhost;Port=3309;Database=login;User=root;Password=joyfe;";
            _connection = new MySqlConnection(connectionString);
        }

        protected MySqlConnection GetSqlConnection()
        {
            return _connection;
        }

        protected async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string storedProcedure, IDictionary<string, object>? parameters = null)
        {
            using (var connection = _connection)
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        SetParameters(command, parameters);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var result = new List<T>();

                        while (await reader.ReadAsync())
                        {
                            result.Add(MapToEntity<T>(reader));
                        }

                        return result;
                    }
                }
            }
        }

        protected async Task<int> ExecuteNonQueryAsync(string storedProcedure, IDictionary<string, object>? parameters = null)
        {
            using (var connection = _connection)
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        SetParameters(command, parameters);
                    }

                    await command.ExecuteNonQueryAsync();

                    var resultParameter = command.Parameters["@Result"];
                    int result = (int)resultParameter.Value;
                    return result;
                }
            }
        }

        protected void SetParameters(MySqlCommand command, IDictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
            }
        }


        protected abstract T MapToEntity<T>(IDataReader reader);
    }
}
