using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class InmuebleController : Controller
{

    private readonly ILogger<InmuebleController> _logger;

    public InmuebleController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult Index()
    {
              var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
        RepositorioInmueble ru = new RepositorioInmueble();
        var lista = ru.GetProperties();

        return View(lista);
    }
[Authorize]
    public IActionResult Create()
    {
              var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
        var repoProprietor = new RepositorioPropietario(); // Crear una instancia de RepositorioProprietor
        var proprietors = repoProprietor.GetProprietors();
        var propertyModel = new Inmueble();

        // Pasar la lista de propietarios al ViewBag o a través de un ViewModel
        ViewBag.Proprietors = proprietors;

        // Pasar la instancia de Property como modelo a la vista
        return View(propertyModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public IActionResult Crear(Inmueble proprietors)
    {
        RepositorioInmueble ru = new RepositorioInmueble();
        ru.Create(proprietors);
        return RedirectToAction(nameof(Index));
    }
[Authorize]
    public IActionResult Edit(int id)
    {
              var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
        RepositorioInmueble ru = new RepositorioInmueble();
        var inmueble = ru.ObtenerPorId(id);

        if (inmueble == null)
        {
            return NotFound();
        }

        RepositorioPropietario proprietors = new RepositorioPropietario();
        //  p.Propietario = rp.GetProprietorId(p.PropietarioId);
        ViewBag.Proprietors = proprietors.GetProprietors();
        return View(inmueble);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Inmueble inmueble)
    {

        inmueble.id_Inmuebles = id;

        // Aquí debes guardar los cambios en el inmueble utilizando tu repositorio

        RepositorioInmueble rI = new RepositorioInmueble();
        rI.Modificacion(inmueble);

        return RedirectToAction(nameof(Index));


    }
    [Authorize]
    public IActionResult Detail(int id)
    {

        try
        {
                  var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;

            RepositorioInmueble ru = new RepositorioInmueble();
            var cont = ru.ObtenerPorId(id);

            return View(cont);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

[Authorize]
    public IActionResult Delet(int id)
    {
        try
        {
            if (id > 0)
            {
      var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
                RepositorioInmueble ru = new RepositorioInmueble();
                var inmueble = ru.ObtenerPorId(id);
                RepositorioPropietario rp = new RepositorioPropietario();
                var propietario = rp.GetProprietorId(inmueble.PropietarioId);
                inmueble.ProprietorName = propietario.Nombre;
                return View(inmueble);
            }
            else
            {

                return View();
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Error al intentar eliminar el propietario." + ex;
            return RedirectToAction(nameof(Index));
        }

    }

         [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
    //[ValidateAntiForgeryToken]
    public IActionResult Delet(int id, Inmueble inmueble)
    {
        try
        {
            RepositorioInmueble ru = new RepositorioInmueble();
            var proprietor = ru.Low(id);
            TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {//poner breakpoints para detectar errores
            TempData["ErrorMessage"] = "Error al intentar eliminar el propietario." + ex;
            return RedirectToAction(nameof(Index));
        }
    }

   [Authorize]
    public IActionResult Disponible()
    {
              var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
        RepositorioInmueble ru = new RepositorioInmueble();
        var lista = ru.GetPropertiesDisponibles();

        return View(lista);
    }

}