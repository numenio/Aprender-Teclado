using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace LógicaAplicación
{
    public struct repeticiónCarácter
    {
        public char carácter;
        public int repetición;
    }

    public class AdminEstadísticas
    {
        //public List<estadísticaLección> estadísticasLecciones = new List<estadísticaLección>();
        public estadísticaLección estadística = new estadísticaLección();
        

        public AdminEstadísticas(int idUsr, int idAct, int idLecc)
        {
            estadística.idUsuario = idUsr;
            estadística.idActividad = idAct;
            estadística.idLección = idLecc;
            estadística.fechaHora = DateTime.Now;
        }

        public AdminEstadísticas() { }

        private void cerrarEstadística()
        {
            estadística.aciertos = this.calcularTotalAciertos();
            estadística.errores = this.calcularTotalErrores();
            Hashtable repeticiónErrores = contarRepeticionesErrores(estadística.error);
            Hashtable repeticiónErrores2 = contarRepeticionesErrores(estadística.error);
            Hashtable repeticiónErrores3 = contarRepeticionesErrores(estadística.error);
            estadística.caracterConMásErrores = this.buscarCarácterConMásErrores(repeticiónErrores);
            estadística.caracterSegundoConMásErrores = this.buscarSegundoCarácterConMásErrores(repeticiónErrores2);
            estadística.caracterTerceroConMásErrores = this.buscarTercerCarácterConMásErrores(repeticiónErrores3);
            estadística.caracteresPorMinuto = this.calcularCaracteresPorMinuto();
            estadística.duración = this.calcularDuraciónEnMinutos();
            estadística.porcentajeAciertos = this.calcularPorcentajeAciertos();
            estadística.porcentajeErrores = this.calcularPorcentajeErrores();
            //estadística.swlecciónTerminada
            //estadística.idEstadística = corregir que cargue la última estadística disponible
        }

        public bool guardarEstadísticas(int idUsr, int idAct, int idLección) //idAct e idLección son para la ruta del guardado
        {
            try
            {
                XElement xml = new XElement("estadísticas",
                    //new XElement("idEstadística", estadística.idEstadística),
                    new XElement("idUsuario", estadística.idUsuario),
                    new XElement("idActividad", estadística.idActividad),
                    new XElement("idLección", estadística.idLección),
                    new XElement("swLecciónTerminada", estadística.swlecciónTerminada),
                    new XElement("fecha", estadística.fechaHora.ToShortDateString()),
                    new XElement("hora", estadística.fechaHora.ToShortTimeString()),
                    //new XElement("palabrasPorMinuto", estadística.palabrasPorMinuto),
                    new XElement("caracteresPorMinuto", estadística.caracteresPorMinuto),
                    new XElement("aciertos", estadística.aciertos),
                    new XElement("porcentajeAciertos", estadística.porcentajeAciertos),
                    new XElement("duración", estadística.duración),
                    new XElement("errores", estadística.errores),
                    new XElement("porcentajeErrores", estadística.porcentajeErrores),
                    new XElement("caracterConMásErrores_Letra", estadística.caracterConMásErrores.carácter),
                    new XElement("caracterConMásErrores_Repetición", estadística.caracterConMásErrores.repetición),
                    new XElement("carácterSegundoConMásErrores_Letra", estadística.caracterSegundoConMásErrores.carácter),
                    new XElement("carácterSegundoConMásErrores_Repetición", estadística.caracterSegundoConMásErrores.repetición),
                    new XElement("caracterTerceroConMásErrores_Letra", estadística.caracterTerceroConMásErrores.carácter),
                    new XElement("caracterTerceroConMásErrores_Repetición", estadística.caracterTerceroConMásErrores.repetición)
                    );

                string rutaInicial = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsr.ToString() + "\\" + idAct.ToString() + "\\" + idLección.ToString() + "\\";

                if (!Directory.Exists(rutaInicial)) //si no existe todavía el directorio
                {
                    //si no está la carpeta del usuario
                    if (!Directory.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsr.ToString()))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsr.ToString());
                    }

                    //si no está la carpeta de la actividad
                    if (!Directory.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsr.ToString() + "\\" + idAct.ToString()))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsr.ToString() + "\\" + idAct.ToString());
                    }

                    //si no está la carpeta de la lección dentro de la de la actividad
                    if (!Directory.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsr.ToString() + "\\" + idAct.ToString() + "\\" + idLección.ToString()))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsr.ToString() + "\\" + idAct.ToString() + "\\" + idLección.ToString());
                    }
                }

                string nombreArchivo = obtenerÚltimoNúmeroEstadísticaEnCarpeta(rutaInicial).ToString();
                xml.Save(rutaInicial + nombreArchivo + ".xml");

                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }

        int obtenerÚltimoNúmeroEstadísticaEnCarpeta(string ruta)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(ruta);
            System.IO.FileSystemInfo[] files = di.GetFileSystemInfos();

            int max = 0;

            //se busca el archivo con el nombre de mayor valor
            foreach(FileSystemInfo archivo in files)
            {
                int aux;
                int.TryParse(archivo.Name.Substring(0, archivo.Name.Length - archivo.Extension.Length), out aux);
                if (aux > max) max = aux;
            }

            return ++max; //se devuelve el mayor valor de los existentes sumándole uno
        }

        //---------------------------------
        //corregir: hacer cargarListaEstadísticas y cargarÚltimaEstadística para poder guardar con la última disponible
        //---------------------------------
        public static List<string> cargarListaIntentos(int idActividad, int idLección, int idUsuario)
        {
            List<string> intentos = new List<string>();
            string rutaInicial = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsuario.ToString() + "\\" + idActividad.ToString() + "\\";

            //se cargan los archivos que están en la lección dada
            if (Directory.Exists(rutaInicial + idLección.ToString()))
            {
                DirectoryInfo carpeta = new DirectoryInfo(rutaInicial + idLección.ToString());
                FileInfo[] archivos = carpeta.GetFiles();
                foreach (FileInfo archivo in archivos)
                {
                    intentos.Add(archivo.Name.Substring(0, archivo.Name.Length - ".xml".Length));
                }
            }

            return intentos;
        }

        //public static List<string> cargarListaIntentos(int idActividad, int idLección, int idUsuario)
        //{
        //    List<string> intentos = new List<string>();
        //    intentos = AdminEstadísticas.cargarListaIntentos(idActividad, idLección);

        //}


        public static bool ActividadTerminada(int idUsr, int idActividad, string pathAct)
        {
            List<int> idLecciones = new List<int>();

            GrupoLecciones lecciones = new GrupoLecciones(pathAct);
            foreach (Lección miLección in lecciones.lecciones)
                idLecciones.Add(miLección.id);

            return AdminEstadísticas.ActividadTerminada(idUsr, idActividad, idLecciones); //se lo pasa al otro método y se devuelve su resultado
        }


        public static bool ActividadTerminada(int idUsr, int idActividad, List<int> listaLecciones)
        {
            string rutaInicial =  Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\";

            //si no existe el directorio del usuario, es que aún no ha hecho nada
            if (!Directory.Exists(rutaInicial + idUsr.ToString()))
                return false;

            //si no existe la actividad es que aún no se la hizo, no está terminada
            if (!Directory.Exists(rutaInicial + idUsr.ToString() + "\\" + idActividad.ToString()))
                return false;
            
            //si no tienen su carpeta todas las lecc de la act, la act no está terminada
            foreach (int idLecc in listaLecciones)
                if (!LecciónTerminada(idUsr, idActividad, idLecc)) return false;

            return true; //si pasa todos los controles, está terminada
        }


        public static bool LecciónTerminada(int idUsr, int idActividad, int idLección)
        {
            string rutaInicial =  Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsr.ToString() + "\\" + idActividad.ToString() + "\\";

            //si no existe la lección
            if (Directory.Exists(rutaInicial + idLección.ToString()))
                return true;
            else
                return false;
        }


        
        
        public void cargarEstadística(int idUsuario, int idActividad, int idLección, int idEstadística) //se carga desde el id de la estadística
        {
            //XmlDocument xml = new XmlDocument();
            if (File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsuario.ToString() + "\\" + idActividad.ToString() + "\\" + idLección.ToString() + "\\" + idEstadística.ToString() + ".xml"))
            {
                XDocument xml = XDocument.Load(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Estadísticas\" + idUsuario.ToString() + "\\" + idActividad.ToString() + "\\" + idLección.ToString() + "\\" + idEstadística.ToString() + ".xml");

                //XElement elemento = xml.Element("fecha");
                string fecha = xml.Element("estadísticas").Element("fecha").Value;// elemento.Value;

                //elemento = xml.Element("hora");
                string hora = xml.Element("estadísticas").Element("hora").Value;
                int año, mes, día, hour, minutos;
                año = int.Parse(fecha.Substring(fecha.Length - 4));
                mes = int.Parse(fecha.Substring(3, 2));
                día = int.Parse(fecha.Substring(0, 2));
                hour = int.Parse(hora.Substring(0, 2));
                minutos = int.Parse(hora.Substring(3, hora.Length - 3));
                estadística.fechaHora = new DateTime(año, mes, día, hour, minutos, 0); //corregir que cargue la fecha y la hora desde los string

                //elemento = xml.Element("caracteresPorMinuto");
                estadística.caracteresPorMinuto = int.Parse(xml.Element("estadísticas").Element("caracteresPorMinuto").Value);

                //elemento = xml.Element("aciertos");
                estadística.aciertos = int.Parse(xml.Element("estadísticas").Element("aciertos").Value);

                //elemento = xml.Element("porcentajeAciertos");
                estadística.porcentajeAciertos = int.Parse(xml.Element("estadísticas").Element("porcentajeAciertos").Value);

                //elemento = xml.Element("duración");
                estadística.duración = int.Parse(xml.Element("estadísticas").Element("duración").Value);

                //elemento = xml.Element("errores");
                estadística.errores = int.Parse(xml.Element("estadísticas").Element("errores").Value);

                //elemento = xml.Element("porcentajeErrores");
                estadística.porcentajeErrores = int.Parse(xml.Element("estadísticas").Element("porcentajeErrores").Value);

                //elemento = xml.Element("caracterConMásErrores_Letra");
                repeticiónCarácter miCarácter = new repeticiónCarácter();
                miCarácter.carácter = xml.Element("estadísticas").Element("caracterConMásErrores_Letra").Value.ToCharArray()[0];

                //elemento = xml.Element("caracterConMásErrores_Repetición");
                miCarácter.repetición = int.Parse(xml.Element("estadísticas").Element("caracterConMásErrores_Repetición").Value);

                estadística.caracterConMásErrores = miCarácter; //primero con más errores

                //elemento = xml.Element("carácterSegundoConMásErrores_Letra");
                miCarácter.carácter = xml.Element("estadísticas").Element("carácterSegundoConMásErrores_Letra").Value.ToCharArray()[0];

                //elemento = xml.Element("carácterSegundoConMásErrores_Repetición");
                miCarácter.repetición = int.Parse(xml.Element("estadísticas").Element("carácterSegundoConMásErrores_Repetición").Value);

                estadística.caracterSegundoConMásErrores = miCarácter; //segundo con más errores

                //elemento = xml.Element("caracterTerceroConMásErrores_Letra");
                miCarácter.carácter = xml.Element("estadísticas").Element("caracterTerceroConMásErrores_Letra").Value.ToCharArray()[0];

                //elemento = xml.Element("caracterTerceroConMásErrores_Repetición");
                miCarácter.repetición = int.Parse(xml.Element("estadísticas").Element("caracterTerceroConMásErrores_Repetición").Value);

                estadística.caracterTerceroConMásErrores = miCarácter; //tercero con más errores
            }
            
        //    XmlNodeList preferencias = xml.GetElementsByTagName("preferencias");

        //    XmlNodeList pref = ((XmlElement)preferencias[0]).GetElementsByTagName("nombreUsuario");
        //    this.nombreUsuario = pref[0].InnerText;

        //    pref = ((XmlElement)preferencias[0]).GetElementsByTagName("tamañoLetra");
        //    this.tamañoLetra = Int32.Parse(pref[0].InnerText);

        //    pref = ((XmlElement)preferencias[0]).GetElementsByTagName("nivel");
        //    switch (pref[0].InnerText)
        //    {
        //        case "principiante":
        //            this.nivel = nivelActividad.principiante;
        //            break;
        //        case "intermedio":
        //            this.nivel = nivelActividad.intermedio;
        //            break;
        //        case "experto":
        //            this.nivel = nivelActividad.experto;
        //            break;
        //    }

        //    pref = ((XmlElement)preferencias[0]).GetElementsByTagName("nombreSintetizador");
        //    this.nombreSintetizador = pref[0].InnerText;


        //    pref = ((XmlElement)preferencias[0]).GetElementsByTagName("velocidadSintetizador");
        //    this.velocidadSintetizador = Int32.Parse(pref[0].InnerText);

        //    pref = ((XmlElement)preferencias[0]).GetElementsByTagName("últimaActividad");
        //    this.últimaActividad = Int32.Parse(pref[0].InnerText);

        //    pref = ((XmlElement)preferencias[0]).GetElementsByTagName("últimaLección");
        //    this.últimaLección = Int32.Parse(pref[0].InnerText);

        //    pref = ((XmlElement)preferencias[0]).GetElementsByTagName("swPorTiempo");
        //    this.swPorTiempo = Boolean.Parse(pref[0].InnerText);

        //    pref = ((XmlElement)preferencias[0]).GetElementsByTagName("cantSegundos");
        //    this.cantSegundosPorTiempo = Int32.Parse(pref[0].InnerText);
        }

        private int calcularPorcentajeErrores()
        {
            int result;
            try
            {
                //lo siguiente es más exacto, pero como redondea a integer, no da bien al cuenta al sumar los porcentajes de errores y aciertos
                //result = estadística.error.Count * 100 / calcularTotalCaracteres();
                result = 100 - estadística.porcentajeAciertos;
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        private int calcularPorcentajeAciertos()
        {
            int result;
            try
            {
                result = estadística.acierto.Count * 100 / calcularTotalCaracteres();
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        private int calcularCaracteresPorMinuto()
        {
            try
            {
                return calcularTotalCaracteres() / calcularDuraciónEnMinutos();
            }
            catch
            {
                return 0;
            }
        }

        private int calcularDuraciónEnMinutos()
        {
            int result;

            try
            {
                //result = (int)estadística.fechaHora.Subtract(DateTime.Now).TotalMinutes;
                result = (int)DateTime.Now.Subtract(estadística.fechaHora).TotalMinutes;
                if (result == 0) result = 1;
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        private int calcularTotalAciertos()
        {
            return estadística.acierto.Count;
        }

        private int calcularTotalErrores()
        {
            return estadística.error.Count;
        }

        private int calcularTotalCaracteres()
        {
            return estadística.error.Count + estadística.acierto.Count;
        }

        public bool cargarLetraAcierto(char letra)
        {
            estadística.acierto.Add(letra);
            cerrarEstadística();
            return true;
        }

        public bool cargarLetraError(char letra)
        {
            estadística.error.Add(letra);
            cerrarEstadística();
            return true;
        }

        private repeticiónCarácter buscarCarácterConMásErrores(Hashtable listaErrores)
        {
            repeticiónCarácter result = new repeticiónCarácter();
            result.carácter = '-';
            result.repetición = 0;

            foreach (DictionaryEntry de in listaErrores)
            {
                string cadenaValor = (string)de.Value;
                if (int.Parse(cadenaValor) > result.repetición)//(int)de.Value > result.repetición)
                {
                    result.repetición = int.Parse(cadenaValor);// (int)de.Value;
                    result.carácter = (char)de.Key;
                }
            }

            return result;
        }

        private repeticiónCarácter buscarSegundoCarácterConMásErrores(Hashtable listaErrores)
        {
            //if (this.estadística.caracterConMásErrores.carácter != null)
            //    listaErrores.Remove(estadística.caracterConMásErrores.carácter); //así no se procesa dos veces lo mismo
            //else
                listaErrores.Remove(buscarCarácterConMásErrores(listaErrores).carácter); //se quita el primer carácter
            
            return buscarCarácterConMásErrores(listaErrores); //se devuelve mayor valor de los que quedan
        }

        private repeticiónCarácter buscarTercerCarácterConMásErrores(Hashtable listaErrores)
        {
            //if (this.estadística.caracterConMásErrores.carácter != null)
            //    listaErrores.Remove(estadística.caracterConMásErrores.carácter); //así no se procesa dos veces lo mismo
            //else
                listaErrores.Remove(buscarCarácterConMásErrores(listaErrores).carácter); //se quita el primer carácter
            return buscarSegundoCarácterConMásErrores(listaErrores); //en buscar segundo carácter se quita nuevamente el primer valor (el que tiene mayor repeticiones, que sería el segundo), luego se devuelve mayor valor de los que quedan (que sería el tercero)
        }

        private Hashtable contarRepeticionesErrores(List<char> listaErrores)
        {
            Hashtable misCaracteres = new Hashtable();
            foreach (char miCarácter in listaErrores)
            {
                if (!misCaracteres.ContainsKey(miCarácter)) //si no contiene el carácter, se lo añade
                {
                    misCaracteres.Add(miCarácter, "1");
                }
                else //si la letra ya está, se añade un valor 
                {
                    string cadenaValor = (string)misCaracteres[miCarácter];
                    int valor = int.Parse(cadenaValor); //(int)misCaracteres[miCarácter];
                    valor++;
                    misCaracteres[miCarácter] = valor.ToString();
                }
            }

            return misCaracteres;
        }

        //private Hashtable ordenarTabla(Hashtable miTabla)
        //{
        //    Hashtable misCaracteresOrdenados = new Hashtable();
        //    char carácter = ' ';
        //    int maxValor = 0;

        //    foreach (DictionaryEntry de in miTabla)
        //    {
        //        if ((int)de.Value > maxValor)
        //        {
        //            maxValor = (int)de.Value;
        //            carácter = (char)de.Key;
        //        }
        //    }

        //    return misCaracteresOrdenados;
        //}

        //private List<char> filtrarRepeticiones(List<char> lista)
        //{
        //    List<char> result = new List<char>();

        //    foreach (char miCarácter in lista)
        //    {
        //        if (!result.Contains(miCarácter)) result.Add(miCarácter); //solamente se añade el carácter si no estaba
        //    }
        //    return result;
        //}
    }

    public class estadísticaLección
    {
        public int idEstadística { get; set; }
        public int idUsuario {get; set;}
        public int idActividad {get; set;}
        public int idLección {get; set;}
        public bool swlecciónTerminada {get; set;}
        public DateTime fechaHora {get; set;}
        //public int palabrasPorMinuto {get; set;}
        public int caracteresPorMinuto {get; set;}
        public int aciertos {get; set;}
        public int porcentajeAciertos {get; set;}
        public int duración {get; set;}
        public int errores {get; set;}
        public int porcentajeErrores {get; set;}
        public repeticiónCarácter caracterConMásErrores {get; set;}
        public repeticiónCarácter caracterSegundoConMásErrores { get; set; }
        public repeticiónCarácter caracterTerceroConMásErrores { get; set; }

        public List<char> error = new List<char>();
        public List<char> acierto = new List<char>();

        public estadísticaLección()
        {
              idEstadística = 0;
              idUsuario  = 0;
              idActividad  = 0;
              idLección  = 0;
              swlecciónTerminada = false;
              caracteresPorMinuto  = 0;
              aciertos  = 0;
              porcentajeAciertos  = 0;
              duración  = 0;
              errores  = 0;
              porcentajeErrores  = 0;
              repeticiónCarácter caracterAux = new repeticiónCarácter();
              caracterAux.carácter = '-';
              caracterAux.repetición = 0;
              caracterConMásErrores = caracterAux;
              caracterConMásErrores = caracterAux;
              caracterSegundoConMásErrores = caracterAux;
              caracterTerceroConMásErrores = caracterAux;
        }

    }
}
