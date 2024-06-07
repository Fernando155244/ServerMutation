using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

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
        /*Con esta función buscamos saber si tenemos o no conexión*/
        [WebMethod]
        public bool HelloWorld()
        {
            /*No resivimos nada, no se busca hacer nada, solo se regresa un true para decir que estamos conectados*/
            return true;
        }
    }
}
