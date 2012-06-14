using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Datos;

namespace LógicaAplicación
{
    public class Lección
    {
        public int id { get; set; }
        //public string nombre { get; set; }
        //public string sección { get; set; }
        public string texto { get; set; }
        //public tipoActividad tipo { get; set; }

        //el constructor recibe una fila con la lección
        public Lección (DataRow miFila)
        {
            this.id = Convert.ToInt32(miFila.ItemArray[0]);
            //this.nombre = miFila.ItemArray[1].ToString();
            //this.sección = miFila.ItemArray[2].ToString();
            this.texto = miFila.ItemArray[1].ToString();
            //switch (miFila.ItemArray[3].ToString())
            //{
            //    case "letras":
            //        this.tipo = tipoActividad.letras;
            //        break;
            //    case "números":
            //        this.tipo = tipoActividad.números;
            //        break;
            //    case "palabras":
            //        this.tipo = tipoActividad.palabras;
            //        break;
            //    case "símbolos":
            //        this.tipo = tipoActividad.símbolos;
            //        break;
            //    case "sílabas":
            //        this.tipo = tipoActividad.sílabas;
            //        break;
            //}
        }
    }
}
