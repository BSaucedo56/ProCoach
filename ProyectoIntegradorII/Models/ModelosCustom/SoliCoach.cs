using System.ComponentModel.DataAnnotations;

namespace ProyectoIntegradorII.Models.ModelosCustom
{
    public class SoliCoach
    {
        public int idCoach { get; set; }
        public int tipoSesion { get; set; }
        public int tipoServicio { get; set; }
        public decimal precio { get; set; }
        public int cantidadSesiones { get; set; }
        public int cantidadHoras { get; set; }
        public decimal monto { get; set; }
        [Required(ErrorMessage = "El campo Nombres no debe estar vacio")][StringLength(50)] public string nombres { get; set; }
        [Required(ErrorMessage = "El campo Apellidos no debe estar vacio")][StringLength(50)] public string apellidos { get; set; }
        [Required(ErrorMessage = "El campo Dirección no debe estar vacio")][StringLength(50)] public string direccion { get; set; }
        [Required(ErrorMessage = "El campo Teléfono no debe estar vacio")][StringLength(9, ErrorMessage = "{0} la longitud debe estar entre {2} y {1}.", MinimumLength = 9)] public string telefono { get; set; }
        [Required(ErrorMessage = "El campo Correo no debe estar vacio")][StringLength(50)] public string correo { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un tipo de documento")] public int tipoDocumento { get; set; }
        [Required(ErrorMessage = "El campo N° de Documento no debe estar vacio")][StringLength(15, ErrorMessage = "{0} la longitud debe estar entre {2} y {1}.", MinimumLength = 8)] public string numDocumento { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un pais")] public int pais { get; set; }
    }
}
