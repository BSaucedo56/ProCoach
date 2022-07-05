//LIBRERIAS DE TRABAJO
using Microsoft.Data.SqlClient; //ACCESO A LOS DATOS DE LA BD COACHBD
using Microsoft.AspNetCore.Session;
using System.Data;
using ProyectoIntegradorII.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoIntegradorII.Datos;
using ProyectoIntegradorII.Models.ModelosCustom;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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

        IEnumerable<Usuario> ListaUsuario()
        {
            List<Usuario> temporal = new List<Usuario>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                SqlCommand cmd = new SqlCommand("exec usp_valida_usuario", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Usuario obj = new Usuario()
                    {
                        idUsuario = dr.GetInt32(0),
                        nombre_usuario = dr.GetString(1),
                        contrasena = dr.GetString(2),
                        id_tipousuario = dr.GetInt32(3),
                    };
                    temporal.Add(obj);
                }
            }
            return temporal;
        }

        Usuario ValidarUsuario(string usuario, string clave)
        {
            return ListaUsuario().Where(u => u.nombre_usuario == usuario && u.contrasena == clave).FirstOrDefault();
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
            var _usuario = ValidarUsuario(reg.nombre_usuario, reg.contrasena);

            if (_usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, _usuario.nombre_usuario),
                    new Claim("nombre_usuario", _usuario.nombre_usuario)
                };

                if (_usuario.id_tipousuario == 1)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Cliente"));
                }

                if (_usuario.id_tipousuario == 2)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Coach"));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                HttpContext.Session.SetString(sesion, xusuario);
            }


            if (string.IsNullOrEmpty(xusuario))
            {
                ModelState.AddModelError("", "Usuario o Clave Incorrecta");

                return View(await Task.Run(() => reg));

            }

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

        IEnumerable<ServicioInf> servicios(string nombre_usuario)
        {
            List<ServicioInf> temporal = new List<ServicioInf>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("exec USP_MOSTRAR_SERVICIOS @NOMBRE_USUARIO", cn);
                    cmd.Parameters.AddWithValue("@NOMBRE_USUARIO", nombre_usuario);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ServicioInf obj = new ServicioInf()
                        {
                            id_servicio = dr.GetInt32(0),
                            nombre_usuario = dr.GetString(1),
                            nombresApellidos = dr.GetString(2),
                            tiposervicio = dr.GetString(3),
                            precio = dr.GetDecimal(4),
                            cantsesiones = dr.GetInt32(5),
                            canthoras = dr.GetInt32(6),
                            tiposesion = dr.GetString(7),
                            correo = dr.GetString(8),
                            checkint = dr.GetString(9),
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

        public IActionResult Servicios(int p = 1)
        {
            var sesionUsuario = HttpContext.Session.GetString(sesion);
            ViewBag.usuario = sesionUsuario;

            InfUsuario infU = new InfUsuario();
            infU.nombre_usuario = sesionUsuario;
            var inf = usuarioinfo(infU.nombre_usuario).Where(c => c.nombre_usuario == infU.nombre_usuario).FirstOrDefault();

            if (inf != null)
            {
                ViewBag.nombre = inf.nombresApellidos;
                ViewBag.foto = inf.foto;
                ViewBag.tipo = inf.tipousuario;
            }
            else
            {
                return RedirectToAction("Login", "Acceso");
            }
                
            var usuariosesion = sesionUsuario;
            IEnumerable<ServicioInf> temporal = servicios(usuariosesion);
            int f = 5;
            int c = temporal.Count();

            int npags = c % f == 0 ? c / f : c / f + 1;

            ViewBag.p = p;
            ViewBag.npags = npags;
            ViewBag.etiqueta = string.Concat((p + 1), " de ", npags);

            return View(temporal.Skip(p * f).Take(f));
        }

        IEnumerable<SesionInf> sesiones(string nombre_usuario)
        {
            List<SesionInf> temporal = new List<SesionInf>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("exec USP_MOSTRAR_SESIONES @NOMBRE_USUARIO", cn);
                    cmd.Parameters.AddWithValue("@NOMBRE_USUARIO", nombre_usuario);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        SesionInf obj = new SesionInf()
                        {
                            id_servicio = dr.GetInt32(0),
                            nombre_usuario = dr.GetString(1),
                            nombresApellidos = dr.GetString(2),
                            fechasesion = dr.GetDateTime(3),
                            precio = dr.GetDecimal(4),
                            correo = dr.GetString(5),
                            checkint = dr.GetString(6),
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

        public IActionResult Sesiones(int p = 1)
        {
            var sesionUsuario = HttpContext.Session.GetString(sesion);
            ViewBag.usuario = sesionUsuario;

            InfUsuario infU = new InfUsuario();
            infU.nombre_usuario = sesionUsuario;
            var inf = usuarioinfo(infU.nombre_usuario).Where(c => c.nombre_usuario == infU.nombre_usuario).FirstOrDefault();

            if (inf != null)
            {
                ViewBag.nombre = inf.nombresApellidos;
                ViewBag.foto = inf.foto;
                ViewBag.tipo = inf.tipousuario;
            }
            else
            {
                return RedirectToAction("Login", "Acceso");
            }
            
            var usuariosesion = sesionUsuario;
            IEnumerable<SesionInf> temporal = sesiones(usuariosesion);

            int f = 5;
            int c = temporal.Count();

            int npags = c % f == 0 ? c / f : c / f + 1;

            ViewBag.p = p;
            ViewBag.npags = npags;
            ViewBag.etiqueta = string.Concat((p + 1), " de ", npags);

            return View(temporal.Skip(p * f).Take(f));
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Acceso");
        }

        public IActionResult ServicioAceptar(int idservicio)
        {
            var aceptarSesion = AceptarServicio(idservicio);

            return RedirectToAction("Servicios", new { p = 0 });
        }

        public string AceptarServicio(int idservicio)
        {
            string mensaje = "";
            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_ACEPTAR_SERVICIO_PRUEBA", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_SERVICIO", idservicio);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Servicio Actualizado";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        public IActionResult ServicioRechazar(int idservicio)
        {
            var aceptarSesion = RechazarServicio(idservicio);

            return RedirectToAction("Servicios", new { p = 0 });
        }

        public string RechazarServicio(int idservicio)
        {
            string mensaje = "";
            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_RECHAZAR_SERVICIO", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_SERVICIO", idservicio);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Servicio Actualizado";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        public IActionResult SesionAceptar(int idservicio)
        {
            var aceptarSesion = AceptarSesion(idservicio);

            return RedirectToAction("Sesiones", new { p = 0 });
        }

        public string AceptarSesion(int idservicio)
        {
            string mensaje = "";
            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_ACEPTAR_SERVICIO_SESION", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_SERVICIO", idservicio);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Sesion Actualizado";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        public IActionResult SesionRechazar(int idservicio)
        {
            var aceptarSesion = AceptarRechazar(idservicio);

            return RedirectToAction("Sesiones", new { p = 0 });
        }

        public string AceptarRechazar(int idservicio)
        {
            string mensaje = "";
            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_RECHAZAR_SESION", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_SERVICIO", idservicio);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Sesion Actualizado";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }
    }
}
