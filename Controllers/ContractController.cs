using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tpInmobliaria.Models;

namespace tpInmobliaria.Controllers;

public class ContractController : Controller{

    private readonly ILogger<ContractController> _logger;

    public ContractController(ILogger<ContractController> logger){
        _logger = logger;
    }
    
    public IActionResult Index(){
    RepositorioContract ru= new RepositorioContract();
        var lista= ru.GetContracts();
        
        return View(lista);
    }
    
      

public IActionResult CreateContract(){

    return View();
}

} 