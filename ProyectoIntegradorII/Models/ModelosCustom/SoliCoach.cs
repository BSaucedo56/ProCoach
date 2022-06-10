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
        public decimal monto { get { return precio * cantidadHoras; } }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public int tipoDocumento { get; set; }
        public string numDocumento { get; set; }
        public int pais { get; set; }
    }
}
