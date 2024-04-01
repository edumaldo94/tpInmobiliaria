using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class PropietarioController : Controller{

    private readonly ILogger<PropietarioController> _logger;

    public PropietarioController(ILogger<PropietarioController> logger){
        _logger = logger;
    }
    public IActionResult Index(){
        RepositorioPropietario ru= new RepositorioPropietario();
        var lista= ru.GetProprietors();
        
        return View(lista);
    }
    
      public IActionResult Create()
        {
            return View();
        }
        public IActionResult Crear(Propietario proprietors){
            RepositorioPropietario ru= new RepositorioPropietario();
            ru.High(proprietors);
            return RedirectToAction(nameof(Index));
        }

[HttpGet]
        public IActionResult Edit(int id){
if(id > 0){
RepositorioPropietario ru= new RepositorioPropietario ();
var proprietor = ru.GetProprietorId(id);
return View(proprietor);
}else{
    
  return View();  
}
        }

	
		//public ActionResult Edit(int id, IFormCollection collection)
        [HttpPost]
	//[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Propietario entidad)
		{
		
			// Si en lugar de IFormCollection ponemos Propietario, el enlace de datos lo hace el sistema
			Propietario? p = null;
            RepositorioPropietario ru= new RepositorioPropietario ();
		 

				p = ru.GetProprietorId(id);
				p.Nombre= entidad.Nombre;
				p.Apellido = entidad.Apellido;
				p.Dni = entidad.Dni;
               p.Email = entidad.Email;
				p.Telefono = entidad.Telefono;
                
				ru.Modification(p);
				TempData["Mensaje"] = "Datos guardados correctamente";
				return RedirectToAction(nameof(Index));
			
	
		}

		[HttpGet]
        public IActionResult Delet(int id){
try{
if(id > 0){

RepositorioPropietario ru= new RepositorioPropietario ();
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
		public IActionResult Delet(int id, Propietario entidad)
		{
			try
			{
				RepositorioPropietario ru= new RepositorioPropietario ();
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