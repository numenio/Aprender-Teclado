using System;
using System.Collections.Generic;
using System.Text;

namespace LógicaAplicación
{
    public class ResultadoActividad
    {
        public bool letraCorrecta { get; set; }
        public bool continúaActividad { get; set; }
        public bool tiempoAgotado { get; set; }
        public bool esperarPorAcento { get; set; }
        public bool nuevaLección { get; set; }
    }
}
