namespace tpInmobliaria.Models;

public class Usuario{

    	public enum enRoles
	{
		Administrador = 1,
		Empleado = 2,
	}


    public int? UsuarioId { get; set; }
    public string? Nombre{ get; set; }    
    public string? Apellido { get; set; }
    public string? Password { get; set; }
    public string? Correo { get; set; }

    public int Rol { get; set; }

    public string? Avatar{get;set;}

    public string? PasswordAnterior{get;set;}

    public IFormFile? ImgAvatar{get;set;}
    public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

   public static IDictionary<int, string> ObtenerRoles()
		{
			SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
			Type tipoEnumRol = typeof(enRoles);
			foreach (var valor in Enum.GetValues(tipoEnumRol))
			{
				roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
			}
			return roles;
		}
}