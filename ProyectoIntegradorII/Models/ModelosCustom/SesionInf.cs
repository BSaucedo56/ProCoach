namespace ProyectoIntegradorII.Models.ModelosCustom
{
    public class SesionInf
    {
        public int id_sesion { get; set; }
        public int id_servicio { get; set; }
        public string nombre_usuario { get; set; }
        public string nombresApellidos { get; set; }
        public DateTime fechasesion { get; set; }
        public decimal precio { get; set; }
        public string correo { get; set; }
        public string checkint { get; set; }
        public string comentario { get; set; }
    }
}
