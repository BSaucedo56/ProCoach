//LIBRERIAS DE TRABAJO
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProyectoIntegradorII.Models
{
    [Table("Usuario",Schema = "dbo")]
    public class Usuario
    {
        [Key] public int idUsuario { get; set; }
        [Required(ErrorMessage = "El campo Username no debe estar vacio"), Display(Name = "Usuario")]  public string nombre_usuario { get; set; }
        [Required(ErrorMessage = "El campo Password no debe estar vacio"), Display(Name = "Contraseña")] [DataType(DataType.Password)] public string contrasena { get; set; }
        public int id_tipousuario { get; set; }
    }
}
