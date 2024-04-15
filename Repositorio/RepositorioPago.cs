using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using tpInmobliaria.Models;


namespace tpInmobliaria.Models;

public class RepositorioPago {

readonly string ConnectionString = "Server=localhost;Database=inmo;User=root;Password=;";



public RepositorioPago(){


}

public IList<Pago> GetTodosPagos()
{
      IList<Pago> res = new List<Pago>();
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql =@"SELECT p.*, c.InquilinoId, i.Nombre, i.Apellido
                    FROM pagos p
                    INNER JOIN contratos c ON p.ContratoId = c.id_Contrato
                    INNER JOIN inquilinos i ON c.InquilinoId = i.id_Inquilino";

 
        using (var command = new MySqlCommand(sql, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
              while (reader.Read())
                        {
                            Pago c = new Pago
                            {
                                PagoId = reader.GetInt32(0),
                                ContratoId = reader.GetInt32(1),
                                NumeroPago = reader.GetInt32(2),
                                Concepto = reader.GetString(3),
                                FechaPago = reader.GetDateTime(4),
                                Importe = reader.GetDouble(5),
                                EstadoPago = reader.GetString(6),
                               Contrato = new Contrato
                        {
                            InquilinoId = reader.GetInt32(7) // Suponiendo que el id del inquilino se encuentra en la columna 7
                        },
                            InquilinoNombre = reader.GetString(8), // Suponiendo que el nombre del inquilino se encuentra en la columna 8
                        InquilinoApellido = reader.GetString(9)
                            };

                        

                            res.Add(c);
                        }
                        connection.Close();
                    }
                }
                return res;
            
    }
}

public int High(Pago pago)
{
    int id = 0;
    using(var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"INSERT INTO pagos ({nameof(Pago.ContratoId)}, {nameof(Pago.NumeroPago)}, {nameof(Pago.Concepto)}, {nameof(Pago.FechaPago)}, {nameof(Pago.Importe)}, {nameof(Pago.EstadoPago)})
                     VALUES (@{nameof(Pago.ContratoId)}, @{nameof(Pago.NumeroPago)}, @{nameof(Pago.Concepto)}, @{nameof(Pago.FechaPago)}, @{nameof(Pago.Importe)}, @{nameof(Pago.EstadoPago)});
                     SELECT LAST_INSERT_ID();";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue($"@{nameof(Pago.ContratoId)}", pago.ContratoId);
            command.Parameters.AddWithValue($"@{nameof(Pago.NumeroPago)}", pago.NumeroPago);
            command.Parameters.AddWithValue($"@{nameof(Pago.Concepto)}", pago.Concepto);
            command.Parameters.AddWithValue($"@{nameof(Pago.FechaPago)}", pago.FechaPago);
            command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
            command.Parameters.AddWithValue($"@{nameof(Pago.EstadoPago)}", pago.EstadoPago);
           


            connection.Open();
            id = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
    }
    return id;
}

public int Modification(Pago pago)
{
    int res = -1;
    
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"UPDATE pagos
                    SET {nameof(Pago.NumeroPago)} = @{nameof(Pago.NumeroPago)},
                        {nameof(Pago.Concepto)} = @{nameof(Pago.Concepto)},
                        {nameof(Pago.FechaPago)} = @{nameof(Pago.FechaPago)},
                          {nameof(Pago.EstadoPago)} = @{nameof(Pago.EstadoPago)}
                    WHERE {nameof(Pago.PagoId)} = @{nameof(Pago.PagoId)}";

        using (var command = new MySqlCommand(sql, connection))
        {
               //    command.Parameters.AddWithValue($"@{nameof(Pago.ContratoId)}", pago.ContratoId);
            command.Parameters.AddWithValue($"@{nameof(Pago.NumeroPago)}", pago.NumeroPago);
            command.Parameters.AddWithValue($"@{nameof(Pago.Concepto)}", pago.Concepto);
            command.Parameters.AddWithValue($"@{nameof(Pago.FechaPago)}", pago.FechaPago);
          //  command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
            command.Parameters.AddWithValue($"@{nameof(Pago.EstadoPago)}", pago.EstadoPago);
            command.Parameters.AddWithValue($"@{nameof(Pago.PagoId)}", pago.PagoId);

            connection.Open();

            res = command.ExecuteNonQuery();

            connection.Close();
        }
    }

    return res;
}



public Pago? GetPagoId(int id)
{
    Pago? p = null;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"SELECT p.{nameof(Pago.PagoId)}, p.{nameof(Pago.ContratoId)}, p.{nameof(Pago.NumeroPago)}, 
                    p.{nameof(Pago.Concepto)}, p.{nameof(Pago.FechaPago)}, p.{nameof(Pago.Importe)}, 
                    p.{nameof(Pago.EstadoPago)}, c.InmuebleId, i.Ubicacion, i.Direccion, i.Ambientes, i.Uso, i.Tipo, i.Precio,
                    c.InquilinoId, inq.nombre, inq.apellido, inq.dni, inq.telefono, inq.email,
                    c.Fecha_Inicio, c.Fecha_Fin, c.Monto, c.Estado
             FROM pagos p
             INNER JOIN contratos c ON p.{nameof(Pago.ContratoId)} = c.id_Contrato
             INNER JOIN inmuebles i ON c.InmuebleId = i.id_Inmuebles
             INNER JOIN inquilinos inq ON c.InquilinoId = inq.id_Inquilino
             WHERE p.{nameof(Pago.PagoId)} = @{nameof(Pago.PagoId)}";


        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue($"@{nameof(Pago.PagoId)}", id);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    p = new Pago
                    {
                        PagoId = reader.GetInt32(nameof(Pago.PagoId)),
                        ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                        NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                        Concepto = reader.GetString(nameof(Pago.Concepto)),
                        FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                        Importe = reader.GetDouble(nameof(Pago.Importe)),
                        EstadoPago = reader.GetString(nameof(Pago.EstadoPago)),
                        Contrato = new Contrato
                        {
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            Inmueble = new Inmueble
                            {
                               Ubicacion=reader.GetString("Ubicacion"),
                                Direccion = reader.GetString("Direccion"),
                                Ambientes=reader.GetInt32("Ambientes"),
                                Uso=reader.GetString("Uso"),
                                Tipo=reader.GetString("Tipo"),
                                Precio=reader.GetDouble("Precio")
                            },
                              InquilinoId = reader.GetInt32("InquilinoId"),
                            Inquilino = new Inquilino
                            {
                                Nombre=reader.GetString("Nombre"),
                                Apellido=reader.GetString("Apellido"),
                                Dni=reader.GetString("Dni"),
                             Telefono=reader.GetString("Telefono"),
                             Email=reader.GetString("Email")
                             
                            },
                            Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                            Fecha_Fin =reader.GetDateTime("Fecha_Fin"),
                            Monto = reader.GetDouble("Monto"),
                            Estado = reader.GetString("Estado")
                        }
                    };
                }
            }
        }
    }

    return p;
}


public int Low(int id)
{
    int res = -1;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = @$"UPDATE pagos
                        SET EstadoPago = 'No Abono'
                        WHERE {nameof(Pago.PagoId)} = @id";
        
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
public List<Pago> GetPagosPorContratoId(int contratoId)
{
    List<Pago> pagos = new List<Pago>();

    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @"SELECT * FROM pagos WHERE ContratoId = @contratoId";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@contratoId", contratoId);

            connection.Open();
            
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Pago pago = new Pago
                    {
                        PagoId = reader.GetInt32(nameof(Pago.PagoId)),
                        ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                        NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                        Concepto = reader.GetString(nameof(Pago.Concepto)),
                        FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                        Importe = reader.GetDouble(nameof(Pago.Importe)),
                        EstadoPago = reader.GetString(nameof(Pago.EstadoPago))
                    };

                    pagos.Add(pago);
                }
            }
        }
    }

    return pagos;
}


 public void AgregarPago(Pago pago)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @"INSERT INTO pagos (ContratoId, NumeroPago, Concepto, FechaPago, Importe, EstadoPago) 
                        VALUES (@ContratoId, @NumeroPago, @Concepto, @FechaPago, @Importe, @EstadoPago)";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ContratoId", pago.ContratoId);
                command.Parameters.AddWithValue("@NumeroPago", pago.NumeroPago);
                command.Parameters.AddWithValue("@Concepto", pago.Concepto);
                command.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                command.Parameters.AddWithValue("@Importe", pago.Importe);
                command.Parameters.AddWithValue("@EstadoPago", pago.EstadoPago);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

public bool InquilinoTienePagosPendientes(int ContratoId)
{
    using (var connection = new MySqlConnection(ConnectionString))
    {
        connection.Open();

        var sql = "SELECT COUNT(*) FROM pagos WHERE EstadoPago = 'No Abono' AND ContratoId = @ContratoId";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@ContratoId", ContratoId);

            int count = Convert.ToInt32(command.ExecuteScalar());

            return count > 0;
        }
    }
}

}