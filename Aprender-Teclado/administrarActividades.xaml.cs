using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using LógicaAplicación;
using System.IO;
using Voz;

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para administrarActividades.xaml
	/// </summary>
	public partial class administrarActividades : Window
	{
        List<lecciónIndice> lecciones = new List<lecciónIndice>();

		public administrarActividades()
		{
			this.InitializeComponent();
			this.linkModificarActividad.txtTextoAMostrar.Text = "Modificar Actividad";
			this.linkEliminarActividad.txtTextoAMostrar.Text="Eliminar Actividad";

            cargarActividades();
		}

        private void cargarActividades()
        {
            List<string> listaActividades = new List<string>(); //la lista de las actividades
            lecciones = AdminActividades.cargarListaActividades();

            foreach (lecciónIndice lecc in lecciones) //se cargan las activ
            {
                string cadena = lecc.Nombre + ", " + lecc.Tipo.ToString() + ", " + lecc.Nivel + ", " + lecc.Autor;
                listaActividades.Add(cadena);
            }

            listActividades.DataContext = listaActividades;
        }

        private void linkModificarActividad_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (listActividades.SelectedItem == null) //si no ha elegido actividad
                MessageBox.Show("No se ha elegido ninguna actividad para modificar, por favor elija una antes de hacer clic aquí", "Falta elegir");
            else //si eligió actividad para modificar
            {
                editorDeActividades edit = new editorDeActividades();
                edit.IDActAModificar = lecciones[listActividades.SelectedIndex].IdLección.ToString();
                edit.Show();
                this.Close();
            }
        }

        private void linkEliminarActividad_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (listActividades.SelectedItem == null) //si no ha elegido actividad
                MessageBox.Show("No se ha elegido ninguna actividad para eliminar, por favor elija una antes de hacer clic aquí", "Falta elegir");
            else //si eligió actividad para modificar
            {
                //se elimina la entrada del índice
                XDocument miXML = XDocument.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\índiceLecciones.xml"); //Cargar el archivo           

                var consul = from item in miXML.Elements("root").Elements("item")
                             where item.Attribute("ID").Value == lecciones[listActividades.SelectedIndex].IdLección.ToString()
                             select item;
                consul.ToList().ForEach(x => x.Remove()); //Remover elemento a elemento.
                miXML.Save(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\índiceLecciones.xml");

                File.Delete(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\Lecciones\" + lecciones[listActividades.SelectedIndex].IdLección.ToString() + ".xml");

                cargarActividades();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                voz.hablarAsync("Volviendo a la práctica, apretá efe uno para saber qué tenés que escribir");
                this.Close();
            }
        }
	}
}