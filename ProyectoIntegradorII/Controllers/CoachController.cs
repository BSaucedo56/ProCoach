//LIBRERIAS DE TRABAJO
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient; //ACCESO A LOS DATOS DE LA BD COACHBD
using ProyectoIntegradorII.Models;

namespace ProyectoIntegradorII.Controllers
{
    public class CoachController : Controller
    {
        //CADENA DE CONEXIÓN 
        const string cadena = @"server=BRYAN; database=COACHDB; Trusted_Connection=True; " + "MultipleActiveResultSets=True; TrustServerCertificate=False; Encrypt=False";

        IEnumerable<Especialidad> especialidades()
        {
            List<Especialidad> temporal = new List<Especialidad>();

            using (SqlConnection cn = new SqlConnection(cadena)) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_ESPECIALIDAD", cn);
                cn.Open(); 
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read())
                {
                    Especialidad obj = new Especialidad()
                    {
                        idEspecialidad = dr.GetInt32(0),
                        descripcion = dr.GetString(1),
                    };
                    temporal.Add(obj);
                }
            }
            return temporal;
        }

        IEnumerable<CertificacionICF> certificacionesICF()
        {
            List<CertificacionICF> temporal = new List<CertificacionICF>();

            using (SqlConnection cn = new SqlConnection(cadena)) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_CERTIFICACIONICF", cn);
                cn.Open(); //ACTIVA LA CONEXIÓN
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read())
                {
                    CertificacionICF obj = new CertificacionICF()
                    {
                        idCertificacion = dr.GetInt32(0),
                        certificacion = dr.GetString(1),
                    };
                    temporal.Add(obj);
                }
            }
            return temporal;
        }

        IEnumerable<Metodo> metodos()
        {
            List<Metodo> temporal = new List<Metodo>();

            using (SqlConnection cn = new SqlConnection(cadena)) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_METODOSCOACHING", cn);
                cn.Open(); //ACTIVA LA CONEXIÓN
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read())
                {
                    Metodo obj = new Metodo()
                    {
                        idMetodo = dr.GetInt32(0),
                        nombreMetodo = dr.GetString(1),
                    };
                    temporal.Add(obj);
                }
            }
            return temporal;
        }

        IEnumerable<Idioma> idiomas()
        {
            List<Idioma> temporal = new List<Idioma>();

            using (SqlConnection cn = new SqlConnection(cadena)) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_IDIOMA", cn);
                cn.Open(); //ACTIVA LA CONEXIÓN
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read()) // MIENTRAS SE LEA LAS FILAS
                {
                    Idioma obj = new Idioma()
                    {
                        idIdioma = dr.GetInt32(0),
                        idioma = dr.GetString(1),
                    };
                    temporal.Add(obj);
                }
            }
            return temporal;
        }

        IEnumerable<ECoach> coaches(string e = "", string co = "", string m = "", string i = "")
        {
            List<ECoach> temporal = new List<ECoach>();
            if (e == "" && co == "" && m == "" && i == "") return temporal;

            using (SqlConnection cn = new SqlConnection(cadena)) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_ENCONTRAR_COACH2 @ESPECIALIDAD,@CERTIFICACIONICF,@METODOCOACHING,@IDIOMA", cn);
                cmd.Parameters.AddWithValue("@ESPECIALIDAD", e);
                cmd.Parameters.AddWithValue("@CERTIFICACIONICF", co);
                cmd.Parameters.AddWithValue("@METODOCOACHING", m);
                cmd.Parameters.AddWithValue("@IDIOMA", i);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ECoach obj = new ECoach()
                    {
                        coach = dr.GetString(0),
                        idEspecialidad = dr.GetInt32(1),
                        especialidad = dr.GetString(2),
                        idCertificacion = dr.GetInt32(3),
                        certificacionICF = dr.GetString(4),
                        idMetodo = dr.GetInt32(5),
                        metodoCoaching = dr.GetString(6),
                        idIdioma = dr.GetInt32(7),
                        idioma = dr.GetString(8),
                        pais = dr.GetString(9),
                    };
                    temporal.Add(obj);
                }
            }
            return temporal;
        }

        public async Task<IActionResult> Consultas(string e = "", string co = "", string m = "", string i = "")
        {
            IEnumerable<ECoach> temporal = coaches(e, co, m, i);

            ViewBag.especialidades = new SelectList(especialidades(), "idEspecialidad", "descripcion");
            ViewBag.certificaciones = new SelectList(certificacionesICF(), "idCertificacion", "certificacion");
            ViewBag.metodos = new SelectList(metodos(), "idMetodo", "nombreMetodo");
            ViewBag.idiomas = new SelectList(idiomas(), "idIdioma", "idioma");

            return View(await Task.Run(() => temporal));
        }

        //[HttpPost]
        //public IActionResult GetCoach(string e = "", string co = "", string m = "", string i = "")
        //{
        //    return View();
        //}
        
    

        //public IActionResult Index()
        //    {
        //        return View();
        //    }
    }
}
