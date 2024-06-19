using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Runtime.ConstrainedExecution;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace ServerMutation
{
    /// <summary>
    /// Descripción breve de Mutation
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Mutation : System.Web.Services.WebService
    {
        string conexion = @"server=mysqludl155244.mysql.database.azure.com;port=3306;uid=Fernando155244;pwd=Luminosc155244;database=cambintest_limpia";

        /*Con esta función buscamos saber si tenemos o no conexión*/
        [WebMethod]
        public bool HelloWorld()
        {
            /*No recibimos nada, no se busca hacer nada, solo se regresa un true para decir que estamos conectados*/
            return true;
        }
        /*Con esta función podemos analizar si la conexión es correcta*/
        [WebMethod]
        public bool Login(int NoSolicitud, int Tipo)
        {
            try
            {
                //Partes de conexión
                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataAdapter da = new MySqlDataAdapter();
                DataSet ds = new DataSet();

                //Datos de conexion
                conn.ConnectionString = conexion;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                //Con esto definimos si es de tipo cambios o permutas
                if (Tipo == 0)
                {
                    //Mandamos la solicitud de los datos para  cambios
                    cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres,s.id,s.f_registro, s.estado_actual, s.observaciones_cancelacion, s.observaciones_rechazo FROM trabajador t JOIN solicitudes_cambios s ON s.trabajador = t.id WHERE s.id = '{NoSolicitud}';";
                }
                else if (Tipo == 1)
                {
                    //Mandamos la solicitud de los datos para permutas
                    cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres,s.id,s.f_registro, s.estado_actual, s.observacion_ur, s.observacion_ur_2, s.observaciones_cancelacion, s.observaciones_dgp, s.observaciones_rechazo FROM trabajador t JOIN solicitudes_permuta s ON s.id_trabajador = t.id WHERE s.id = '{NoSolicitud}';";
                }

                //Mandamos a consulta
                da.SelectCommand = cmd;
                conn.Open();
                da.Fill(ds);
                conn.Close();
                //Si el resultado es cero usuarios entonces mandamos un false
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    //Si el resultado es distinto de cero usuarios entonces mandamos un true
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false; }
        }
        /*Con esta función mandamos a llamar los datos del solicitante para que nos diga el avance de la misma solicitud*/
        [WebMethod]
        public DataSet Inicio(int NoSolicitud, int Tipo)
        {
            try
            {
                //Declaramos las partes de la conexión
                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataAdapter da = new MySqlDataAdapter();
                DataSet ds = new DataSet();

                //Datos de conexion
                conn.ConnectionString = conexion;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                //Mandamos la solicitud de los datos
                if (Tipo == 0)
                {
                    //Mandamos la solicitud de los datos si es cambios
                    cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres, s.estado_actual, s.opcion_1,s.opcion_2, s.nivel_educativo, t.rfc FROM trabajador t JOIN solicitudes_cambios s ON s.trabajador = t.id WHERE s.id = '{NoSolicitud}';";
                }
                else if (Tipo == 1)
                {
                    //Mandamos la solicitud de los datos si es permutas
                    cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres, s.estado_actual, s.opcion_1, s.nivel_educativo, t.rfc FROM trabajador t JOIN solicitudes_permuta s ON s.id_trabajador = t.id WHERE s.id = '{NoSolicitud}';";
                }
                da.SelectCommand = cmd;
                //mandamos a consultas
                conn.Open();
                da.Fill(ds);
                conn.Close();
                //Retornamos los valores definidos
                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        [WebMethod]
        public DataSet Revision(int NoSolicitud, int Tipo)
        {
            try
            { //Declaramos las partes de la conexión
                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataAdapter da = new MySqlDataAdapter();
                DataSet ds = new DataSet();

                //Datos de conexion
                conn.ConnectionString = conexion;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                //Mandamos la solicitud de los datos
                if (Tipo == 0)
                {
                    //Mandamos la solicitud de los datos para cambios
                    cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres, s.estado_actual, s.opcion_1,s.opcion_2, s.nivel_educativo, t.rfc FROM trabajador t JOIN solicitudes_cambios s ON s.trabajador = t.id WHERE s.id = '{NoSolicitud}';";
                }
                else if (Tipo == 1)
                {
                    //Mandamos la solicitud de los datos para permutas
                    cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres, s.estado_actual, s.opcion_1, s.nivel_educativo, t.rfc FROM trabajador t JOIN solicitudes_permuta s ON s.id_trabajador = t.id WHERE t.rfc = 'OORF0004'AND s.id = '{NoSolicitud}';";
                }
                //mandamos a consultas
                da.SelectCommand = cmd;
                conn.Open();
                da.Fill(ds);
                conn.Close();
                //Retornasmos los valores definidos
                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        /*Con esta función definimos los valores de la grafica/tabla, cada valor es para tener definido que valor tenemos que ingresar a la 
         pantalla dependiendo del interes del solicitante*/
        [WebMethod]
        public DataSet Graficas(int referencias, int tipo, int certificadas, int canceladas, int status)
        {
            try
            {
                //Declaramos las partes de la conexión
                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataAdapter da = new MySqlDataAdapter();
                DataSet ds = new DataSet();

                //Datos de conexion
                conn.ConnectionString = conexion;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                //Empezamos por las referencias debido a que es el valor que define si veremos graficas de estados o po nivel educativo
                if (referencias == 0) //Ahora la referencia 0 es la que indica Nivel educativo
                {
                    //cmd.CommandText = $"SELECT s.nivel_educativo, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados ";
                    if (tipo == 1)
                    {
                        //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados GROUP BY s.estado_actual;
                        //Tratandose de los certificados tenemo 2 valores si o no
                        cmd.CommandText = $"SELECT s.nivel_educativo, COUNT(s.id) FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados ";
                        if (certificadas == 1 || status == 1)
                        {
                            cmd.CommandText = $"{cmd.CommandText} WHERE s.certificada_ur = 1";
                        }
                        else if (certificadas == 0 || status == 2)
                        {
                            cmd.CommandText = $"{cmd.CommandText} WHERE s.certificada_ur = 0";
                        }
                        //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 GROUP BY s.estado_actual;
                    }//Si es tipo 2 significa que quiere las permutas
                    else if (tipo == 2)
                    {
                        //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados GROUP BY s.estado_actual;
                        //Tratandose de los certificados tenemo 2 valores si o no
                        cmd.CommandText = $"SELECT s.nivel_educativo, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados ";
                        if (certificadas == 1 || status == 1)
                        {
                            cmd.CommandText = $"{cmd.CommandText} WHERE s.certificado_ur = 1";
                        }
                        else if (certificadas == 0 || status == 2)
                        {
                            cmd.CommandText = $"{cmd.CommandText} WHERE s.certificado_ur = 0";
                        }
                        //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 GROUP BY s.estado_actual;
                    }
                    else
                    {
                        //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados GROUP BY s.estado_actual;
                        //Tratandose de los certificados tenemo 2 valores si o no
                        cmd.CommandText = $"SELECT s.nivel_educativo, COUNT(s.id) FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados ";
                        if (certificadas == 1 || status == 1)
                        {
                            cmd.CommandText = $"{cmd.CommandText} WHERE s.certificada_ur = 1";
                        }
                        else if (certificadas == 0 || status == 2)
                        {
                            cmd.CommandText = $"{cmd.CommandText} WHERE s.certificada_ur = 0";
                        }
                        //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 GROUP BY s.estado_actual;
                    }

                    //Mismo caso, para cancelar solo tenemos 2 valores, si o no y en este caso solo nos interesa si el resultado es si
                    if (canceladas == 1)
                    {
                        cmd.CommandText = $"{cmd.CommandText} OR s.cancelada = 1";
                        //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 OR s.cancelada = 1 GROUP BY s.estado_actual;
                    }

                    cmd.CommandText = $"{cmd.CommandText} GROUP BY s.nivel_educativo ORDER BY s.nivel_educativo;";
                    //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 OR s.cancelada = 1 GROUP BY s.estado_actual ORDER BY s.estado_actual;
                }
                else
                {
                    //En esta area iremos generando los filtros dependiendo de cada valor, en la app son fijos, así que no hay error a tratar practicamente
                    if (tipo == 0 && certificadas == 1 && canceladas == 1 && status == 0)
                    {
                        //En caso de que se desee ver todo mandamos a pedir todo
                        cmd.CommandText = $" SELECT SUM(s.estado_actual + s2.estado_actual), e.nombre , COUNT(s.id)";
                        //Tratandose de los certificados tenemo 2 valores si o no
                    }
                    else
                    { //En caso contrario empezamos a desarrollar todo el codigo para analizar las posibilidades
                      //Si tipo es 1 significa que quieren solo los cambios
                        if (tipo == 1)
                        {
                            cmd.CommandText = $"SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados";
                            //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados GROUP BY s.estado_actual;
                            //Tratandose de los certificados tenemo 2 valores si o no

                            if (certificadas == 1 || status == 1)
                            {
                                cmd.CommandText = $"{cmd.CommandText} WHERE s.certificada_ur = 1";
                            }
                            else if (certificadas == 0 || status == 2)
                            {
                                cmd.CommandText = $"{cmd.CommandText} WHERE s.certificada_ur = 0";
                            }
                            //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 GROUP BY s.estado_actual;
                        }//Si es tipo 2 significa que quiere las permutas
                        else if (tipo == 2)
                        {
                            cmd.CommandText = $"SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados";
                            //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados GROUP BY s.estado_actual;
                            //Tratandose de los certificados tenemo 2 valores si o no

                            if (certificadas == 1 || status == 1)
                            {
                                cmd.CommandText = $"{cmd.CommandText} WHERE s.certificada_ur = 1";
                            }
                            else if (certificadas == 0 || status == 2)
                            {
                                cmd.CommandText = $"{cmd.CommandText} WHERE s.certificada_ur = 0";
                            }
                            //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 GROUP BY s.estado_actual;
                        }
                        else
                        {
                            //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados GROUP BY s.estado_actual;
                            //Tratandose de los certificados tenemo 2 valores si o no
                            cmd.CommandText = $"SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados";
                            if (certificadas == 1 || status == 1)
                            {
                                cmd.CommandText = $"{cmd.CommandText} WHERE s.certificado_ur = 1";
                            }
                            else if (certificadas == 0 || status == 2)
                            {
                                cmd.CommandText = $"{cmd.CommandText} WHERE s.certificado_ur = 0";
                            }
                            //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 GROUP BY s.estado_actual;
                        }

                        //Mismo caso, para cancelar solo tenemos 2 valores, si o no y en este caso solo nos interesa si el resultado es si
                        if (canceladas == 1)
                        {
                            cmd.CommandText = $"{cmd.CommandText} OR s.cancelada = 1";
                            //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 OR s.cancelada = 1 GROUP BY s.estado_actual;
                        }

                        cmd.CommandText = $"{cmd.CommandText} GROUP BY s.estado_actual ORDER BY s.estado_actual;";
                        //SELECT e.nombre, s.estado_actual, COUNT(s.id) FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados WHERE s.certificado_ur = 2 OR s.cancelada = 1 GROUP BY s.estado_actual ORDER BY s.estado_actual;

                    }
                }
                //FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados JOIN solicitudes_permuta s2 ON s2.estado_actual = e.idestados GROUP BY e.nombre;
                //mandamos a consultas
                da.SelectCommand = cmd;
                conn.Open();
                da.Fill(ds);
                conn.Close();
                //Retornasmos los valores definidos
                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public DataSet Directorio()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                MySqlDataAdapter da = new MySqlDataAdapter();
                DataSet ds = new DataSet();

                //Datos de conexion
                conn.ConnectionString = conexion;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                //Mandamos la solicitud de los datos
                cmd.CommandText = $"Aqui irá la consulta de la BD";
                da.SelectCommand = cmd;
                conn.Open();
                da.Fill(ds);
                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
