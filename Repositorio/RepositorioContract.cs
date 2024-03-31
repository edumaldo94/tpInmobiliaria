using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioContract {

readonly string ConnectionString = "Server=localhost;Database=inmobmaldonado;User=root;Password=;";



public RepositorioContract(){


}

public IList<Contract> GetContracts()
{
    var contracts  = new List<Contract>();
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = "SELECT id_Contract, id_Property, id_Tenant, Start_date, End_date, Rental_amount, Penalize, StateC FROM contract";

        using (var command = new MySqlCommand(sql, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
              while (reader.Read())
                    {
                        contracts.Add(new Contract
                        {
                            id_Contract = reader.GetInt32("id_Contract"),
                            id_Property = reader.GetInt32("id_Property"),
                            id_Tenant = reader.GetInt32("id_Tenant"),
                            Start_date = reader.IsDBNull(reader.GetOrdinal("Start_date")) ? null : reader.GetDateTime("Start_date"),
                            End_date = reader.IsDBNull(reader.GetOrdinal("End_date")) ? null : reader.GetDateTime("End_date"),
                            Rental_amount = reader.IsDBNull(reader.GetOrdinal("Rental_amount")) ? null : reader.GetDouble("Rental_amount"),
                            Penalize = reader.IsDBNull(reader.GetOrdinal("Penalize")) ? null : reader.GetDouble("Penalize"),
                            StateC = reader.IsDBNull(reader.GetOrdinal("StateC")) ? null : reader.GetString("StateC")
                        });
                    }
                }
            }
        }

        return contracts;
    }

}