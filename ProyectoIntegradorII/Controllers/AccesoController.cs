//LIBRERIAS DE TRABAJO
using Microsoft.Data.SqlClient; //ACCESO A LOS DATOS DE LA BD COACHBD
using Microsoft.AspNetCore.Session;
using System.Data;
using ProyectoIntegradorII.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoIntegradorII.Datos;
using ProyectoIntegradorII.Models.ModelosCustom;

namespace ProyectoIntegradorII.Controllers
{
    public class AccesoController : Controller
    {
        string sesion = "";

        string Ingreso(string nombre, string clave)
        {
            string ingreso = "";

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                cn.Open();
                try
                {
                    SqlCommand cm = new SqlCommand("USP_ACCESO_USUARIO", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@NOMBRE_USUARIO", nombre);
                    cm.Parameters.AddWithValue("@CONTRASENA", clave);
                    SqlDataReader dr = cm.ExecuteReader();
                    if (dr.Read()) 
                    {
                        ingreso = nombre;
                    }
                }
                catch (Exception)
                {
                    ingreso = "";
                }

                finally
                {
                    cn.Close();
                }
            }

            return ingreso;

        }

        public async Task<IActionResult> Login()
        {
            HttpContext.Session.SetString(sesion, ""); //Asigno el valor "" al session

            return View(await Task.Run(() => new Usuario())); //Se envia un nuevo usuario al Login para ingresar los datos

        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario reg)
        {
            if (!ModelState.IsValid) //Si no está validado (REQUIRED)
            {
                ModelState.AddModelError("", "Ingrese los datos"); //Se activa el error si no se ingresó los datos

                return View(await Task.Run(() => reg));

            }
            
            //Se ingresaron los datos
            string xusuario = Ingreso(reg.nombre_usuario, reg.contrasena);

            if (string.IsNullOrEmpty(xusuario))
            {
                ModelState.AddModelError("", "Usuario o Clave Incorrecta");

                return View(await Task.Run(() => reg));

            }

            //Si todo está ok, se manda a la vista logueado
            HttpContext.Session.SetString(sesion, xusuario);

            return RedirectToAction("Logueado", "Acceso");

        }

        IEnumerable<InfUsuario> usuarioinfo(string nombre_usuario)
        {
            List<InfUsuario> temporal = new List<InfUsuario>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("exec USP_OBTENER_INFO_USUARIO @NOMBRE_USUARIO", cn);
                    cmd.Parameters.AddWithValue("@NOMBRE_USUARIO", nombre_usuario);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        InfUsuario obj = new InfUsuario()
                        {
                            nombre_usuario = dr.GetString(0),
                            foto = dr.GetString(1),
                            nombresApellidos = dr.GetString(2),
                            tipousuario = dr.GetString(3),
                        };
                        temporal.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    cn.Close();
                }
            }
            return temporal;
        }

        public IActionResult Logueado()
        {
            // El contenido del Session (sesion) se almacena en un ViewBag
            ViewBag.usuario = HttpContext.Session.GetString(sesion);

            InfUsuario infU = new InfUsuario();
            infU.nombre_usuario = HttpContext.Session.GetString(sesion);
            var inf = usuarioinfo(infU.nombre_usuario).Where(c => c.nombre_usuario == infU.nombre_usuario).FirstOrDefault();

            ViewBag.nombre = inf.nombresApellidos;
            ViewBag.foto = inf.foto;
            ViewBag.tipo = inf.tipousuario;

            return View();

        }

        IEnumerable<ServicioInf> solicitudes(string nombre_usuario)
        {
            List<ServicioInf> temporal = new List<ServicioInf>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("exec USP_SOLICITUDES_COACH_2 @NOMBRE_USUARIO", cn);
                    cmd.Parameters.AddWithValue("@NOMBRE_USUARIO", nombre_usuario);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ServicioInf obj = new ServicioInf()
                        {
                            nombre_usuario = dr.GetString(0),
                            nombresApellidos = dr.GetString(1),
                            fechasesion = dr.GetDateTime(2),
                            tiposesion = dr.GetString(3),
                            tiposervicio = dr.GetString(4),
                            totalHoras = dr.GetInt32(5),
                            monto = dr.GetDecimal(6),
                        };
                        temporal.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    cn.Close();
                }
            }
            return temporal;
        }

        public IActionResult Sesiones()
        {
            ViewBag.usuario = HttpContext.Session.GetString(sesion);

            InfUsuario infU = new InfUsuario();
            infU.nombre_usuario = HttpContext.Session.GetString(sesion);
            var inf = usuarioinfo(infU.nombre_usuario).Where(c => c.nombre_usuario == infU.nombre_usuario).FirstOrDefault();

            ViewBag.nombre = inf.nombresApellidos;
            ViewBag.foto = inf.foto;
            ViewBag.tipo = inf.tipousuario;

            var usuariosesion = HttpContext.Session.GetString(sesion);
            IEnumerable<ServicioInf> temporal = solicitudes(usuariosesion);

            return View(temporal);
        }

        //FALTA
        public async Task<IActionResult> CerrarSesion()
        {

            return View();
        }
    }
}
