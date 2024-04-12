using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tpInmobliaria.Models;

public class Contrato
{
    [DisplayName("N°")]
    public int id_Contrato { get; set; }
    public int InmuebleId { get; set; }
    public int InquilinoId { get; set; }

    [DisplayName("Fecha de inicio")]
    public DateTime? Fecha_Inicio { get; set; }
    [DisplayName("Fecha de Finalizacion")]
    public DateTime? Fecha_Fin { get; set; }

    [DisplayName("Monto del Alquiler")]
    public double? Monto { get; set; }
    
    [DisplayName("Estado")]
    public string? Estado { get; set; }
 public int? EstadoC { get; set; }

    public Inmueble? Inmueble { get; set; }
    public Inquilino? Inquilino { get; set; }
}