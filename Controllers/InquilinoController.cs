using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class InquilinoController : Controller{

    private readonly ILogger<InquilinoController> _logger;

    public InquilinoController(ILogger<InquilinoController> logger){
        _logger = logger;
    }
public IActionResult Index()
{
    RepositorioInquilino ru = new RepositorioInquilino();
    var lista = ru.GetTenants();

    return View(lista);
}


    
      public IActionResult Create()
        {
            return View();
        }
        public IActionResult Crear(Inquilino tenants){
            RepositorioInquilino ru= new RepositorioInquilino();
            ru.High(tenants);
            return RedirectToAction(nameof(Index));
        }

[HttpGet]
        public IActionResult Edit(int id){
if(id > 0){
RepositorioInquilino ru= new RepositorioInquilino ();
var tenants = ru.GetTenantId(id);
return View(tenants);
}else{
    
  return View();  
}
        }

	
		//public ActionResult Edit(int id, IFormCollection collection)
        [HttpPost]
	//[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Inquilino entidad)
		{
		
			// Si en lugar de IFormCollection ponemos Propietario, el enlace de datos lo hace el sistema
			Inquilino? p = null;
            RepositorioInquilino ru= new RepositorioInquilino ();
		 

				p = ru.GetTenantId(id);
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

RepositorioInquilino ru= new RepositorioInquilino ();
var tenants = ru.GetTenantId(id);
return View(tenants);
}else{
    
  return View();  
}
}catch(Exception ex){
		 TempData["ErrorMessage"] = "Error al intentar eliminar el propietario." + ex;
        return RedirectToAction(nameof(Index));
}

        }

		[HttpPost, ActionName("Delet")]
		//[ValidateAntiForgeryToken]
		public IActionResult Delet(int id, Inquilino entidad)
		{
			try
			{
				RepositorioInquilino ru= new RepositorioInquilino ();
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