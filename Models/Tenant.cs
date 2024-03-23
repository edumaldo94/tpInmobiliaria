using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tpInmobliaria.Models;

    public class Tenant
    {
        public int id_Tenant { get; set; }
   
         [DisplayName("Nombre")]
        public string? Name { get; set; }
         [Required]
         [DisplayName("Apellido")]
        public string? Last_Name { get; set; }
         [DisplayName("Documento")]
          [Required]
        public int? Dni { get; set; }
 [DisplayName("F.Nacimiento")] 
  [Required]
        public DateTime? Birthdate { get; set; }
  [DisplayName("Género")]
   [Required]
        public char? Sex { get; set; }
         [DisplayName("Direccion")]
          [Required]
        public string? Address { get; set; }
         [DisplayName("Telefono")]
          [Required]
        public string? Phone { get; set; }
         [DisplayName("Correo")]
          [Required]
        public string? Email { get; set; }
             [DisplayName("Estado")]
       public int? StateT { get; set; }
        
     
    }

