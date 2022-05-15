namespace ProyectoIntegradorII.Models
{
    public class Especialidad
    {
        public int idEspecialidad { get; set; }
        public string descripcion { get; set; }
    }

    public class EspecialidadCoach
    {
        public int idEspecialidad { get; set; }
        public int idCoach { get; set; }
    }
}
