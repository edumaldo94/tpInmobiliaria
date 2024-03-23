using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioTenant {

readonly string ConnectionString = "Server=localhost;Database=inmobmaldonado;User=root;Password=;";



public RepositorioTenant(){


}

public IList<Tenant> GetTenants(){
    var tenants= new List<Tenant>();
    using (var connection = new MySqlConnection(ConnectionString)){

        var sql= @$"SELECT {nameof(Tenant.id_Tenant)}, {nameof(Tenant.Name)}, {nameof(Tenant.Last_Name)}, {nameof(Tenant.Birthdate)}, {nameof(Tenant.Sex)}, {nameof(Tenant.Address)}, {nameof(Tenant.Phone)}, {nameof(Tenant.Dni)}, {nameof(Tenant.Email)},{nameof(Tenant.StateT)}
        From tenant  WHERE {nameof(Tenant.StateT)} = 1";
        using (var command = new MySqlCommand(sql,connection)){
            connection.Open();
            using (var reader= command.ExecuteReader()){
while (reader.Read())
{
    tenants.Add(new Tenant{
        id_Tenant = reader.GetInt32(nameof(Tenant.id_Tenant)),
        Name = reader.GetString(nameof(Tenant.Name)),
        Last_Name = reader.GetString(nameof(Tenant.Last_Name)),
        Dni = reader.GetInt32(nameof(Tenant.Dni)),
        Birthdate = reader.GetDateTime(nameof(Tenant.Birthdate)),
        Sex = reader.GetChar(nameof(Tenant.Sex)),
        Address = reader.GetString(nameof(Tenant.Address)),
        Phone = reader.GetString(nameof(Tenant.Phone)),
        Email = reader.GetString(nameof(Tenant.Email)),
       StateT = reader.GetInt32(nameof(Tenant.StateT)),

    });
    
}

            }
        }
    }


return tenants;
}

public int High(Tenant tenants){
    int id=0;
    using(var connection = new MySqlConnection(ConnectionString)){
          var sql = @$"INSERT INTO tenant ({nameof(Tenant.Name)}, {nameof(Tenant.Last_Name)}, {nameof(Tenant.Birthdate)}, {nameof(Tenant.Sex)}, {nameof(Tenant.Address)}, {nameof(Tenant.Phone)}, {nameof(Tenant.Dni)}, {nameof(Tenant.Email)}, {nameof(Tenant.StateT)})
                    VALUES (@{nameof(Tenant.Name)}, @{nameof(Tenant.Last_Name)}, @{nameof(Tenant.Birthdate)}, @{nameof(Tenant.Sex)}, @{nameof(Tenant.Address)}, @{nameof(Tenant.Phone)}, @{nameof(Tenant.Dni)}, @{nameof(Tenant.Email)}, 1);
                    SELECT LAST_INSERT_ID();";

        using (var command= new MySqlCommand(sql, connection)){
            command.Parameters.AddWithValue($"@{nameof(Tenant.Name)}", tenants.Name);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Last_Name)}", tenants.Last_Name);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Dni)}", tenants.Dni);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Birthdate)}", tenants.Birthdate);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Sex)}", tenants.Sex);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Address)}", tenants.Address);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Phone)}", tenants.Phone);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Email)}", tenants.Email);
            

            connection.Open();
            id= Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
    }
return id;
}
public Tenant? GetTenantId(int id){
    Tenant? tenants= null;
    using (var connection = new MySqlConnection(ConnectionString)){

   var sql = @$"SELECT {nameof(Tenant.id_Tenant)},{nameof(Tenant.Name)}, {nameof(Tenant.Last_Name)}, {nameof(Tenant.Birthdate)}, {nameof(Tenant.Sex)}, {nameof(Tenant.Address)}, {nameof(Tenant.Phone)}, {nameof(Tenant.Dni)}, {nameof(Tenant.Email)}
        FROM tenant
        WHERE {nameof(Tenant.id_Tenant)} = @{nameof(Tenant.id_Tenant)}";

        using (var command = new MySqlCommand(sql,connection)){
            command.Parameters.AddWithValue($"@{nameof(Tenant.id_Tenant)}", id);
            connection.Open();
            using (var reader= command.ExecuteReader()){
if (reader.Read())
{
    tenants =new Tenant{
        id_Tenant = reader.GetInt32(nameof(Tenant.id_Tenant)),
        Name = reader.GetString(nameof(Tenant.Name)),
        Last_Name = reader.GetString(nameof(Tenant.Last_Name)),
        Dni = reader.GetInt32(nameof(Tenant.Dni)),
        Birthdate = reader.GetDateTime(nameof(Tenant.Birthdate)),
        Sex = reader.GetChar(nameof(Tenant.Sex)),
        Address = reader.GetString(nameof(Tenant.Address)),
        Phone = reader.GetString(nameof(Tenant.Phone)),
        Email = reader.GetString(nameof(Tenant.Email)),
       

    };
}

            }
        }
    }


return tenants;
}

public int  Modification(Tenant tenants)
{
    	int res = -1;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"UPDATE tenant
                    SET {nameof(Tenant.Name)} = @{nameof(Tenant.Name)},
                        {nameof(Tenant.Last_Name)} = @{nameof(Tenant.Last_Name)},
                        {nameof(Tenant.Dni)} = @{nameof(Tenant.Dni)},
                        {nameof(Tenant.Birthdate)} = @{nameof(Tenant.Birthdate)},
                        {nameof(Tenant.Sex)} = @{nameof(Tenant.Sex)},
                        {nameof(Tenant.Address)} = @{nameof(Tenant.Address)},
                        {nameof(Tenant.Phone)} = @{nameof(Tenant.Phone)},
                        {nameof(Tenant.Email)} = @{nameof(Tenant.Email)}
                    WHERE {nameof(Tenant.id_Tenant)} = @{nameof(Tenant.id_Tenant)}";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue($"@{nameof(Tenant.Name)}", tenants.Name);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Last_Name)}", tenants.Last_Name);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Dni)}", tenants.Dni);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Birthdate)}", tenants.Birthdate);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Sex)}", tenants.Sex);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Address)}", tenants.Address);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Phone)}", tenants.Phone);
            command.Parameters.AddWithValue($"@{nameof(Tenant.Email)}", tenants.Email);
            command.Parameters.AddWithValue($"@{nameof(Tenant.id_Tenant)}", tenants.id_Tenant);

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
        string sql = @$"UPDATE tenant
                        SET stateT = 0
                        WHERE {nameof(Tenant.id_Tenant)} = @id";
        
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