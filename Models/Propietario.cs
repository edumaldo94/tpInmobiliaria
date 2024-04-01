using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tpInmobliaria.Models;

    public class Propietario
    {
        public int id_Propietario { get; set; }
   
        [DisplayName("Nombre")]
        public string? Nombre { get; set; }


        [DisplayName("Apellido")]
        public string? Apellido { get; set; }


        [DisplayName("Nombre")]
        public string? Dni { get; set; }



       [DisplayName("Email")]
        public string? Email { get; set; }


       [DisplayName("Telefono")]
        public string? Telefono { get; set; }
        
       [DisplayName("Estado")]
        public int? EstadoP { get; set; }
     
    }

