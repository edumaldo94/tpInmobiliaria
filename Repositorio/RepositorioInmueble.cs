using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioInmueble
{

    readonly string ConnectionString = "Server=localhost;Database=inmo;User=root;Password=;";



    public RepositorioInmueble()
    {


    }

    public IList<Inmueble> GetProperties()
    {
        var properties = new List<Inmueble>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @"SELECT 
                        prop.id_Inmuebles, 
                        prop.propietarioId, 
                        prop.direccion, 
                        prop.ambientes, 
                        prop.uso, 
                        prop.tipo, 
                        prop.precio, 
                        prop.disponible,
                        prop.estadoIn,
                        pro.nombre AS ProprietorName,
                        pro.apellido AS ProprietorLastName
                    FROM 
                        inmuebles prop
                    INNER JOIN 
                        propietarios pro ON prop.propietarioId = pro.id_Propietario
                        WHERE prop.estadoIn = 1";

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        properties.Add(new Inmueble
                        {
                            id_Inmuebles = reader.GetInt32(0),
                            PropietarioId = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetInt32(3),
                            Uso = reader.GetString(4),
                            Tipo = reader.GetString(5),
                            Precio = reader.GetFloat(6),
                            Disponible = reader.GetString(7),
                            EstadoIn = reader.GetInt32(8),
                            ProprietorName = reader.IsDBNull(reader.GetOrdinal("ProprietorName")) ? null : reader.GetString("ProprietorName"),
                            ProprietorLastName = reader.IsDBNull(reader.GetOrdinal("ProprietorLastName")) ? null : reader.GetString("ProprietorLastName")
                        });
                    }
                }
            }
        }

        return properties;
    }


    public int Create(Inmueble property)
    {
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO inmuebles ({nameof(Inmueble.PropietarioId)}, {nameof(Inmueble.Latitud)}, {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Ubicacion)}, {nameof(Inmueble.Direccion)}, {nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.Disponible)}, {nameof(Inmueble.EstadoIn)})
                    VALUES (@{nameof(Inmueble.PropietarioId)}, @{nameof(Inmueble.Latitud)}, @{nameof(Inmueble.Longitud)}, @{nameof(Inmueble.Ubicacion)}, @{nameof(Inmueble.Direccion)}, @{nameof(Inmueble.Ambientes)}, @{nameof(Inmueble.Uso)}, @{nameof(Inmueble.Tipo)}, @{nameof(Inmueble.Precio)}, @{nameof(Inmueble.Disponible)}, 1);
                    SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.PropietarioId)}", property.PropietarioId);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Latitud)}", property.Latitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Longitud)}", property.Longitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Ubicacion)}", property.Ubicacion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", property.Direccion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Ambientes)}", property.Ambientes);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Uso)}", property.Uso);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Tipo)}", property.Tipo);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Precio)}", property.Precio);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Disponible)}", property.Disponible);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.EstadoIn)}", property.EstadoIn);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return id;
    }


    public Inmueble ObtenerPorId(int id)
    {
        Inmueble? p = null;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            string sql = $"SELECT * FROM Inmuebles" +
                $" WHERE id_Inmuebles=@id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    p = new Inmueble
                    {
                        id_Inmuebles = reader.GetInt32(0),
                        PropietarioId = reader.GetInt32(1),
                        Latitud = reader.IsDBNull(2) ? null : reader.GetFloat(2),
                        Longitud = reader.IsDBNull(3) ? null : reader.GetFloat(3),
                        Ubicacion = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Direccion = reader.GetString(5),
                        Ambientes = reader.GetInt32(6),
                        Uso = reader.GetString(7),
                        Tipo = reader.GetString(8),
                        Precio = reader.GetFloat(9),
                        Disponible = reader.GetString(10),
                        EstadoIn = reader.GetInt32(11),

                    };
                }
                connection.Close();
            }
        }

        return p;
    }

    public void Modificacion(Inmueble inmueble)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @"UPDATE inmuebles
                    SET PropietarioId = @PropietarioId,
                        Latitud = @Latitud,
                        Longitud = @Longitud,
                        Ubicacion = @Ubicacion,
                        Direccion = @Direccion,
                        Ambientes = @Ambientes,
                        Uso = @Uso,
                        Tipo = @Tipo,
                        Precio = @Precio,
                        Disponible = @Disponible,
                        EstadoIn = @EstadoIn
                    WHERE id_Inmuebles = @id_Inmuebles";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@PropietarioId", inmueble.PropietarioId);
                command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
                command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
                command.Parameters.AddWithValue("@Ubicacion", inmueble.Ubicacion);
                command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@Uso", inmueble.Uso);
                command.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
                command.Parameters.AddWithValue("@Precio", inmueble.Precio);
                command.Parameters.AddWithValue("@Disponible", inmueble.Disponible);
                command.Parameters.AddWithValue("@EstadoIn", inmueble.EstadoIn);
                command.Parameters.AddWithValue("@id_Inmuebles", inmueble.id_Inmuebles);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public int Low(int id)
    {
        int res = -1;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = @$"UPDATE inmuebles
                        SET estadoIn = 0
                        WHERE {nameof(Inmueble.id_Inmuebles)} = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
}