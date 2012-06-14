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
using L�gicaAplicaci�n;
using Voz;

namespace Aprender_Teclado
{
    /// <summary>
    /// L�gica de interacci�n para Actividades.xaml
    /// </summary>
    public partial class Lecciones : Window
    {
        int idUsr;
        int idActividad;
        tipoActividad tipoAct;
        string pathLecci�n;
        public bool swVolviendo { get; set; }
        
        

        public Lecciones(string PathLecci�n, tipoActividad tipo, int idUsuario, int idAct)
        {
            this.InitializeComponent();
            idUsr = idUsuario;
            idActividad = idAct;
            tipoAct = tipo;
            pathLecci�n = PathLecci�n;
            List<string> listaLecciones = new List<string>(); //la lista de las lecciones
            listaLecciones = AdminActividades.cargarListaLecciones(PathLecci�n);
            //listLecciones.DataContext = listaLecciones;
            cargarLecciones(listaLecciones);

            listLecciones.Focus();
        }

        private void listLecciones_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return)
            {
                abrirLecci�n();
            }

            if (e.Key == Key.Escape)
            {
                Actividades formAct = new Actividades(idUsr);
                formAct.swVolviendo = true;
                formAct.Show();
                this.Close();
            }
        }

        private void abrirLecci�n()
        {
            if (listLecciones.SelectedItem != null)
            {
                MainWindow formPpal = new MainWindow(idUsr, pathLecci�n, tipoAct, idActividad, listLecciones.SelectedIndex);
                formPpal.Show();
                this.Close();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)//si usa las flechas
            {
                if (listLecciones.Items.Count != 0)
                {
                    if (listLecciones.SelectedItem != null)
                    {
                        voz.hablarAsync("Lecci�n " + ((elementoDeLista)listLecciones.SelectedItem).cajaTexto.Text);

                        MediaPlayer reproductor = new MediaPlayer();
                        reproductor.Close();
                        string rutaInicial = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Sonidos\";

                        if (AdminEstad�sticas.Lecci�nTerminada(idUsr, idActividad, int.Parse(((elementoDeLista)listLecciones.SelectedItem).cajaTexto.Text)))
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
                voz.hablarAsync("Volviendo a la lista de lecciones. Eleg� con flecha abajo cu�l quer�s abrir y acept� con enter");
            else
                voz.hablarAsync("Abriendo la lista de lecciones de la actividad para practicar " + tipoAct + " . Eleg� con flecha abajo cu�l quer�s abrir y acept� con enter");
        }

        private void btnEmpezar_Click(object sender, RoutedEventArgs e)
        {
            abrirLecci�n();
        }

        private void listLecciones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            abrirLecci�n();
        }

        private void cargarLecciones(List<string> lecciones)
        {
            listLecciones.Items.Clear();
            foreach (string lecci�n in lecciones) //se cargan los usuarios en la lista
            {
                elementoDeLista mielemento = new elementoDeLista(lecci�n);
                mielemento.Width = listLecciones.Width - 20;
                mielemento.cajaTexto.TextAlignment = TextAlignment.Center;

                if (!AdminEstad�sticas.Lecci�nTerminada(idUsr, idActividad, int.Parse(lecci�n)))
                    mielemento.completo.Visibility = Visibility.Hidden;
                //mielemento.cajaTexto.Margin = new Thickness(27, 7.333, 26.667, -2.003);
                listLecciones.Items.Add(mielemento);
            }
        }
    }
}