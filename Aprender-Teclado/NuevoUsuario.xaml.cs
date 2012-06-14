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
using LógicaAplicación;
using Voz;

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para Usuarios.xaml
	/// </summary>
	public partial class NuevoUsuario : Window
	{
        bool swEmpezando;

        public NuevoUsuario()
		{
			this.InitializeComponent();
            swEmpezando = true;
            txtNombre.Focus();
			// A partir de este punto se requiere la inserción de código para la creación del objeto.
		}

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            guardarUsuario();
        }

        private void guardarUsuario()
        {
            if (txtNombre.Text.Trim() == "")
            {
                voz.hablarAsync("No escribiste tu nombre, escribilo y después apretá enter. Para cancelar ingresar un usuario nuevo, apretá escape");
                txtNombre.Focus();
                return;
            }

            if (txtApellido.Text.Trim() == "")
            {
                voz.hablarAsync("Olvidaste de escribir tu apellido, escribilo y después apretá enter. Para cancelar ingresar un usuario nuevo, apretá escape");
                txtApellido.Focus();
                return;
            }

            if (AdminUsuarios.guardarNuevoUsuario(txtNombre.Text, txtApellido.Text))
                voz.hablarAsync("Usuario " + txtNombre.Text + " " + txtApellido.Text + ", guardado exitosamente. Ahora elegí con las flechas el nombre de usuario con el que quieras empezar el programa y aceptá con enter");
            else
                voz.hablarAsync("Hubo un problema al guardar el nuevo usuario, por favor contacte al desarrollador");

            this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("Volviendo a la lista de usuarios del programa. Elegí con las flechas con qué nombre querés empezar y aceptá con enter");
            this.Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                voz.hablarAsync("Volviendo a la lista de usuarios del programa. Elegí con las flechas con qué nombre querés empezar y aceptá con enter");
                this.Close();
            }

            if (e.Key == Key.Return) //si aprieta enter
            {
                if (txtApellido.IsFocused) //y el foco está en apellido
                {
                    if (txtApellido.Text.Trim() == "") //y apellido tiene escrito algo
                    {
                        voz.hablarAsync("Olvidaste de escribir tu apellido, escribilo y después apretá enter. Para cancelar ingresar un usuario nuevo, apretá escape");
                        txtApellido.Focus();
                    }
                    else
                        guardarUsuario(); //se guarda el usuario
                }


                if (txtNombre.IsFocused) //y el foco está en nombre
                {
                    if (txtNombre.Text.Trim() == "") //y nombre tiene escrito algo
                    {
                        voz.hablarAsync("No escribiste tu nombre, escribilo y después apretá enter. Para cancelar ingresar un usuario nuevo, apretá escape");
                        txtNombre.Focus();
                    }
                    else
                    {
                        if (txtApellido.Text.Trim() == "") //si no hay nada escrito
                            txtApellido.Focus(); //se pasa a apellido
                        else
                            guardarUsuario(); //si ya hay apellido, se guarda el usuario
                    }
                }
            }
        }

        private void txtNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            if (swEmpezando == true)
            {
                voz.hablarAsync("Para añadir un nuevo usuario a Aprender Teclado, escribí primero tu nombre y apretá enter. Si querés cancelar ingresar un nuevo usuario, apretá escape.");
                swEmpezando = false;
            }
            else
                voz.hablarAsync("Escribí tu nombre y apretá enter, para cancelar usá escape");
        }

        private void txtApellido_GotFocus(object sender, RoutedEventArgs e)
        {
            voz.hablarAsync("Ahora escribí tu apellido y apretá enter para guardar el nuevo usuario. Si querés cancelar, apretá escape.");
        }

        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
            decirTexto(txtNombre, e);
        }

        private void txtApellido_KeyUp(object sender, KeyEventArgs e)
        {
            decirTexto(txtApellido, e);
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
	}
}