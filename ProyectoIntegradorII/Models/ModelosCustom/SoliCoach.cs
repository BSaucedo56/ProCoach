using System.ComponentModel.DataAnnotations;

namespace ProyectoIntegradorII.Models.ModelosCustom
{
    public class SoliCoach
    {
        [Required] public int idCoach { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un tipo de sesión")] public int tipoSesion { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un tipo de servicio")] public int tipoServicio { get; set; }
        [Required] public decimal precio { get; set; }
        [Required(ErrorMessage = "Debe seleccionar la cantidad de sesiones")] public int cantidadSesiones { get; set; }
        [Required(ErrorMessage = "Debe seleccionar la cantidad de horas")] public int cantidadHoras { get; set; }
        [Required(ErrorMessage = "El monto no debe estar vacio")] public decimal monto { get; set; }
        [Required(ErrorMessage = "El campo Nombres no debe estar vacio")][StringLength(50)] public string nombres { get; set; }
        [Required(ErrorMessage = "El campo Apellidos no debe estar vacio")][StringLength(50)] public string apellidos { get; set; }
        [StringLength(50)] public string? direccion { get; set; }
        [StringLength(11, ErrorMessage = "{0} la longitud debe estar entre {2} y {1}.", MinimumLength = 9)] public string? telefono { get; set; }
        [Required(ErrorMessage = "El campo Correo no debe estar vacio")][StringLength(50)] public string correo { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un tipo de documento")] public int tipoDocumento { get; set; }
        [Required(ErrorMessage = "El campo N° de Documento no debe estar vacio")][StringLength(15, ErrorMessage = "{0} la longitud debe estar entre {2} y {1}.", MinimumLength = 8)] public string numDocumento { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un pais")] public int pais { get; set; }
    }
}
