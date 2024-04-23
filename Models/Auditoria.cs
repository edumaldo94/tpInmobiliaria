using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tpInmobliaria.Models;

public class Auditoria
{
    [DisplayName("N°")]
    public int? IdAuditor { get; set; }
    public int? ContratoId	 { get; set; }
    public int? PagoId { get; set; }

    [DisplayName("Fecha")]
    public DateTime? FechaHora { get; set; }
    
    [DisplayName("Usuario")]
    public string? UsuarioNombre { get; set; }

     [DisplayName("Accion")]
 public string? Accion { get; set; } 
}