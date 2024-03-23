using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class ProprietorController : Controller{

    private readonly ILogger<ProprietorController> _logger;

    public ProprietorController(ILogger<ProprietorController> logger){
        _logger = logger;
    }
    public IActionResult Index(){
        RepositorioProprietor ru= new RepositorioProprietor();
        var lista= ru.GetProprietors();
        
        return View(lista);
    }
    
      public IActionResult Create()
        {
            return View();
        }
        public IActionResult Crear(Proprietor proprietors){
            RepositorioProprietor ru= new RepositorioProprietor();
            ru.High(proprietors);
            return RedirectToAction(nameof(Index));
        }

[HttpGet]
        public IActionResult Edit(int id){
if(id > 0){
RepositorioProprietor ru= new RepositorioProprietor ();
var proprietor = ru.GetProprietorId(id);
return View(proprietor);
}else{
    
  return View();  
}
        }

	
		//public ActionResult Edit(int id, IFormCollection collection)
        [HttpPost]
	//[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Proprietor entidad)
		{
		
			// Si en lugar de IFormCollection ponemos Propietario, el enlace de datos lo hace el sistema
			Proprietor? p = null;
            RepositorioProprietor ru= new RepositorioProprietor ();
		 

				p = ru.GetProprietorId(id);
				p.Name= entidad.Name;
				p.Last_Name = entidad.Last_Name;
				p.Dni = entidad.Dni;
                p.Birthdate=entidad.Birthdate;
                p.Sex=p.Sex;
                p.Address=entidad.Address;
				p.Phone = entidad.Phone;
                p.Email = entidad.Email;
				ru.Modificacion(p);
				TempData["Mensaje"] = "Datos guardados correctamente";
				return RedirectToAction(nameof(Index));
			
	
		}

		[HttpGet]
        public IActionResult Delet(int id){
try{
if(id > 0){

RepositorioProprietor ru= new RepositorioProprietor ();
var proprietor = ru.GetProprietorId(id);
return View(proprietor);
}else{
    
  return View();  
}
}catch(Exception exception){
	throw;
}

        }

		[HttpPost, ActionName("Delet")]
		//[ValidateAntiForgeryToken]
		public IActionResult Delet(int id, Proprietor entidad)
		{
			try
			{
				RepositorioProprietor ru= new RepositorioProprietor ();
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