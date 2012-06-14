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
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Voz;

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para editorDeActividades.xaml
	/// </summary>
	public partial class editorDeActividades : Window
	{
        public string IDActAModificar {get; set;}

        public editorDeActividades()
		{
			this.InitializeComponent();
            voz.hablarAsync("Abriendo el editor de actividades. Como este editor es para docentes, sólo se puede usar con un lector de pantallas como yaus o ene ve de a. Para salir pulse escape");
		}

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombreActividad.Text == "") //se controla que haya un nombre para la actividad
            {
                MessageBox.Show("No se ha escrito un nombre para la actividad, por favor escriba uno.", "Cuidado!");
                txtNombreActividad.Focus();
                return;
            }

            if (txtAutor.Text == "") //se controla que haya un autor
            {
                MessageBox.Show("No se ha escrito el nombre del autor de la actividad, por favor escriba uno.", "Cuidado!");
                txtAutor.Focus();
                return;
            }

            if (txtTextoActividad.Text == "") //se controla que haya un texto en la actividad
            {
                MessageBox.Show("No se ha escrito el texto de las lecciones, por favor escriba qué quiere que practiquen sus alumnos.", "Cuidado!");
                txtTextoActividad.Focus();
                return;
            }

            //si pasa los tres controles
            try
            {
                string últIdActividad = this.últimoIdActividad().ToString();
//------------------si se está guardando una actividad nueva--------------------------
                if (IDActAModificar == null) 
                {
                    //se guarda el índice de la actividad
                    XmlDocument doc = new XmlDocument();
                    doc.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\índiceLecciones.xml");

                    XmlElement elem = doc.CreateElement("item");
                    elem.SetAttribute("ID", últIdActividad);
                    elem.SetAttribute("Nombre", txtNombreActividad.Text.Trim());
                    elem.SetAttribute("Tipo", cmbTipo.Text);
                    elem.SetAttribute("Dificultad", cmbDificultad.Text);
                    elem.SetAttribute("Path", "\\Lecciones\\" + últIdActividad + ".xml");
                    elem.SetAttribute("Autor", txtAutor.Text.Trim());
                    doc.DocumentElement.AppendChild(elem);
                    doc.Save(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\índiceLecciones.xml");
                }
       //----------------------si se está modificando la actividad ------------------------------
                else
                {
                    //se modifica la entrada del índice
                    XDocument miXML = XDocument.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\índiceLecciones.xml"); //Cargar el archivo           

                    var consul = from item in miXML.Elements("root").Elements("item")
                                 where item.Attribute("ID").Value == IDActAModificar
                                 select item;
                    consul.ToList()[0].ReplaceAttributes(
                        new XAttribute("ID", IDActAModificar),
                        new XAttribute("Nombre", txtNombreActividad.Text.Trim()),
                        new XAttribute("Tipo", cmbTipo.Text),
                        new XAttribute("Dificultad", cmbDificultad.Text),
                        new XAttribute("Path", "\\Lecciones\\" + IDActAModificar + ".xml"),
                        new XAttribute("Autor", txtAutor.Text.Trim())
                        ); //Modificar los atributos del elemento
                    miXML.Save(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\índiceLecciones.xml");
                }

//========================================================================================================
//--- código común para act nuevas y modificadas: se guardan las lecciones y se resetean los campos -------

                //se guarda la actividad
                XmlDocument docAct = new XmlDocument();
                docAct.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
"<root Ver=\"1\">" + "</root>");

                int contador = 0;
                int posiciónEnter = 0;
                string renglón;
                string textoQueQueda = txtTextoActividad.Text;
                string textoArreglado="";


                //se arregla que no quede sólo un /n o un /r, tiene que ser /n/r
                while (textoQueQueda.IndexOf("\r") != -1)
                {
                    if (textoQueQueda.Substring(0, 1) == "\n") textoQueQueda = textoQueQueda.Substring(1, textoQueQueda.Length - 1);

                    posiciónEnter = textoQueQueda.IndexOf("\r");

                    if (textoQueQueda.Substring(posiciónEnter - 1, 1) != "\n") //si lo anterior al /r no es /n
                    {
                        textoArreglado += textoQueQueda.Substring(0, posiciónEnter);
                        textoArreglado += "\n\r";
                    }
                    else
                    {
                        textoArreglado +=  textoQueQueda.Substring(0, posiciónEnter - 1);
                        textoArreglado += "\n\r";
                    }

                    textoQueQueda = textoQueQueda.Substring(posiciónEnter + 1);
                }

                textoQueQueda = textoArreglado;

                do //cada párrafo es una lección
                {
                    if (textoQueQueda.IndexOf("\n") == -1) //si es un sólo párrafo
                    {
                        posiciónEnter = textoQueQueda.Length;
                        renglón = textoQueQueda.Substring(0, posiciónEnter); //se corta el párrafo del texto
                        textoQueQueda = "";
                    }
                    else
                    {
                        posiciónEnter = textoQueQueda.IndexOf("\n"); //se busca el fin del párrafo
                        renglón = textoQueQueda.Substring(0, posiciónEnter); //se corta el párrafo del texto 
                        textoQueQueda = textoQueQueda.Substring(posiciónEnter + 1, textoQueQueda.Length - posiciónEnter - 2); //se guarda lo que queda del texto para el próximo corte
                        textoQueQueda = textoQueQueda.TrimStart();
                    }


                    if (renglón.Trim() != "") //si no es un renglón sin letras
                    {
                        XmlElement elemAct = docAct.CreateElement("item");
                        contador++;

                        elemAct.SetAttribute("ID", contador.ToString());
                        elemAct.SetAttribute("Texto", renglón);

                        docAct.DocumentElement.AppendChild(elemAct);
                    }
                    //}
                    textoQueQueda = textoQueQueda.Trim();

                } while (textoQueQueda != "");


                if (IDActAModificar == null)
                {
                    docAct.Save(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\Lecciones\" + últIdActividad + ".xml");
                    MessageBox.Show("La actividad se guardó exitosamente.", "Éxito!");
                }
                else
                {
                    //se borra el archivo a modificar
                    File.Delete(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\Lecciones\" + IDActAModificar + ".xml");
                    //se guarda el nuevo archivo con las modificaciones
                    docAct.Save(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\Lecciones\" + IDActAModificar + ".xml");
                    MessageBox.Show("La actividad se modificó exitosamente.", "Éxito!");
                }
                
                //se resetean los campos
                txtTextoActividad.Text = "";
                txtNombreActividad.Text = "";
                txtAutor.Text = "";
                cmbDificultad.SelectedIndex = 0;
                cmbTipo.SelectedIndex = 0;
                txtNombreActividad.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un problema al guardar la actividad, por favor contacte con el desarrollador del programa. Mensaje del error: " + ex.Message);
            }   
        }

        private int últimoIdActividad()
        {
            int idUsuario = 0;
            XmlDocument doc = new XmlDocument();
            
            doc.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\índiceLecciones.xml");

            XmlNode nodo = doc.LastChild.LastChild; //se elige el nodo adecuado
            XmlAttributeCollection atributos = nodo.Attributes;

            if (atributos.Count > 0) //si hay atributos
            {
                XmlAttribute id = atributos[0];
                idUsuario = Int32.Parse(id.Value);
            }

            return idUsuario + 1; //le sumamos un nuevo id
        }

        private void linkEliminarActividad_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            administrarActividades adminAct = new administrarActividades();
            adminAct.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.linkEliminarActividad.txtTextoAMostrar.Text = "Modificar o Eliminar una actividad ya guardada";
            cmbDificultad.Items.Add("Principiante");
            cmbDificultad.Items.Add("Intermedio");
            cmbDificultad.Items.Add("Experto");
            cmbDificultad.SelectedIndex = 0; //se selecciona el primer elemento

            cmbTipo.Items.Add("Letras");
            cmbTipo.Items.Add("Sílabas");
            cmbTipo.Items.Add("Palabras");
            cmbTipo.Items.Add("Números");
            cmbTipo.Items.Add("Símbolos");
            cmbTipo.SelectedIndex = 0; //se selecciona el primer elemento

            txtTextoActividad.Text = "";

            if (IDActAModificar != null) //si es modificar una actividad ya guardada
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones\índiceLecciones.xml"); //se carga el índice

                XmlNode nodoAct = doc.LastChild.LastChild; //invento para instanciar xmlnode, no sé cómo hacerlo de otra forma. LastChild no se usa porque se reemplaza en el foreach por el nodo adecuado

                XmlNodeList leccionesÍndice = doc.GetElementsByTagName("root");
                XmlNodeList listaÍndice = ((XmlElement)leccionesÍndice[0]).GetElementsByTagName("item");

                foreach (XmlElement nodo in listaÍndice)
                {
                    if (nodo.GetAttribute("ID") == IDActAModificar) //si es el nodo buscado
                    {
                        nodoAct = nodo;
                        break;
                    }
                }


                XmlAttributeCollection atributos = nodoAct.Attributes;

                string pathAct = "";
                if (atributos.Count > 0) //si hay atributos
                {
                    txtNombreActividad.Text = atributos[1].Value;
                    cmbTipo.SelectedValue = atributos[2].Value;
                    cmbDificultad.SelectedValue = atributos[3].Value;

                    pathAct = atributos[4].Value;
                    txtAutor.Text = atributos[5].Value;
                }


                //se carga la actividad con la ruta que está en el índice
                XmlDocument docAct = new XmlDocument();
                docAct.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Lecciones" + pathAct);

                XmlNodeList lecciones = docAct.GetElementsByTagName("root");
                XmlNodeList lista = ((XmlElement)lecciones[0]).GetElementsByTagName("item");

                foreach (XmlElement nodo in lista)
                {
                    txtTextoActividad.Text += nodo.GetAttribute("Texto") + "\n\r";
                }
            }

            txtNombreActividad.Focus();
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