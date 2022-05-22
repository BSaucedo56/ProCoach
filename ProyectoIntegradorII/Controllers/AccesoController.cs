//LIBRERIAS DE TRABAJO
using Microsoft.Data.SqlClient; //ACCESO A LOS DATOS DE LA BD COACHBD
using Microsoft.AspNetCore.Session;
using System.Data;
using ProyectoIntegradorII.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoIntegradorII.Datos;

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

        public IActionResult Logueado()
        {
            // El contenido del Session (sesion) se almacena en un ViewBag
            ViewBag.usuario = HttpContext.Session.GetString(sesion);

            return View();

        }

        //FALTA
        public async Task<IActionResult> CerrarSesion()
        {

            return View();
        }
    }
}
