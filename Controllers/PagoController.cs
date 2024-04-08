using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class PagoController : Controller
{

    private readonly ILogger<PagoController> _logger;

    public PagoController(ILogger<PagoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioPago ru = new RepositorioPago();
        var lista = ru.GetTodosPagos();

        return View(lista);
    }



    public IActionResult Create()
    {

        RepositorioContrato contrato = new RepositorioContrato();
        // RepositorioInmueble inmueble = new RepositorioInmueble();

        ViewBag.Contratos = contrato.GetContracts();
        // ViewBag.Inmueble =  inmueble.GetProperties();
        return View();
    }


    public IActionResult Crear(Pago pago)
    {
        RepositorioPago rp = new RepositorioPago();
        rp.High(pago);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {

        RepositorioPago rep = new RepositorioPago();

        var pago = rep.GetPagoId(id);


        RepositorioContrato contrato = new RepositorioContrato();
        // RepositorioInmueble inmueble = new RepositorioInmueble();

        ViewBag.Contratos = contrato.GetContracts();

        return View(pago);
    }

    // POST: Contrato/Edit/5
    [HttpPost]

    public ActionResult Edit(int id, Pago c)
    {
        try
        {
            RepositorioPago c2 = new RepositorioPago();
            c2.Modification(c);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View();
        }
    }


    public IActionResult Delet(int id)
    {
        try
        {
            if (id > 0)
            {
                
                RepositorioPago ru = new RepositorioPago();
                var pago = ru.GetPagoId(id);
                return View(pago);
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


    [HttpPost, ActionName("Delet")]
    public IActionResult Delet(int id, Pago c)
    {
        try
        {
            RepositorioPago P2 = new RepositorioPago();
           var pago= P2.Low(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View();
        }
    }

public IActionResult Detail(int id){

try
{
    RepositorioPago P2 = new RepositorioPago();
    var pago= P2.GetPagoId(id);
    return View(pago);
}
catch (System.Exception)
{
    
    throw;
}

}
}