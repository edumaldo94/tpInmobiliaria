using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioAuditoria
{

    readonly string ConnectionString = "Server=localhost;Database=inmo;User=root;Password=;";



    public RepositorioAuditoria()
    {


    }
  public void RegistrarAccionAuditoria(int? contratoId, int? pagoId, string usuarioNombre, string accion)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string sql = @"INSERT INTO Auditoria (ContratoId, PagoId, UsuarioNombre, Accion, FechaHora)
                                VALUES (@ContratoId, @PagoId, @UsuarioNombre, @Accion, @FechaHora)";

                MySqlCommand command = new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue("@ContratoId", contratoId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PagoId", pagoId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UsuarioNombre", usuarioNombre);
                command.Parameters.AddWithValue("@Accion", accion);
                command.Parameters.AddWithValue("@FechaHora", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }
    }
public Auditoria? ObtenerAuditoriaPorContratoId(int contratoId)
{
    Auditoria? auditoria = null;
   
    string sqlQuery = "SELECT * FROM Auditoria WHERE ContratoId = @ContratoId";

    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
    {
        MySqlCommand command = new MySqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@ContratoId", contratoId);

        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            auditoria = new Auditoria
            {
                IdAuditor = reader.GetInt32("IdAuditor"),
                ContratoId = reader.GetInt32("ContratoId"),
                PagoId = reader.IsDBNull(reader.GetOrdinal("PagoId")) ? (int?)null : reader.GetInt32("PagoId"),
                FechaHora = reader.GetDateTime("FechaHora"),
                UsuarioNombre = reader.GetString("UsuarioNombre"),
                Accion = reader.GetString("Accion")
            };
        }

        reader.Close();
    }

    return auditoria;
}

    public Auditoria? ObtenerAuditoriaPorBajaContratoId(int contratoId)
{
    Auditoria? auditoria = null;
   
    string sqlQuery = "SELECT * FROM Auditoria WHERE ContratoId = @ContratoId  and Accion='Fin de Contrato y Pago de multa'";

    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
    {
        MySqlCommand command = new MySqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@ContratoId", contratoId);

        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            auditoria = new Auditoria
            {
                IdAuditor = reader.GetInt32("IdAuditor"),
                ContratoId = reader.GetInt32("ContratoId"),
                PagoId = reader.IsDBNull(reader.GetOrdinal("PagoId")) ? (int?)null : reader.GetInt32("PagoId"),
                FechaHora = reader.GetDateTime("FechaHora"),
                UsuarioNombre = reader.GetString("UsuarioNombre"),
                Accion = reader.GetString("Accion")
            };
        }

        reader.Close();
    }

    return auditoria;
}


///PAGO///


public List<Auditoria> ObtenerAuditoriaPorPagoId(int pagoId)
{
    List<Auditoria> auditorias = new List<Auditoria>();
   
    string sqlQuery = @"SELECT * 
                        FROM Auditoria 
                        WHERE PagoId = @PagoId  
                        AND (Accion = 'Abono'OR Accion = 'Creación de Contrato y Pago')";

    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
    {
        MySqlCommand command = new MySqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@PagoId", pagoId);

        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Auditoria auditoria = new Auditoria
            {
                IdAuditor = reader.GetInt32("IdAuditor"),
                ContratoId = reader.GetInt32("ContratoId"),
                PagoId = reader.IsDBNull(reader.GetOrdinal("PagoId")) ? (int?)null : reader.GetInt32("PagoId"),
                FechaHora = reader.GetDateTime("FechaHora"),
                UsuarioNombre = reader.GetString("UsuarioNombre"),
                Accion = reader.GetString("Accion")
            };
            auditorias.Add(auditoria);
        }

        reader.Close();
    }

    return auditorias;
}

    public List<Auditoria> ObtenerAuditoriaPorBajaPagoId(int pagoId)
{
 List<Auditoria> auditorias = new List<Auditoria>();
   
 string sqlQuery = @"SELECT * 
                    FROM Auditoria 
                    WHERE PagoId = @PagoId  
                    AND (Accion ='No Abono' OR Accion ='Fin de Contrato y Pago de multa')";

    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
    {
        MySqlCommand command = new MySqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@PagoId", pagoId);

        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
             Auditoria auditoria = new Auditoria
            {
                IdAuditor = reader.GetInt32("IdAuditor"),
                ContratoId = reader.GetInt32("ContratoId"),
                PagoId = reader.IsDBNull(reader.GetOrdinal("PagoId")) ? (int?)null : reader.GetInt32("PagoId"),
                FechaHora = reader.GetDateTime("FechaHora"),
                UsuarioNombre = reader.GetString("UsuarioNombre"),
                Accion = reader.GetString("Accion")
            };
              auditorias.Add(auditoria);
        }

        reader.Close();
    }

    return auditorias;
}
}
