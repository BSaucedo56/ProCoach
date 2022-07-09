namespace ProyectoIntegradorII.Models.ModelosCustom
{
    public class ServicioInf
    {
        public int id_servicio { get; set; }
        public string nombre_usuario { get; set; }
        public string nombresApellidos { get; set; }
        public string tiposervicio { get; set; }
        public decimal precio { get; set; }
        public int cantsesiones { get; set; }
        public int canthoras { get; set; }
        public string tiposesion { get; set; }
        public string correo { get; set; }
        public string checkint { get; set; }
        public int idNivelSatisfacion { get; set; }
        public string nivelSatisfacion { get; set; }
        public string color { get; set; }
    }
}
