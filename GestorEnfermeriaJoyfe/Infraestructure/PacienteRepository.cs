using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using GestorEnfermeriaJoyfe.Domain;

namespace GestorEnfermeriaJoyfe.Infraestructure
{
    public class PacienteRepository : UserRepository, IPacienteRepository
    {
        public List<PacienteModel> All()
        {
            List<PacienteModel> pacientes = new List<PacienteModel>();

            using (var connection = GetSqlConnection())
            {
                connection.Open();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Pacientes";

                    // Utilizando un lector de datos para leer el resultado de la consulta.
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pacientes.Add(new PacienteModel()
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                Apellido1 = reader.GetString("apellido1"),
                                Apellido2 = reader.GetString("apellido2"),
                                Curso = reader.GetString("curso"),
                            });
                        }
                    }
                }

            }
            return pacientes;
        }

        public PacienteModel Create(PacienteModel model)
        {
            try
            {
                using (var connection = GetSqlConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO Pacientes (nombre, apellido1, apellido2, curso) VALUES (@nombre, @apellido1, @apellido2, @curso); SELECT LAST_INSERT_ID();";
                        command.Parameters.AddWithValue("@nombre", model.Nombre);
                        command.Parameters.AddWithValue("@apellido1", model.Apellido1);
                        command.Parameters.AddWithValue("@apellido2", model.Apellido2);
                        command.Parameters.AddWithValue("@curso", model.Curso);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            model.Id = Convert.ToInt32(result);
                            return model;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return null;
        }


        public bool Delete(int id)
        {
            try
            {
                using (var connection = GetSqlConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "DELETE FROM Pacientes WHERE id = @id";
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(PacienteModel model)
        {
            try
            {
                using (var connection = GetSqlConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "UPDATE Pacientes SET nombre = @nombre, apellido1 = @apellido1, apellido2 = @apellido2, curso = @curso WHERE id = @id";
                        command.Parameters.AddWithValue("@id", model.Id);
                        command.Parameters.AddWithValue("@nombre", model.Nombre);
                        command.Parameters.AddWithValue("@apellido1", model.Apellido1);
                        command.Parameters.AddWithValue("@apellido2", model.Apellido2);
                        command.Parameters.AddWithValue("@curso", model.Curso);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        PacienteModel IPacienteRepository.GetById(int id)
        {
            PacienteModel paciente = null;

            using (var connection = GetSqlConnection())
            {
                connection.Open();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Pacientes WHERE id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            paciente = new PacienteModel()
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                Apellido1 = reader.GetString("apellido1"),
                                Apellido2 = reader.GetString("apellido2"),
                                Curso = reader.GetString("curso"),
                            };
                        }
                    }
                }
            }

            return paciente;
        }
    }
}
