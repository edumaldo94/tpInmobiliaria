using tpInmobliaria.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MailKit.Net.Smtp;
using MimeKit;

namespace tpInmobliaria.Api //<-- No hay punto y coma aquí
{
   
    [Route("api/[controller]")]
     [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmuebleController : ControllerBase
    {
        private readonly DataContext applicationDbContext;
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IWebHostEnvironment environment;

        public InmuebleController(DataContext applicationDbContext, IConfiguration config, IHttpContextAccessor contextAccessor, IWebHostEnvironment environment)
        {
            this.applicationDbContext = applicationDbContext;
            this.config= config;
            this.contextAccessor = contextAccessor;
            this.environment = environment;
        }

        [HttpGet("obtener")]
        public async Task<ActionResult<List<Inmueble>>> Get()
        {

            try
            {

                var email = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

                var propietario = await applicationDbContext.Propietarios.FirstOrDefaultAsync(x => x.Email == email);

                var inmuebles = await applicationDbContext.Inmuebles.Where(x => x.PropietarioId == propietario.id_Propietario).ToListAsync();


                return inmuebles;


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());

            }




        }

        [HttpGet("contrato")]
        public async Task<ActionResult<List<Inmueble>>> GetAlquilados()
        {

            try
            {

                var email = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var propietario = await applicationDbContext.Propietarios.FirstOrDefaultAsync(x => x.Email == email);
                var inmuebles = await applicationDbContext.Inmuebles.Join(
                    applicationDbContext.Contratos.Where(x => x.Fecha_Fin > DateTime.Now && x.Fecha_Inicio < DateTime.Now),
                    inm => inm.id_Inmuebles,
                    com => com.InmuebleId,
                    (inm, com) => inm)
                    .Where(x => x.PropietarioId == propietario.id_Propietario).ToListAsync();

                return inmuebles;


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());

            }
        }

        [HttpPut("{id}")]//Cambiar Estado
        public async Task<ActionResult<Inmueble>> Put([FromForm] byte estado, int id)
        {
            var e = "no";
            if(estado == 1)
            {
                e = "si";
            }
            else
            {
                e = "no";
            }

            try
            {

                var email = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var propietario = await applicationDbContext.Propietarios.FirstOrDefaultAsync(x => x.Email == email);
                Inmueble inmuebleV = await applicationDbContext.Inmuebles.FirstOrDefaultAsync(x => x.id_Inmuebles == id && x.PropietarioId == propietario.id_Propietario);
                if (inmuebleV == null)
                {
                    return NotFound();
                }

                inmuebleV.Disponible = e;
                applicationDbContext.Update(inmuebleV);
                await applicationDbContext.SaveChangesAsync();
                return inmuebleV;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

   [HttpPut("toogleEstado/{id}")]
    [Authorize]
    public async Task<IActionResult> ToogleEstado(int id)
    {
        try
        {
        var usuario = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
           
     var user = await applicationDbContext.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);

 var inmueble = await applicationDbContext.Inmuebles
            .Where(e => e.id_Inmuebles == id)
            .ToListAsync();
            if (inmueble == null && inmueble.Count > 0) return NotFound();
            if (inmueble[0].PropietarioId != user.id_Propietario) return Unauthorized("Acceso denegado");
            inmueble[0].Disponible = "No";
     applicationDbContext.Update(inmueble[0]);

// Guardar los cambios en la base de datos
await applicationDbContext.SaveChangesAsync();

            return Ok(inmueble);
        }
        catch (Exception e)
        {
            // Manejo de errores
            return BadRequest(e.Message);
        }
    }

    //==========================================
    [HttpGet("{id}")]
   
    public async Task<IActionResult> GetInmueble(int id)
    {
        try
        {
           var usuario = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var user = await applicationDbContext.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);

  var inmueble = await applicationDbContext.Inmuebles
            .Where(e => e.id_Inmuebles == id)
            .ToListAsync();
 
            if (inmueble != null && inmueble.Count > 0)
            {
             
                if (inmueble[0].PropietarioId != user.id_Propietario) 
            {
                return Unauthorized("Acceso denegado");
            }
            else
            {
                // Si coincide, devuelve el inmueble
                return Ok(inmueble);
            }
            }

            else
            {
                return NotFound("Inmueble no encontrado");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpGet("alquiladas")]
    [Authorize]
    public async Task<IActionResult> GetAlquiladas()
    {
        try
        {
         var usuario = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            
            var user = await applicationDbContext.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
            var fecha = DateTime.Today;

        
        var inmuebles = await applicationDbContext.Contratos
            .Where(e => e.Inmueble.PropietarioId == user.id_Propietario && 
                        e.Estado == "Activo" && 
                        e.Fecha_Fin >= fecha)
            .Select(e => e.Inmueble)
            .ToListAsync();

            Console.WriteLine("COUNT: " + inmuebles.Count);
            return Ok(inmuebles);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //==========================================
    [HttpPost("crear")]
    public async Task<IActionResult> CrearInmueble([FromBody] Inmueble inmueble)
    {
        try
        {
            Console.WriteLine("FOTO: " + inmueble.Foto);
            var usuario = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var user = await applicationDbContext.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
            Inmueble inmuebleF = inmueble;
            inmueble.PropietarioId = user.id_Propietario;
            applicationDbContext.Inmuebles.Add(inmueble);
            applicationDbContext.SaveChanges(); // Guarda los cambios en la base de datos

            string nombreFoto = $"img_inmueble_{user.id_Propietario}_{inmueble.id_Inmuebles}.jpg";

           if (inmuebleF.Foto.Contains(","))
            {
                inmuebleF.Foto = inmuebleF.Foto.Split(',')[1];
            }

            // Convierte la cadena base64 en bytes
            byte[] imageBytes = Convert.FromBase64String(inmuebleF.Foto);

            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads","inmuebles");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = nombreFoto;
            string pathCompleto = Path.Combine(path, fileName);
            // inmueble.Foto = Path.Combine("/Uploads", fileName);


            // Crea una memoria en la secuencia de bytes
            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                // Crea una imagen a partir de la secuencia de bytes
                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                image.Save(pathCompleto, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            inmueble.Foto = $"uploads/inmuebles/{nombreFoto}";
            applicationDbContext.Update(inmueble);

            await applicationDbContext.SaveChangesAsync();

            return Ok(inmueble);
        }
        catch (Exception ex)
        {
            return BadRequest("Error al convertir la cadena base64 a imagen: " + ex.Message);
        }
    }

} 
}

//<-- No hay punto y coma aquí

        /*[HttpPost("crear")]
        public async Task<ActionResult<Inmueble>> Post([FromBody] Inmueble inmueble)
        {

            try
            {

                var email = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                var propietario = await applicationDbContext.Propietarios.FirstOrDefaultAsync(x => x.Email == email);
                inmueble.PropietarioId = propietario.IdPropietario;
                if (inmueble.ImagenGuardar != null)
                {
                    var stream1 = new MemoryStream(Convert.FromBase64String(inmueble.ImagenGuardar));
                    IFormFile ImagenInmo = new FormFile(stream1, 0, stream1.Length, "inmueble", ".jpg");
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Random r = new Random();
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "inmueble_" + inmueble.PropietarioId + r.Next(0, 100000) + Path.GetExtension(ImagenInmo.FileName);
                    string pathCompleto = Path.Combine(path, fileName);

                    inmueble.Imagen = Path.Combine("Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        ImagenInmo.CopyTo(stream);
                    }
                    applicationDbContext.Add(inmueble);
                    await applicationDbContext.SaveChangesAsync();
                    return inmueble;
                }
                else
                {

                    return BadRequest("debe incluir una imagen ");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }*/
    

