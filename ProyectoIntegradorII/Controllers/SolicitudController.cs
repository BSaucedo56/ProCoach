using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient; //ACCESO A LOS DATOS DE LA BD COACHBD
using ProyectoIntegradorII.Datos;
using ProyectoIntegradorII.Models;
using ProyectoIntegradorII.Models.ModelosCustom;
using System.Data;

namespace ProyectoIntegradorII.Controllers
{
    public class SolicitudController : Controller
    {
        IEnumerable<Pais> paises()
        {
            List<Pais> temporal = new List<Pais>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_LISTAR_PAISES", cn); // Select a la tabla paises
                cn.Open(); //Abrir la conexión
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read()) //Lee cada uno de los registros
                {
                    Pais obj = new Pais()
                    {
                        idPais = dr.GetInt32(0),
                        pais = dr.GetString(1),
                    };
                    temporal.Add(obj); //crea cada elemento en temporal
                }
            }
            return temporal;
        }

        IEnumerable<TipoDocumento> tiposdocumentos()
        {
            List<TipoDocumento> temporal = new List<TipoDocumento>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_LISTAR_TIPO_DOCUMENTO", cn); // Select a la tabla tipodocumento
                cn.Open(); //Abrir la conexión
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read()) //Lee cada uno de los registros
                {
                    TipoDocumento obj = new TipoDocumento()
                    {
                        idDocumento = dr.GetInt32(0),
                        documento = dr.GetString(1),
                    };
                    temporal.Add(obj); //crea cada elemento en temporal
                }
            }
            return temporal;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
