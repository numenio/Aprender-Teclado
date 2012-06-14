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
using System.IO;
using LógicaAplicación;
using Voz;

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para Actividades.xaml
	/// </summary>
	public partial class Actividades : Window
	{
        int idUsr;
        List<lecciónIndice> lecciones;
        public bool swVolviendo {get; set;}

		public Actividades(int idUsuario)
		{
			this.InitializeComponent();
            idUsr = idUsuario;
            List<string> listaActividades = new List<string>(); //la lista de las actividades
            lecciones = AdminActividades.cargarListaActividades();

            foreach (lecciónIndice lecc in lecciones) //se cargan las activ
            {
                string cadena = lecc.Nombre + ", práctica de " + lecc.Tipo + ", nivel " + lecc.Nivel + ", autor " + lecc.Autor;
                listaActividades.Add(cadena);
                elementoDeLista elemento = new elementoDeLista(cadena);
                if (!AdminEstadísticas.ActividadTerminada(idUsr, lecc.IdLección, lecc.PathLección))
                {
                    elemento.completo.Visibility = Visibility.Hidden;
                }

                listActividades.Items.Add(elemento);
            }

            //listActividades.DataContext = listaActividades;//AdminActividades.cargarListaActividades();
            

            listActividades.Focus();
		}

        private void listActividades_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Return)
            {
                abrirActividad();
            }

            if (e.Key == Key.Escape)
            {
                voz.hablarAsync("No se puede retroceder más porque estás en el menú de inicio de Aprender Teclado. Elegí con flecha arriba o abajo qué actividad querés abrir y aceptá con enter");
            }
        }

        private void abrirActividad()
        {
            if (listActividades.SelectedItem != null)
            {
                Lecciones formLecciones = new Lecciones(lecciones[listActividades.SelectedIndex].PathLección, lecciones[listActividades.SelectedIndex].Tipo, idUsr, lecciones[listActividades.SelectedIndex].IdLección); //el índice del listBox es el mismo que el de la lista de lecciones
                formLecciones.swVolviendo = false;
                formLecciones.Show();
                this.Close();
            }
        }

        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)//si usa las flechas
            {
                if (listActividades.Items.Count != 0)
                {
                    if (listActividades.SelectedItem != null)
                    {
                        voz.hablarAsync(((elementoDeLista)listActividades.SelectedItem).cajaTexto.Text);//.ToString());
                        MediaPlayer reproductor = new MediaPlayer();
                        reproductor.Close();
                        string rutaInicial = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Sonidos\";
                        
                        if (AdminEstadísticas.ActividadTerminada(idUsr, lecciones[listActividades.SelectedIndex].IdLección, lecciones[listActividades.SelectedIndex].PathLección))
                        {
                            if (File.Exists(rutaInicial + "hecho.wav"))
                            {
                                reproductor.Open(new Uri(rutaInicial + "hecho.wav"));
                                reproductor.Play();
                            }
                        }
                        else
                        {
                            if (File.Exists(rutaInicial + "noHecha.wav"))
                            {
                                reproductor.Open(new Uri(rutaInicial + "noHecha.wav"));
                                reproductor.Play();
                            }
                        }
                    }
                }
            }

            if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)//si usa las flechas
            {
                voz.callar();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (swVolviendo)
                voz.hablarAsync("Volviendo a la lista de las actividades. Elegí con flecha abajo cuál querés abrir y aceptá con enter");
            else
                voz.hablarAsync("Abriendo la lista de actividades que tiene Aprender Teclado. Elegí con flecha abajo cuál querés abrir y aceptá con enter");
        }

        private void btnVerLecciones_Click(object sender, RoutedEventArgs e)
        {
            abrirActividad();
        }

        private void listActividades_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            abrirActividad();
        }
	}
}