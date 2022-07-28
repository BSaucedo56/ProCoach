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
using Microsoft.AspNetCore.Mvc.Rendering;

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
                            idNivelSatisfacion = dr.GetInt32(10),
                            nivelSatisfacion = dr.GetString(11),
                            color = dr.GetString(12),
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

        IEnumerable<NivelSatisfacion> listaNivelSatisfacion()
        {
            List<NivelSatisfacion> temporal = new List<NivelSatisfacion>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                SqlCommand cmd = new SqlCommand("exec USP_LISTADO_NIVELSATISFACION", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    NivelSatisfacion obj = new NivelSatisfacion()
                    {
                        idNivelSatisfacion = dr.GetInt32(0),
                        nivelSatisfacion = dr.GetString(1),
                    };
                    temporal.Add(obj);
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

            ViewBag.contar = temporal.Count();

            ServicioInf nuevo = new ServicioInf();
            nuevo.nombre_usuario = inf.nombre_usuario;
            var infser = servicios(nuevo.nombre_usuario).Where(c => c.nombre_usuario == nuevo.nombre_usuario).FirstOrDefault();

            if (infser != null)
            {
                ViewBag.idservicio = infser.id_servicio;
                ViewBag.idNivelSatisfacion = new SelectList(listaNivelSatisfacion(), "idNivelSatisfacion", "nivelSatisfacion", infser.idNivelSatisfacion);
                ViewBag.niveldesatisfacion = infser.idNivelSatisfacion;
            }

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
                            id_sesion = dr.GetInt32(0),
                            id_servicio = dr.GetInt32(1),
                            nombre_usuario = dr.GetString(2),
                            nombresApellidos = dr.GetString(3),
                            fechasesion = dr.GetDateTime(4),
                            precio = dr.GetDecimal(5),
                            correo = dr.GetString(6),
                            checkint = dr.GetString(7),
                            comentario = dr.GetString(8),
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

            ViewBag.contar = temporal.Count();

            return View(temporal.Skip(p * f).Take(f));
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Acceso");
        }

        public IActionResult ActualizarServicios(int idservicio, string coachcheckint)
        {
            var aceptarServicio = ActualizarServicio(idservicio, coachcheckint);

            return RedirectToAction("Servicios", new { p = 0 });
        }

        public string ActualizarServicio(int idservicio, string coachcheckint)
        {
            string mensaje = "";
            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_ACTUALIZAR_SERVICIO", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_SERVICIO", idservicio);
                    cmd.Parameters.AddWithValue("@COACHCHECKIN", coachcheckint);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Servicio Actualizado";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        public IActionResult ActualizarSesiones(int idsesion, int idservicio, string coachcheckint)
        {
            var aceptarSesion = ActualizarSesion(idsesion, idservicio, coachcheckint);

            return RedirectToAction("Sesiones", new { p = 0 });
        }

        public string ActualizarSesion(int idsesion, int idservicio, string coachcheckint)
        {
            string mensaje = "";
            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_ACTUALIZAR_SESION", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_SESION", idsesion);
                    cmd.Parameters.AddWithValue("@ID_SERVICIO", idservicio);
                    cmd.Parameters.AddWithValue("@COACHCHECKIN", coachcheckint);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Sesion Actualizado";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        [HttpPost]
        public IActionResult CalificarCoach(int id_servicio, int idNivelSatisfacion)
        {

            var calificar = CalificarServicio(id_servicio, idNivelSatisfacion);

            return RedirectToAction("Servicios", new { p = 0 });
        }

        public string CalificarServicio(int id_servicio, int idNivelSatisfacion)
        {
            string mensaje = "";
            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_CALIFICAR_SERVICIO_PRUEBA", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_SERVICIO", id_servicio);
                    cmd.Parameters.AddWithValue("@ID_NIVELSATISFACION", idNivelSatisfacion);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Servicio Actualizado";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            return mensaje;
        }

        IEnumerable<SesionInf> listarsesiones()
        {
            List<SesionInf> temporal = new List<SesionInf>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("exec USP_LISTAR_SESIONES", cn);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        SesionInf obj = new SesionInf()
                        {
                            id_sesion = dr.GetInt32(0),
                            id_servicio = dr.GetInt32(1),
                            nombre_usuario = dr.GetString(2),
                            nombresApellidos = dr.GetString(3),
                            fechasesion = dr.GetDateTime(4),
                            precio = dr.GetDecimal(5),
                            correo = dr.GetString(6),
                            checkint = dr.GetString(7),
                            comentario = dr.GetString(8),
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

        IEnumerable<SesionInf> listarsesionesCoach()
        {
            List<SesionInf> temporal = new List<SesionInf>();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("exec USP_LISTAR_SESIONES_COACH", cn);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        SesionInf obj = new SesionInf()
                        {
                            id_sesion = dr.GetInt32(0),
                            id_servicio = dr.GetInt32(1),
                            nombre_usuario = dr.GetString(2),
                            nombresApellidos = dr.GetString(3),
                            fechasesion = dr.GetDateTime(4),
                            precio = dr.GetDecimal(5),
                            correo = dr.GetString(6),
                            checkint = dr.GetString(7),
                            comentario = dr.GetString(8),
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

        SesionInf Buscar(int id)
        {
            return listarsesiones().Where(c => c.id_sesion == id).FirstOrDefault();
        }

        SesionInf BuscarSesionCoach(int id)
        {
            return listarsesionesCoach().Where(c => c.id_sesion == id).FirstOrDefault();
        }

        public IActionResult CoachVerComentario(int id)
        {
            SesionInf sesi = BuscarSesionCoach(id);
            ViewBag.comentario = sesi.comentario;

            return PartialView("_PartialCoachVerComentario");
        }

        public IActionResult VerDetalle(int id)
        {
            SesionInf sesi = Buscar(id);

            ViewBag.coach = sesi.nombresApellidos;
            ViewBag.fecha = sesi.fechasesion;
            ViewBag.precio = sesi.precio;
            ViewBag.estado = sesi.checkint;
            ViewBag.idsesion = sesi.id_sesion;
            ViewBag.idservicio = sesi.id_servicio;
            ViewBag.comentario = sesi.comentario;

            return PartialView("_PartialVerDetalleSesion");
        }

        public IActionResult ActualizarComentarios(SesionInf reg)
        {
            var aceptarSesion = ActualizarComentario(reg);

            return RedirectToAction("Sesiones", new { p = 0 });
        }

        public string ActualizarComentario(SesionInf reg)
        {
            string mensaje = "";
            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_ACTUALIZAR_COMENTARIO_SESION", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_SESION", reg.id_sesion);
                    cmd.Parameters.AddWithValue("@ID_SERVICIO", reg.id_servicio);
                    cmd.Parameters.AddWithValue("@COMENTARIO", reg.comentario);
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
