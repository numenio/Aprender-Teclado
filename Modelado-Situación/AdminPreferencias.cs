using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace LógicaAplicación
{
    public class AdminPreferencias
    {

        private int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        //public nivelActividad nivel { get; set; }
        public string nombreLetra { get; set; }
        public int tamañoLetra { get; set; }
        public string colorLetra { get; set; }
        public string colorFondo { get; set; }
        public string nombreSintetizador { get; set; }
        public int velocidadSintetizador { get; set; }
        //public int últimaActividad { get; set; }
        //public int últimaLección { get; set; }
        public bool swPorTiempo { get; set; }
        public int cantSegundosPorTiempo { get; set; }
        public bool swMostrarLetrasEnMayúsculas { get; set; }
        
        public lugarAcento lugarDelAcento { get; set; }
        
        public AdminPreferencias(int númeroUsuario)
        {
            idUsuario = númeroUsuario;
            cargarPreferencias();
        }

        private void cargarPreferencias() //se carga desde el id del usuario que se carga en el constructor de la clase
        {
            this.lugarDelAcento = lugarAcento.derechaDeLaEñe;

            XmlDocument xml = new XmlDocument();
            if (File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Preferencias\" + this.idUsuario.ToString() + ".xml"))
                xml.Load(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Preferencias\" + this.idUsuario.ToString() + ".xml");
            else
                xml.Load(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Preferencias\Default.xml");

            XmlNodeList preferencias = xml.GetElementsByTagName("preferencias");

            XmlNodeList pref = ((XmlElement)preferencias[0]).GetElementsByTagName("nombreUsuario");
            this.nombreUsuario = pref[0].InnerText;

            pref = ((XmlElement)preferencias[0]).GetElementsByTagName("nombreLetra");
            this.nombreLetra = pref[0].InnerText;

            pref = ((XmlElement)preferencias[0]).GetElementsByTagName("tamañoLetra");
            this.tamañoLetra = Int32.Parse(pref[0].InnerText);

            pref = ((XmlElement)preferencias[0]).GetElementsByTagName("colorLetra");
            this.colorLetra= pref[0].InnerText;

            pref = ((XmlElement)preferencias[0]).GetElementsByTagName("colorFondo");
            this.colorFondo = pref[0].InnerText;

            //pref = ((XmlElement)preferencias[0]).GetElementsByTagName("nivel");
            //switch (pref[0].InnerText)
            //{
            //    case "principiante":
            //        this.nivel = nivelActividad.principiante;
            //        break;
            //    case "intermedio":
            //        this.nivel = nivelActividad.intermedio;
            //        break;
            //    case "experto":
            //        this.nivel = nivelActividad.experto;
            //        break;
            //}

            pref = ((XmlElement)preferencias[0]).GetElementsByTagName("nombreSintetizador");
            this.nombreSintetizador = pref[0].InnerText;

            pref = ((XmlElement)preferencias[0]).GetElementsByTagName("velocidadSintetizador");
            this.velocidadSintetizador = Int32.Parse(pref[0].InnerText);

            //pref = ((XmlElement)preferencias[0]).GetElementsByTagName("últimaActividad");
            //this.últimaActividad = Int32.Parse(pref[0].InnerText);

            //pref = ((XmlElement)preferencias[0]).GetElementsByTagName("últimaLección");
            //this.últimaLección = Int32.Parse(pref[0].InnerText);

            pref = ((XmlElement)preferencias[0]).GetElementsByTagName("swPorTiempo");
            this.swPorTiempo = Boolean.Parse(pref[0].InnerText);

            pref = ((XmlElement)preferencias[0]).GetElementsByTagName("swMostrarLetrasEnMayúsculas");
            this.swMostrarLetrasEnMayúsculas = Boolean.Parse(pref[0].InnerText);

            pref = ((XmlElement)preferencias[0]).GetElementsByTagName("cantSegundos");
            this.cantSegundosPorTiempo = Int32.Parse(pref[0].InnerText);
        }

        public bool guardarPreferencias() //se guardan con el id del usuario
        {
            try 
            {
                XElement xml = new XElement("preferencias",
                    new XElement("nombreUsuario", this.nombreUsuario),
                    //new XElement("nivel", this.nivel),
                    new XElement("tamañoLetra", this.tamañoLetra),
                    new XElement("nombreLetra", this.nombreLetra),
                    new XElement("nombreSintetizador", this.nombreSintetizador),
                    new XElement("velocidadSintetizador", this.velocidadSintetizador),
                    //new XElement("últimaActividad", this.últimaActividad),
                    //new XElement("últimaLección", this.últimaLección),
                    new XElement("colorLetra", this.colorLetra),
                    new XElement("colorFondo", this.colorFondo),
                    new XElement("swPorTiempo", this.swPorTiempo),
                    new XElement("cantSegundos", this.cantSegundosPorTiempo),
                    new XElement("swMostrarLetrasEnMayúsculas", "true")
                    );

                xml.Save(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Preferencias\" + this.idUsuario.ToString() + ".xml");

                return true;
            } 
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }
    }

    public enum lugarAcento
    {
        derechaDeLaEñe,
        arribaDeLaEñe
    }
}
