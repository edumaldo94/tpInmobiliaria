using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class PagoController : Controller
{

    private readonly ILogger<PagoController> _logger;

    public PagoController(ILogger<PagoController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult Index()
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        RepositorioPago ru = new RepositorioPago();
        var lista = ru.GetTodosPagos();

        return View(lista);
    }


    [Authorize]
    public IActionResult Create(int? contId)
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;

        RepositorioContrato contrato = new RepositorioContrato();



        if (contId == null)
        {
            ViewBag.Contratos = contrato.GetContracts();

            // Si contratoId es null, obtener todos los contratos
            return View();
        }
        else
        {

            // Si contratoId no es null, obtener el contrato correspondiente
            ViewBag.Contratos = contrato.GetContractId(contId.Value);
            return View(ViewBag.Contratos);
        }
        //  ViewBag.ContratoId = contrato.GetContractId(contratoId); 

        //  return View();

    }
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Pago pago)
    {
        RepositorioPago rp = new RepositorioPago();
        rp.High(pago);


        return RedirectToAction(nameof(Index));
    }

    [Authorize]

    public IActionResult Edit(int id)
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        RepositorioPago rep = new RepositorioPago();

        var pago = rep.GetPagoId(id);


        RepositorioContrato contrato = new RepositorioContrato();
        // RepositorioInmueble inmueble = new RepositorioInmueble();

        ViewBag.Contratos = contrato.GetContracts();

        return View(pago);
    }
    [Authorize]
    // POST: Contrato/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
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

    [Authorize]
    public IActionResult Delet(int id)
    {
        try
        {
            var claims = User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol = Rol;
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



    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Administrador")]
    public IActionResult Delet(int id, Pago c)
    {
        try
        {
            RepositorioPago P2 = new RepositorioPago();
            var pago = P2.Low(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View();
        }
    }
    [Authorize]
    public IActionResult Detail(int id)
    {


        try
        {
            var claims = User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol = Rol;
            RepositorioPago P2 = new RepositorioPago();
            var pago = P2.GetPagoId(id);
            return View(pago);
        }
        catch (System.Exception)
        {

            throw;
        }

    }
    [Authorize]
    public IActionResult PagosDeCont(int ContratoId)
    {

        try
        {
            var claims = User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol = Rol;
            RepositorioInmueble inmueble = new RepositorioInmueble();
            RepositorioPago P2 = new RepositorioPago();
            RepositorioContrato cont = new RepositorioContrato();
            IList<Pago> pago = P2.GetPagosPorContratoId(ContratoId);
            if (ContratoId == 0)
            {
                // Manejar el caso en el que ContratoId es cero
                return BadRequest("ContratoId no puede ser cero");
            }


            ViewBag.Pagos = pago;
            bool inquilinoTienePagosPendientes = P2.InquilinoTienePagosPendientes(ContratoId);

            ViewBag.PagoPendientes = inquilinoTienePagosPendientes;

            ViewBag.Contrato = cont.GetContractId(ContratoId);

            // Iterar sobre la lista de pagos para aplicar las validaciones
            foreach (var pagos in pago)
            {
                // Obtener el PagoId de cada pago
                int pagoId = pagos.PagoId.Value;

                // Obtener la fecha de inicio y fin del contrato
                DateTime fechaInicioContrato = ViewBag.Contrato.Fecha_Inicio;
                DateTime fechaFinContrato = ViewBag.Contrato.Fecha_Fin;

                // Obtener la fecha de pago del pago actual
                DateTime fechaPago = pagos.FechaPago.Value;


                // Verificar si la fecha de pago excede la fecha de fin del contrato
                if (fechaPago.AddMonths(-1) > fechaFinContrato)
                {
                    // Agregar un mensaje de error al ModelState
                    ModelState.AddModelError(string.Empty, $"El pago con PagoId {pagoId} excede la fecha de fin del contrato.");
                }

                var ultimoPago = pago.LastOrDefault();
                if (fechaPago != null && fechaPago.Month == ViewBag.Contrato.Fecha_Fin.Month &&
            fechaPago.Year == ViewBag.Contrato.Fecha_Fin.Year)
                {
                    ViewBag.MostrarNuevoPago = true; // No mostrar el botón "Nuevo Pago"
                    cont.FinalizarContrato(ViewBag.Contrato.id_Contrato);
                    inmueble.DisponibleInmuSi(ViewBag.Contrato.InmuebleId);

                }
                else
                {
                    ViewBag.MostrarNuevoPago = false; // Mostrar el botón "Nuevo Pago"
                }


            }


            // Si no hay errores de validación, retornar la vista con la lista de pagos
            return View(pago);
        }
        catch (System.Exception)
        {
            // Manejar la excepción adecuadamente
            throw;
        }
    }
    [Authorize]
    public ActionResult NuevoPago(int ContratoId)
    {
        try
        {
            var claims = User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol = Rol;
            RepositorioContrato contrato = new RepositorioContrato();
            ViewBag.Contratos = contrato.GetContractId(ContratoId);
            RepositorioPago pago = new RepositorioPago();
            ViewBag.pagos = pago.GetPagosPorContratoId(ContratoId);
            bool inquilinoTienePagosPendientes = pago.InquilinoTienePagosPendientes(ContratoId);

            DateTime ultimaFechaPago = pago.ObtenerUltimaFechaPago(ViewBag.Contratos.id_Contrato);

            // Calcular la fecha del siguiente pago
            DateTime siguienteFechaPago = ultimaFechaPago.AddMonths(1); // Por ejemplo, se genera un pago mensualmente

            // Verificar si la fecha actual está cerca de la fecha límite de pago
            if (DateTime.Today > ultimaFechaPago.AddDays(5))
            {
                TempData["AlertaPagoAtrasado"] = true; // Mostrar alerta de pago atrasado en la vista
            }
            //double montoDelPago =  ViewBag.Contratos.Monto;
            // Crear el nuevo pago
            //    contrato.CrearPago(ViewBag.Contratos.contratoId, siguienteFechaPago,montoDelPago);

            return View();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /*
            public IActionResult GenerarNuevoPago(int contratoId)
        {
            RepositorioPago pago = new RepositorioPago();
              RepositorioContrato contratoC = new RepositorioContrato();
            // Obtener la fecha del último pago del contrato
            DateTime ultimaFechaPago = pago.ObtenerUltimaFechaPago(contratoId);

            // Calcular la fecha del siguiente pago
            DateTime siguienteFechaPago = ultimaFechaPago.AddMonths(1); // Por ejemplo, se genera un pago mensualmente

            // Verificar si la fecha actual está cerca de la fecha límite de pago
            if (DateTime.Today > ultimaFechaPago.AddDays(5))
            {
                TempData["AlertaPagoAtrasado"] = true; // Mostrar alerta de pago atrasado en la vista
            }

            // Crear el nuevo pago
            contratoC.CrearPago(contratoId, siguienteFechaPago);

            return RedirectToAction("Detalles", "Contrato", new { id = contratoId });
        }
    }
    */
}