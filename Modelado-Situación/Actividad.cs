using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Datos;

namespace LógicaAplicación
{
    public class Actividad
    {
        private GrupoLecciones GrupoLección;
        private Lección lecciónActual;
        private Lección lecciónAnterior;
        private string letraActualLección;

        int idLecciónActual;

        public int IdLecciónActual
        {
            get { return idLecciónActual; }
            set { idLecciónActual = value; }
        }

        int idLecciónAnterior;

        public int IdLecciónAnterior
        {
            get { return idLecciónAnterior; }
            set { idLecciónAnterior = value; }
        }

        bool swhayLecciónNueva;

        public bool swHayLecciónNueva
        {
            get { return swhayLecciónNueva; }
            set { swhayLecciónNueva = value; }
        }

        bool swhayPalabraNueva;

        public bool swHayPalabraNueva
        {
            get { return swhayPalabraNueva; }
            set { swhayPalabraNueva = value; }
        }

        public string LetraActualLección
        {
            get { return letraActualLección; }
            //set { letraActualLección = value; }
        }
        private string letraAnteriorLección;

        public string LetraAnteriorLección
        {
            get { return letraAnteriorLección; }
            //set { letraAnteriorLección = value; }
        }
        private string letraActualUsuario;

        public string LetraActualUsuario
        {
            get { return letraActualUsuario; }
            //set { letraActualUsuario = value; }
        }
        private string palabraActualLección;

        public string PalabraActualLección
        {
            get { return palabraActualLección; }
            //set { palabraActualLección = value; }
        }
        private string palabraAnteriorLección;

        public string PalabraAnteriorLección
        {
            get { return palabraAnteriorLección; }
            //set { palabraAnteriorLección = value; }
        }
        private string palabraActualUsuario;

        public string PalabraActualUsuario
        {
            get { return palabraActualUsuario; }
            //set { palabraActualUsuario = value; }
        }
        private string textoLecciónAúnNoEscrito;

        public string TextoLecciónAúnNoEscrito
        {
            get { return textoLecciónAúnNoEscrito; }
            //set { textoLecciónAúnNoEscrito = value; }
        }
        private string textoLecciónYaEscrito;

        public string TextoLecciónYaEscrito
        {
            get { return textoLecciónYaEscrito; }
            //set { textoLecciónYaEscrito = value; }
        }
        private int númeroLecciónActual;

        public int NúmeroLecciónActual
        {
            get { return númeroLecciónActual; }
            //set { númeroLecciónActual = value; }
        }

        //tipoActividad tipo;
        //public tipoActividad Tipo
        //{
        //    get { return tipo; }
        //    set { tipo = value; }
        //}

        private enum comparación {igual, distinta, esperarAcento}
        private enum cargar { cargada, noQuedanLetrasLección, noQuedanLecciones }

        public Actividad(GrupoLecciones quéLecciones, int númeroLección = 0) //número lección es por si se carga desde preferencias la lección en que se quedó el usuario
        {
            GrupoLección = quéLecciones;
            lecciónActual = GrupoLección.lecciones[númeroLección];
            actualizarInfo(númeroLección);
            cargarSiguientePalabraLección();
            cargarSiguienteLetraLección();
            
            //tipo = lecciónActual.tipo;
            //cargarLecciónSiguiente();
        }

        private void actualizarInfo(int númeroLección)
        {
            letraActualLección = "";
            //letraAnteriorLección = "";
            //letraActualUsuario = "";
            //palabraActualLección = "";
            //palabraAnteriorLección = "";
            textoLecciónAúnNoEscrito = lecciónActual.texto;
            textoLecciónYaEscrito = "";
            númeroLecciónActual = númeroLección;
            swhayPalabraNueva = true;
            idLecciónAnterior = idLecciónActual;
            idLecciónActual = lecciónActual.id;
        }

        private comparación chequearLetraUsuario(string entrada)
        {
            letraActualUsuario = entrada;
            //============  si la letra a escribir no tiene acentos ni diéresis =================
            if (!auxLetras.esLetraConAcento(letraActualLección)) 
            {
                if (letraActualLección == letraActualUsuario)
                    return comparación.igual;
                else
                    return comparación.distinta;
            }
            //============  si la letra a escribir tiene acento o diéresis ==================
            else 
	        {
                comparación resultado;

                if (letraAnteriorLección == "`" || letraAnteriorLección == "´" || letraAnteriorLección == "^" || letraAnteriorLección == "¨") //si la letra anterior fue un acento, y se está esperando por la vocal
                {
                    if (auxLetras.obtenerVocalSinAcento(letraActualLección) == letraActualUsuario) //si es la vocal esperada
                        resultado = comparación.igual;
                    else
                        resultado = comparación.distinta;
                }
                else //si se espera que escriba un acento
                {

                    if (auxLetras.chequearTipoAcento(letraActualLección) == auxLetras.chequearTipoAcento(letraActualUsuario))
                    {
                        letraAnteriorLección = auxLetras.obtenerAcentoDeVocal(letraActualLección); //se carga el acento como la letra anterior de la lección
                        resultado = comparación.esperarAcento;
                    }
                    else
                        resultado = comparación.distinta;
                }

                return resultado;
	        }
        }

        

        

        private comparación chequearPalabraUsuario(string entrada)
        {
            palabraActualUsuario = entrada;
            if (palabraActualLección == palabraActualUsuario)
                return comparación.igual;
            else
                return comparación.distinta;
        }

        private cargar cargarSiguienteLetraLección()
        {
            cargar result = cargar.cargada;

            if (textoLecciónAúnNoEscrito.Length == 0) //si ya no hay texto en la lección, se intenta cargar una siguiente
            {
                result = cargarLecciónSiguiente();
            }
            else
            {
                //if (textoLecciónAúnNoEscrito.Substring(0, 1) != " ") //si el primer carácter no es espacio
                //    textoLecciónAúnNoEscrito = " " + textoLecciónAúnNoEscrito;
                if (textoLecciónAúnNoEscrito != "")
                {
                    if (textoLecciónAúnNoEscrito.Substring(0, 1) == " ") //si hay más de un espacio
                    {
                        do //se le quitan todos los espacios
                        {
                            textoLecciónAúnNoEscrito = textoLecciónAúnNoEscrito.Substring(1, textoLecciónAúnNoEscrito.Length - 1);
                            if (textoLecciónAúnNoEscrito == "") break; //si ya no hay más texto, se sale del bucle
                        } while (textoLecciónAúnNoEscrito.Substring(0, 1) == " ");
   
                        textoLecciónAúnNoEscrito = " " + textoLecciónAúnNoEscrito; //se le agrega un espacio
                    }
                }
            }


            if (result != cargar.noQuedanLecciones && !this.swHayLecciónNueva) //si todavía hay lecciones en la actividad y no es que recién se está cargando la última letra de la actividad
            {
                letraAnteriorLección = letraActualLección;
                textoLecciónYaEscrito += letraAnteriorLección;
                letraActualLección = textoLecciónAúnNoEscrito.Substring(0, 1);
                textoLecciónAúnNoEscrito = textoLecciónAúnNoEscrito.Substring(1, textoLecciónAúnNoEscrito.Length - 1);
                if (letraAnteriorLección == " ")
                {
                    swHayPalabraNueva = true;
                    cargarSiguientePalabraLección();
                }
                else
                {
                    swHayPalabraNueva = false;
                }
            }

            return result;
        }

        private void cargarSiguientePalabraLección()
        {
            palabraAnteriorLección = palabraActualLección;
            int posiciónEspacio = textoLecciónAúnNoEscrito.IndexOf(" ");
            if (posiciónEspacio != -1)
                palabraActualLección = letraActualLección + textoLecciónAúnNoEscrito.Substring(0, posiciónEspacio);
            else
                palabraActualLección = letraActualLección + textoLecciónAúnNoEscrito.Substring(0, textoLecciónAúnNoEscrito.Length);

            palabraActualLección = palabraActualLección.TrimStart();

            palabraActualUsuario = "";
        }

        private cargar cargarLecciónSiguiente()
        {
            númeroLecciónActual++;
            if (númeroLecciónActual >= GrupoLección.lecciones.Count) //si ya no hay lecciones para cargar
            {
                númeroLecciónActual--;
                return cargar.noQuedanLecciones;
            }
            else //si aún hay lecciones
            {
                lecciónAnterior = lecciónActual;
                lecciónActual = GrupoLección.lecciones[númeroLecciónActual];
                actualizarInfo(númeroLecciónActual);
                letraActualLección = " ";
                this.swHayLecciónNueva = true;
                return cargar.cargada;
            }
        }

        public ResultadoActividad ingresarLetraUsuario(string letra)
        {
            ResultadoActividad result = new ResultadoActividad();
            comparación comparaciónLetraUsuario;
            cargar resultCarga = cargar.cargada;    
            if (swHayPalabraNueva == true) swHayPalabraNueva = false; //si la anterior fue palabra nueva, se resetea
            if (this.swHayLecciónNueva == true) this.swHayLecciónNueva = false; //si la anterior fue lección nueva, se resetea

            comparaciónLetraUsuario = chequearLetraUsuario(letra);

            if (comparaciónLetraUsuario == comparación.igual) //se chequea si la letra del usuario es correcta
            {
                palabraActualUsuario += letra;
                resultCarga = cargarSiguienteLetraLección();
                result.letraCorrecta = true;
            }
            else
            {
                if (auxLetras.esLetraConAcento(letraActualLección)) result.esperarPorAcento = true; //si antes se escribió el acento correctamente y se esperaba por la vocal correcta, pero el usuario apretó una errónea, no se le pide que escriba de nuevo el acento, sólo la vocal
                result.letraCorrecta = false;
            }

            if (comparaciónLetraUsuario == comparación.esperarAcento)
            {
                result.esperarPorAcento = true;
                result.letraCorrecta = true;
            }

            if (this.swHayLecciónNueva) //si hay una lección nueva dentro de la misma actividad
                result.nuevaLección = true;
            else
                result.nuevaLección = false;

            //si hay aún letras en la lección o lecciones en la actividad
            if (resultCarga == cargar.cargada) 
                result.continúaActividad = true;
            else
                result.continúaActividad = false;

            return result;
        }

        public List<int> cargarListaLecciones()
        {
            List<int> listaLecciones = new List<int>();
            foreach (Lección miLección in GrupoLección.lecciones)
            {
                listaLecciones.Add(miLección.id);
            }
            return listaLecciones;
        }
    }
}