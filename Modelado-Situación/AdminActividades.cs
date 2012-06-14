using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace LógicaAplicación
{
    public class AdminActividades
    {
        private IndiceLecciones índice = new IndiceLecciones();
        private List<lecciónIndice> listaActividades;
        //private List<lecciónIndice> listaActividadesFiltradas;
        private GrupoLecciones lecciones;
        private Actividad act;
        //private nivelActividad nivelActual;
        private Timer reloj;
        private bool actPorTiempo;
        private bool tiempoAgotado;
        private string letraActualLección;
        private string letraAnteriorLección;
        private tipoActividad tipoActividad;
        private string letraActualUsuario;
        private string palabraActualLección;
        private string palabraAnteriorLección;
        private string textoLecciónAúnNoEscrito;
        private string palabraActualUsuario;
        private string textoLecciónYaEscrito;
        private bool swHayPalabraNueva;
        private bool swHayLecciónNueva;
        private int idLecciónActual;
        private int idLecciónAnterior;

        public bool swhayLecciónNueva
        {
            get { return swHayLecciónNueva; }
            //set { swHayLecciónNueva = value; }
        }

        public int IdLecciónActual
        {
            get { return idLecciónActual; }
            //set { idLecciónActual = value; }
        }

        public int IdLecciónAnterior
        {
            get { return idLecciónAnterior; }
            //set { idLecciónActual = value; }
        }

        public bool swhayPalabraNueva
        {
            get { return swHayPalabraNueva; }
            set { swHayPalabraNueva = value; }
        }

        public tipoActividad TipoActividad
        {
            get { return tipoActividad; }
            //set { tipoActividad = value; }
        }

        public string LetraActualLección
        {
            get { return letraActualLección; }
            //set { letraActualLección = value; }
        }

        public string LetraAnteriorLección
        {
            get { return letraAnteriorLección; }
            //set { letraAnteriorLección = value; }
        }

        public string LetraActualUsuario
        {
            get { return letraActualUsuario; }
            //set { letraActualUsuario = value; }
        }

        public string PalabraActualLección
        {
            get { return palabraActualLección; }
            //set { palabraActualLección = value; }
        }

        public string PalabraAnteriorLección
        {
            get { return palabraAnteriorLección; }
            //set { palabraAnteriorLección = value; }
        }

        public string PalabraActualUsuario
        {
            get { return palabraActualUsuario; }
            //set { palabraActualUsuario = value; }
        }

        public string TextoLecciónAúnNoEscrito
        {
            get { return textoLecciónAúnNoEscrito; }
            //set { textoLecciónAúnNoEscrito = value; }
        }

        public string TextoLecciónYaEscrito
        {
            get { return textoLecciónYaEscrito; }
            //set { textoLecciónYaEscrito = value; }
        }
        //private int númeroLecciónActual;

        public AdminActividades(string rutaLecciones, tipoActividad tipoLección, int índiceLección, bool usarTiempo = false)//(nivelActividad nivel, bool usarTiempo = false, int índiceActividad = 0, int índiceLección = 0)
        {
            listaActividades = índice.leer();
            //listaActividadesFiltradas = filtrarActiviadesPorNivel(listaActividades, nivel);
            //lecciones = new GrupoLecciones(listaActividadesFiltradas[índiceActividad].PathLección);

            lecciones = new GrupoLecciones(rutaLecciones);//listaActividades[índiceActividad].PathLección);

            //nivelActual = nivel;
            act = new Actividad(lecciones, índiceLección);
            actPorTiempo = usarTiempo;

            //switch (tipoLección) //el tipo de las lecciones
            //{
            //    case "letras":
            //        tipoActividad = tipoActividad.letras;
            //        break;
            //    case "números":
            //        tipoActividad = tipoActividad.números;
            //        break;
            //    case "palabras":
            //        tipoActividad = tipoActividad.palabras;
            //        break;
            //    case "símbolos":
            //        tipoActividad = tipoActividad.símbolos;
            //        break;
            //    case "sílabas":
            //        tipoActividad = tipoActividad.sílabas;
            //        break;
            //}

            this.tipoActividad = tipoLección;

            actualizarInfoActividad();
        
            if (usarTiempo)
            {
                reloj = new Timer(); //corregir que cargue la preferencia de cuántos segundos pasar para que se considere palabra nula
                reloj.Interval = 3000;
                reloj.Enabled = true;
                reloj.Elapsed += new ElapsedEventHandler(reloj_Elapsed);
            }
        }

        void actualizarInfoActividad()
        {
            letraActualLección = act.LetraActualLección;
            letraAnteriorLección = act.LetraAnteriorLección;
            letraActualUsuario = act.LetraActualUsuario;
            palabraActualLección = act.PalabraActualLección;
            palabraAnteriorLección = act.PalabraAnteriorLección;
            palabraActualUsuario = act.PalabraActualUsuario;
            textoLecciónAúnNoEscrito = act.TextoLecciónAúnNoEscrito;
            textoLecciónYaEscrito = act.TextoLecciónAúnNoEscrito;
            swHayPalabraNueva = act.swHayPalabraNueva;
            swHayLecciónNueva = act.swHayLecciónNueva;
            idLecciónActual = act.IdLecciónActual;
            idLecciónAnterior = act.IdLecciónAnterior;
        }

        void reloj_Elapsed(object sender, ElapsedEventArgs e)
        {
            tiempoAgotado = true;
            //reloj.Stop();
        }

        private List<lecciónIndice> filtrarActiviadesPorNivel(List<lecciónIndice> actividades, nivelActividad nivel)
        {
            List<lecciónIndice> result = new List<lecciónIndice>();

            foreach (lecciónIndice act in actividades)
            {
                if (nivel == act.Nivel)
                    result.Add(act);
            }

            return result;
        }

        

        //private bool hayNivelSiguiente()
        //{
        //    if (nivelActual == nivelActividad.experto)
        //        return false;
        //    else
        //        return true;
        //}
        
        public ResultadoActividad ingresarLetraUsuario(string letra)
        {
            ResultadoActividad result = new ResultadoActividad();
            //if (actPorTiempo)
            //{
            //    tiempoAgotado = false;
            //    //reloj.Start(); //si es por tiempo se inicia el reloj
            //    reloj.Enabled = true;
            //}

            result = act.ingresarLetraUsuario(letra);
            
            if (actPorTiempo && tiempoAgotado) //si es por tiempo y se agotó el mismo
                result.tiempoAgotado=true;
            else
                result.tiempoAgotado=false;

            actualizarInfoActividad();
            //reloj.Stop();
            //reloj.Enabled = false;

            tiempoAgotado = false; //se reinicia la variable
            return result;
        }

        public List<int> cargarListaLecciones()
        {
            return act.cargarListaLecciones();
        }

        //public List<lecciónIndice> pasarNivelSiguiente()
        //{
        //    if (nivelActual == nivelActividad.principiante) nivelActual = nivelActividad.intermedio;
        //    if (nivelActual == nivelActividad.intermedio) nivelActual = nivelActividad.experto;
        //    return filtrarActiviadesPorNivel(listaActividades, nivelActual);
        //}

        //public static List<string> cargarListaActividades()
        public static List<lecciónIndice> cargarListaActividades()
        {
            //List<string> listaActividades = new List<string>();
            List<lecciónIndice> lista = new List<lecciónIndice>();
            IndiceLecciones índice = new IndiceLecciones();
            lista = índice.leer();

            //foreach (lecciónIndice lecc in lista)
            //{
            //    string cadena = lecc.Nivel + ", " + lecc.Nivel + ", " + lecc.Autor;
            //    listaActividades.Add(cadena);
            //}

            //return listaActividades;
            return lista;
        }

        public static List<string> cargarListaLecciones(string ruta)
        {
            List<string> listaLecciones = new List<string>();
            GrupoLecciones miGrupo = new GrupoLecciones(ruta);
            
            foreach (Lección lecc in miGrupo.lecciones)
            {
                //string cadena = lecc.nombre;
                listaLecciones.Add(lecc.id.ToString());
            }

            return listaLecciones;
        }

    }

    public enum nivelActividad {principiante, intermedio, experto}

    public enum tipoActividad { letras, sílabas, palabras, números, símbolos }
}
