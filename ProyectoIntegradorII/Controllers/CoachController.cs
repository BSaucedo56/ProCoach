//LIBRERIAS DE TRABAJO
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient; //ACCESO A LOS DATOS DE LA BD COACHBD
using ProyectoIntegradorII.Datos;
using ProyectoIntegradorII.Models;

namespace ProyectoIntegradorII.Controllers
{
    public class CoachController : Controller
    {
        IEnumerable<Especialidad> especialidades()
        {
            List<Especialidad> temporal = new List<Especialidad>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_ESPECIALIDAD", cn); // Select a la tabla especialidad
                cn.Open(); //Abrir la conexión
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read()) //Lee cada uno de los registros
                {
                    Especialidad obj = new Especialidad()
                    {
                        idEspecialidad = dr.GetInt32(0),
                        descripcion = dr.GetString(1),
                    };
                    temporal.Add(obj); //crea cada elemento en temporal
                }
            }
            return temporal;
        }

        IEnumerable<CertificacionICF> certificacionesICF()
        {
            List<CertificacionICF> temporal = new List<CertificacionICF>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_CERTIFICACIONICF", cn); // Select a la tabla certificacionicf
                cn.Open(); //ACTIVA LA CONEXIÓN
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read()) //Lee cada uno de los registros
                {
                    CertificacionICF obj = new CertificacionICF()
                    {
                        idCertificacion = dr.GetInt32(0),
                        certificacion = dr.GetString(1),
                    };
                    temporal.Add(obj); //crea cada elemento en temporal
                }
            }
            return temporal;
        }

        IEnumerable<Metodo> metodos()
        {
            List<Metodo> temporal = new List<Metodo>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_METODOSCOACHING", cn); // Select a la tabla metodo
                cn.Open(); //ACTIVA LA CONEXIÓN
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read()) //Lee cada uno de los registros
                {
                    Metodo obj = new Metodo()
                    {
                        idMetodo = dr.GetInt32(0),
                        nombreMetodo = dr.GetString(1),
                    };
                    temporal.Add(obj); //crea cada elemento en temporal
                }
            }
            return temporal;
        }

        IEnumerable<Idioma> idiomas()
        {
            List<Idioma> temporal = new List<Idioma>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_IDIOMA", cn); // Select a la tabla idioma
                cn.Open(); //ACTIVA LA CONEXIÓN
                SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                while (dr.Read()) // MIENTRAS SE LEA LAS FILAS
                {
                    Idioma obj = new Idioma()
                    {
                        idIdioma = dr.GetInt32(0),
                        idioma = dr.GetString(1),
                    };
                    temporal.Add(obj); //crea cada elemento en temporal
                }
            }
            return temporal;
        }

        IEnumerable<ECoach> coaches(string e, string co, string m, string i)
        {
            if (e == null)
            {
                e = "";
            }
            if (co == null)
            {
                co = "";
            }
            if (m == null)
            {
                m = "";
            }
            if (i == null)
            {
                i = "";
            }

            List<ECoach> temporal = new List<ECoach>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_ENCONTRAR_COACH @ESPECIALIDAD,@CERTIFICACIONICF,@METODOCOACHING,@IDIOMA", cn);
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
                        idCoach = dr.GetInt32(0),
                        coach = dr.GetString(1),
                        idEspecialidad = dr.GetInt32(2),
                        especialidad = dr.GetString(3),
                        idCertificacion = dr.GetInt32(4),
                        certificacionICF = dr.GetString(5),
                        idMetodo = dr.GetInt32(6),
                        metodoCoaching = dr.GetString(7),
                        idIdioma = dr.GetInt32(8),
                        idioma = dr.GetString(9),
                        pais = dr.GetString(10),
                    };
                    temporal.Add(obj);
                }
            }
            return temporal;
        }

        public IActionResult EncontrarCoach()
        {
            try
            {
                ViewBag.e = new SelectList(especialidades(), "idEspecialidad", "descripcion");
                ViewBag.co = new SelectList(certificacionesICF(), "idCertificacion", "certificacion");
                ViewBag.m = new SelectList(metodos(), "idMetodo", "nombreMetodo");
                ViewBag.i = new SelectList(idiomas(), "idIdioma", "idioma");
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.Message;
            }

            return View();
        }
        public IActionResult EncontrarCoaches(string e, string co, string m, string i, int pag = 1)
        {
            ViewBag.e = new SelectList(especialidades(), "idEspecialidad", "descripcion");
            ViewBag.co = new SelectList(certificacionesICF(), "idCertificacion", "certificacion");
            ViewBag.m = new SelectList(metodos(), "idMetodo", "nombreMetodo");
            ViewBag.i = new SelectList(idiomas(), "idIdioma", "idioma");

            try
            {
                IEnumerable<ECoach> temporal = coaches(e, co, m, i);

                if (pag < 1)
                {
                    pag = 1;
                }

                const int pageSize = 5;

                int recsCount = temporal.Count();

                var pager = new Pager(recsCount, pag, pageSize);

                int recSkip = (pag - 1) * pageSize;

                int esp = Convert.ToInt32(e);
                int coc = Convert.ToInt32(co);
                int met = Convert.ToInt32(m);
                int idi = Convert.ToInt32(i);

                ViewBag.esp = esp;
                ViewBag.coc = coc;
                ViewBag.met = met;
                ViewBag.idi = idi;

                this.ViewBag.Pager = pager;

                TempData["PartialCoach"] = temporal.Skip(recSkip).Take(pager.PageSize);
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.Message;
            }

            return View("EncontrarCoach");
        }

        //INFO COACH

        IEnumerable<ECoach> coachinfo()
        {

            List<ECoach> temporal = new List<ECoach>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                SqlCommand cmd = new SqlCommand("exec USP_LISTAR_INFCOACHES", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ECoach obj = new ECoach()
                    {
                        urlAvatar = dr.GetString(0),
                        idCoach = dr.GetInt32(1),
                        coach = dr.GetString(2),
                        idPais = dr.GetInt32(3),
                        pais = dr.GetString(4),
                        idIdioma = dr.GetInt32(5),
                        idioma = dr.GetString(6),
                        telefono = dr.GetString(7),
                        correo = dr.GetString(8),
                        idCertificacion = dr.GetInt32(9),
                        certificacionICF = dr.GetString(10),
                        idMetodo = dr.GetInt32(11),
                        metodoCoaching = dr.GetString(12),
                        idEspecialidad = dr.GetInt32(13),
                        especialidad = dr.GetString(14),
                        idExperiencia = dr.GetInt32(15),
                        anioExperiencia = dr.GetString(16),
                        precio = dr.GetInt32(17),
                    };
                    temporal.Add(obj);
                }
            }
            return temporal;
        }

        public IActionResult InfoCoach(int id)
        {
            var inf = coachinfo().Where(c => c.idCoach == id).FirstOrDefault();
            return PartialView("_PartialCoachInfo", inf);
        }

        public IActionResult SoliCoach(int id)
        {
            var inf = coachinfo().Where(c => c.idCoach == id).FirstOrDefault();
            return PartialView("_PartialCoachSolicitar", inf);
        }
    }
}
