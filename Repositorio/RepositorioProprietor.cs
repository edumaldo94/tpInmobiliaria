using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioProprietor {

readonly string ConnectionString = "Server=localhost;Database=inmobmaldonado;User=root;Password=;";



public RepositorioProprietor(){


}

public IList<Proprietor> GetProprietors(){
    var proprietors= new List<Proprietor>();
    using (var connection = new MySqlConnection(ConnectionString)){

        var sql= @$"SELECT {nameof(Proprietor.id_Proprietor)}, {nameof(Proprietor.Name)}, {nameof(Proprietor.Last_Name)}, {nameof(Proprietor.Birthdate)}, {nameof(Proprietor.Sex)}, {nameof(Proprietor.Address)}, {nameof(Proprietor.Phone)}, {nameof(Proprietor.Dni)}, {nameof(Proprietor.Email)},{nameof(Proprietor.StateP)}
        From proprietor  WHERE {nameof(Proprietor.StateP)} = 1";
        using (var command = new MySqlCommand(sql,connection)){
            connection.Open();
            using (var reader= command.ExecuteReader()){
while (reader.Read())
{
    proprietors.Add(new Proprietor{
        id_Proprietor = reader.GetInt32(nameof(Proprietor.id_Proprietor)),
        Name = reader.GetString(nameof(Proprietor.Name)),
        Last_Name = reader.GetString(nameof(Proprietor.Last_Name)),
        Dni = reader.GetInt32(nameof(Proprietor.Dni)),
        Birthdate = reader.GetDateTime(nameof(Proprietor.Birthdate)),
        Sex = reader.GetChar(nameof(Proprietor.Sex)),
        Address = reader.GetString(nameof(Proprietor.Address)),
        Phone = reader.GetString(nameof(Proprietor.Phone)),
        Email = reader.GetString(nameof(Proprietor.Email)),
       StateP = reader.GetInt32(nameof(Proprietor.StateP)),

    });
}

            }
        }
    }


return proprietors;
}

public int High(Proprietor proprietors){
    int id=0;
    using(var connection = new MySqlConnection(ConnectionString)){
     var sql = @$"INSERT INTO proprietor ({nameof(Proprietor.Name)},{nameof(Proprietor.Last_Name)},{nameof(Proprietor.Dni)},{nameof(Proprietor.Birthdate)},{nameof(Proprietor.Sex)},{nameof(Proprietor.Address)},{nameof(Proprietor.Phone)},{nameof(Proprietor.Email)},{nameof(Proprietor.StateP)})
        VALUES (@{nameof(Proprietor.Name)},@{nameof(Proprietor.Last_Name)},@{nameof(Proprietor.Dni)},@{nameof(Proprietor.Birthdate)},@{nameof(Proprietor.Sex)},@{nameof(Proprietor.Address)},@{nameof(Proprietor.Phone)},@{nameof(Proprietor.Email)}, 1);
        SELECT LAST_INSERT_ID();";

        using (var command= new MySqlCommand(sql, connection)){
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Name)}", proprietors.Name);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Last_Name)}", proprietors.Last_Name);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Dni)}", proprietors.Dni);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Birthdate)}", proprietors.Birthdate);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Sex)}", proprietors.Sex);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Address)}", proprietors.Address);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Phone)}", proprietors.Phone);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Email)}", proprietors.Email);
            

            connection.Open();
            id= Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
    }
return id;
}
public Proprietor? GetProprietorId(int id){
    Proprietor? proprietors= null;
    using (var connection = new MySqlConnection(ConnectionString)){

   var sql = @$"SELECT {nameof(Proprietor.id_Proprietor)},{nameof(Proprietor.Name)},{nameof(Proprietor.Last_Name)},{nameof(Proprietor.Birthdate)},{nameof(Proprietor.Sex)},{nameof(Proprietor.Address)},{nameof(Proprietor.Phone)},{nameof(Proprietor.Dni)},{nameof(Proprietor.Email)}
        FROM proprietor
        WHERE {nameof(Proprietor.id_Proprietor)} = @{nameof(Proprietor.id_Proprietor)}";

        using (var command = new MySqlCommand(sql,connection)){
            command.Parameters.AddWithValue($"@{nameof(Proprietor.id_Proprietor)}", id);
            connection.Open();
            using (var reader= command.ExecuteReader()){
if (reader.Read())
{
    proprietors =new Proprietor{
        id_Proprietor = reader.GetInt32(nameof(Proprietor.id_Proprietor)),
        Name = reader.GetString(nameof(Proprietor.Name)),
        Last_Name = reader.GetString(nameof(Proprietor.Last_Name)),
        Dni = reader.GetInt32(nameof(Proprietor.Dni)),
        Birthdate = reader.GetDateTime(nameof(Proprietor.Birthdate)),
        Sex = reader.GetChar(nameof(Proprietor.Sex)),
        Address = reader.GetString(nameof(Proprietor.Address)),
        Phone = reader.GetString(nameof(Proprietor.Phone)),
        Email = reader.GetString(nameof(Proprietor.Email)),
       

    };
}

            }
        }
    }


return proprietors;
}

public int  Modificacion(Proprietor proprietors)
{
    	int res = -1;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"UPDATE proprietor
                    SET {nameof(Proprietor.Name)} = @{nameof(Proprietor.Name)},
                        {nameof(Proprietor.Last_Name)} = @{nameof(Proprietor.Last_Name)},
                        {nameof(Proprietor.Dni)} = @{nameof(Proprietor.Dni)},
                        {nameof(Proprietor.Birthdate)} = @{nameof(Proprietor.Birthdate)},
                        {nameof(Proprietor.Sex)} = @{nameof(Proprietor.Sex)},
                        {nameof(Proprietor.Address)} = @{nameof(Proprietor.Address)},
                        {nameof(Proprietor.Phone)} = @{nameof(Proprietor.Phone)},
                        {nameof(Proprietor.Email)} = @{nameof(Proprietor.Email)}
                    WHERE {nameof(Proprietor.id_Proprietor)} = @{nameof(Proprietor.id_Proprietor)}";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Name)}", proprietors.Name);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Last_Name)}", proprietors.Last_Name);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Dni)}", proprietors.Dni);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Birthdate)}", proprietors.Birthdate);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Sex)}", proprietors.Sex);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Address)}", proprietors.Address);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Phone)}", proprietors.Phone);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.Email)}", proprietors.Email);
            command.Parameters.AddWithValue($"@{nameof(Proprietor.id_Proprietor)}", proprietors.id_Proprietor);

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
        string sql = @$"UPDATE proprietor
                        SET stateP = 0
                        WHERE {nameof(Proprietor.id_Proprietor)} = @id";
        
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