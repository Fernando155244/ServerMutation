using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

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
            /*No resivimos nada, no se busca hacer nada, solo se regresa un true para decir que estamos conectados*/
            return true;
        }
        [WebMethod]
        public bool Login(int NoSolicitud, int Tipo)
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
            if(Tipo == 0)
            {
                //Mandamos la solicitud de los datos
                cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres,s.id,s.f_registro, s.estado_actual, s.observaciones_cancelacion, s.observaciones_rechazo FROM trabajador t JOIN solicitudes_cambios s ON s.trabajador = t.id WHERE s.id = '{NoSolicitud}';";
            }else if (Tipo == 1)
            {
                cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres,s.id,s.f_registro, s.estado_actual, s.observacion_ur, s.observacion_ur_2, s.observaciones_cancelacion, s.observaciones_dgp, s.observaciones_rechazo FROM trabajador t JOIN solicitudes_permuta s ON s.id_trabajador = t.id WHERE s.id = '{NoSolicitud}';";
            }
            
            
            da.SelectCommand = cmd;
            conn.Open();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        [WebMethod]
        public DataSet Inicio(int NoSolicitud, int Tipo)
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
            if (Tipo == 0)
            {
                //Mandamos la solicitud de los datos
                cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres, s.estado_actual, s.opcion_1,s.opcion_2, s.nivel_educativo, t.rfc FROM trabajador t JOIN solicitudes_cambios s ON s.trabajador = t.id WHERE s.id = '{NoSolicitud}';";
            }
            else if (Tipo == 1)
            {
                cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres, s.estado_actual, s.opcion_1, s.nivel_educativo, t.rfc FROM trabajador t JOIN solicitudes_permuta s ON s.id_trabajador = t.id WHERE s.id = '{NoSolicitud}';";
            }
            da.SelectCommand = cmd;
            conn.Open();
            da.Fill(ds);
            conn.Close();
            return ds;
        }
        [WebMethod]
        public DataSet Revision(int NoSolicitud, int Tipo)
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
            //cmd.CommandText = $"Aqui irá la consulta de la BD";
            if (Tipo == 0)
            {
                //Mandamos la solicitud de los datos
                cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres, s.estado_actual, s.opcion_1,s.opcion_2, s.nivel_educativo, t.rfc FROM trabajador t JOIN solicitudes_cambios s ON s.trabajador = t.id WHERE s.id = '{NoSolicitud}';";
            }
            else if (Tipo == 1)
            {
                cmd.CommandText = $"SELECT t.paterno, t.materno, t.nombres, s.estado_actual, s.opcion_1, s.nivel_educativo, t.rfc FROM trabajador t JOIN solicitudes_permuta s ON s.id_trabajador = t.id WHERE t.rfc = 'OORF0004'AND s.id = '{NoSolicitud}';";
            }
            
            da.SelectCommand = cmd;
            conn.Open();
            da.Fill(ds);
            conn.Close();
            return ds;
        }
        [WebMethod]
        public DataSet Graficas(int tipo, bool certificadas, bool canceladas, int status, int referencias)
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
            if 
            da.SelectCommand = cmd;
            conn.Open();
            da.Fill(ds);
            conn.Close();
            return ds;
        }
        public DataSet Directorio()
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
    }
}
