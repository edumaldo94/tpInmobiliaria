using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class ContratoController : Controller
{

    private readonly ILogger<ContratoController> _logger;

    public ContratoController(ILogger<ContratoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioContrato ru = new RepositorioContrato();

        var lista = ru.GetContracts();

        return View(lista);
    }



    public IActionResult Create()
    {

        RepositorioInmueble inmueble = new RepositorioInmueble();
        RepositorioInquilino tenant = new RepositorioInquilino();

        ViewBag.Inmueble = inmueble.GetProperties();
        ViewBag.Tenants = tenant.GetTenants();
        return View();
    }
    public IActionResult Crear(Contrato contract)
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

    public IActionResult Edit(int id)
    {

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


    public IActionResult Delet(int id)
    {
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
              
            RepositorioContrato c2 = new RepositorioContrato();
            var cont = c2.GetContractId(id);
            return View(cont);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public bool VerificarDisponibilidadInmueble(double monto, int inmuebleId, DateTime fechaInicio, DateTime fechaFin)
    {
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        // Obtener todos los contratos que se superponen en fechas con el contrato actual
        var contratosSuperpuestos = repositorioContrato.ObtenerContratosSuperpuestos(inmuebleId, fechaInicio, fechaFin);

        // Si no hay contratos superpuestos, el inmueble está disponible
        return contratosSuperpuestos.Count == 0;
    }
    public double CalcularMultaTerminacionAnticipada(DateTime fechaTerminacion, DateTime fechaInicio, double monto)
    {
        // Calcular la cantidad de meses entre la fecha de inicio y la fecha de terminación anticipada
        int mesesTranscurridos = (fechaTerminacion.Month - fechaInicio.Month) + 12 * (fechaTerminacion.Year - fechaInicio.Year);

        // Calcular la multa según la cantidad de meses transcurridos
        double multa = 0;
        if (mesesTranscurridos < 0)
        {
            // Si la fecha de terminación anticipada es anterior a la fecha de inicio del contrato
            // No se aplica multa
            multa = 0;
        }
        else if (mesesTranscurridos < 6)
        {
            // Si se cumplió menos de la mitad del tiempo original de alquiler
            multa = 2 * monto;
        }
        else
        {
            // Caso contrario, se paga un mes adicional de multa
            multa = monto;
        }

        return multa;
    }

    [HttpPost]
    public IActionResult TerminateEarly(int contratoId, DateTime fechaTerminacion)
    {
        RepositorioContrato repositorioContrato = new RepositorioContrato();
        RepositorioPago rPago = new RepositorioPago();
        // Obtener el contrato a terminar anticipadamente
        var contrato = repositorioContrato.GetContractId(contratoId);

    TimeSpan duracionContrato = (contrato.Fecha_Fin ?? DateTime.MinValue) - (contrato.Fecha_Inicio ?? DateTime.MinValue);

    //    TimeSpan duracionReal = fechaTerminacion - contrato.Fecha_Inicio;
TimeSpan duracionReal = (fechaTerminacion) - (contrato.Fecha_Inicio ?? DateTime.MinValue);

        double mesesContrato = duracionContrato.TotalDays / 30;
        double mesesReal = duracionReal.TotalDays / 30;
   double multa = 0;
        if ((mesesContrato / 2) < mesesReal)
        {
               multa = (contrato.Monto ?? 0) * 2;

       }

        // Calcular la multa por terminación anticipada
        //  double multa = CalcularMultaTerminacionAnticipada(contrato.Fecha_Fin.Value, contrato.Fecha_Inicio.Value, contrato.Monto.Value);
      //  double multa = repositorioContrato.CalcularMulta(contratoId, fechaTerminacion);
        // Crear un nuevo pago para registrar la multa
        Pago multaPago = new Pago
        {
            ContratoId = contrato.id_Contrato,
            NumeroPago = rPago.GetPagosPorContratoId(contrato.id_Contrato).Count + 1, // Obtener el número de pago siguiente
            Concepto = "---",
            FechaPago = fechaTerminacion,
            Importe = multa,
            EstadoPago = "Multa por terminación anticipada"

        };
        
        // Guardar el nuevo pago en la base de datos
        rPago.AgregarPago(multaPago);
       
        // Redireccionar a la vista de detalles del contrato
        //  return RedirectToAction("Detail", "Contrato", new { contratoId = contrato.id_Contrato });
        //     return RedirectToAction(nameof(Detail), "Contrato", new { contratoId = contrato});
        return View("Detail", contrato);
    }


    public IActionResult RenewView(int contratoId)
    {

        RepositorioContrato repositorioContrato = new RepositorioContrato();

        var contrato = repositorioContrato.GetContractId(contratoId);


        RepositorioInmueble inmueble = new RepositorioInmueble();
        RepositorioInquilino tenant = new RepositorioInquilino();

        ViewBag.Inmueble = inmueble.GetProperties();
        ViewBag.Tenants = tenant.GetTenants();
        return View("Renovar", contrato);
    }


    [HttpPost]
    public IActionResult Renew(Contrato contrato)
    {
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