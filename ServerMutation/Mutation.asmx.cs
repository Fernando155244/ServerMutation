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
        string conexion = @"server=IP;port=port;uid=User;pwd=Password;database=NombreBaseDatos";

        /*Con esta función buscamos saber si tenemos o no conexión*/
        [WebMethod]
        public bool HelloWorld()
        {
            /*No resivimos nada, no se busca hacer nada, solo se regresa un true para decir que estamos conectados*/
            return true;
        }
        [WebMethod]
        public bool Login (int NoSolicitud, String rfc, int Tipo)
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
            //Mandamos la solicitud de los datos
            cmd.CommandText = $"select * from Usuarios where Solicitud='{NoSolicitud}' and rfc='{rfc}'and tipo={Tipo}";
            da.SelectCommand = cmd;
            conn.Open();
            da.Fill(ds);
            conn.Close();
            if(ds.Tables.Count != 0)
            {
                return true ;
            }else { 
                return false ; 
            }
        }
        [WebMethod]
        public DataSet Inicio(int NoSolicitud)
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
        [WebMethod]
        public DataSet Revision (int NoSolicitud)
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
            da.SelectCommand = cmd;
            conn.Open();
            da.Fill(ds);
            conn.Close();
            return ds;
        }
        public DataSet Directorio ()
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
