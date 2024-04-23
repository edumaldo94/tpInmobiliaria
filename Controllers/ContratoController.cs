using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class ContratoController : Controller
{

    private readonly ILogger<ContratoController> _logger;

    public ContratoController(ILogger<ContratoController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult Index()
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        RepositorioContrato ru = new RepositorioContrato();

        var lista = ru.GetContracts();

        return View(lista);
    }


    [Authorize]
    public IActionResult Create()
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        RepositorioInmueble inmueble = new RepositorioInmueble();
        RepositorioInquilino tenant = new RepositorioInquilino();

        ViewBag.Inmueble = inmueble.GetPropertiesDisponibles();
        ViewBag.Tenants = tenant.GetTenants();
        return View();
    }

    public IActionResult PPCrear(Contrato contract)
    {
        RepositorioContrato ru = new RepositorioContrato();
        if (!ru.InmuebleDisponible(contract.InmuebleId, contract.Fecha_Inicio.Value, contract.Fecha_Fin.Value))
        {
            ModelState.AddModelError(string.Empty, "El inmueble no está disponible para el período de tiempo especificado.");
            RepositorioInmueble inmueble = new RepositorioInmueble();
            RepositorioInquilino tenant = new RepositorioInquilino();

            ViewBag.Inmueble = inmueble.GetProperties();
            ViewBag.Tenants = tenant.GetTenants();
            return View("Create", contract); // Mostrar la vista de creación de contrato con el mensaje de error
        }
        ru.High(contract);
        return RedirectToAction(nameof(Index));
    }
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Contrato contract)
    {
        RepositorioContrato ru = new RepositorioContrato();
        RepositorioPago rPago = new RepositorioPago();
        RepositorioInmueble inmueble = new RepositorioInmueble();
        RepositorioInquilino tenant = new RepositorioInquilino();
        RepositorioAuditoria auditor = new RepositorioAuditoria();
        Pago pagoPa = new Pago();
        var userName = User.Identity.Name;

        // Asignar el nombre de usuario al contrato

        if (!ru.InmuebleDisponible(contract.InmuebleId, contract.Fecha_Inicio.Value, contract.Fecha_Fin.Value))
        {
            ModelState.AddModelError(string.Empty, "El inmueble no está disponible para el período de tiempo especificado.");


            ViewBag.Inmueble = inmueble.GetProperties();
            ViewBag.Tenants = tenant.GetTenants();
            return View("Create", contract); // Mostrar la vista de creación de contrato con el mensaje de error
        }
      
        // Guardar el contrato en la base de datos
        int Ccc = ru.High(contract);

        //int buscarElNuevoContr= ru.GetContractId(Ccc);
        // Calcular la fecha del primer pago
        DateTime fechaInicio = contract.Fecha_Inicio.Value;
        DateTime fechaPrimerPago = fechaInicio; // Por ejemplo, se genera un pago mensualmente
        Double Monto = contract.Monto.Value;
        // Crear el primer pago
        inmueble.DisponibleInmuNo(contract.InmuebleId);
        ru.CrearPago(Ccc, fechaPrimerPago, Monto);
        pagoPa = rPago.GetUltimoPagoPorContratoId(Ccc);
        auditor.RegistrarAccionAuditoria(Ccc, pagoPa.PagoId, userName, "Creación de Contrato y Pago");
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public IActionResult Edit(int id)
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        RepositorioContrato repositorioContrato = new RepositorioContrato();

        var contrato = repositorioContrato.GetContractId(id);


        RepositorioInmueble inmueble = new RepositorioInmueble();
        RepositorioInquilino tenant = new RepositorioInquilino();

        ViewBag.Inmueble = inmueble.GetProperties();
        ViewBag.Tenants = tenant.GetTenants();

        return View(contrato);
    }

    // POST: Contrato/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]

    public ActionResult Edit(int id, Contrato c)
    {
        try
        {
            RepositorioContrato c2 = new RepositorioContrato();
            c2.Modification(c);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View();
        }
    }

    [Authorize(Policy = "Administrador")]
    public IActionResult Delet(int id)
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        try
        {
            if (id > 0)
            {

                RepositorioContrato ru = new RepositorioContrato();
                var contract = ru.GetContractId(id);
                return View(contract);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delet(int id, Contrato c)
    {
        try
        {
            RepositorioContrato c2 = new RepositorioContrato();
            c2.Low(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View();
        }
    }


    public IActionResult Detail(int id)
    {

        try
        {
            var claims = User.Claims;
            string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            ViewBag.Rol = Rol;
            RepositorioAuditoria auditor = new RepositorioAuditoria();
            RepositorioContrato c2 = new RepositorioContrato();

            var auditoria = auditor.ObtenerAuditoriaPorContratoId(id);
            var cont = c2.GetContractId(id);
            ViewBag.Auditoria = auditoria;
            ViewBag.AuditoriaB = auditor.ObtenerAuditoriaPorBajaContratoId(id);
            return View(cont);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    [HttpPost]
    // [Authorize(Policy = "Administrador")]
    [Authorize]
    public IActionResult TerminateEarly(int contratoId, DateTime fechaTerminacion)
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        RepositorioPago rPago = new RepositorioPago();
        RepositorioInmueble inmueble = new RepositorioInmueble();
        RepositorioAuditoria auditoria = new RepositorioAuditoria();
        Pago pagoPa = new Pago();
        // Obtener el contrato a terminar anticipadamente
        var contrato = repositorioContrato.GetContractId(contratoId);
        if (fechaTerminacion < contrato.Fecha_Inicio || fechaTerminacion > contrato.Fecha_Fin)
        {
            TempData["ErrorMessage"] = "La fecha de terminación no está dentro del rango de fechas del contrato";
            // Si la fecha de terminación no está dentro del rango, redireccionar a alguna vista de error o mostrar un mensaje al usuario
            return View("Detail", contrato);
        }
        var ultimoPago = rPago.GetUltimoPagoPorContratoId(contratoId);
        DateTime FechaUltimoP = ultimoPago.FechaPago.Value;
        // Verificar si el inquilino ha pagado el mes anterior
        if (FechaUltimoP.Month != fechaTerminacion.AddMonths(-1).Month)
        {
            TempData["ErrorMessage"] = "No puedes terminar el contrato anticipadamente porque no has pagado el mes anterior.";
            // Redireccionar a alguna vista de error o mostrar un mensaje al usuario
            return View("Detail", contrato);
        }
        TimeSpan duracionContrato = contrato.Fecha_Fin.Value - contrato.Fecha_Inicio.Value;
        TimeSpan duracionReal = fechaTerminacion - contrato.Fecha_Inicio.Value;

        double mesesContrato = duracionContrato.TotalDays;
        double mesesReal = duracionReal.TotalDays;
        double multa = 0;


        if ((mesesContrato / 2) < mesesReal)
        {
            multa = contrato.Monto.Value * 2;

        }
        else
        {
            multa = contrato.Monto.Value;
        }


        Pago multaPago = new Pago
        {
            ContratoId = contrato.id_Contrato,
            NumeroPago = rPago.GetPagosPorContratoId(contrato.id_Contrato).Count + 1, // Obtener el número de pago siguiente
            Concepto = fechaTerminacion.ToString("MMM"),
            FechaPago = fechaTerminacion,
            Importe = multa,
            EstadoPago = "Multa por terminación anticipada"

        };
        var userName = User.Identity.Name;
        // Guardar el nuevo pago en la base de datos
        rPago.AgregarPago(multaPago);
        repositorioContrato.FinalizarContrato(contrato.id_Contrato);
        inmueble.DisponibleInmuSi(contrato.InmuebleId);
        pagoPa = rPago.GetUltimoPagoPorContratoId(contrato.id_Contrato);
        auditoria.RegistrarAccionAuditoria(contrato.id_Contrato, pagoPa.PagoId, userName, "Fin de Contrato y Pago de multa");
        // Redireccionar a la vista de detalles del contrato
        //  return RedirectToAction("Detail", "Contrato", new { contratoId = contrato.id_Contrato });
        //     return RedirectToAction(nameof(Detail), "Contrato", new { contratoId = contrato});
        //  return View("Detail", contrato);
        return RedirectToAction(nameof(Index));
    }

    private bool PagosEstanAlDia(int contratoId)
    {
        RepositorioPago rPago = new RepositorioPago();

        // Obtener todos los pagos asociados al contrato
        var pagos = rPago.GetPagosPorContratoId(contratoId);

        // Verificar si hay algún pago pendiente
        foreach (var pago in pagos)
        {
            if (pago.FechaPago > DateTime.Today)
            {
                // Si hay algún pago con fecha posterior a hoy, significa que no está al día
                return false;
            }
        }
        return true;
    }
    public IActionResult RenewView(int contratoId)
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        RepositorioContrato repositorioContrato = new RepositorioContrato();

        var contrato = repositorioContrato.GetContractId(contratoId);


        RepositorioInmueble inmueble = new RepositorioInmueble();
        RepositorioInquilino tenant = new RepositorioInquilino();

        ViewBag.Inmueble = inmueble.GetProperties();
        ViewBag.Tenants = tenant.GetTenants();
        return View("Renovar", contrato);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Renew(Contrato contrato)
    {
        var claims = User.Claims;
        string Rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        ViewBag.Rol = Rol;
        if (ModelState.IsValid) // Verificar si el modelo es válido
        {

            bool superpuesto = VerificarSuperposicionContratos(contrato);

            if (superpuesto)
            {
                ModelState.AddModelError(string.Empty, "El nuevo contrato se superpone con otro contrato existente.");
                //return View("Renovar", contrato);

                RepositorioInmueble inmueble = new RepositorioInmueble();
                RepositorioInquilino tenant = new RepositorioInquilino();

                ViewBag.Inmueble = inmueble.GetProperties();
                ViewBag.Tenants = tenant.GetTenants();
                return View("Renovar", contrato);
            }
            // Guardar el contrato en la base de datos
            RepositorioContrato repositorioContrato = new RepositorioContrato();
            int nuevoContratoId = repositorioContrato.High(contrato);
            var cont = repositorioContrato.GetContractId(nuevoContratoId);
            DateTime fechaInicio = contrato.Fecha_Inicio.Value;
            DateTime fechaPrimerPago = fechaInicio; // Por ejemplo, se genera un pago mensualmente
            Double Monto = contrato.Monto.Value;
            // Crear el primer pago

            repositorioContrato.CrearPago(nuevoContratoId, fechaPrimerPago, Monto);
            // Redireccionar a la vista de detalles del nuevo contrato
            //   return RedirectToAction("Detail", "Contrato", cont);
            return View("Detail", cont);
        }
        else
        {
            // Si el modelo no es válido, mostrar la vista de renovación nuevamente con los errores
            return View("Renovar", contrato);
        }
    }

    private bool VerificarSuperposicionContratos(Contrato contrato)
    {
        // Obtener todos los contratos que se superponen en fechas con el nuevo contrato
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        var contratosSuperpuestos = repositorioContrato.ObtenerContratosSuperpuestos(contrato.InmuebleId, contrato.Fecha_Inicio.Value, contrato.Fecha_Fin.Value);

        // Si hay algún contrato superpuesto, devolver true; de lo contrario, devolver false
        return contratosSuperpuestos.Any();
    }


}