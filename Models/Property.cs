using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tpInmobliaria.Models;

    public class Property
    {
        public int id_Property { get; set; }
   public int id_Proprietor { get; set; }

         [DisplayName("Direccion")]
 public string? Address { get; set; }

         [DisplayName("Tipo")]
        public string? Type { get; set; }
         [DisplayName("Uso")]
        public string? Use_type { get; set; }
         [DisplayName("Coordenadas")]
          [Required]
        public double? Coordinates { get; set; }

         [DisplayName("Precio")]
          [Required]
        public double? Price { get; set; }

       public string? StateP { get; set; }
        
    }
