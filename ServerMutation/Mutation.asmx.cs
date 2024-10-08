﻿using System;
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
using Org.BouncyCastle.Asn1.Mozilla;

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
        string conexion = @"server=127.0.0.1;port=3306;uid=root;pwd='';database=cambintest_limpia";

        /*Con esta función buscamos saber si tenemos o no conexión*/
        [WebMethod]
        public bool HelloWorld()
        {
            /*No recibimos nada, no se busca hacer nada, solo se regresa un true para decir que estamos conectados*/
            return true;
        }
        /*Con esta función podemos analizar si la conexión es correcta*/
        [WebMethod]
        public bool Login(int NoSolicitud, String rfc, int Tipo)
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
                    cmd.CommandText = $"SELECT s.id FROM trabajador t JOIN solicitudes_cambios s ON s.trabajador = t.id WHERE s.id = {NoSolicitud} AND t.rfc ='{rfc}';";
                }
                else if (Tipo == 1)
                {
                    //Mandamos la solicitud de los datos para permutas
                    cmd.CommandText = $"SELECT s.id FROM trabajador t JOIN solicitudes_permuta s ON s.id_trabajador = t.id WHERE s.id = {NoSolicitud} AND t.rfc ='{rfc}';";
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
                    //Comando para evitar que se queden valores vacíos, usar en todo IFNULL(,"Vacio")
                    //Mandamos la solicitud de los datos si es cambios
                    cmd.CommandText = $"SELECT IFNULL(s.id,\"Vacio\") as id, IFNULL(t.paterno,\"Vacio\") as paterno, IFNULL(t.materno,\"Vacio\") as materno, IFNULL(t.nombres,\"Vacio\") as nombre, IFNULL(esa.nombre,\"Vacio\") as Actual, IFNULL(esb.nombre,\"Vacio\") as Opcion1, IFNULL(esc.nombre,\"Vacio\") as Opcion2, IFNULL(n.nivel,\"Vacio\") as nivel, IFNULL(t.rfc,\"Vacio\") as rfc, IFNULL(s.tipo_solicitud,\"Vacio\") as tipo_solicitud, IFNULL(s.solicitud_real,\"Vacio\") as solicitud_real, IFNULL(s.f_registro,\"Vacio\") as f_registro, IFNULL(s.cancelada,\"Vacio\") as cancelada, IFNULL(s.observaciones_cancelacion,\"Vacio\") as observaciones_cancelacion, IFNULL(s.validada_dgp,\"Vacio\") as validada_dgp, IFNULL(s.certificada_ur ,\"Vacio\") as certificada_ur, iFNULL(ts.f_certificacion,\"Vacio\") as f_cetificada_ur, IFNULL(ts.observaciones,\"Vacio\") as obsevaciones, IFNULL(s.marcada,\"Vacio\") as marcada, IFNULL(ts.estatus,\"Vacio\") as estatus FROM trabajador t RIGHT JOIN solicitudes_cambios s ON s.trabajador = t.id LEFT JOIN estados esa ON s.estado_actual = esa.idestados LEFT JOIN estados esb ON s.opcion_1 = esb.idestados LEFT JOIN estados esc ON s.opcion_2 = esc.idestados LEFT JOIN nivel_educativo n ON n.idnivel_educativo = s.nivel_educativo LEFT JOIN t_sol_cambios_certificadas ts ON s.id = ts.solicitud WHERE s.id = '{NoSolicitud}';";
                }
                else if (Tipo == 1)
                {
                    //Mandamos la solicitud de los datos si es permutas
                    cmd.CommandText = $"SELECT s.id, IFNULL(t.paterno,\"Vacio\") as paterno, IFNULL(t.materno,\"Vacio\") as materno, IFNULL(t.nombres,\"Vacio\") as nombres, IFNULL(esa.nombre,\"Vacio\") as Actual , IFNULL(esb.nombre,\"Vacio\") as Opcion, IFNULL(n.nivel,\"Vacio\") as nivel, IFNULL(t.rfc,\"Vacio\") as rfc, IFNULL(s.f_registro,\"Vacio\") as f_registro, IFNULL(s.solicitud_capturada_100,\"Vacio\") as solicitud_real, IFNULL(s.cancelada,\"Vacio\") as cancelada, IFNULL(s.observaciones_cancelacion,\"Vacio\") as observaciones_cancelacion, IFNULL(s.validacion_dgp,\"Vacio\") as validada_dgp, IFNULL(s.f_validacion_dgp,\"Vacio\") as f_validacion_dgp, IFNULL(s.observaciones_dgp,\"Vacio\") as observaciones_dgp, IFNULL(s.certificado_ur,\"Vacio\") as certificada_ur, IFNULL(s.f_certificacion_ur,\"Vacio\") as f_certificacion_ur, IFNULL(s.observacion_ur,\"Vacio\") as observacion_ur, IFNULL(s.marcada,\"Vacio\") as marcada, IFNULL(ts.observaciones,\"Vacio\") as obsevaciones,IFNULL(ts.estatus,\"Vacio\") as estatus FROM trabajador t RIGHT JOIN solicitudes_permuta s ON s.id_trabajador = t.id LEFT JOIN estados esa ON s.estado_actual = esa.idestados LEFT JOIN estados esb ON s.opcion_1 = esb.idestados LEFT JOIN nivel_educativo n ON n.idnivel_educativo = s.nivel_educativo LEFT JOIN solicitudes_canceladas sc ON s.id = sc.solicitud LEFT JOIN t_sol_permutas_certificadas ts ON s.id = ts.solicitud  WHERE s.id = '{NoSolicitud}';";
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
        public DataSet Cancelación (int NoSolicitud , int Tipo)
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
            if(Tipo == 0)
            {
                cmd.CommandText = $"SELECT IFNULL(C.f_registro,\"Vacio\") as f_cancelacion FROM solicitudes_canceladas C WHERE C.solicitud ={NoSolicitud} AND C.ts =1";
            }
            else if (Tipo == 1)
            {
                cmd.CommandText = $"SELECT IFNULL(C.f_registro,\"Vacio\") as f_cancelacion FROM solicitudes_canceladas C WHERE C.solicitud ={NoSolicitud} AND C.ts =2";
            }
            //mandamos a consultas
            da.SelectCommand = cmd;
            conn.Open();
            da.Fill(ds);
            conn.Close();
            //Retornasmos los valores definidos
            return ds;
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
                    cmd.CommandText = $"SELECT IFNULL(t.paterno,\"Vacion\") as paterno, IFNULL(t.materno,\"Vacio\") as materno, IFNULL(t.nombres,\"Vacio\") as nombre, IFNULL(esa.nombre,\"Vacio\") as Actual, IFNULL(esb.nombre ,\"Vacio\") as Opcion1, IFNULL(esc.nombre,\"Vacio\") as Opcion2, IFNULL(n.nivel,\"Vacio\") as nivel, IFNULL(t.rfc,\"Vacio\") as rfc FROM trabajador t RIGHT JOIN solicitudes_cambios s ON s.trabajador = t.id LEFT JOIN estados esa ON s.estado_actual = esa.idestados LEFT JOIN estados esb ON s.opcion_1 = esb.idestados LEFT JOIN estados esc ON s.opcion_2 = esc.idestados LEFT JOIN nivel_educativo n ON n.idnivel_educativo = s.nivel_educativo WHERE s.id =  '{NoSolicitud}';";
                }
                else if (Tipo == 1)
                {
                    //Mandamos la solicitud de los datos para permutas
                    cmd.CommandText = $"SELECT IFNULL(t.paterno,\"Vacio\") as paterno, IFNULL(t.materno,\"Vacio\") as materno, IFNULL(t.nombres,\"Vacio\") as nombres, IFNULL(esa.nombre,\"Vacio\") as Actual , IFNULL(esb.nombre,\"Vacio\") as Opcion, IFNULL(n.nivel,\"Vacio\") as nivel, IFNULL(t.rfc,\"Vacio\") as rfc FROM trabajador t RIGHT JOIN solicitudes_permuta s ON s.id_trabajador = t.id LEFT JOIN estados esa ON s.estado_actual = esa.idestados LEFT JOIN estados esb ON s.opcion_1 = esb.idestados LEFT JOIN nivel_educativo n ON n.idnivel_educativo = s.nivel_educativo WHERE s.id = '{NoSolicitud}';";
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
                        cmd.CommandText = $"SELECT n.nivel, COUNT(s.id) as Cant FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados JOIN nivel_educativo N ON N.idnivel_educativo=s.nivel_educativo ";
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
                        cmd.CommandText = $"SELECT n.nivel, COUNT(s.id) as Cant FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados JOIN nivel_educativo N ON N.idnivel_educativo=s.nivel_educativo";
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
                        cmd.CommandText = $"SELECT n.nivel, COUNT(s.id) as Cant FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados JOIN nivel_educativo N ON N.idnivel_educativo=s.nivel_educativo ";
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
                        cmd.CommandText = $" SELECT SUM(s.estado_actual + s2.estado_actual), e.nombre , COUNT(s.id) as Cant";
                        //Tratandose de los certificados tenemo 2 valores si o no
                    }
                    else
                    { //En caso contrario empezamos a desarrollar todo el codigo para analizar las posibilidades
                      //Si tipo es 1 significa que quieren solo los cambios
                        if (tipo == 1)
                        {
                            cmd.CommandText = $"SELECT e.nombre, s.estado_actual, COUNT(s.id) as Cant FROM solicitudes_cambios s JOIN estados e ON s.estado_actual = e.idestados";
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
                            cmd.CommandText = $"SELECT e.nombre, s.estado_actual, COUNT(s.id) as Cant FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados";
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
                            cmd.CommandText = $"SELECT e.nombre, s.estado_actual, COUNT(s.id) as Cant FROM solicitudes_permuta s JOIN estados e ON s.estado_actual = e.idestados";
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
    }
}
