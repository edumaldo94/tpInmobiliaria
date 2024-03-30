using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tpInmobliaria.Models;

public class Contract
{
    public int id_Contract { get; set; }
    public int id_Property { get; set; }
    public int id_Tenant { get; set; }

    [DisplayName("Fecha de inicio")]
    public DateTime? Start_date { get; set; }
    [DisplayName("Fecha de Finalizacion")]
    public DateTime? End_date { get; set; }

    [DisplayName("Monto del Alquiler")]
    public double? Rental_amount { get; set; }
    [DisplayName("Multa")]
    public double? Penalize { get; set; }
    [DisplayName("Estado")]
    public string? StateC { get; set; }

}