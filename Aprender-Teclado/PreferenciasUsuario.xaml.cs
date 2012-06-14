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
//using System.Xml.Linq;
//using System.IO;
using Voz;
using LógicaAplicación;

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para Preferencias.xaml
	/// </summary>
	public partial class Preferencias : Window
	{
        enum colores {Blanco, Negro, Rojo, Verde, Azul, Amarillo, Naranja, Violeta}
        AdminPreferencias adminPref;
        bool swEmpezando;
        bool swColorRepetido = false;

		public Preferencias(int idUsuario)
		{
			this.InitializeComponent();
            adminPref = new AdminPreferencias(idUsuario);

            foreach (FontFamily fuente in Fonts.SystemFontFamilies)
                cmbTipoLetra.Items.Add(fuente.ToString());
            
            //----se cargan los tamaños de letra en dos tandas-----
            for (int i = 2; i < 41; i+=2 )
                cmbTamañoLetra.Items.Add(i.ToString());

            for (int i = 42; i < 150; i += 4)
                cmbTamañoLetra.Items.Add(i.ToString());
            //-----------------------------------------------------
           
            foreach (colores color in Enum.GetValues(typeof(colores)))
            {
                cmbColorLetra.Items.Add(color.ToString());
                cmbColorFondo.Items.Add(color.ToString());
            }

            foreach (string unaVoz in voz.listarVocesPorIdioma("Español"))
                cmbVoces.Items.Add(unaVoz);

            for (int i=1; i<21; i++)
                cmbVelocidadVoz.Items.Add(i.ToString());


            if (Fonts.SystemFontFamilies.Contains(new FontFamily(adminPref.nombreLetra))) //si al fuente está en el sistema en cuestión
                cmbTipoLetra.SelectedValue = adminPref.nombreLetra;
            else
                cmbTipoLetra.SelectedIndex = 0;

            if (voz.listarVocesPorIdioma("Español").Contains(adminPref.nombreSintetizador)) //si el sintetizador guardado está en la pc
                cmbVoces.SelectedValue=adminPref.nombreSintetizador;
            else
                cmbVoces.SelectedIndex = 0;

            if (cmbTamañoLetra.Items.Contains(adminPref.tamañoLetra.ToString()))
                cmbTamañoLetra.SelectedValue = adminPref.tamañoLetra.ToString();
            else
                cmbTamañoLetra.SelectedValue = "40";

            if (cmbColorFondo.Items.Contains(adminPref.colorFondo))
                cmbColorFondo.SelectedValue = adminPref.colorFondo;
            else
                cmbColorFondo.SelectedIndex = 0;

            if (cmbColorLetra.Items.Contains(adminPref.colorLetra))
                cmbColorLetra.SelectedValue = adminPref.colorLetra;
            else
                cmbColorLetra.SelectedIndex = 0;

            if (cmbVelocidadVoz.Items.Contains(adminPref.velocidadSintetizador.ToString()))
                cmbVelocidadVoz.SelectedValue = adminPref.velocidadSintetizador.ToString();
            else
                cmbVelocidadVoz.SelectedIndex = 0;

            txtNombre.Text = adminPref.nombreUsuario;
		}

        //-------------  el botón guardar  ------------------
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbColorFondo.Text == cmbColorLetra.Text) //si es el mismo color del fondo y la letra, no se permite
            {
                voz.hablarAsync("No se puede asignar el mismo color a las letras y al fondo porque no se verían las letras, por favor cambie uno de esos dos colores. Elegí con las flechas qué color de letras querés y aceptá con enter");
                swColorRepetido = true;
                cmbColorLetra.Focus();
                return;
            }

            try //si pasa el control
            {
                adminPref.colorLetra = cmbColorLetra.Text;
                adminPref.colorFondo = cmbColorFondo.Text;
                adminPref.nombreSintetizador = cmbVoces.Text;
                adminPref.nombreUsuario = txtNombre.Text.Trim();
                adminPref.nombreLetra = cmbTipoLetra.Text;
                adminPref.tamañoLetra = int.Parse(cmbTamañoLetra.Text);
                adminPref.velocidadSintetizador = int.Parse(cmbVelocidadVoz.Text);

                adminPref.guardarPreferencias();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un problema al guardar las preferencias. Por favor contacte al desarrollador. Mensaje del error: " + ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("Abriendo las preferencias del usuario, movete con TAB para elegir qué preferencia querés modificar. Acordate que las preferencias no se guardan hasta que le des enter al botón que dice: guardar preferencias. Para volver atrás, apretá escape. Estás en un cuadro de texto para escribir tu nombre");
            swEmpezando = true;
            txtNombre.Focus();
        }

        private void txtNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!swEmpezando)
            {
                string mensaje = "Escribí tu nombre";

                if (txtNombre.Text.Trim() != "")
                    mensaje += ". Ya está escrito: " + txtNombre.Text;

                voz.hablarAsync(mensaje);
            }
        }

        private void decirTexto(TextBox miTxt, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (miTxt.Text == "")
                    voz.hablarAsync("Ya está todo borrado");
                else
                    voz.hablarAsync("Borrando, queda " + miTxt.Text);

                return;
            }

            if (e.Key == Key.Space)
            {
                voz.hablarAsync("espacio");
                return;
            }

            voz.hablarAsync(miTxt.Text);
        }

        private void decirCombo(ComboBox miCombo)
        {
            voz.hablarAsync(miCombo.Items[miCombo.SelectedIndex].ToString()); //se selecciona el siguiente elemento
        }

        private void txtNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (!swEmpezando)
                if (e.Key == Key.Enter)
                    cmbTipoLetra.Focus();
                else
                    decirTexto(txtNombre, e);
            else
                swEmpezando = false;
        }

        private void txtNombre_Unloaded(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("Volviendo a la hoja de prácticas, para saber qué tenés que escribir, apretá F1");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void cmbTipoLetra_GotFocus(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("Ahora elegí con las flechas el tipo de letra que querés que se vea en la pantalla y apretá enter para seleccionar. Está elegida la letra, " + cmbTipoLetra.Text);
        }

        private void cmbTamañoLetra_GotFocus(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("Elegí con las flechas qué tamaño querés que tenga la letra que se ve en pantalla y aceptá con enter. Está elegido el tamaño de letra, " + cmbTamañoLetra.Text);
        }

        private void cmbColorLetra_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!swColorRepetido)
                voz.hablarAsync("Buscá con las flechas el color que prefieras que tengan las letras que se muestren en pantalla y aceptá con enter. Recordá que no pueden tener el mismo color las letras y el fondo. Está elegido el color de letra, " + cmbColorLetra.Text);
            else
                swColorRepetido = false;
        }

        private void cmbColorFondo_GotFocus(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("Buscá con las flechas el color del fondo que te sea más útil y elegilo con enter. Recordá que no pueden tener el mismo color las letras y el fondo. Está elegido para el fondo el color, " + cmbColorFondo.Text);
        }

        private void cmbVoces_GotFocus(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("Ahora elegí con las flechas qué voz querés que te hable y aceptá con enter. Está elegida la voz, " + cmbVoces.Text);
        }

        private void cmbVelocidadVoz_GotFocus(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("Por último, elegí con las flechas a la velocidad que querés que te hable la voz y apretá enter para seleccionarla. Está seleccionada la velocidad número, " + cmbVelocidadVoz.Text);
        }

        private void btnGuardar_GotFocus(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("botón Guardar preferencias. Apretá enter para guardar los cambios que hiciste en tus preferencias, o escape para salir sin guardar nada");
        }

        private void cmbTipoLetra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            decirCombo(cmbTipoLetra);
        }

        private void cmbTamañoLetra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            decirCombo(cmbTamañoLetra);
        }

        private void cmbColorLetra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            decirCombo(cmbColorLetra);
        }

        private void cmbColorFondo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            decirCombo(cmbColorFondo);
        }

        private void cmbVoces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            voz.cambiarVoz(cmbVoces.SelectedItem.ToString());
            decirCombo(cmbVoces);
        }

        private void cmbVelocidadVoz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            voz.cambiarVelocidad(int.Parse(cmbVelocidadVoz.SelectedItem.ToString()));
            decirCombo(cmbVelocidadVoz);
        }

        private void cmbTipoLetra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmbTamañoLetra.Focus();
        }

        private void cmbTamañoLetra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmbColorLetra.Focus();
        }

        private void cmbColorLetra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmbColorFondo.Focus();
        }

        private void cmbColorFondo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmbVoces.Focus();
        }

        private void cmbVoces_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmbVelocidadVoz.Focus();
        }

        private void cmbVelocidadVoz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnGuardar.Focus();
        }


	}
}