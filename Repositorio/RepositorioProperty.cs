using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioProperty {

readonly string ConnectionString = "Server=localhost;Database=inmobmaldonado;User=root;Password=;";



public RepositorioProperty(){


}

public IList<Property> GetProperts(){
    var properties = new List<Property>();
    using (var connection = new MySqlConnection(ConnectionString)){

        var sql= @$"SELECT {nameof(Property.id_Proprietor)}, {nameof(Property.Address)}, {nameof(Property.Type)}, {nameof(Property.Use_type)}, {nameof(Property.Coordinates)}, {nameof(Property.Price)}, {nameof(Property.StateP)}
        From property";
        using (var command = new MySqlCommand(sql,connection)){
            connection.Open();
            using (var reader= command.ExecuteReader()){
while (reader.Read())
{
    properties .Add(new Property{
        id_Property = reader.GetInt32("id_Property"),
                        id_Proprietor = reader.GetInt32("id_Proprietor"),
                     Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString("Address"),
                       Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? null : reader.GetString("Type"),
                        Use_type = reader.IsDBNull(reader.GetOrdinal("Use_type")) ? null : reader.GetString("Use_type"),
                        Coordinates = reader.IsDBNull(reader.GetOrdinal("Coordinates")) ? null : reader.GetDouble("Coordinates"),
                        Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? null : reader.GetDouble("Price"),
                        StateP = reader.IsDBNull(reader.GetOrdinal("StateP")) ? null : reader.GetString("StateP")
                    });
}

            }
        }
    }


return properties;
}


public int Create(Property property)
{
    int id = 0;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"INSERT INTO property ({nameof(Property.id_Proprietor)}, {nameof(Property.Address)},{nameof(Property.Type)},  {nameof(Property.Use_type)}, {nameof(Property.Coordinates)}, {nameof(Property.Price)}, {nameof(Property.StateP)})
                    VALUES (@{nameof(Property.id_Proprietor)}, @{nameof(Property.Address)},@{nameof(Property.Type)},  @{nameof(Property.Use_type)}, @{nameof(Property.Coordinates)}, @{nameof(Property.Price)}, @{nameof(Property.StateP)});
                    SELECT LAST_INSERT_ID();";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue($"@{nameof(Property.id_Proprietor)}", property.id_Proprietor);
            command.Parameters.AddWithValue($"@{nameof(Property.Address)}", property.Address);
            command.Parameters.AddWithValue($"@{nameof(Property.Type)}", property.Type);
            command.Parameters.AddWithValue($"@{nameof(Property.Use_type)}", property.Use_type);
            command.Parameters.AddWithValue($"@{nameof(Property.Coordinates)}", property.Coordinates);
            command.Parameters.AddWithValue($"@{nameof(Property.Price)}", property.Price);
            command.Parameters.AddWithValue($"@{nameof(Property.StateP)}", property.StateP);

            connection.Open();
            id = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
    }
    return id;
}

}