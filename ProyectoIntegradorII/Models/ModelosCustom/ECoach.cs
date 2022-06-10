using System.ComponentModel.DataAnnotations;

namespace ProyectoIntegradorII.Models.ModelosCustom
{
    public class ECoach
    {
        public string urlAvatar { get; set; }
        public int idCoach { get; set; }

        [Display(Name = "Coach")]
        public string coach { get; set; }

        public int idEspecialidad { get; set; }

        [Display(Name = "Especialidad")]
        public string especialidad { get; set; }

        public int idCertificacion { get; set; }

        [Display(Name = "CertificacionICF")]
        public string certificacionICF { get; set; }

        public int idMetodo { get; set; }

        [Display(Name = "MetodoCoaching")]
        public string metodoCoaching { get; set; }

        public int idIdioma { get; set; }

        [Display(Name = "Idioma")]
        public string idioma { get; set; }

        public int idPais { get; set; }

        [Display(Name = "Pais")]
        public string pais { get; set; }

        public string telefono { get; set; }

        public string correo { get; set; }

        public int idExperiencia { get; set; }

        public string anioExperiencia { get; set; }

        public int precio { get; set; }
    }
}
