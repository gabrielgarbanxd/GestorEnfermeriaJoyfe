using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure.Shared
{
    public abstract class MySqlRepositoryBase<T> where T : class
    {
        private readonly MySqlConnection _connection;
        private readonly IObjectMapper<T>? _mapper;
        private readonly string _connectionString = "Server=localhost;Port=3306;Database=gestor_enfermeria;User=root;Password=1234;";

        public MySqlRepositoryBase(IObjectMapper<T> mapper)
        {
            _connection = new MySqlConnection(_connectionString);
            _mapper = mapper;
        }

        public MySqlRepositoryBase()
        {
            _connection = new MySqlConnection(_connectionString);
            _mapper = null;
        }

        protected MySqlConnection GetSqlConnection()
        {
            return _connection;
        }

        protected async Task<IEnumerable<T>> ExecuteQueryAsync(string storedProcedure, IDictionary<string, object>? parameters = null)
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
                            result.Add(MapToEntity(reader));
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

                    // Configurar el parámetro de salida
                    var resultParameter = command.Parameters.Add("@p_Result", MySqlDbType.Int32);
                    resultParameter.Direction = ParameterDirection.Output;

                    await command.ExecuteNonQueryAsync();

                    // Obtener el valor del parámetro de salida
                    int result = (int)resultParameter.Value;
                    return result;
                }
            }
        }

        protected void SetParameters(MySqlCommand command, IDictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
        }


        protected T MapToEntity(IDataReader reader)
        {
            if (_mapper == null)
            {
                throw new InvalidOperationException("No mapper provided");
            }
            return _mapper.Map(reader);
        }
    }
}
