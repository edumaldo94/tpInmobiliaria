using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace tpInmobliaria.Models;

public class Contrato
{
   [DisplayName("N°")]

   [Key]
   public int id_Contrato { get; set; }

   [ForeignKey(nameof(InmuebleId))]
   public int InmuebleId { get; set; }
   [ForeignKey(nameof(InquilinoId))]
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
   // [NotMapped]
   public Inmueble? Inmueble { get; set; }
   // [NotMapped]
   public Inquilino? Inquilino { get; set; }
   // [NotMapped]
   public List<Pago>? Pagos { get; set; }

}