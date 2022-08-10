//LIBRERIAS DE TRABAJO
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient; //ACCESO A LOS DATOS DE LA BD COACHBD
using ProyectoIntegradorII.Datos;
using ProyectoIntegradorII.Models.ModelosCustom;
using ProyectoIntegradorII.Models;
using System.Data;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MailKit.Net.Smtp;
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
                try
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
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    cn.Close();
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
                try
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
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    cn.Close();
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
                try
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
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    cn.Close();
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
                try
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
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    cn.Close();
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
                try
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

                ViewBag.cantidad = temporal.Count();

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
                try
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

        public async Task<IActionResult> InfoCoach(int id)
        {
            var inf = await Task.Run(() => coachinfo().Where(c => c.idCoach == id).FirstOrDefault());
            return PartialView("_PartialCoachInfo", inf);
        }

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

        ECoach Buscar(int id)
        {
            return coachinfo().Where(c => c.idCoach == id).FirstOrDefault();
        }

        [HttpGet]
        public IActionResult Solicitar(int id)
        {
            ECoach reg = Buscar(id);

            SoliCoach sCoach = new SoliCoach();
            sCoach.idCoach = reg.idCoach;
            sCoach.precio = reg.precio;

            ViewBag.idcoach = sCoach.idCoach;
            ViewBag.coach = reg.coach;
            ViewBag.correo = reg.correo;
            ViewBag.precio = sCoach.precio;

            return View();
        }

        [HttpPost]
        public ActionResult Registrar(int idCoach, int tipoSesion, int tipoServicio, int precio, int cantidadSesiones, int cantidadHoras, int monto)
        {
            if (tipoSesion == 1)
            {
                ViewBag.sesion = "Referencia Estrategica";
            }
            if (tipoSesion == 2)
            {
                ViewBag.sesion = "Coaching";
            }
            if (tipoServicio == 1)
            {
                ViewBag.servicio = "Individual";
            }
            if (tipoServicio == 2)
            {
                ViewBag.servicio = "Paquete";
            }

            ECoach regx = Buscar(idCoach);

            ViewBag.correoCoach = regx.correo;

            ViewBag.coach = regx.coach;
            ViewBag.paises = new SelectList(paises(), "idPais", "pais");
            ViewBag.tipodocumentos = new SelectList(tiposdocumentos(), "idDocumento", "documento");

            SoliCoach s = new SoliCoach();
            s.idCoach = regx.idCoach;
            s.tipoSesion = tipoSesion;
            s.tipoServicio = tipoServicio;
            s.precio = precio;
            s.cantidadSesiones = cantidadSesiones;
            s.cantidadHoras = cantidadHoras;
            s.monto = monto;

            ViewBag.id = s.idCoach;
            ViewBag.ses = tipoSesion;
            ViewBag.ser = tipoServicio;
            ViewBag.precio = precio;
            ViewBag.cantsesiones = cantidadSesiones;
            ViewBag.cantHoras = cantidadHoras;
            ViewBag.monto = monto;
            ViewBag.totalseshor = cantidadSesiones * cantidadHoras;

            return View(s);
        }

        [HttpPost]
        public IActionResult NuevoCliente(SoliCoach reg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResponseClienteNuevo responseClienteNuevo = new ResponseClienteNuevo();
                    responseClienteNuevo = Agregar(reg);

                    ViewBag.mensaje = responseClienteNuevo.mensaje;

                    TempData["SuccessMessage"] = "Le hemos enviado su usuario y contraseña a su correo";

                    string tiposes = "";
                    string tiposer = "";

                    if (reg.tipoSesion == 1)
                    {
                        tiposes = "Referencia Estrategica";
                    }
                    if (reg.tipoSesion == 2)
                    {
                        tiposes = "Coaching";
                    }
                    if (reg.tipoServicio == 1)
                    {
                        tiposer = "Individual";
                    }
                    if (reg.tipoServicio == 2)
                    {
                        tiposer = "Paquete";
                    }

                    ECoach regx = Buscar(reg.idCoach);

                    var cadena = new Conexion();

                    #region envio correo cliente
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse("alerta.coach@inteligencia369.com"));
                    email.To.Add(MailboxAddress.Parse(reg.correo));
                    email.Subject = "Portal Alerta Coach: Usuario y Contraseña";
                    email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = "Estimado cliente "+ responseClienteNuevo.nombresCompletos + "," + 
                               "<br/>" + 
                               "<br/>" +
                               "Se ha creado su perfil para la web Alerta Coach. Se detalla a continuación sus credenciales de acceso:" + 
                               "<br/>" +
                               "<br/>" +
                               "Usuario: " + responseClienteNuevo.nombreUsuario + "<br/>" +
                               "Contraseña: " + responseClienteNuevo.clave + 
                               "<br/>" +
                               "<br/>" +
                               "Detalles de la solicitud:" + 
                               "<br/>" + 
                               "<br/>" +
                               "Usted ha solicitado los servicios del coach: " + regx.coach + "<br/>" +
                               "Tipo de Sesión: " + tiposes + "<br/>" +
                               "Tipo de Servicio: " + tiposer + "<br/>" +
                               "Precio: " + "USD " + reg.precio + "<br/>" +
                               "Total de Horas por Sesiones: " + reg.cantidadHoras * reg.cantidadSesiones + "<br/>" +
                               "Monto: " + "USD " + reg.monto + 
                               "<br/>" + 
                               "<br/>" +
                               "Si tiene más preguntas, contactarnos por medio de este correo." +
                               "<br/>" +
                               "<br/>" +
                               "Gracias," + "<br/>" +
                               "Equipo de Alerta Coach"
                    };
                    using (var emailClient = new SmtpClient())
                    {
                        emailClient.Connect("mail.inteligencia369.com", 587, MailKit
                            .Security.SecureSocketOptions.Auto);
                        emailClient.Authenticate("alerta.coach@inteligencia369.com", "Al3rt4c0ach2022");
                        emailClient.Send(email);
                        emailClient.Disconnect(true);
                    }
                    #endregion

                    string Clinotificado = "Notificado";

                    using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("USP_ACTUALIZAR_SERVICIO_CLIENTE", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CLIENTENOTIFICADO", Clinotificado);
                        cmd.ExecuteNonQuery();
                    }

                    #region envio correo coach
                    string correoCoach = "";

                    using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
                    {
                        SqlCommand cmd = new SqlCommand("SELECT B.CORREO FROM COACH A INNER JOIN PERSONA B ON A.ID_PERSONA = B.ID_PERSONA WHERE A.ID_COACH = @idCoach", cn); // Select a la tabla tipodocumento
                        cn.Open(); //Abrir la conexión
                        cmd.Parameters.AddWithValue("@idcoach", reg.idCoach);
                        SqlDataReader dr = cmd.ExecuteReader(); // LEER DATOS
                        while (dr.Read()) //Lee cada uno de los registros
                        {
                            correoCoach = dr.GetString(0);

                        }
                    }

                    var email2 = new MimeMessage();
                    email2.From.Add(MailboxAddress.Parse("alerta.coach@inteligencia369.com"));
                    email2.To.Add(MailboxAddress.Parse(correoCoach));
                    email2.Subject = "Portal Alerta Coach: Solicitud de Servicio";
                    email2.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = "Estimado Coach," + 
                               "<br/>" + 
                               "<br/>" +
                               "El cliente " + responseClienteNuevo.nombresCompletos + " ha contratado sus servicios" + 
                               "<br/>" +
                               "<br/>" +
                               "Detalle del Servicio: " + "<br/>" +
                               "Tipo de Sesión: " + tiposes + "<br/>" +
                               "Tipo de Servicio: " + tiposer + "<br/>" +
                               "Precio: " + "USD " + reg.precio + "<br/>" +
                               "Total de Horas por Sesiones: " + reg.cantidadHoras * reg.cantidadSesiones + "<br/>" +
                               "Monto: " + "USD " + reg.monto + 
                               "<br/>" +
                               "<br/>" +
                               "Si desea aceptar el servicio diríjase a su cuenta respectiva y confírmelo." +
                               "<br/>" +
                               "<br/>" +
                               "Gracias," + "<br/>" +
                               "Equipo de Alerta Coach"

                    };
                    using (var emailClient2 = new SmtpClient())
                    {
                        emailClient2.Connect("mail.inteligencia369.com", 587, MailKit
                            .Security.SecureSocketOptions.Auto);
                        emailClient2.Authenticate("alerta.coach@inteligencia369.com", "Al3rt4c0ach2022");
                        emailClient2.Send(email2);
                        emailClient2.Disconnect(true);
                    }
                    #endregion

                    string Coachnotificado = "Notificado";

                    using (var cn = new SqlConnection(cadena.getCadenaSQL())) // ESTABLECE LA CONEXIÓN CON LA BD
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("USP_ACTUALIZAR_SERVICIO_COACH", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COACHNOTIFICADO", Coachnotificado);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                TempData["SuccessMessage"] = "No se ha podido guardar sus datos";
            }

            return RedirectToAction("EncontrarCoach");
        }

        public ResponseClienteNuevo Agregar(SoliCoach reg)
        {

            ResponseClienteNuevo temporal = new ResponseClienteNuevo();

            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                cn.Open();
                SqlTransaction tr = cn.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_REGISTRO_INICIAL_2", cn, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_COACH", reg.idCoach);
                    cmd.Parameters.AddWithValue("@TIPOSESION", reg.tipoSesion);
                    cmd.Parameters.AddWithValue("@TIPOSERVICIO", reg.tipoServicio);
                    cmd.Parameters.AddWithValue("@PRECIO", reg.precio);
                    cmd.Parameters.AddWithValue("@CANTIDADSESIONES", reg.cantidadSesiones);
                    cmd.Parameters.AddWithValue("@CANTIDADHORAS", reg.cantidadHoras);
                    cmd.Parameters.AddWithValue("@MONTO", reg.monto);
                    cmd.Parameters.AddWithValue("@NOMBRES", reg.nombres);
                    cmd.Parameters.AddWithValue("@APELLIDOS", reg.apellidos);
                    cmd.Parameters.AddWithValue("@DIRECCION", (reg.direccion) ?? "");
                    cmd.Parameters.AddWithValue("@TELEFONO", (reg.telefono) ?? "");
                    cmd.Parameters.AddWithValue("@CORREO", reg.correo);
                    cmd.Parameters.AddWithValue("@TIPODOCUMENTO", reg.tipoDocumento);
                    cmd.Parameters.AddWithValue("@NUMDOCUMENTO", reg.numDocumento);
                    cmd.Parameters.AddWithValue("@PAIS", reg.pais);
                    cmd.Parameters.Add("@NOMBRES_OUT", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@APELLIDOS_OUT", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ID_NOMBRE_USUARIO_CUSTOM_OUT", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@CLAVE_OUT", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MENSAJE_OUT", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    tr.Commit();

                    temporal.nombresCompletos = cmd.Parameters["@NOMBRES_OUT"].Value.ToString() + " " + cmd.Parameters["@APELLIDOS_OUT"].Value.ToString();
                    temporal.nombreUsuario = cmd.Parameters["@ID_NOMBRE_USUARIO_CUSTOM_OUT"].Value.ToString();
                    temporal.clave = cmd.Parameters["@CLAVE_OUT"].Value.ToString();
                    temporal.mensaje = cmd.Parameters["@MENSAJE_OUT"].Value.ToString();

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    tr.Rollback();
                }

                finally { cn.Close(); }

            }

            return temporal;

        }
    }
}
