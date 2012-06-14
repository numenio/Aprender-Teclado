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
using Voz;
using LógicaAplicación;

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para VerEstadísticas.xaml
	/// </summary>
	public partial class VerEstadísticas : Window
	{
        int idUsuario=0;
        bool swPrimerUso = true;
        List<lecciónIndice> lecciones;
        enum gridActivo { usuarios, actividades, lecciones, intentos }

		public VerEstadísticas()
		{
			this.InitializeComponent();
			
            //se cargan los usuarios
			List<string> usuarios = AdminUsuarios.cargarListaUsuarios();
            if (usuarios.Count != 0)
                listUsuarios.DataContext = usuarios;
            

            //se cargan las actividades
            List<string> listaActividades = new List<string>(); //la lista de las actividades
            lecciones = AdminActividades.cargarListaActividades();

            foreach (lecciónIndice lecc in lecciones) //se cargan las activ
            {
                string cadena = lecc.Nombre + ", práctica de " + lecc.Tipo + ", nivel " + lecc.Nivel + ", autor " + lecc.Autor;
                listaActividades.Add(cadena);
            }

            listActividades.DataContext = listaActividades;

            listUsuarios.IsEnabled = false;
            listActividades.IsEnabled = false;
            listLecciones.IsEnabled = false;
            listRealiaciones.IsEnabled = false;

            mostrarAyuda(gridActivo.usuarios);

            listUsuarios.Focus();

            voz.hablarAsync("Abriendo las estadísticas del programa. Como esta parte es para docentes, se necesita un lector de pantallas para usarla, tal como yaus o ene ve de a. Para salir pulse escape");
		}

        private void listActividades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //se cargan las lecciones de la actividad seleccionada
            List<string> misLecciones = new List<string>(); //la lista de las lecciones
            misLecciones = AdminActividades.cargarListaLecciones(lecciones[listActividades.SelectedIndex].PathLección);
            listLecciones.DataContext = misLecciones;
            if (swPrimerUso) 
                mostrarAyuda(gridActivo.lecciones);
        }

        private void mostrarAyuda(gridActivo quéAyuda)
        {
            esconderMsjAyuda();

            switch (quéAyuda)
            {
                case gridActivo.usuarios:
                    gridMsjUsuario.Visibility = Visibility.Visible;
                    listUsuarios.IsEnabled = true;
                    break;
                case gridActivo.actividades:
                    gridMsjActividad.Visibility = Visibility.Visible;
                    listActividades.IsEnabled = true;
                    break;
                case gridActivo.lecciones:
                    gridMsjLección.Visibility = Visibility.Visible;
                    listLecciones.IsEnabled = true;
                    break;
                case gridActivo.intentos:
                    gridMsjIntentos.Visibility = Visibility.Visible;
                    listRealiaciones.IsEnabled = true;
                    break;
            }
            
        }

        private void esconderMsjAyuda()
        {
            gridMsjUsuario.Visibility = Visibility.Hidden;
            gridMsjActividad.Visibility = Visibility.Hidden;
            gridMsjLección.Visibility = Visibility.Hidden;
            gridMsjIntentos.Visibility = Visibility.Hidden;
        }

        private void listUsuarios_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            usuarios();
        }

        private void usuarios()
        {
            if (listUsuarios.SelectedItem != null)
            {
                idUsuario = AdminUsuarios.buscarIdUsuario(listUsuarios.SelectedItem.ToString());
                if (swPrimerUso)
                    mostrarAyuda(gridActivo.actividades);
            }
            resetearInfoEstadísticas();
        }

        private void listLecciones_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            clicLecciones();
        }

        private void clicLecciones()
        {
            resetearInfoEstadísticas();
            if (listActividades.SelectedItem != null && listLecciones.SelectedItem != null && listUsuarios.SelectedItem != null)
            {
                List<string> intentos = AdminEstadísticas.cargarListaIntentos(lecciones[listActividades.SelectedIndex].IdLección, int.Parse(listLecciones.SelectedItem.ToString()), idUsuario);
                listRealiaciones.DataContext = intentos;
                if (intentos.Count == 0) //si no hay algún intento
                    intentos.Add("Sin prácticas");

                if (swPrimerUso)
                    mostrarAyuda(gridActivo.intentos);
            }
        }

        private void listActividades_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            actividades();
        }

        private void actividades()
        {
            //se cargan las lecciones de la actividad seleccionada
            List<string> misLecciones = new List<string>(); //la lista de las lecciones
            if (listActividades.SelectedItem != null)
            {
                misLecciones = AdminActividades.cargarListaLecciones(lecciones[listActividades.SelectedIndex].PathLección);
                listLecciones.DataContext = misLecciones;
                if (swPrimerUso)
                    mostrarAyuda(gridActivo.lecciones);
            }
            resetearInfoEstadísticas();
        }

        private void listRealiaciones_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            realizaciones();
            
        }

        private void realizaciones()
        {
            swPrimerUso = false;
            esconderMsjAyuda();
            AdminEstadísticas adminEst = new AdminEstadísticas();
            if (listActividades.SelectedItem != null && listLecciones.SelectedItem != null && listUsuarios.SelectedItem != null && listRealiaciones.SelectedItem != null && listRealiaciones.SelectedItem.ToString() != "Sin prácticas")
            {
                adminEst.cargarEstadística(idUsuario, lecciones[listActividades.SelectedIndex].IdLección, int.Parse(listLecciones.SelectedItem.ToString()), int.Parse(listRealiaciones.SelectedItem.ToString()));

                txtFecha.Text = "Fecha: " + adminEst.estadística.fechaHora.ToShortDateString() + ". Hora: " + adminEst.estadística.fechaHora.ToShortTimeString();

                txtDuración.Text = "Duración de la lección: " + adminEst.estadística.duración + " minutos";

                txtAciertos.Text = "Aciertos: " + adminEst.estadística.aciertos + " (" + adminEst.estadística.porcentajeAciertos + "%)";

                txtErrores.Text = "Errores: " + adminEst.estadística.errores + " (" + adminEst.estadística.porcentajeErrores + "%)";

                txtCaracteresMinuto.Text = "Caracteres por minuto: " + adminEst.estadística.caracteresPorMinuto;

                txtMásErrores.Text = "1º carácter con más errores: " + adminEst.estadística.caracterConMásErrores.carácter + " (" + adminEst.estadística.caracterConMásErrores.repetición + " veces)";

                txtSegundoErrores.Text = "2º carácter con más errores: " + adminEst.estadística.caracterSegundoConMásErrores.carácter + " (" + adminEst.estadística.caracterSegundoConMásErrores.repetición + " veces)";

                txtTerceroErrores.Text = "3º carácter con más errores: " + adminEst.estadística.caracterTerceroConMásErrores.carácter + " (" + adminEst.estadística.caracterTerceroConMásErrores.repetición + " veces)";

                medidor1.actualizarGráfico(adminEst.estadística.aciertos, adminEst.estadística.errores, Brushes.Green);

            }
        }

        private void resetearInfoEstadísticas()
        {
            txtFecha.Text = "Fecha: - . Hora: -";

            txtDuración.Text = "Duración de la lección: -";

            txtAciertos.Text = "Aciertos: -";

            txtErrores.Text = "Errores: -";

            txtCaracteresMinuto.Text = "Caracteres por minuto: -";

            txtMásErrores.Text = "1º carácter con más errores: -";

            txtSegundoErrores.Text = "2º carácter con más errores: -";

            txtTerceroErrores.Text = "3º carácter con más errores: -";

            List<string> lista = new List<string>();
            lista.Add("Elegir lección");
            listRealiaciones.DataContext = lista;
        }

        private void listUsuarios_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                usuarios();
                listActividades.Focus();
            }
        }

        private void listActividades_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                actividades();
                listLecciones.Focus();
            }
        }

        private void listLecciones_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                clicLecciones();
                listRealiaciones.Focus();
            }
        }

        private void listRealiaciones_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                realizaciones();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                voz.hablarAsync("Volviendo a la práctica, apretá efe uno para saber qué tenés que escribir");
                this.Close();
            }
        }
	}
}