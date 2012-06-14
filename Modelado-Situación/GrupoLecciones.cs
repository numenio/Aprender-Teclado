using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Datos;

namespace LógicaAplicación
{
    public class GrupoLecciones
    {
        public List<Lección> lecciones = new List<Lección>();

        public GrupoLecciones(string ruta)
        {
            AdminDatos ad = new AdminDatos();
            DataSet dataLección = ad.leerLección(ruta);

            foreach (DataRow fila in dataLección.Tables["lección"].Rows)
            {
                Lección miLección = new Lección(fila);
                lecciones.Add(miLección);
            }
        }
    }
}
