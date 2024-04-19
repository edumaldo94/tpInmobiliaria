using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class PropietarioController : Controller
{

    private readonly ILogger<PropietarioController> _logger;

    public PropietarioController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Index()
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        RepositorioPropietario ru = new RepositorioPropietario();
        var lista = ru.GetProprietors();

        return View(lista);
    }
    [Authorize]
    public IActionResult Create()
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Propietario proprietors)
    {
        RepositorioPropietario ru = new RepositorioPropietario();
        ru.High(proprietors);
        return RedirectToAction(nameof(Index));
    }
    [Authorize]
    [HttpGet]

    public IActionResult Edit(int id)
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        if (id > 0)
        {
            RepositorioPropietario ru = new RepositorioPropietario();
            var proprietor = ru.GetProprietorId(id);
            return View(proprietor);
        }
        else
        {

            return View();
        }
    }


    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Propietario entidad)
    {

        // Si en lugar de IFormCollection ponemos Propietario, el enlace de datos lo hace el sistema
        Propietario? p = null;
        RepositorioPropietario ru = new RepositorioPropietario();


        p = ru.GetProprietorId(id);
        p.Nombre = entidad.Nombre;
        p.Apellido = entidad.Apellido;
        p.Dni = entidad.Dni;
        p.Email = entidad.Email;
        p.Telefono = entidad.Telefono;

        ru.Modification(p);
        TempData["Mensaje"] = "Datos guardados correctamente";
        return RedirectToAction(nameof(Index));


    }


    [Authorize]
    public IActionResult Detail(int id)
    {

        try
        {
            var claims = User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol = Rol;
            RepositorioPropietario ru = new RepositorioPropietario();
            var cont = ru.GetProprietorId(id);
            ViewBag.Inmuebles = ru.ObtenerInmueblesPorPropietario(id);
            return View(cont);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    [Authorize]
    [HttpGet]
    public IActionResult Delet(int id)
    {
        try
        {
            var claims = User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol = Rol;
            if (id > 0)
            {

                RepositorioPropietario ru = new RepositorioPropietario();
                var proprietor = ru.GetProprietorId(id);
                return View(proprietor);
            }
            else
            {

                return View();
            }
        }
        catch (Exception exception)
        {
            throw;
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Administrador")]
    public IActionResult Delet(int id, Propietario entidad)
    {
        try
        {

            RepositorioPropietario ru = new RepositorioPropietario();
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



}