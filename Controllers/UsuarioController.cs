using System.Diagnostics;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;


using tpInmobliaria.Models;


namespace tpInmobliaria.Controllers;


public class UsuarioController : Controller{//define una clase que hereda de controller q actúa como un controlador web
  private readonly IConfiguration _configuracion; //Este campo se utiliza para almacenar la configuración de la aplicación, como la configuración del archivo appsettings.json
        private readonly IWebHostEnvironment _environment;//proporciona  información sobre rutas de archivos, nombres de entorno, etc


    private readonly ILogger<UsuarioController> _logger;


    public UsuarioController(ILogger<UsuarioController> logger,IConfiguration configuracion,IWebHostEnvironment environment){
     
        _logger = logger;
         _configuracion = configuracion;
         _environment= environment;
    }
   
 


        // GET: Usuarios
       [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
           try{
           
                var claims =User.Claims;
                string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                ViewBag.Rol=Rol;
                /* */
                RepositorioUsuario repoU= new RepositorioUsuario();
                IList<Usuario> users=repoU.GetObtenerTodos();
                return View(users);
           }catch(Exception ex){
                throw;
           }
        }
        [Authorize(Policy = "Administrador")]
        // GET: Usuarios/Details/5
        public ActionResult Detail(int UsuarioId)
        {  
            try{
               
               
                var claims =User.Claims;
                string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                ViewBag.Rol=Rol;
                /* */
    RepositorioUsuario repoU= new RepositorioUsuario();
                Usuario user=repoU.ObtenerId(UsuarioId);
                return View(user);
            }catch(Exception){
                throw;
            }
        }


        // GET: Usuarios/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            try{
                
            var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
            /**/              
                return View();
            }catch(Exception ex){
                throw;
            }


        }


        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
                public ActionResult Create(Usuario usuario)
        {
            try
            {
                    RepositorioUsuario repoU= new RepositorioUsuario();
                if (!ModelState.IsValid)
                    return View();
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                    password: usuario.Password,
                                    salt: System.Text.Encoding.ASCII.GetBytes(_configuracion["Salt"]),
                                    prf: KeyDerivationPrf.HMACSHA256,
                                    iterationCount: 1000,
                                    numBytesRequested: 256 / 8));
                    usuario.Password = hashed;
                    var nbreRnd = Guid.NewGuid();
                    if (usuario.ImgAvatar != null){
                        string wwwPath = _environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "Uploads");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "avatar_" + nbreRnd + Path.GetExtension(usuario.ImgAvatar.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        usuario.Avatar = Path.Combine("/Uploads", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            usuario.ImgAvatar.CopyTo(stream);
                        }
                    }
                    if(usuario.Avatar==null || usuario.Avatar.Equals("")){
                        usuario.Avatar="sin avatar";
                    }
                    int res = repoU.Create(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                   
                throw;
            }
        }

        [Authorize]

        public ActionResult Edit(int UsuarioId)
        {
       try{
        
                    RepositorioUsuario repoU= new RepositorioUsuario();
              
                var claims =User.Claims;
                string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                ViewBag.Rol=Rol;          
                   
                string usuarioAutenticadoId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (usuarioAutenticadoId == null)
        {
            // Si no se puede obtener el id del usuario autenticado, redirige a una página de error o maneja la situación según tu caso.
             ViewBag.ErrorMessage = "Solo el administrador puede realizar esta acción.";
        }

        if (UsuarioId == 0)
        {
            // Si no se proporciona un UsuarioId válido, puedes redirigir a una página de error o manejar la situación según tu caso.
       ViewBag.ErrorMessage = "Solo el administrador puede realizar esta acción.";
        }

        Usuario user = repoU.ObtenerId(UsuarioId);

        if (user == null)
        {
            // Si el usuario no se encuentra en la base de datos, puedes redirigir a una página de error o manejar la situación según tu caso.
           ViewBag.ErrorMessage = "Solo el administrador puede realizar esta acción.";
        }

        if (!User.IsInRole("Administrador") && user.UsuarioId.ToString() != usuarioAutenticadoId)
        {
            // Si el usuario autenticado no es un administrador y está intentando editar un usuario que no es él mismo, redirige a una página de acceso denegado.
             ViewBag.ErrorMessage = "Solo el administrador puede realizar esta acción.";
        }

        return View(user);
            }catch(Exception ex){
                throw;
            }


        }


        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int UsuarioId, Usuario user)
        {
            try
            {   
                
                var claims =User.Claims;
                string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                ViewBag.Rol=Rol;    
                   
                 
                RepositorioUsuario repoU= new RepositorioUsuario();
          string usuarioAutenticadoId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                       
    
        // Obtener el usuario correspondiente al UsuarioId
        Usuario usuario = repoU.ObtenerId(Convert.ToInt32(usuarioAutenticadoId));

        // Verificar si el usuario existe
        if (usuario == null)
        {
            // Manejar el caso de usuario no encontrado
            return NotFound("El usuario no fue encontrado");
        }

        // Actualizar los campos necesarios del usuario
        user.UsuarioId = usuario.UsuarioId;
        user.Password = usuario.Password;
        user.Avatar = usuario.Avatar;
                repoU.Edit(user);
                return View(user);
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Edit2(int id, Usuario user)
        {
            try
            {  
                 RepositorioUsuario repoU= new RepositorioUsuario();
                 var claims =User.Claims;
                string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                ViewBag.Rol=Rol;    
                   
                  string usuarioAutenticadoId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                       
    
        // Obtener el usuario correspondiente al UsuarioId
        Usuario usuario = repoU.ObtenerId(Convert.ToInt32(usuarioAutenticadoId));
                if (usuario.Avatar != null && !string.IsNullOrEmpty(usuario.Avatar)){
                        string wwwPath = _environment.WebRootPath;
                        string path2 = Path.Combine(wwwPath, "Uploads");
                        string img = Path.Combine(path2, Path.GetFileName(usuario.Avatar));
                        if (System.IO.File.Exists(img))
                        {
                            System.IO.File.Delete(img);
                        }
                    }
                if(user.ImgAvatar!=null){
                        string wwwPath = _environment.WebRootPath;
                        var nbreRnd = Guid.NewGuid();
                        string path = Path.Combine(wwwPath, "Uploads");
                        if (!Directory.Exists(path)){
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "avatar_" + nbreRnd + Path.GetExtension(user.ImgAvatar.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        usuario.Avatar = Path.Combine("/Uploads", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            user.ImgAvatar.CopyTo(stream);
                        }
                }else{
                    usuario.Avatar="";
                }
                repoU.Edit(usuario);
                return RedirectToAction(nameof(Edit),new{UsuarioId=usuario.UsuarioId});
            }catch(Exception ex){
                throw;
            }
        }
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult DeleteAvatar(int UsuarioId)
{
    try
    {
        RepositorioUsuario repoU = new RepositorioUsuario();
     
              
        Usuario usuario = repoU.ObtenerId(UsuarioId);
          if (!User.IsInRole("Administrador"))
                {

                    var usuarioActual = repoU.ObtenerCorreo(User.Identity.Name);
                    if (usuarioActual.UsuarioId != usuario.UsuarioId)
                    {
                        ViewBag.ErrorMessage = "Solo el administrador puede realizar esta acción.";
                            return RedirectToAction(nameof(Index));
                    }
                }
        if (usuario.Avatar != null && !string.IsNullOrEmpty(usuario.Avatar))
        {
            string wwwPath = _environment.WebRootPath;
            string path2 = Path.Combine(wwwPath, "Uploads");
            string img = Path.Combine(path2, Path.GetFileName(usuario.Avatar));
            if (System.IO.File.Exists(img))
            {
                System.IO.File.Delete(img);
            }
            
            usuario.Avatar = "sin avatar";
            repoU.Edit(usuario);
        }
        
        return RedirectToAction(nameof(Edit), new { UsuarioId = UsuarioId });
    }
    catch (Exception ex)
    {
        throw;
    }
}


        // POST: Usuarios/Edit
        [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Edit3(int id, Usuario user)
        {
            try
            {

                    RepositorioUsuario repoU= new RepositorioUsuario();
                       if (!User.IsInRole("Administrador"))
                {

                    var usuarioActual = repoU.ObtenerCorreo(User.Identity.Name);
                    if (usuarioActual.UsuarioId != user.UsuarioId)
                    {
                        ViewBag.Error = "No tiene permiso para modificar otro usuario";
                            return RedirectToAction(nameof(Index));
                    }
                }
                   
                Usuario usuario=repoU.ObtenerId(Convert.ToInt32(user.UsuarioId));
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: user.PasswordAnterior,
                        salt: System.Text.Encoding.ASCII.GetBytes(_configuracion["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                string contraNueva = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: user.Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(_configuracion["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));        
                if(hashed.Equals(usuario.Password)){
                    usuario.Password=contraNueva;
                    TempData["Valido"]="Contraseña cambiada con exito";
                    repoU.Edit(usuario);
                }else{
                    TempData["Error"]="Contraseña Anterior Equivocada";
                }
                return RedirectToAction(nameof(Edit),new{UsuarioId=usuario.UsuarioId});
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        [Authorize(Policy = "Administrador")]
        // GET: Usuarios/Delete/5
        public ActionResult Delet(int UsuarioId)
        {
            try{
                    RepositorioUsuario repoU= new RepositorioUsuario();
               
            var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
            
            Usuario user=repoU.ObtenerId(UsuarioId);
            return View(user);
            }catch(Exception ex){
                throw;
            }


        }


        // POST: Usuarios/Delete/5
        [HttpPost]
          [Authorize(Policy = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Delet(int id, Usuario user)
        {
            try
            {
                    RepositorioUsuario repoU= new RepositorioUsuario();
                // TODO: Add delete logic here
                Usuario usuario=repoU.ObtenerId(Convert.ToInt32(user.UsuarioId));
                if (usuario.Avatar != null && !string.IsNullOrEmpty(usuario.Avatar)){
                        string wwwPath = _environment.WebRootPath;
                        string path2 = Path.Combine(wwwPath, "Uploads");
                        string img = Path.Combine(path2, Path.GetFileName(usuario.Avatar));
                        if (System.IO.File.Exists(img))
                        {
                            System.IO.File.Delete(img);
                        }
                    }
                repoU.Delete(Convert.ToInt32(user.UsuarioId));
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }


        // POST: Usuarios/Login/
        [HttpPost]
    [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Usuario user)
        {
            try
            {
                    RepositorioUsuario repoU= new RepositorioUsuario();
                var volverUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: user.Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(_configuracion["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    var usuario = repoU.ObtenerCorreo(user.Correo);
                    if (usuario == null || usuario.Password != hashed)
                    {
                        ModelState.AddModelError("", "El Correo o la clave son Incorrectos");
                        TempData["returnUrl"] = volverUrl;
                        return View();
                    }
               
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Correo),
                        new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                        new Claim(ClaimTypes.Role, usuario.RolNombre),
                        new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId+""),
                    };


                    var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);


                    await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,

                         new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(volverUrl);
                }
                TempData["returnUrl"] = volverUrl;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


    }


