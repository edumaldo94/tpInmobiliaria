using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioContrato {

readonly string ConnectionString = "Server=localhost;Database=inmo;User=root;Password=;";



public RepositorioContrato(){


}

public IList<Contrato> GetContracts()
{
      IList<Contrato> res = new List<Contrato>();
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = "SELECT * FROM contratos WHERE estadoC=1";

 
        using (var command = new MySqlCommand(sql, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
              while (reader.Read())
                        {
                            Contrato c = new Contrato
                            {
                                id_Contrato = reader.GetInt32(0),
                                InmuebleId = reader.GetInt32(1),
                                InquilinoId = reader.GetInt32(2),
                                Fecha_Inicio = reader.GetDateTime(3),
                                Fecha_Fin = reader.GetDateTime(4),
                                Monto = reader.GetDouble(5),
                                Estado = reader.GetString(6),
                                EstadoC = reader.GetInt32(7),
                            };

                        

                            res.Add(c);
                        }
                        connection.Close();
                    }
                }
                return res;
            
    }
}

public int High(Contrato contract)
{
    int id = 0;
    using(var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"INSERT INTO contratos ({nameof(Contrato.InmuebleId)}, {nameof(Contrato.InquilinoId)}, {nameof(Contrato.Fecha_Inicio)}, {nameof(Contrato.Fecha_Fin)}, {nameof(Contrato.Monto)}, {nameof(Contrato.Estado)},{nameof(Contrato.EstadoC)})
                     VALUES (@{nameof(Contrato.InmuebleId)}, @{nameof(Contrato.InquilinoId)}, @{nameof(Contrato.Fecha_Inicio)}, @{nameof(Contrato.Fecha_Fin)}, @{nameof(Contrato.Monto)}, @{nameof(Contrato.Estado)},1);
                     SELECT LAST_INSERT_ID();";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue($"@{nameof(Contrato.InmuebleId)}", contract.InmuebleId);
            command.Parameters.AddWithValue($"@{nameof(Contrato.InquilinoId)}", contract.InquilinoId);
            command.Parameters.AddWithValue($"@{nameof(Contrato.Fecha_Inicio)}", contract.Fecha_Inicio);
            command.Parameters.AddWithValue($"@{nameof(Contrato.Fecha_Fin)}", contract.Fecha_Fin);
            command.Parameters.AddWithValue($"@{nameof(Contrato.Monto)}", contract.Monto);
            command.Parameters.AddWithValue($"@{nameof(Contrato.Estado)}", contract.Estado);
             command.Parameters.AddWithValue($"@{nameof(Contrato.EstadoC)}", contract.EstadoC);


            connection.Open();
            id = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
    }
    return id;
}

public int Modification(Contrato contract)
{
    int res = -1;
    
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"UPDATE contratos
                    SET {nameof(Contrato.InmuebleId)} = @{nameof(Contrato.InmuebleId)},
                        {nameof(Contrato.InquilinoId)} = @{nameof(Contrato.InquilinoId)},
                        {nameof(Contrato.Fecha_Inicio)} = @{nameof(Contrato.Fecha_Inicio)},
                        {nameof(Contrato.Fecha_Fin)} = @{nameof(Contrato.Fecha_Fin)},
                        {nameof(Contrato.Monto)} = @{nameof(Contrato.Monto)},
                        {nameof(Contrato.Estado)} = @{nameof(Contrato.Estado)}
                    WHERE {nameof(Contrato.id_Contrato)} = @{nameof(Contrato.id_Contrato)}";

        using (var command = new MySqlCommand(sql, connection))
        {
          command.Parameters.AddWithValue($"@{nameof(Contrato.InmuebleId)}", contract.InmuebleId);
            command.Parameters.AddWithValue($"@{nameof(Contrato.InquilinoId)}", contract.InquilinoId);
            command.Parameters.AddWithValue($"@{nameof(Contrato.Fecha_Inicio)}", contract.Fecha_Inicio);
            command.Parameters.AddWithValue($"@{nameof(Contrato.Fecha_Fin)}", contract.Fecha_Fin);
            command.Parameters.AddWithValue($"@{nameof(Contrato.Monto)}", contract.Monto);
            command.Parameters.AddWithValue($"@{nameof(Contrato.Estado)}", contract.Estado);
            command.Parameters.AddWithValue($"@{nameof(Contrato.id_Contrato)}", contract.id_Contrato);

            connection.Open();

            res = command.ExecuteNonQuery();

            connection.Close();
        }
    }

    return res;
}



public Contrato? GetContractId(int id){
    Contrato? c= null;
    using (var connection = new MySqlConnection(ConnectionString)){

   var sql = @$"SELECT {nameof(Contrato.id_Contrato)}, {nameof(Contrato.InmuebleId)}, {nameof(Contrato.InquilinoId)}, {nameof(Contrato.Fecha_Inicio)}, {nameof(Contrato.Fecha_Fin)}, {nameof(Contrato.Monto)}, {nameof(Contrato.Estado)},{nameof(Contrato.EstadoC)}
        FROM contratos
        WHERE {nameof(Contrato.id_Contrato)} = @{nameof(Contrato.id_Contrato)}";

        using (var command = new MySqlCommand(sql,connection)){
            command.Parameters.AddWithValue($"@{nameof(Contrato.id_Contrato)}", id);
            connection.Open();
            using (var reader= command.ExecuteReader()){
if (reader.Read())
{
     c = new Contrato{
        id_Contrato = reader.GetInt32(nameof(Contrato.id_Contrato)),
        InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
        InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
        Fecha_Inicio = reader.GetDateTime(nameof(Contrato.Fecha_Inicio)),
        Fecha_Fin = reader.GetDateTime(nameof(Contrato.Fecha_Fin)),
        Monto = reader.GetDouble(nameof(Contrato.Monto)),
        Estado = reader.GetString(nameof(Contrato.Estado)),
        EstadoC = reader.GetInt32(nameof(Contrato.EstadoC)),
    };
}

            }
        }
    }


return c;
}

public int Low(int id)
{
    int res = -1;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = @$"UPDATE contratos
                        SET estadoC = 0
                        WHERE {nameof(Contrato.id_Contrato)} = @id";
        
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