using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Datos;
//using System.IO;

namespace LógicaAplicación 
{
    public class IndiceLecciones
    {
        
        //private string rutaIndice = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\Español.xml"; 
        

        public List<lecciónIndice> leer()
        {
            string rutaIndice = @"\Recursos\Lecciones\índiceLecciones.xml";
            List<lecciónIndice> lecciones = new List<lecciónIndice>();
            AdminDatos ad = new AdminDatos();
            //List<lecciónIndice> lecciones = ad.leerÍndice(rutaIndice);
            DataSet dataLecciones = ad.leerÍndice(rutaIndice);

            foreach(DataRow fila in dataLecciones.Tables["lecciones"].Rows)
            {
                lecciónIndice miLección = new lecciónIndice();
                miLección.IdLección = Convert.ToInt32(fila["Id"]); //fila.ItemArray[0]);
                miLección.Nombre = fila["Nombre"].ToString();
                switch (fila["Dificultad"].ToString())// fila.ItemArray[1].ToString())
                {
                    case "Principiante":
                        miLección.Nivel = nivelActividad.principiante;
                        break;
                    case "Intermedio":
                        miLección.Nivel = nivelActividad.intermedio;
                        break;
                    default:
                        miLección.Nivel = nivelActividad.experto;
                        break;
                }

                switch (fila["Tipo"].ToString())// fila.ItemArray[1].ToString())
                {
                    case "Letras":
                        miLección.Tipo = tipoActividad.letras;
                        break;
                    case "Números":
                        miLección.Tipo = tipoActividad.números;
                        break;
                    case "Palabras":
                        miLección.Tipo = tipoActividad.palabras;
                        break;
                    case "Sílabas":
                        miLección.Tipo = tipoActividad.sílabas;
                        break;
                    default:
                        miLección.Tipo = tipoActividad.símbolos;
                        break;
                }
                miLección.PathLección = fila["Path"].ToString();// fila.ItemArray[2].ToString();
                miLección.Autor = fila["Autor"].ToString();// fila.ItemArray[3].ToString();
                lecciones.Add(miLección);
            }

            return lecciones;
        }

    }

    
    //clase de cada lección que está en el índice
    public class lecciónIndice
    {
        //privados
        int idLección;
        string nombre;
        tipoActividad tipo;
        nivelActividad nivelActividad;
        string pathLección;
        string autor;

        
        //públicos
        public int IdLección
        {
            get { return idLección; }
            set { idLección = value; }
        }


        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public tipoActividad Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public nivelActividad Nivel
        {
            get { return nivelActividad; }
            set { nivelActividad = value; }
        }
        public string PathLección
        {
            get { return pathLección; }
            set { pathLección = value; }
        }
        public string Autor
        {
            get { return autor; }
            set { autor = value; }
        }
    }
}
