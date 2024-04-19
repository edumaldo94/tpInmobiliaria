using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioPropietario {

readonly string ConnectionString = "Server=localhost;Database=inmo;User=root;Password=;";



public RepositorioPropietario(){


}

public IList<Propietario> GetProprietors(){
    var proprietors= new List<Propietario>();
    using (var connection = new MySqlConnection(ConnectionString)){

        var sql= @$"SELECT {nameof(Propietario.id_Propietario)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Email)},{nameof(Propietario.Telefono)},{nameof(Propietario.EstadoP)}
        From propietarios  WHERE {nameof(Propietario.EstadoP)} = 1";
        using (var command = new MySqlCommand(sql,connection)){
            connection.Open();
            using (var reader= command.ExecuteReader()){
while (reader.Read())
{
    proprietors.Add(new Propietario{
        id_Propietario = reader.GetInt32(nameof(Propietario.id_Propietario)),
        Nombre = reader.GetString(nameof(Propietario.Nombre)),
        Apellido = reader.GetString(nameof(Propietario.Apellido)),
        Dni = reader.GetString(nameof(Propietario.Dni)),
        Email = reader.GetString(nameof(Propietario.Email)),
        Telefono = reader.GetString(nameof(Propietario.Telefono)),
       EstadoP = reader.GetInt32(nameof(Propietario.EstadoP)),

    });
}

            }
        }
    }


return proprietors;
}

public int High(Propietario proprietors){
    int id=0;
    using(var connection = new MySqlConnection(ConnectionString)){
     var sql = @$"INSERT INTO propietarios ({nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Email)},{nameof(Propietario.Telefono)},{nameof(Propietario.EstadoP)})
        VALUES (@{nameof(Propietario.Nombre)}, @{nameof(Propietario.Apellido)}, @{nameof(Propietario.Dni)}, @{nameof(Propietario.Email)},@{nameof(Propietario.Telefono)}, 1);
        SELECT LAST_INSERT_ID();";

        using (var command= new MySqlCommand(sql, connection)){
            command.Parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", proprietors.Nombre);
            command.Parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", proprietors.Apellido);
            command.Parameters.AddWithValue($"@{nameof(Propietario.Dni)}", proprietors.Dni);
            command.Parameters.AddWithValue($"@{nameof(Propietario.Email)}", proprietors.Email);
             command.Parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", proprietors.Telefono);
           

            connection.Open();
            id= Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
    }
return id;
}
public Propietario? GetProprietorId(int id){
    Propietario? proprietors= null;
    using (var connection = new MySqlConnection(ConnectionString)){

   var sql = @$"SELECT {nameof(Propietario.id_Propietario)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Email)},{nameof(Propietario.Telefono)},{nameof(Propietario.EstadoP)}
        FROM propietarios
        WHERE {nameof(Propietario.id_Propietario)} = @{nameof(Propietario.id_Propietario)}";

        using (var command = new MySqlCommand(sql,connection)){
            command.Parameters.AddWithValue($"@{nameof(Propietario.id_Propietario)}", id);
            connection.Open();
            using (var reader= command.ExecuteReader()){
if (reader.Read())
{
    proprietors =new Propietario{
      id_Propietario = reader.GetInt32(nameof(Propietario.id_Propietario)),
        Nombre = reader.GetString(nameof(Propietario.Nombre)),
        Apellido = reader.GetString(nameof(Propietario.Apellido)),
        Dni = reader.GetString(nameof(Propietario.Dni)),
        Email = reader.GetString(nameof(Propietario.Email)),
        Telefono = reader.GetString(nameof(Propietario.Telefono)),
       EstadoP = reader.GetInt32(nameof(Propietario.EstadoP)),
       

    };
}

            }
        }
    }


return proprietors;
}

public int  Modification(Propietario proprietors)
{
    	int res = -1;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"UPDATE propietarios
                    SET {nameof(Propietario.Nombre)} = @{nameof(Propietario.Nombre)},
                        {nameof(Propietario.Apellido)} = @{nameof(Propietario.Apellido)},
                        {nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)},
                         {nameof(Propietario.Email)} = @{nameof(Propietario.Email)},
                        {nameof(Propietario.Telefono)} = @{nameof(Propietario.Telefono)}
                    WHERE {nameof(Propietario.id_Propietario)} = @id";

        using (var command = new MySqlCommand(sql, connection))
        {
       command.Parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", proprietors.Nombre);
            command.Parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", proprietors.Apellido);
            command.Parameters.AddWithValue($"@{nameof(Propietario.Dni)}", proprietors.Dni);
            command.Parameters.AddWithValue($"@{nameof(Propietario.Email)}", proprietors.Email);
             command.Parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", proprietors.Telefono);
             command.Parameters.AddWithValue($"@id", proprietors.id_Propietario);
            connection.Open();

   res = command.ExecuteNonQuery();
connection.Close();
        }
    }

    return res;
}

public int Low(int id)
{
    int res = -1;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = @$"UPDATE propietarios
                        SET estadoP = 0
                        WHERE {nameof(Propietario.id_Propietario)} = @id";
        
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

     public List<Inmueble> ObtenerInmueblesPorPropietario(int propietarioId)
        {
            List<Inmueble> inmuebles = new List<Inmueble>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                // Consulta para obtener los inmuebles del propietario específico
                var sql = "SELECT * FROM inmuebles WHERE propietarioId = @propietarioId";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@propietarioId", propietarioId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                          
                            Inmueble inmueble = new Inmueble
                            {
                                id_Inmuebles = reader.GetInt32(0),
                                Ubicacion= reader.GetString(4),
                                Direccion= reader.GetString(5),
                                Ambientes = reader.GetInt32(6),
                                Uso = reader.GetString(7),
                                Tipo = reader.GetString(8),
                                Precio = reader.GetDouble(9),
                                Disponible = reader.GetString(10),

                           
                            };
                            inmuebles.Add(inmueble);
                        }
                    }
                }
            }

            return inmuebles;
        }

    

}