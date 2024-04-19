using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class InquilinoController : Controller
{

    private readonly ILogger<InquilinoController> _logger;

    public InquilinoController(ILogger<InquilinoController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult Index()
    {
              var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
        RepositorioInquilino ru = new RepositorioInquilino();
        var lista = ru.GetTenants();

        return View(lista);
    }

[Authorize]

    public IActionResult Create()
    {
              var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Inquilino tenants)
    {
        RepositorioInquilino ru = new RepositorioInquilino();
        ru.High(tenants);
        return RedirectToAction(nameof(Index));
    }
[Authorize]
    [HttpGet]
    public IActionResult Edit(int id)
    {
        if (id > 0)
        {
                  var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
            RepositorioInquilino ru = new RepositorioInquilino();
            var tenants = ru.GetTenantId(id);
            return View(tenants);
        }
        else
        {

            return View();
        }
    }


    //public ActionResult Edit(int id, IFormCollection collection)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Inquilino entidad)
    {

        // Si en lugar de IFormCollection ponemos Propietario, el enlace de datos lo hace el sistema
        Inquilino? p = null;
        RepositorioInquilino ru = new RepositorioInquilino();


        p = ru.GetTenantId(id);
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
                  var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;

            RepositorioInquilino ru = new RepositorioInquilino();
            RepositorioContrato ri = new RepositorioContrato();

            ViewBag.Contratos = ri.ObtenerContratosPorInquilino(id);
            var cont = ru.GetTenantId(id);

            return View(cont);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    [HttpGet]
    public IActionResult Delet(int id)
    {
        try
        {
                  var claims =User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol=Rol;
            if (id > 0)
            {
                
                RepositorioInquilino ru = new RepositorioInquilino();
                var tenants = ru.GetTenantId(id);
                return View(tenants);
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

    [HttpPost, ActionName("Delet")]
           [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
    public IActionResult Delet(int id, Inquilino entidad)
    {
        try
        {
            RepositorioInquilino ru = new RepositorioInquilino();
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