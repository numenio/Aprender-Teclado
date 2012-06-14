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
using System.Diagnostics;
using System.IO;
using LógicaAplicación;
using Voz;

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para administrarUsuarios.xaml
	/// </summary>
	public partial class administrarUsuarios : Window
	{
		public administrarUsuarios()
		{
			this.InitializeComponent();
		}

        

        private void IniciarPrograma()
        {
            if (voz.listarVocesPorIdioma("Español").Count == 0) //si no hay voces en español instaladas
            {
                //si está el instalador de la voz de isabel en los recursos del programa, se lo instala
                if (File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\TTS\RealSpeakSpanishIsabel.exe"))
                {
                    MessageBox.Show("No hay instalada ninguna Voz en Español en su computadora. Se va a instalar una voz para corregir esto. Por favor siga las instrucciones del instalador que va a aparecer a continuación.", "No hay voces");
                    System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\TTS\RealSpeakSpanishIsabel.exe");

                }
                else //si algún gil borró el instalador
                {
                    MessageBox.Show("No hay instalada ninguna Voz en Español en su computadora, por favor instale una antes de usar Aprender Teclado", "No hay voces");
                }

                this.Close();
            }
            else
            {
                List<string> usuarios = AdminUsuarios.cargarListaUsuarios();
                if (usuarios.Count == 0) //si es el primer uso del programa
                {
                    NuevoUsuario nuevoUsr = new NuevoUsuario();
                    nuevoUsr.ShowDialog();
                }
                else //si ya hay usuarios en el sistema
                {
                    //listUsuarios.DataContext = usuarios;
                    cargarUsuarios(usuarios);
                    listUsuarios.Focus();
                    voz.cambiarVoz(voz.listarVocesPorIdioma("Español")[0]); //se carga la primer voz en español
                    voz.hablarAsync("Bienvenido o bienvenida a Aprender Teclado. En primer lugar buscá con flecha abajo tu nombre en la lista o apretá efe uno para ingresarte como un usuario nuevo");
                }
            }
        }

        private void cargarUsuarios(List<string> usuarios)
        {
            listUsuarios.Items.Clear();
            foreach (string usuario in usuarios) //se cargan los usuarios en la lista
            {
                elementoDeLista mielemento = new elementoDeLista(usuario);
                mielemento.completo.Visibility = Visibility.Hidden;
                mielemento.Width = listUsuarios.Width - 8;
                mielemento.cajaTexto.TextAlignment = TextAlignment.Center;
                mielemento.cajaTexto.Margin = new Thickness(27, 7.333, 26.667, -2.003);
                listUsuarios.Items.Add(mielemento);
            }
        }

        private void listUsuarios_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                abrirActividades();
            }
        }

        private void abrirActividades()
        {
            if (listUsuarios.SelectedItem != null)
            {
                AdminPreferencias adminPref = new AdminPreferencias(AdminUsuarios.buscarIdUsuario(((elementoDeLista)listUsuarios.SelectedItem).cajaTexto.Text));
                voz.hablarAsync("");
                voz.cambiarVoz(adminPref.nombreSintetizador);
                voz.cambiarVelocidad(adminPref.velocidadSintetizador);

                Actividades act = new Actividades(AdminUsuarios.buscarIdUsuario(((elementoDeLista)listUsuarios.SelectedItem).cajaTexto.Text));
                //act.idUsuario = AdminUsuarios.buscarIdUsuario(listUsuarios.SelectedItem.ToString());
                act.swVolviendo = false;
                act.Show();
                this.Close();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)//si usa las flechas
            {
                if (listUsuarios.Items.Count == 0)
                    voz.hablarAsync("No hay usuarios ingresados al programa. Para añadir tu nombre, apretá efe uno");
                else
                    voz.hablarAsync(((elementoDeLista)listUsuarios.SelectedItem).cajaTexto.Text);
            }

            if (e.Key == Key.F1)
            {
                NuevoUsuario nuevoUsr = new NuevoUsuario();
                nuevoUsr.ShowDialog();
                actualizarListaUsuarios();
            }

            if (e.Key == Key.Delete)
            {
                if (listUsuarios.SelectedItem != null) //si hay algún usuario seleccionado
                {
                    voz.hablarAsync("Usuario " + ((elementoDeLista)listUsuarios.SelectedItem).cajaTexto.Text + " eliminado exitosamente. Elegí con las flechas con qué usuario querés empezar y aceptá con enter. Para añadir un usuario nuevo apretá efe uno");
                    AdminUsuarios.eliminarUsuario(AdminUsuarios.buscarIdUsuario(((elementoDeLista)listUsuarios.SelectedItem).cajaTexto.Text));
                    actualizarListaUsuarios();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            abrirActividades();
        }

        private void listUsuarios_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            abrirActividades();
        }

        private void linkNuevoUsuario_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NuevoUsuario nuevoUsr = new NuevoUsuario();
            nuevoUsr.ShowDialog();
        }

        private void actualizarListaUsuarios()
        {
            //listUsuarios.DataContext = AdminUsuarios.cargarListaUsuarios();
            cargarUsuarios(AdminUsuarios.cargarListaUsuarios());
            listUsuarios.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            linkNuevoUsuario.txtTextoAMostrar.Text = "Ingresar un nuevo usuario";
            IniciarPrograma();
            SplashScreen sp = new SplashScreen("inicio.png");
            sp.Show(false);
            sp.Close(new TimeSpan(0, 0, 6));
        }

	}
}