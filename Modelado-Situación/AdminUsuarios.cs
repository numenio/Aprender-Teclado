using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace LógicaAplicación
{
    public class AdminUsuarios
    {
        public AdminPreferencias adminPref;
        
        public AdminUsuarios(int IdUsuario)
        {
            adminPref = new AdminPreferencias(IdUsuario);
        }

        public AdminUsuarios(string nombre, string apellido)
        {
            XmlDocument doc = new XmlDocument();

            adminPref = new AdminPreferencias(buscarIdUsuario(nombre, apellido));
        }

        public bool guardarPreferenciasUsuario()
        {
            if (adminPref.guardarPreferencias())
                return true;
            else
                return false;
        }
        
        public static bool guardarNuevoUsuario(string nombre, string apellido)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Usuarios\índiceUsuarios.xml");
                if (buscarIdUsuario(nombre, apellido) == 0) //si no hay un usuario ya guardado con ese nombre
                {
                    XmlElement elem = doc.CreateElement("usuario");
                    elem.SetAttribute("id", últimoIdUsuario(doc).ToString());
                    elem.SetAttribute("nombre", nombre);
                    elem.SetAttribute("apellido", apellido);
                    doc.DocumentElement.AppendChild(elem);
                    doc.Save(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Usuarios\índiceUsuarios.xml");

                    
                    XElement xml = new XElement("preferencias",
                        new XElement("nombreUsuario", nombre + " " + apellido),
                        new XElement("tamañoLetra", "130"),
                        new XElement("nombreLetra", "Arial"),
                        new XElement("nombreSintetizador", "ScanSoft Isabel_Dri40_16kHz"), //isabel es el sintetizador por defecto
                        new XElement("velocidadSintetizador", "10"),
                        new XElement("colorLetra", "Amarillo"),
                        new XElement("colorFondo", "Negro"),
                        new XElement("swPorTiempo", "false"),
                        new XElement("cantSegundos", "10"),
                        new XElement("swMostrarLetrasEnMayúsculas", "true")
                        );

                    xml.Save(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Preferencias\" + AdminUsuarios.buscarIdUsuario(nombre + " " + apellido).ToString() + ".xml");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }               
        }

        public static bool eliminarUsuario(int idUsuario)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Usuarios\índiceUsuarios.xml");

                //string nombre, apellido, nombreCompleto;
                //nombreCompleto = buscarNombreUsuarioPorId(doc, idUsuario);
                //nombre = nombreCompleto.Substring(0, nombreCompleto.LastIndexOf(" "));
                //apellido = nombreCompleto.Substring(nombreCompleto.LastIndexOf(" ")+1, nombreCompleto.Length - nombreCompleto.LastIndexOf(" ")-1);

                bool swLoEncontré = false;
                XmlNodeList nodos = doc.GetElementsByTagName("usuario");
                foreach (XmlElement nodo in nodos)
                {
                    if (nodo.GetAttribute("id")==idUsuario.ToString())
                    {
                        doc.LastChild.RemoveChild(nodo);
                        swLoEncontré = true;
                        break;
                    }
                }

                if (!swLoEncontré)
                    return false;
                else
                {
                    doc.Save(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Usuarios\índiceUsuarios.xml"); //se guarda el índice sin el usuario

                    File.Delete(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Preferencias\" + idUsuario.ToString() + ".xml"); //se borran las pref del usuario
                    Directory.Delete(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsuario.ToString(), true); //se borran las estadísticas del usuario
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }    

        }

        public static bool eliminarUsuario(string nombre, string apellido)
        {
            return eliminarUsuario(buscarIdUsuario(nombre, apellido));
        }

        public static List<string> cargarListaUsuarios()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Usuarios\índiceUsuarios.xml");

            List<string> nombres = new List<string>();

            XmlNodeList nodos = doc.GetElementsByTagName("usuarios");
            XmlNodeList usuarios = ((XmlElement)nodos[0]).GetElementsByTagName("usuario");
            foreach (XmlElement usuario in usuarios)
            {
                string nombre=usuario.GetAttribute("nombre");
                string apellido = usuario.GetAttribute("apellido");
                nombres.Add(nombre + " " + apellido);
            }

            return nombres;
        }

        public static int buscarIdUsuario(string nombreCompleto)
        {
            string nombre = nombreCompleto.Substring(0, nombreCompleto.IndexOf(" ")).Trim();
            string apellido = nombreCompleto.Substring(nombreCompleto.IndexOf(" "), nombreCompleto.Length-nombre.Length).Trim();
            return buscarIdUsuario(nombre, apellido);
        }

        private static int últimoIdUsuario(XmlDocument doc)
        {
            int idUsuario = 0;
            if (doc.HasChildNodes && doc.LastChild.HasChildNodes)
            {
                XmlNode nodo = doc.LastChild.LastChild; //se elige el nodo adecuado
                XmlAttributeCollection atributos = nodo.Attributes;

                if (atributos.Count > 0) //si hay atributos
                {
                    XmlAttribute id = atributos[0];
                    idUsuario = Int32.Parse(id.Value);
                }
            }
            return idUsuario + 1; //le sumamos un nuevo id
        }

        private static int buscarIdUsuario(string nombre, string apellido) //se busca el id de un nombre y apellido
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Usuarios\índiceUsuarios.xml");

            int idUsuario=0;
            XmlNodeList nodos = doc.GetElementsByTagName("usuarios");
            XmlNodeList usuarios = ((XmlElement)nodos[0]).GetElementsByTagName("usuario");
            foreach (XmlElement usuario in usuarios)
            {
                if (usuario.GetAttribute("nombre") == nombre && usuario.GetAttribute("apellido") == apellido)
                {
                    idUsuario = Int32.Parse(usuario.GetAttribute("id"));
                    break;
                }
            }

            return idUsuario;
        }

        private static string buscarNombreUsuarioPorId(XmlDocument doc, int id) //se busca el id de un nombre y apellido
        {

            string nombre="", apellido="";
            XmlNodeList nodos = doc.GetElementsByTagName("usuarios");
            XmlNodeList usuarios = ((XmlElement)nodos[0]).GetElementsByTagName("usuario");
            foreach (XmlElement usuario in usuarios)
            {
                if (usuario.GetAttribute("id") == id.ToString())
                {
                    nombre = usuario.GetAttribute("nombre");
                    apellido = usuario.GetAttribute("apellido");
                    break;
                }
            }

            return nombre + " " + apellido;
        }


        
    }
}
