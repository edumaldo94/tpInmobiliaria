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
                      //          Disponible = reader.GetString(10),

                           
                            };
                            inmuebles.Add(inmueble);
                        }
                    }
                }
            }

            return inmuebles;
        }

    

}


/*

	///////////////////////////////VER  MAÑANA///////////////////////////////////
	///


	   [HttpGet("user")]
    [Authorize]

    public async Task<ActionResult<Propietario>> GetUser() // devuelve el propietario logueado
    {
        try
        {
            var usuario =  HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (usuario == null) return Unauthorized("Token no válido");
            var dbUser = await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
            if (dbUser == null) return BadRequest("El usuario no existe");
            return dbUser;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpPut("editar")]
    [Authorize]
    public async Task<IActionResult> Editar([FromForm] Propietario propietario)
    {
        try
        {
            var usuario = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
           
     var user = await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
            int cantidad = contexto.Propietarios.Count(u => u.Email == propietario.Email && u.Email != user.Email);
            Console.WriteLine("CORREO DESDE EDITAR: "+propietario.Email);


            if (cantidad > 0)
            {
                Propietario? prop = null;
                return Ok(prop);
            }

            if (usuario == null) return Unauthorized("Token incorrecto");
            var dbUser = await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == user.Email);

            if (dbUser == null) return BadRequest("No se encontro el usuario");

            

            dbUser.Nombre = !string.IsNullOrEmpty(propietario.Nombre) ? propietario.Nombre : dbUser.Nombre;
            dbUser.Apellido = !string.IsNullOrEmpty(propietario.Apellido) ? propietario.Apellido : dbUser.Apellido;
            dbUser.Dni = !string.IsNullOrEmpty(propietario.Dni) ? propietario.Dni : dbUser.Dni;
            dbUser.Telefono = !string.IsNullOrEmpty(propietario.Telefono) ? propietario.Telefono : dbUser.Telefono;
            dbUser.Email = !string.IsNullOrEmpty(propietario.Email) ? propietario.Email : dbUser.Email;
            dbUser.Avatar = !string.IsNullOrEmpty(propietario.Avatar) ? propietario.Avatar : dbUser.Avatar;
            Console.WriteLine("correo: " + dbUser.Email);

            if (propietario.Clave != null && propietario.Clave != "" && propietario.Clave != dbUser.Clave)
            {
                dbUser.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: propietario.Clave,
                salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
            }
            contexto.Update(dbUser);
            contexto.SaveChanges();
       
            Console.WriteLine("claveOriginal: " + propietario.Clave);
            Console.WriteLine("claveNueva: " + dbUser.Clave);
            Console.WriteLine("correoOriginal: " + propietario.Email);
            Console.WriteLine("correoNuevo: " + dbUser.Email);

            if (propietario.Email!=user.Email
                || !string.IsNullOrWhiteSpace(propietario.Clave)
                ||!string.IsNullOrEmpty(propietario.Clave))
            {
                return Ok("Reloguear");
            }
            

            var key = new SymmetricSecurityKey(                                                                                                 
                    System.Text.Encoding.ASCII.GetBytes(
                        config["TokenAuthentication:SecretKey"]
                    )
                );

            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dbUser.Email),
                    new Claim("FullName", dbUser.Nombre + " " + dbUser.Apellido),
                    // new Claim(ClaimTypes.Role, "Propietario")
                };

            var token = new JwtSecurityToken(
                issuer: config["TokenAuthentication:Issuer"],
                audience: config["TokenAuthentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credenciales
            );
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

  

    [HttpPost("email")]
    //[AllowAnonymous]
    public async Task<IActionResult> GetByEmail([FromForm] string correo)
    {
        try
        { //método sin autenticar, busca el propietario x email
		  //var usuario = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
           
    // var user = await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
            Console.WriteLine($"Email: {correo}");
            var propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == correo);
            var link = "";
            string localIPv4 = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList
                .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                 ?.ToString();
            var dominio = environment.IsDevelopment() ? localIPv4 : "www.misitio.com";

            if (propietario != null)
            {
                var key = new SymmetricSecurityKey(
                                   System.Text.Encoding.ASCII.GetBytes(
                                       config["TokenAuthentication:SecretKey"]
                                   )
                               );
                var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, propietario.Email),
                    new Claim("FullName", propietario.Nombre + " " + propietario.Apellido),
                };

                var token = new JwtSecurityToken(
                    issuer: config["TokenAuthentication:Issuer"],
                    audience: config["TokenAuthentication:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(24),
                    signingCredentials: credenciales
                );


                link = $"https://{dominio}:5000/api/Propietario/token?access_token={new JwtSecurityTokenHandler().WriteToken(token)}";


                Console.WriteLine(link);

                string subject = "Pedido de Recuperacion de Contraseña";
                string body = @$"<html>
                <body>
                    <h1>Recuperación de Contraseña</h1>
                    <p>Estimado {propietario.Nombre},</p>
                    <p>Hemos recibido una solicitud para restablecer tu contraseña.</p>
                    <p>Por favor, haz clic en el siguiente enlace para crear una nueva contraseña:</p>
                    <p><a href='{link}'>Restablecer Contraseña</a></p>
                    <p>Si no solicitaste el restablecimiento de contraseña, puedes ignorar este correo electrónico.</p>
                    <p>Este enlace expirará en 24 horas por motivos de seguridad.</p>
                    <p>Atentamente,</p>
                    <p>Tu equipo de soporte</p>
                </body>
            </html>";

                await enviarMail(correo, subject, body);

                return Ok(propietario);
            }
            else
            {
                return BadRequest("Nombre de usuario o clave incorrectos");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("ERRRROR");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("token")]
    [Authorize]
    public async Task<IActionResult> Token()
    {
        try
        {
            var perfil = new
            {
                Email = User.Identity?.Name,
                Nombre = User.Claims.First(x => x.Type == "FullName").Value,
            };
            Console.WriteLine("ASDASD0" + perfil.Nombre);
            Random rand = new Random(Environment.TickCount);
            string randomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            string nuevaClave = "";
            for (int i = 0; i < 8; i++)
            {
                nuevaClave += randomChars[rand.Next(0, randomChars.Length)];
            }

            string subject = "Nueva Clave de Ingreso";
            string body = @$"<html>
                <body>
                    <h1>Recuperación de Contraseña</h1>
                    <p>Estimado {perfil.Nombre},</p>
                    <p>Hemos generado una nueva contraseña para tu cuenta.</p>
                    <p>Tu nueva contraseña es: <strong>{nuevaClave}</strong></p>
                    <p>Por favor, inicia sesión con esta nueva contraseña y cámbiala lo antes posible.</p>
                    <p>Si no solicitaste un cambio de contraseña, por favor contáctanos de inmediato.</p>
                    <p>Atentamente,</p>
                    <p>Tu equipo de soporte</p>
                </body>
            </html>";
            await enviarMail(perfil.Email, subject, body);

            var propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == perfil.Email);

            if (propietario != null)
            {
                propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: nuevaClave,
                salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
                contexto.Update(propietario);
                contexto.SaveChanges();
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

 
    [HttpGet("sendEmail")]
    private async Task<IActionResult> enviarMail(string email, string subject, string body)
    {
        var emailMessage = new MimeMessage();
if (email == null)
{
    return BadRequest("La dirección de correo electrónico es nula o vacía");
}
        emailMessage.From.Add(new MailboxAddress("Sistema", config["SMTPUser"]));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = body, };


        using (var client = new SmtpClient())
        {
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
            await client.AuthenticateAsync(config["SMTPUser"], config["SMTPPass"]);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
        return Ok();
    }


}
*/