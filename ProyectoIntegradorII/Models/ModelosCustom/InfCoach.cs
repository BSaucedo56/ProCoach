using System.ComponentModel.DataAnnotations;

namespace ProyectoIntegradorII.Models.ModelosCustom
{
    public class InfCoach
    {
        public int idCoach { get; set; }

        public string coach { get; set; }

        public int idPais { get; set; }

        public string pais { get; set; }

        public int idIdioma { get; set; }

        public string idioma { get; set; }

        public string telefono { get; set; }

        public string correo { get; set; }

        public int idCertificacion { get; set; }

        [Display(Name = "CERTIFICACIÓN ICF:")]
        public string certificacionICF { get; set; }

        public int idMetodo { get; set; }

        [Display(Name = "MÉTODOS COACHING:")]
        public string metodoCoaching { get; set; }

        public int idEspecialidad { get; set; }

        [Display(Name = "ESPECIALIDAD:")]
        public string especialidad { get; set; }

        public int idExperiencia { get; set; }

        [Display(Name = "EXPERIENCIA:")]
        public string anioExperiencia { get; set; }



        
    }
}
