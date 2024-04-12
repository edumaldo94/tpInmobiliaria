using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tpInmobliaria.Models;

public class Pago
{
    public int? PagoId { get; set; }
    public int? ContratoId { get; set; }
    
    
       public Contrato Contrato { get; set; }
         [DisplayName("Numero de Pago")]
    public int? NumeroPago { get; set; }
     [DisplayName("Concepto")]
    public string? Concepto { get; set; }

    [DisplayName("Fecha de Pago")]
    public DateTime? FechaPago { get; set; }
  
    [DisplayName("Importe")]
    public double? Importe { get; set; }
    
    [DisplayName("Estado de Pago")]
    public string? EstadoPago { get; set; }

       public string? InquilinoNombre { get; set; }
           [DisplayName("Apellido Inquilino")]
    public string? InquilinoApellido { get; set; }

    public Inmueble? inmueble{get;set;}

}