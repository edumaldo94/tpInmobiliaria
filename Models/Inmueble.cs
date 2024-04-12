using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tpInmobliaria.Models;

public class Inmueble
{
        public int id_Inmuebles { get; set; }
        public int PropietarioId { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string? Ubicacion { get; set; }

        [DisplayName("Direccion")]
        public string? Direccion { get; set; }
        public int? Ambientes { get; set; }
        [DisplayName("Uso")]
        public string? Uso { get; set; }

        [DisplayName("Tipo")]
        public string? Tipo { get; set; }


        [DisplayName("Precio")]
        [Required]
        public double? Precio { get; set; }
        public string? Disponible { get; set; }
        public int? EstadoIn { get; set; }
        [DisplayName("Nombre Propietario")]
        public string? ProprietorName { get; set; }
        [DisplayName("Apellido Propietario")]
        public string? ProprietorLastName { get; set; }

          public Propietario? Propietario;
}
