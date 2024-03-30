using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class PropertyController : Controller{

    private readonly ILogger<PropertyController> _logger;

    public PropertyController(ILogger<PropertyController> logger){
        _logger = logger;
    }
    public IActionResult Index(){
        RepositorioProperty ru= new RepositorioProperty();
        var lista= ru.GetProperties();
        
        return View(lista);
    }
    
      public IActionResult Create(){
        var repoProprietor = new RepositorioProprietor(); // Crear una instancia de RepositorioProprietor
    var proprietors = repoProprietor.GetProprietors();
              var propertyModel = new Property();

    // Pasar la lista de propietarios al ViewBag o a través de un ViewModel
    ViewBag.Proprietors = proprietors;

    // Pasar la instancia de Property como modelo a la vista
    return View(propertyModel);
        }
        public IActionResult Crear(Property proprietors){
            RepositorioProperty ru= new RepositorioProperty();
            ru.Create(proprietors);
            return RedirectToAction(nameof(Index));
        }

public IActionResult CreateContract(){

    return View();
}

} 