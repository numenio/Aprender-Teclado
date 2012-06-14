using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Datos
{
    //clase para centralizar el manejo de datos. El parámetro 'tipo' de los métodos podría hacer que se lea de una BD en lugar de un XML
    public class AdminDatos
    {
        public DataSet leerÍndice(string ruta, tipoÍndice tipo = tipoÍndice.xml)
        {
            DataSet índice = new DataSet();
            if (tipo == tipoÍndice.xml)
            {
                LectorXML miLectorXML = new LectorXML();
                índice = miLectorXML.LeerIndice(ruta);
            }
            return índice;
        }

        public DataSet leerLección(string ruta, tipoÍndice tipo = tipoÍndice.xml)
        {
            DataSet lección = new DataSet();
            if (tipo == tipoÍndice.xml)
            {
                LectorXML miLectorXML = new LectorXML();
                lección = miLectorXML.leerLección(ruta);
            }
            return lección;
        }
    }
}
