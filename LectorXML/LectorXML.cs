using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Diagnostics;
using System.IO;
//using System.Xml.Linq;

namespace Datos
{
    class LectorXML
    {
        private string rutaEnsamblado = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

        public DataSet LeerIndice (string ruta)
        {
            DataSet índice = new DataSet();
            DataTable dt = new DataTable("lecciones");
            dt.Columns.Add("Id");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Dificultad");
            dt.Columns.Add("Path");
            dt.Columns.Add("Autor");

            if (File.Exists(rutaEnsamblado + ruta)) //si existe el índice
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(rutaEnsamblado + ruta);

                XmlNodeList lecciones = xml.GetElementsByTagName("root");
                XmlNodeList lista = ((XmlElement)lecciones[0]).GetElementsByTagName("item");

                foreach (XmlElement nodo in lista)
                {
                    string id = nodo.GetAttribute("ID");
                    string nombre = nodo.GetAttribute("Nombre");
                    string tipo = nodo.GetAttribute("Tipo");
                    string dificultad = nodo.GetAttribute("Dificultad");
                    string path = nodo.GetAttribute("Path");
                    string autor = nodo.GetAttribute("Autor");
                    DataRow fila = dt.NewRow();
                    fila["Id"] = id;
                    fila["Nombre"] = nombre;
                    fila["Tipo"] = tipo;
                    fila["Dificultad"] = dificultad;
                    fila["Path"] = path;
                    fila["Autor"] = autor;
                    dt.Rows.Add(fila);
                }
            }
            else
            {
                corregirÍndiceFaltante();
                //LeerIndice(ruta);
            }

            índice.Tables.Add(dt);
            return índice;
        }

        public DataSet leerLección(string ruta)
        {
            DataSet lección = new DataSet();

            DataTable dt = new DataTable("lección");
            dt.Columns.Add("id");
            //dt.Columns.Add("nombre");
            dt.Columns.Add("texto");

            try
            {
                if (File.Exists(rutaEnsamblado + "\\Recursos\\Lecciones\\" + ruta)) //si existe el índice
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(rutaEnsamblado + "\\Recursos\\Lecciones\\" + ruta);
                    XmlNodeList listaRoot = xml.GetElementsByTagName("root");
                    XmlNodeList nodos = ((XmlElement)listaRoot[0]).GetElementsByTagName("item");

                    foreach (XmlElement nodo in nodos)
                    {
                        string id = nodo.GetAttribute("ID");
                        //string nombre = nodo.GetAttribute("Nombre");
                        //string sección = nodo.GetAttribute("Sección");
                        string texto = nodo.GetAttribute("Texto");

                        DataRow fila = dt.NewRow();
                        fila["id"] = id;
                        //fila["nombre"] = nombre;
                        //fila["sección"] = sección;
                        fila["texto"] = texto;

                        dt.Rows.Add(fila);
                    }
                }
                else
                {
                    corregirLecciónFaltante(ruta.Substring("\\Lecciones\\".Length, ruta.Length - "\\Lecciones\\".Length - ".xml".Length));
                }

                lección.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
            return lección;
        }

        //public DataSet leerPreferencias(string ruta)
        //{

        //}

        //public DataSet leerEstadísticas(string ruta)
        //{

        //}

        void corregirÍndiceFaltante()
        {
            //se guarda la actividad
            XmlDocument docIndice = new XmlDocument();
            docIndice.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
"<root ver=\"1\" Level=\"0\">" + "</root>");

            XmlElement elemAct = docIndice.CreateElement("item"); //se crea la lección de muestra
            elemAct.SetAttribute("ID", "1");
            elemAct.SetAttribute("Nombre", "Lección de ejemplo");
            elemAct.SetAttribute("Tipo", "Letras");
            elemAct.SetAttribute("Dificultad", "Principiante");
            elemAct.SetAttribute("Path", "\\Lecciones\\666.xml");
            elemAct.SetAttribute("Autor", "Guillermo Toscani");

            docIndice.DocumentElement.AppendChild(elemAct);
            string ruta = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\índiceLecciones.xml";
            if (File.Exists(ruta)) //si ya está el archivo de la lección, lo eliminamos para escribirlo de nuevo
                File.Delete(ruta);
               
            docIndice.Save(ruta); //se guarda el archivo

            corregirLecciónFaltante("666"); //guardamos la lección de ejemplo que hace referencia el índice
        }

        void corregirLecciónFaltante(string quéLección)
        {
            XmlDocument docAct = new XmlDocument();
            docAct.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
"<root Ver=\"1\">" + "</root>");

            XmlElement elemAct = docAct.CreateElement("item"); //se crea la lección de muestra
            elemAct.SetAttribute("ID", "1");
            elemAct.SetAttribute("Texto", "asdf ñlkj qwer poiu zxcv -.,m");

            docAct.DocumentElement.AppendChild(elemAct);
            string ruta = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\Lecciones\"+ quéLección +".xml";
            if (File.Exists(ruta)) //si ya está el archivo de la lección, lo eliminamos para escribirlo de nuevo
                File.Delete(ruta);

            docAct.Save(ruta); //se guarda el archivo
        }
    }

}
