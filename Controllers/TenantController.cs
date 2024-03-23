using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class TenantController : Controller{

    private readonly ILogger<TenantController> _logger;

    public TenantController(ILogger<TenantController> logger){
        _logger = logger;
    }
public IActionResult Index()
{
    RepositorioTenant ru = new RepositorioTenant();
    var lista = ru.GetTenants();

    return View(lista);
}


    
      public IActionResult Create()
        {
            return View();
        }
        public IActionResult Crear(Tenant tenants){
            RepositorioTenant ru= new RepositorioTenant();
            ru.High(tenants);
            return RedirectToAction(nameof(Index));
        }

[HttpGet]
        public IActionResult Edit(int id){
if(id > 0){
RepositorioTenant ru= new RepositorioTenant ();
var tenants = ru.GetTenantId(id);
return View(tenants);
}else{
    
  return View();  
}
        }

	
		//public ActionResult Edit(int id, IFormCollection collection)
        [HttpPost]
	//[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Tenant entidad)
		{
		
			// Si en lugar de IFormCollection ponemos Propietario, el enlace de datos lo hace el sistema
			Tenant? p = null;
            RepositorioTenant ru= new RepositorioTenant ();
		 

				p = ru.GetTenantId(id);
				p.Name= entidad.Name;
				p.Last_Name = entidad.Last_Name;
				p.Dni = entidad.Dni;
                p.Birthdate=entidad.Birthdate;
                p.Sex=p.Sex;
                p.Address=entidad.Address;
				p.Phone = entidad.Phone;
                p.Email = entidad.Email;
				ru.Modification(p);
				TempData["Mensaje"] = "Datos guardados correctamente";
				return RedirectToAction(nameof(Index));
			
	
		}

		[HttpGet]
        public IActionResult Delet(int id){
try{
if(id > 0){

RepositorioTenant ru= new RepositorioTenant ();
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
		public IActionResult Delet(int id, Tenant entidad)
		{
			try
			{
				RepositorioTenant ru= new RepositorioTenant ();
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