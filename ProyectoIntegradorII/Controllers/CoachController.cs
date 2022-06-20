//LIBRERIAS DE TRABAJO
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient; //ACCESO A LOS DATOS DE LA BD COACHBD
using ProyectoIntegradorII.Datos;
using ProyectoIntegradorII.Models.ModelosCustom;
using ProyectoIntegradorII.Models;
using System.Data;

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

        ECoach Buscar(int id)
        {
            return coachinfo().Where(c => c.idCoach == id).FirstOrDefault();
        }

        public IActionResult SoliCoach(int id)
        {
            ECoach reg = Buscar(id);

            SoliCoach sCoach = new SoliCoach();
            sCoach.idCoach = reg.idCoach;
            sCoach.precio = reg.precio;

            ViewBag.coach = reg.coach;
            ViewBag.precio = sCoach.precio;

            return PartialView("_PartialCoachSolicitar");
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

        [HttpPost]
        public ActionResult SoliCoach(int id, int ses, int ser, decimal pre, int cantses, int canthor, decimal mon)
        {
            if (ses == 1)
            {
                ViewBag.sesion = "Referencia Estrategica";
            }
            if (ses == 2)
            {
                ViewBag.sesion = "Coaching";
            }
            if (ser == 1)
            {
                ViewBag.servicio = "Individual";
            }
            if (ser == 2)
            {
                ViewBag.servicio = "Paquete";
            }

            ECoach regx = Buscar(id);

            ViewBag.coach = regx.coach;
            ViewBag.paises = new SelectList(paises(), "idPais", "pais");
            ViewBag.tipodocumentos = new SelectList(tiposdocumentos(), "idDocumento", "documento");

            SoliCoach reg = new SoliCoach();
            reg.idCoach = regx.idCoach;
            reg.tipoSesion = ses;
            reg.tipoServicio = ser;
            reg.precio = pre;
            reg.cantidadSesiones = cantses;
            reg.cantidadHoras = canthor;
            reg.monto = mon;

            ViewBag.id = reg.idCoach;
            ViewBag.ses = ses;
            ViewBag.ser = ser;
            ViewBag.precio = pre;
            ViewBag.cantsesiones = cantses;
            ViewBag.cantHoras = canthor;
            ViewBag.monto = mon;
            ViewBag.totalseshor = cantses * canthor;

            return View(reg);
        }

        [HttpPost]
        public IActionResult NuevoCliente(SoliCoach reg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.mensaje = Agregar(reg);

                    TempData["SuccessMessage"] = "Le hemos enviado su usuario y contraseña XD";

                    //var email = new MimeMessage();
                    //email.From.Add(MailboxAddress.Parse("proyectointegradorcoach@gmail.com"));
                    //email.To.Add(MailboxAddress.Parse(reg.correo));
                    //email.Subject = "Usuario y Contraseña";
                    //email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    //{
                    //    Text = "Tu Usuario es: Carlos20191 y tu Contraseña es: h173h121"
                    //};
                    //using (var emailClient = new SmtpClient())
                    //{
                    //    emailClient.Connect("smtp.gmail.com", 587, MailKit
                    //        .Security.SecureSocketOptions.StartTls);
                    //    emailClient.Authenticate("proyectointegradorcoach@gmail.com", "encontrarcoach");
                    //    emailClient.Send(email);
                    //    emailClient.Disconnect(true);
                    //}
                }

                //var email = new MimeMessage();
                //email.From.Add(MailboxAddress.Parse("proyectointegradorcoach@gmail.com"));
                //email.To.Add(MailboxAddress.Parse("bsaucedo250300@gmail.com"));
                //email.Subject = "USUARIO Y CONTRASEÑA";
                //email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "Tu usuario es: , y tu contraseña es: " };

                //using var smtp = new SmtpClient();
                //smtp.Connect("smtp.gmail.com", 25, MailKit.Security.SecureSocketOptions.StartTls);
                //smtp.Authenticate("proyectointegradorcoach@gmail.com", "encontrarcoach");
                //smtp.Send(email);
                //smtp.Disconnect(true);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                TempData["SuccessMessage"] = "No se ha podido guardar sus datos";
            }

            return RedirectToAction("EncontrarCoach");
        }

        public string Agregar(SoliCoach reg)
        {
            string mensaje = "";
            var cadena = new Conexion();

            using (var cn = new SqlConnection(cadena.getCadenaSQL()))
            {
                cn.Open();
                SqlTransaction tr = cn.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_SOLICITAR_COACH_CLIENTE", cn, tr);
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
                    cmd.Parameters.AddWithValue("@DIRECCION", reg.direccion);
                    cmd.Parameters.AddWithValue("@TELEFONO", reg.telefono);
                    cmd.Parameters.AddWithValue("@CORREO", reg.correo);
                    cmd.Parameters.AddWithValue("@TIPODOCUMENTO", reg.tipoDocumento);
                    cmd.Parameters.AddWithValue("@NUMDOCUMENTO", reg.numDocumento);
                    cmd.Parameters.AddWithValue("@PAIS", reg.pais);
                    cmd.ExecuteNonQuery();

                    tr.Commit();

                    mensaje = "Datos registrados con exito";
                }

                catch (Exception ex) { mensaje = ex.Message; tr.Rollback(); }

                finally { cn.Close(); }

            }

            return mensaje;

        }
    }
}
