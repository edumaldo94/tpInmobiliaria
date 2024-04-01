using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class ContratoController : Controller{

    private readonly ILogger<ContratoController> _logger;

    public ContratoController(ILogger<ContratoController> logger){
        _logger = logger;
    }
    
    public IActionResult Index(){
    RepositorioContrato ru= new RepositorioContrato();
        var lista= ru.GetContracts();
        
        return View(lista);
    }
    
      

 public IActionResult Create()
        {
            return View();
        }
        public IActionResult Crear(Contrato contract){
            RepositorioContrato ru= new RepositorioContrato();
            ru.High(contract);
            return RedirectToAction(nameof(Index));
        }

 public IActionResult Edit(int id)
        {
            RepositorioContrato c =new RepositorioContrato();
            var contract= c.GetContractId(id);
            return View(contract);
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