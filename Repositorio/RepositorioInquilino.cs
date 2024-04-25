using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioInquilino {

readonly string ConnectionString = "Server=localhost;Database=inmo;User=root;Password=;";



public RepositorioInquilino(){


}

public IList<Inquilino> GetTenants(){
    var tenants= new List<Inquilino>();
    using (var connection = new MySqlConnection(ConnectionString)){

        var sql= @$"SELECT {nameof(Inquilino.id_Inquilino)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, 
        {nameof(Inquilino.Dni)}, {nameof(Inquilino.Email)},{nameof(Inquilino.Telefono)},{nameof(Inquilino.Estado)}
        From inquilinos  WHERE {nameof(Inquilino.Estado)} = 1";
        using (var command = new MySqlCommand(sql,connection)){
            connection.Open();
            using (var reader= command.ExecuteReader()){
while (reader.Read())
{
    tenants.Add(new Inquilino{
      id_Inquilino = reader.GetInt32(nameof(Inquilino.id_Inquilino)),
        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
        Dni = reader.GetString(nameof(Inquilino.Dni)),
        Email = reader.GetString(nameof(Inquilino.Email)),
        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
       Estado = reader.GetString(nameof(Inquilino.Estado)),

    });
    
}

            }
        }
    }


return tenants;
}

public int High(Inquilino tenants){
    int id=0;
    using(var connection = new MySqlConnection(ConnectionString)){
          var sql = @$"INSERT INTO inquilinos ({nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Email)},{nameof(Inquilino.Telefono)},{nameof(Inquilino.Estado)})
                    VALUES  (@{nameof(Inquilino.Nombre)}, @{nameof(Inquilino.Apellido)}, @{nameof(Inquilino.Dni)}, @{nameof(Inquilino.Email)},@{nameof(Inquilino.Telefono)}, 1);
                    SELECT LAST_INSERT_ID();";

        using (var command= new MySqlCommand(sql, connection)){
             command.Parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", tenants.Nombre);
            command.Parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", tenants.Apellido);
            command.Parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", tenants.Dni);
            command.Parameters.AddWithValue($"@{nameof(Inquilino.Email)}", tenants.Email);
             command.Parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", tenants.Telefono);
            

            connection.Open();
            id= Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
    }
return id;
}
public Inquilino? GetTenantId(int id){
    Inquilino? tenants= null;
    using (var connection = new MySqlConnection(ConnectionString)){

   var sql = @$"SELECT {nameof(Inquilino.id_Inquilino)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Email)},{nameof(Inquilino.Telefono)},{nameof(Inquilino.Estado)}
        FROM inquilinos
        WHERE {nameof(Inquilino.id_Inquilino)} = @{nameof(Inquilino.id_Inquilino)}";

        using (var command = new MySqlCommand(sql,connection)){
            command.Parameters.AddWithValue($"@{nameof(Inquilino.id_Inquilino)}", id);
            connection.Open();
            using (var reader= command.ExecuteReader()){
if (reader.Read())
{
    tenants =new Inquilino{
           id_Inquilino = reader.GetInt32(nameof(Inquilino.id_Inquilino)),
        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
        Dni = reader.GetString(nameof(Inquilino.Dni)),
        Email = reader.GetString(nameof(Inquilino.Email)),
        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
       Estado = reader.GetString(nameof(Inquilino.Estado)),
       

    };
}

            }
        }
    }


return tenants;
}

public int  Modification(Inquilino tenants)
{
    	int res = -1;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"UPDATE inquilinos
                       SET {nameof(Inquilino.Nombre)} = @{nameof(Inquilino.Nombre)},
                        {nameof(Inquilino.Apellido)} = @{nameof(Inquilino.Apellido)},
                        {nameof(Inquilino.Dni)} = @{nameof(Inquilino.Dni)},
                         {nameof(Inquilino.Email)} = @{nameof(Inquilino.Email)},
                        {nameof(Inquilino.Telefono)} = @{nameof(Inquilino.Telefono)}
                    WHERE {nameof(Inquilino.id_Inquilino)} = @id";

        using (var command = new MySqlCommand(sql, connection))
        {
              command.Parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", tenants.Nombre);
            command.Parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", tenants.Apellido);
            command.Parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", tenants.Dni);
            command.Parameters.AddWithValue($"@{nameof(Inquilino.Email)}", tenants.Email);
             command.Parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", tenants.Telefono);
            command.Parameters.AddWithValue($"@id", tenants.id_Inquilino);
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
        string sql = @$"UPDATE inquilinos
                        SET estado = 0
                        WHERE {nameof(Inquilino.id_Inquilino)} = @id";
        
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