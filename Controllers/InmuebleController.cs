using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class InmuebleController : Controller{

    private readonly ILogger<InmuebleController> _logger;

    public InmuebleController(ILogger<InmuebleController> logger){
        _logger = logger;
    }
    public IActionResult Index(){
        RepositorioInmueble ru= new RepositorioInmueble();
        var lista= ru.GetProperties();
        
        return View(lista);
    }
    
      public IActionResult Create(){
        var repoProprietor = new RepositorioPropietario(); // Crear una instancia de RepositorioProprietor
    var proprietors = repoProprietor.GetProprietors();
              var propertyModel = new Inmueble();

    // Pasar la lista de propietarios al ViewBag o a través de un ViewModel
    ViewBag.Proprietors = proprietors;

    // Pasar la instancia de Property como modelo a la vista
    return View(propertyModel);
        }
        public IActionResult Crear(Inmueble proprietors){
            RepositorioInmueble ru= new RepositorioInmueble();
            ru.Create(proprietors);
            return RedirectToAction(nameof(Index));
        }

  public IActionResult Edit(int id)
        {
             RepositorioInmueble ru= new RepositorioInmueble();
            var inmueble  = ru.ObtenerPorId(id);

  if (inmueble == null)
    {
        return NotFound();
    }
 
RepositorioPropietario proprietors =new RepositorioPropietario();
          //  p.Propietario = rp.GetProprietorId(p.PropietarioId);
              ViewBag.Proprietors = proprietors .GetProprietors();
            return View(inmueble );
        }
[HttpPost]

public IActionResult Edit(int id, Inmueble inmueble)
{
   
   inmueble.id_Inmuebles= id;

        // Aquí debes guardar los cambios en el inmueble utilizando tu repositorio
   
        RepositorioInmueble rI= new RepositorioInmueble();
        rI.Modificacion(inmueble);
   
        return RedirectToAction(nameof(Index));
     
  
}

  public IActionResult Delet(int id){
try{
if(id > 0){

RepositorioInmueble ru= new RepositorioInmueble ();
var inmueble = ru.ObtenerPorId(id);
RepositorioPropietario rp= new RepositorioPropietario ();
var propietario= rp.GetProprietorId(inmueble.PropietarioId);
  inmueble.ProprietorName = propietario.Nombre;
return View(inmueble);
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
		public IActionResult Delet(int id, Inmueble inmueble)
		{
			try
			{
				RepositorioInmueble ru= new RepositorioInmueble ();
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