namespace ProyectoIntegradorII.Models
{
    public class Metodo
    {
        public int idMetodo { get; set; }
        public string nombreMetodo { get; set; }

    }

    public class MetodoCoach
    {
        public int idMetodo { get; set; }
        public int idCoach { get; set; }
    }
}
