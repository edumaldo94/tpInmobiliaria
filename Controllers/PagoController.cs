using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class PagoController : Controller{

    private readonly ILogger<PagoController> _logger;

    public PagoController(ILogger<PagoController> logger){
        _logger = logger;
    }
    
    public IActionResult Index(){
    RepositorioPago ru= new RepositorioPago();
     var lista= ru.GetTodosPagos();
      
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


        public IActionResult Crear(Pago pago){
            RepositorioPago rp= new RepositorioPago();
            rp.High(pago);
            return RedirectToAction(nameof(Index));
        }

public IActionResult Edit(int id)
{
 
    RepositorioContrato repositorioContrato = new RepositorioContrato();
  
    var contrato = repositorioContrato.GetContractId(id);
    
 
    RepositorioInmueble inmueble = new RepositorioInmueble();
    RepositorioInquilino tenant = new RepositorioInquilino();
    
    ViewBag.Inmueble = inmueble.GetProperties();
    ViewBag.Tenants =  tenant.GetTenants();

    return View(contrato);
}

        // POST: Contrato/Edit/5
        [HttpPost]
     
        public ActionResult Edit(int id, Contrato c)
        {
            try
            {
RepositorioContrato c2 =new RepositorioContrato();
                c2.Modification(c);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }


        public IActionResult Delet(int id){
try{
if(id > 0){

RepositorioContrato ru= new RepositorioContrato ();
var contract = ru.GetContractId(id);
return View(contract);
}else{
    
  return View();  
}
}catch(Exception exception){
	throw;
}
        }
  [HttpPost]
       
     
        public IActionResult Delet(int id, Contrato c)
        {
            try
            {
RepositorioContrato c2 =new RepositorioContrato();
                c2.Low(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

} 