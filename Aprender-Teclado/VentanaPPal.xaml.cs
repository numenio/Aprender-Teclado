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
using System.Data;
using LógicaAplicación;
using System.IO;
using Voz;

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        int idUsuario;
        int idActividad;
        int idLección;
        
        AdminActividades adminAct;
        AdminPreferencias adminPref;
        AdminEstadísticas adminEst;
        int posiciónCarácterActualEnPalabra;
        tipoActividad tipoAct;
        string rutaAct;
        bool swShiftActivo;
        bool swAltActivo;
        string textoEspacio;
        teclado miTeclado;
        Brush miPincel = Brushes.Green;
        
		public MainWindow(int idUsr, string rutaLección, tipoActividad tipoActividad, int idAct, int idLecciones)
		{
			this.InitializeComponent();

            idActividad = idAct;
            idLección = idLecciones;
            idUsuario = idUsr;
            
            adminAct = new AdminActividades(rutaLección, tipoActividad, idLecciones);//adminPref.nivel);
            adminEst = new AdminEstadísticas(idUsr, idActividad, idLección);
            //medidor1.actualizarGráfico(adminEst.estadística.aciertos, adminEst.estadística.errores);
            
            tipoAct = adminAct.TipoActividad;
            rutaAct = rutaLección; //para volver a las lecciones si aprieta escape

            actualizarPreferencias(idUsuario);

            miTeclado = new teclado(); //se carga el teclado
            cambiarTamañoTeclado();
            txtTexto.Margin = new Thickness(53, this.Width / 4, 49, 0);
            
            this.LayoutRoot.Children.Add(miTeclado);

            //se actualizan las estadísticas
            actualizarTxtEstadísticas();
            cargarPalabra();
            voz.hablarAsync(elegirMensajeInicialParaHablar());

            //ResultadoActividad result = new ResultadoActividad();
            //result = adminAct.ingresarLetraUsuario("d");
            //if (result.continúaActividad == true)
            //    this.Title = "continúa";
            
            //adminPref.guardarPreferencias();
            //AdminUsuarios.guardarNuevoUsuario("fer", "lópez"); //si no puede guardar devuelve false
            //AdminUsuarios.eliminarUsuario("juan", "lópez"); //si no puede eliminar devuelve false
            //AdminUsuarios.eliminarUsuario(1);
            //listaVoces.ItemsSource = voz.listarVocesPorIdioma("Español");
            //listaVoces.ItemsSource = AdminUsuarios.cargarListaUsuarios();
            //Lección lecciónparaBorrar = Lección.leerLección(borrame[0].PathLección);
			// A partir de este punto se requiere la inserción de código para la creación del objeto.
		}

        private void actualizarPreferencias(int idUsuario)
        {
            adminPref = new AdminPreferencias(idUsuario);
            txtTexto.FontFamily = new FontFamily(adminPref.nombreLetra);
            txtTexto.FontSize = adminPref.tamañoLetra;

            if (adminPref.tamañoLetra < 60) //si la letra es chica, no hay problema de escribir espacio
                textoEspacio = " (espacio)";
            else
                textoEspacio = " (esp)"; //si la letra es grande, se abrevia
			
			
            txtTexto.Foreground = traducirColor(adminPref.colorLetra);
            
            this.Background = traducirColor(adminPref.colorFondo);

            if (this.Background == Brushes.Yellow || this.Background == Brushes.White)
            {
                lblInfo.Foreground = Brushes.Black;
                txtEstadísticas.Foreground = Brushes.Black;
                txtTítuloAciertos.Foreground = Brushes.Black;
            }
            else
            {
                lblInfo.Foreground = Brushes.Yellow;
                txtEstadísticas.Foreground = Brushes.Yellow;
                txtTítuloAciertos.Foreground = Brushes.Yellow;
            }

            if (this.Background == Brushes.Green) //se cambia el pincel del medidor si el fondo es verde
                miPincel = Brushes.Yellow;
            else
                miPincel = Brushes.Green;

            medidor1.actualizarGráfico(adminEst.estadística.aciertos, adminEst.estadística.errores, miPincel); //se actualiza el medidor

            animarTexto();
            voz.cambiarVoz(adminPref.nombreSintetizador);
            voz.cambiarVelocidad(adminPref.velocidadSintetizador);
        }

        //void animarTeclado(Key teclaApretada)
        //{
        //    switch (teclaApretada)
        //    {
        //        case Key.A:
        //            teclado.BeginStoryboard(tecla);
        //            break;
        //        case Key.B:
        //            teclado.b.FontSize = 24;
        //            break;
        //    }
        //}

        private void ventana_KeyDown(object sender, KeyEventArgs e)
        {
            
            string textoParaHablar;

            ResultadoActividad result = new ResultadoActividad();
            string letra = e.Key.ToString().ToLower();

            try
            {
                if (e.Key == Key.RightShift || e.Key == Key.LeftShift) //si se aprieta el shift
                    swShiftActivo = true;

                if (e.Key == Key.RightAlt || e.Key == Key.LeftAlt) //si se aprieta el alt
                    swAltActivo = true;

                if (e.Key == Key.Capital)
                    if (e.IsToggled)
                        voz.hablarAsync("Bloqueador de mayúsculas activado");
                    else
                        voz.hablarAsync("Bloqueador de mayúsculas desactivado");

                if (e.Key == Key.NumLock)
                    if (e.IsToggled)
                        voz.hablarAsync("Bloqueador de números activado");
                    else
                        voz.hablarAsync("Bloqueador de números desactivado");

                bool swArribaEñe = false;
                if (adminPref.lugarDelAcento == lugarAcento.arribaDeLaEñe)
                    swArribaEñe = true;

                letra = auxLetras.corregirCarácter(letra, swShiftActivo, swAltActivo, swArribaEñe);

                if (auxLetras.esCarácterLegible(letra))
                {
                    if (letra == "space") letra = " ";
                    result = adminAct.ingresarLetraUsuario(letra.ToString());
                    if (result.continúaActividad)
                    {
                        if (result.letraCorrecta && !result.esperarPorAcento) //si la letra es correcta pero no hay que esperar por una vocal por el acento
                        {
                            posiciónCarácterActualEnPalabra++;
                        }
                    }
                    else
                    {
                        MediaPlayer reproductor = new MediaPlayer(); //se reproduce el sonido
                        reproductor.Close();
                        string rutaInicial = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Recursos\Sonidos\";
                        if (File.Exists(rutaInicial + "finActividad.wav"))
                        {
                            reproductor.Open(new Uri(rutaInicial + "finActividad.wav"));
                            reproductor.Play();
                        }

                        adminEst.guardarEstadísticas(idUsuario, idActividad, adminAct.IdLecciónActual); //se guarda la estadística

                        string mensaje = "Muy bien! Terminaste todas las lecciones de esta actividad para practicar ";
                        switch (adminAct.TipoActividad)
                        {
                            case tipoActividad.palabras:
                                mensaje += " palabras";
                                break;
                            case tipoActividad.sílabas:
                                mensaje += " sílabas";
                                break;
                            case tipoActividad.letras:
                                mensaje += " letras";
                                break;
                            case tipoActividad.números:
                                mensaje += " números";
                                break;
                            case tipoActividad.símbolos:
                                mensaje += " símbolos";
                                break;
                        }

                        mensaje += "!. Ahora vas a ir al menú principal así podés elegir otra actividad para seguir practicando";

                        voz.hablar(mensaje);


                        //Lecciones ventanaAct = new Lecciones(rutaAct, tipoAct, idUsuario);
                        //ventanaAct.swVolviendo = true;
                        //ventanaAct.Show();
                        //this.Close();
                        Actividades ventanaAct = new Actividades(idUsuario);
                        ventanaAct.swVolviendo = true;
                        ventanaAct.Show();
                        this.Close();
                        return;
                    }

                    textoParaHablar = elegirMensajeParaHablar(result);

                    if (result.letraCorrecta) //se cargan las estadísticas
                        adminEst.cargarLetraAcierto(char.Parse(letra));
                    else
                        adminEst.cargarLetraError(char.Parse(letra));

                    medidor1.actualizarGráfico(adminEst.estadística.aciertos, adminEst.estadística.errores, miPincel); //se actualiza el medidor


                    if (result.tiempoAgotado)
                        textoParaHablar += ", tiempo agotado";


                    if (adminAct.swhayPalabraNueva)
                        cargarPalabra();

                    if (adminAct.swhayLecciónNueva)
                        adminEst.guardarEstadísticas(idUsuario, idActividad, adminAct.IdLecciónAnterior); //cambiar 1,1 por el número de la act y de la lecc

                    e.Handled = true; //se evita que se procese la tecla en el sistema, esto por los acentos y diéresis
                    //se actualizan las estadísticas
                    actualizarTxtEstadísticas();

                    animarTexto();

                    voz.hablarAsync(textoParaHablar);
                }

                if (e.Key == Key.Escape) //si aprieta escape, volver a las lecciones
                {
                    Lecciones ventanaAct = new Lecciones(rutaAct, tipoAct, idUsuario, idActividad);
                    ventanaAct.swVolviendo = true;
                    ventanaAct.Show();
                    this.Close();
                }

                if (e.Key == Key.F1) //F1, recordar texto actual a escribir y lo ya escrito
                {
                    recordarPalabraActual();
                }

                if (e.Key == Key.F2) //F2, deletrear palabra
                {
                    deletrearPalabraActual();
                }

                if (e.Key == Key.F3) //F3, ayuda de qué dedo apretar
                {
                    char miLetra;
                    char.TryParse(adminAct.LetraActualLección, out miLetra);
                    voz.hablarAsync(elegirAyudaQuéDedoUsar(miLetra));
                }

                if (e.Key == Key.F4) //F4, cambia el estado de visibilidad del teclado en pantalla
                {
                    if (miTeclado.IsVisible)
                    {
                        miTeclado.Visibility = Visibility.Hidden;
                        txtTexto.Margin = new Thickness(53, this.Width / 4, 49, 0);
                        voz.hablarAsync("Ocultando el teclado en pantalla");
                    }
                    else
                    {
                        miTeclado.Visibility = Visibility.Visible;
                        txtTexto.Margin = new Thickness(53, 36, 49, 0);
                        voz.hablarAsync("Haciendo visible el teclado en pantalla");
                    }
                }

                if (e.Key == Key.F5) //F5, cambiar el acento entre la derecha de la eñe y arriba de la misma
                {
                    if (adminPref.lugarDelAcento == lugarAcento.arribaDeLaEñe)
                    {
                        adminPref.lugarDelAcento = lugarAcento.derechaDeLaEñe;
                        voz.hablarAsync("Configurando el acento agudo a la derecha de la eñe");
                    }
                    else
                    {
                        adminPref.lugarDelAcento = lugarAcento.arribaDeLaEñe;
                        voz.hablarAsync("Configurando el acento agudo arriba y a la derecha de la eñe");
                    }
                }

                if (e.Key == Key.F6) //F6, lee las estadísticas
                {
                    actualizarMensajeEstadísticasVoz();
                }

                if (e.Key == Key.F9) //F9, abre el editor de actividades
                {
                    abrirActividades();
                }

                if (e.Key == Key.F8) //F8, abre el estadísticas
                {
                    abrirEstadísticas();
                }

                if (e.Key == Key.F12) //F12, abre las preferencias
                {
                    abrirPreferencias();
                }

                if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)//si usa las flechas
                {
                    voz.callar();
                }

                //e.Handled = true; //se evita que se procese la tecla en el sistema, esto por los acentos, diéresis y efes
            }
            catch (Exception ex)
            {
                MessageBox.Show("Para enviar al desarrollador. Mensaje del error: " + ex.Message);
            }
        }

        private void actualizarTxtEstadísticas()
        {
            string mensajeEstadística = "Aciertos = " + adminEst.estadística.aciertos.ToString() + " (" + adminEst.estadística.porcentajeAciertos.ToString() + "%)\n" +
            "Errores = " + adminEst.estadística.errores.ToString() + " (" + adminEst.estadística.porcentajeErrores.ToString() + "%)\n" +
            "Car. x min = " + adminEst.estadística.caracteresPorMinuto.ToString() + "\n" +
            "Ranking errores:\n" +
            "1º: " + adminEst.estadística.caracterConMásErrores.carácter + " (" + adminEst.estadística.caracterConMásErrores.repetición + ")\n" +
            "2º: " + adminEst.estadística.caracterSegundoConMásErrores.carácter + " (" + adminEst.estadística.caracterSegundoConMásErrores.repetición + ")\n" +
            "3º: " + adminEst.estadística.caracterTerceroConMásErrores.carácter + " (" + adminEst.estadística.caracterTerceroConMásErrores.repetición + ")\n";
            txtEstadísticas.Text = mensajeEstadística;
        }

        private void actualizarMensajeEstadísticasVoz()
        {
            string mensajeEstadística = "Hasta ahora has tenido el siguiente desempeño. Escribiste " + (adminEst.estadística.errores + adminEst.estadística.aciertos).ToString() + " caracteres. De esos caracteres tuviste " + adminEst.estadística.aciertos.ToString() + " aciertos, que es el " + adminEst.estadística.porcentajeAciertos.ToString() + " porciento. Cometiste " +
            adminEst.estadística.errores.ToString() + " errores, que es el " + adminEst.estadística.porcentajeErrores.ToString() + " porciento. " +
            "Has escrito a una velocidad de " + adminEst.estadística.caracteresPorMinuto.ToString() +
            " caracteres por minuto. El carácter con más errores es " + auxLetras.traducirCarácter(adminEst.estadística.caracterConMásErrores.carácter.ToString()) + ", con " + adminEst.estadística.caracterConMásErrores.repetición + elegirSingularPlural(adminEst.estadística.caracterConMásErrores.repetición) +
            "El segundo carácter con más errores es " + auxLetras.traducirCarácter(adminEst.estadística.caracterSegundoConMásErrores.carácter.ToString()) + ", con " + adminEst.estadística.caracterSegundoConMásErrores.repetición + elegirSingularPlural(adminEst.estadística.caracterSegundoConMásErrores.repetición) +
            "Y el tercer carácter con más errores es " + auxLetras.traducirCarácter(adminEst.estadística.caracterTerceroConMásErrores.carácter.ToString()) + ", con " + adminEst.estadística.caracterTerceroConMásErrores.repetición  + elegirSingularPlural(adminEst.estadística.caracterTerceroConMásErrores.repetición);
            voz.hablarAsync(mensajeEstadística);
        }

        private string elegirSingularPlural(int repeticiones)
        {
            string devol = "";
            if (repeticiones == 1)
                devol = " repetición. ";
            else
                devol = " repeticiones. ";

            return devol;
        }

        private void cargarPalabra()
        {
            if (adminPref.swMostrarLetrasEnMayúsculas)
                txtTexto.Text = adminAct.PalabraActualLección.ToUpper();
            else
                txtTexto.Text = adminAct.PalabraActualLección;

            txtTexto.Text += textoEspacio;

            posiciónCarácterActualEnPalabra = 0; //si no hay palabra nueva
            animarTexto();
        }

        private string elegirMensajeParaHablar(ResultadoActividad result)
        {
            string mensaje="";

            if (result.letraCorrecta)
                mensaje = "Muy bien, ";
            else
                mensaje = "Te equivocaste, ";


            switch (adminAct.TipoActividad)
            {
                //=============== palabras ======================
                case tipoActividad.palabras:
                    if (adminAct.swhayPalabraNueva)
                    {
                        mensaje += " Terminaste de escribir " + auxLetras.traducirCarácter(adminAct.PalabraAnteriorLección) + ". Ahora escribí la palabra " + auxLetras.traducirCarácter(adminAct.PalabraActualLección);
                    }
                    else
                    {
                        if (adminAct.LetraActualLección == " ") //si terminó la palabra
                            mensaje += " Terminaste  de escribir la palabra " + auxLetras.traducirCarácter(adminAct.PalabraActualLección) + ", ahora apretá espacio";
                        else //si todavía queda parte de la palabra por escribir
                        {
                            if (result.letraCorrecta && !result.esperarPorAcento) //si es la letra correcta y no hay que esperar por el acento
                                mensaje += " hasta ahora escribiste" + auxLetras.traducirCarácter(adminAct.PalabraActualUsuario);
                            else if (result.letraCorrecta && result.esperarPorAcento) //si acaba de acertar el acento
                                mensaje += auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + ". Ahora escribí la vocal " + auxLetras.obtenerVocalSinAcento(adminAct.LetraActualLección) + " para formar " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                            else if (!result.letraCorrecta && result.esperarPorAcento) //si ya acertó el acento, pero erró la vocal que se esperaba; o si erró el tipo de acento que se esperaba
                            {
                                mensaje += " apretaste " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + " pero tenías que apretar ";
                                if (!auxLetras.esAcento(adminAct.LetraAnteriorLección)) //si lo que tiene que escribir es el acento
                                    mensaje += auxLetras.traducirCarácter(auxLetras.obtenerAcentoDeVocal(adminAct.LetraActualLección));
                                else
                                    mensaje += auxLetras.traducirCarácter(auxLetras.obtenerVocalSinAcento(adminAct.LetraActualLección));

                                mensaje += " para escribir " + auxLetras.traducirCarácter(adminAct.LetraActualLección);

                                if (auxLetras.esLetraConAcento(adminAct.LetraAnteriorLección) || auxLetras.esAcento(adminAct.LetraAnteriorLección)) //si ya escribió el acento
                                    mensaje += ". Ya escribiste " + auxLetras.traducirCarácter(adminAct.LetraAnteriorLección);
                            }

                            else if (!result.letraCorrecta && !result.esperarPorAcento) //si no es la letra esperada pero a su vez no había que esperar por la vocal del acento
                            {
                                mensaje += " apretaste " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + " pero tenías que apretar " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                                if (adminAct.PalabraActualUsuario != "") //si el usuario ha escrito algo
                                    mensaje += ". Llevás escrito" + auxLetras.traducirCarácter(adminAct.PalabraActualUsuario);
                                else
                                    mensaje += ". Todavía no has escrito nada";
                            }
                        }
                    }
                    break;
                //=============== sílabas ======================
                case tipoActividad.sílabas:
                    if (adminAct.swhayPalabraNueva)
                        mensaje += " Terminaste de escribir " + auxLetras.traducirCarácter(adminAct.PalabraAnteriorLección) + ". Ahora escribí la sílaba " + auxLetras.traducirCarácter(adminAct.PalabraActualLección);
                    else
                        if (adminAct.LetraActualLección == " ")
                            mensaje += " Terminaste  de escribir la sílaba " + auxLetras.traducirCarácter(adminAct.PalabraActualLección) + ", ahora apretá espacio";
                        else
                        {
                            if (result.letraCorrecta && !result.esperarPorAcento)
                                mensaje += "  hasta ahora escribiste" + auxLetras.traducirCarácter(adminAct.PalabraActualUsuario);

                            if (result.letraCorrecta && result.esperarPorAcento) //si acaba de acertar el acento
                                mensaje += auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + ". Ahora escribí la vocal " + auxLetras.obtenerVocalSinAcento(adminAct.LetraActualLección) + " para formar " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                            else if (!result.letraCorrecta && result.esperarPorAcento) //si ya acertó el acento, pero erró la vocal que se esperaba; o si erró el tipo de acento que se esperaba
                            {
                                mensaje += " apretaste " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + " pero tenías que apretar ";
                                if (!auxLetras.esAcento(adminAct.LetraAnteriorLección)) //si lo que tiene que escribir es el acento
                                    mensaje += auxLetras.traducirCarácter(auxLetras.obtenerAcentoDeVocal(adminAct.LetraActualLección));
                                else
                                    mensaje += auxLetras.traducirCarácter(auxLetras.obtenerVocalSinAcento(adminAct.LetraActualLección));

                                mensaje += " para escribir " + auxLetras.traducirCarácter(adminAct.LetraActualLección);

                                if (auxLetras.esLetraConAcento(adminAct.LetraAnteriorLección) || auxLetras.esAcento(adminAct.LetraAnteriorLección)) //si ya escribió el acento
                                    mensaje += ". Ya escribiste " + auxLetras.traducirCarácter(adminAct.LetraAnteriorLección);
                            }

                            else if (!result.letraCorrecta && !result.esperarPorAcento) //si no es la letra esperada pero a su vez no había que esperar por la vocal del acento
                            {
                                mensaje += " apretaste " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + " pero tenías que apretar " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                                if (adminAct.PalabraActualUsuario != "") //si el usuario ha escrito algo
                                    mensaje += ". Llevás escrito" + auxLetras.traducirCarácter(adminAct.PalabraActualUsuario);
                                else
                                    mensaje += ". Todavía no has escrito nada";
                            }


                        }
                    break;
                //=============== números ======================
                case tipoActividad.números:
                    if (adminAct.swhayPalabraNueva)
                        mensaje += " Terminaste de escribir " + auxLetras.traducirCarácter(adminAct.PalabraAnteriorLección) + ". Ahora escribí el número " + auxLetras.traducirCarácter(adminAct.PalabraActualLección);
                    else
                        if (adminAct.LetraActualLección == " ")
                            mensaje += " Terminaste  de escribir el número " + auxLetras.traducirCarácter(adminAct.PalabraActualLección) + ", ahora apretá espacio";
                        else
                        {
                            //if (result.letraCorrecta)
                            //    mensaje += " hasta ahora escribiste el número " + auxLetras.traducirCarácter(adminAct.PalabraActualUsuario);
                            //else
                            //{
                            //    mensaje += " apretaste " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + " pero tenías que apretar " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                            //    if (adminAct.PalabraActualUsuario != "") //si el usuario ha escrito algo
                            //        mensaje += ". Llevás escrito" + auxLetras.traducirCarácter(adminAct.PalabraActualUsuario);
                            //    else
                            //        mensaje += ". Todavía no has escrito nada";



                            if (result.letraCorrecta && !result.esperarPorAcento)
                                mensaje += "  hasta ahora escribiste el número " + auxLetras.traducirCarácter(adminAct.PalabraActualUsuario);

                            if (result.letraCorrecta && result.esperarPorAcento) //si acaba de acertar el acento
                                mensaje += auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + ". Ahora escribí la vocal " + auxLetras.obtenerVocalSinAcento(adminAct.LetraActualLección) + " para formar " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                            else if (!result.letraCorrecta && result.esperarPorAcento) //si ya acertó el acento, pero erró la vocal que se esperaba; o si erró el tipo de acento que se esperaba
                            {
                                mensaje += " apretaste " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + " pero tenías que apretar ";
                                if (!auxLetras.esAcento(adminAct.LetraAnteriorLección)) //si lo que tiene que escribir es el acento
                                    mensaje += auxLetras.traducirCarácter(auxLetras.obtenerAcentoDeVocal(adminAct.LetraActualLección));
                                else
                                    mensaje += auxLetras.traducirCarácter(auxLetras.obtenerVocalSinAcento(adminAct.LetraActualLección));

                                mensaje += " para escribir " + auxLetras.traducirCarácter(adminAct.LetraActualLección);

                                if (auxLetras.esLetraConAcento(adminAct.LetraAnteriorLección) || auxLetras.esAcento(adminAct.LetraAnteriorLección)) //si ya escribió el acento
                                    mensaje += ". Ya escribiste " + auxLetras.traducirCarácter(adminAct.LetraAnteriorLección);
                            }

                            else if (!result.letraCorrecta && !result.esperarPorAcento) //si no es la letra esperada pero a su vez no había que esperar por la vocal del acento
                            {
                                mensaje += " apretaste " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + " pero tenías que apretar " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                                if (adminAct.PalabraActualUsuario != "") //si el usuario ha escrito algo
                                    mensaje += ". Llevás escrito" + auxLetras.traducirCarácter(adminAct.PalabraActualUsuario);
                                else
                                    mensaje += ". Todavía no has escrito nada";
                            }
                        }
                        
                    break;
                //=============== letras y símbolos ======================
                default:
                    if (result.letraCorrecta && !result.esperarPorAcento)
                        mensaje += " Tecla apretada: " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + ". Ahora apretá la tecla " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                    else if (result.letraCorrecta && result.esperarPorAcento) //si acaba de acertar el acento
                        mensaje += auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + ". Ahora escribí la vocal " + auxLetras.obtenerVocalSinAcento(adminAct.LetraActualLección) + " para formar " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                    else if (!result.letraCorrecta && result.esperarPorAcento) //si ya acertó el acento, pero erró la vocal que se esperaba; o si erró el tipo de acento que se esperaba
                    {
                        mensaje += " apretaste " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + " pero tenías que apretar ";
                        if (!auxLetras.esAcento(adminAct.LetraAnteriorLección)) //si lo que tiene que escribir es el acento
                            mensaje += auxLetras.traducirCarácter(auxLetras.obtenerAcentoDeVocal(adminAct.LetraActualLección));
                        else
                            mensaje += auxLetras.traducirCarácter(auxLetras.obtenerVocalSinAcento(adminAct.LetraActualLección));

                        mensaje += " para escribir " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                    }

                    else if (!result.letraCorrecta && !result.esperarPorAcento) //si no es la letra esperada pero a su vez no había que esperar por la vocal del acento
                    {
                        mensaje += " apretaste " + auxLetras.traducirCarácter(adminAct.LetraActualUsuario) + " pero tenías que apretar " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                    }
                    break;
            }

            if (!result.letraCorrecta)
            {
                char letra;
                char.TryParse(adminAct.LetraActualLección, out letra);
                mensaje += elegirAyudaQuéDedoUsar(letra);
            }

            return mensaje;
        }

        private string elegirMensajeInicialParaHablar()
        {
            string mensaje = " Comenzando la lección número " + adminAct.IdLecciónActual.ToString()  + " para practicar ";
            switch (adminAct.TipoActividad)
            {
                case tipoActividad.palabras:
                    mensaje += " palabras. Para empezar, escribí: " + adminAct.PalabraActualLección;
                    break;
                case tipoActividad.sílabas:
                    mensaje += " sílabas. Para empezar, escribí " + adminAct.PalabraActualLección;
                    break;
                case tipoActividad.letras:
                    mensaje += " letras. Para empezar, apretá la tecla " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                    break;
                case tipoActividad.números:
                    mensaje += " números. Para empezar, escribí el número " + adminAct.PalabraActualLección;
                    break;
                case tipoActividad.símbolos:
                    mensaje += " símbolos. Para empezar, escribí el símbolo " + auxLetras.traducirCarácter(adminAct.LetraActualLección);
                    break;
            }

            adminAct.swhayPalabraNueva = false;

            return mensaje;
        }

        void recordarPalabraActual()
        {
            string cadena = "Tenés que escribir "; 
            if (adminAct.TipoActividad != tipoActividad.letras && adminAct.TipoActividad != tipoActividad.símbolos) //si no es ni letras ni símbolos
            {
                cadena += adminAct.PalabraActualLección;
                if (adminAct.PalabraActualUsuario != "")
                    cadena += ". Llevás escrito " + adminAct.PalabraActualUsuario;
                else
                    cadena += ". Todavía no escribiste nada";
            }
            else
            {
                cadena += auxLetras.traducirCarácter(adminAct.LetraActualLección);
            }

            voz.hablarAsync(cadena);
        }

        void deletrearPalabraActual()
        {
            string cadena="";
            switch (adminAct.TipoActividad)
            {
                case tipoActividad.letras:
                    cadena="No te puedo deletrear porque esta lección es para practicar letras. La letra que tenés que escribir es: ";
                    break;
                case tipoActividad.números:
                    cadena = "El número que tenés que escribir es: ";
                    break;
                case tipoActividad.palabras:
                    cadena = "La palabra a escribir deletreada es: ";
                    break;
                case tipoActividad.sílabas:
                    cadena = "La sílaba a escribir deletreada es: ";
                    break;
                case tipoActividad.símbolos:
                    cadena = "No te puedo deletrear porque esta lección es para practicar símbolos. El símbolo que tenés que escribir es: ";
                    break;
            }

            if (adminAct.TipoActividad != tipoActividad.letras && adminAct.TipoActividad != tipoActividad.símbolos) //si no es ni letras ni símbolos
            {
                foreach (char letra in adminAct.PalabraActualLección)
                {
                    cadena += auxLetras.traducirCarácter(letra.ToString()) + ", ";
                }
            }
            else
            {
                cadena += auxLetras.traducirCarácter(adminAct.LetraActualLección);
            }


            voz.hablarAsync(cadena);
        }

        private void btnAdminActividades_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            abrirActividades();
            //this.Close();
        }

        private static void abrirActividades()
        {
            editorDeActividades editorAct = new editorDeActividades();
            editorAct.ShowDialog();
        }

        private void btnPreferencias_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            abrirPreferencias();
        }

        private void abrirPreferencias()
        {
            Preferencias pref = new Preferencias(idUsuario);
            pref.ShowDialog();
            actualizarPreferencias(idUsuario);
        }

        SolidColorBrush traducirColor(string color)
        {
            SolidColorBrush miColor = new SolidColorBrush();
            switch (color)
            {
                case "Negro":
                    miColor = Brushes.Black;
                    break;
                case "Blanco":
                    miColor = Brushes.White;
                    break;
                case "Rojo":
                    miColor = Brushes.Red;
                    break;
                case "Verde":
                    miColor = Brushes.Green;
                    break;
                case "Azul":
                    miColor = Brushes.Blue;
                    break;
                case "Amarillo":
                    miColor = Brushes.Yellow;
                    break;
                case "Naranja":
                    miColor = Brushes.Orange;
                    break;
                case "Violeta":
                    miColor = Brushes.Violet;
                    break;
            }

            return miColor;
        }

        private void ventana_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
                swShiftActivo = false;

            if (e.Key == Key.RightAlt || e.Key == Key.LeftAlt)
                swAltActivo = false;

        }


        //corregir, si hay lección nueva, se espera espacio y el usuario erra, pinta la primer letra
        private void animarTexto()
        {
            TextEffectCollection colecciónEfectos = new TextEffectCollection();
            TextEffect efecto = new TextEffect();

            if (!adminAct.swhayLecciónNueva) //si no es lección nueva
            {
                efecto.PositionStart = posiciónCarácterActualEnPalabra;
            }
            else
            {
                efecto.PositionStart = txtTexto.Text.Length - textoEspacio.Length;
            }


            if (posiciónCarácterActualEnPalabra < txtTexto.Text.Length - textoEspacio.Length && !adminAct.swhayLecciónNueva) //si está en la palabra y no es una lección nueva
                efecto.PositionCount = 1;
            else
                efecto.PositionCount = textoEspacio.Length;



            //ScaleTransform transformación = new ScaleTransform(2, 2);
            //efecto.Transform = transformación;
            efecto.Foreground = seleccionarPincelAnimación();
            colecciónEfectos.Add(efecto);
            txtTexto.TextEffects = colecciónEfectos;
        }

        Brush seleccionarPincelAnimación()
        {
            Brush miPincel;
            switch (adminPref.colorLetra)
            {
                case "Blanco":
                    miPincel = Brushes.Black;
                    break;
                case "Rojo":
                    miPincel = Brushes.Green;
                    break;
                case "Verde":
                    miPincel = Brushes.Red;
                    break;
                case "Azul":
                    miPincel = Brushes.Yellow;
                    break;
                case "Amarillo":
                    miPincel = Brushes.Blue;
                    break;
                case "Naranja":
                    miPincel = Brushes.GreenYellow;
                    break;
                case "Violeta":
                    miPincel = Brushes.Orange;
                    break;
                case "Negro":
                    miPincel = Brushes.Yellow;
                    break;
                default:
                    miPincel = Brushes.Black;
                    break;
            }

            if (this.Background == miPincel) //si el pincel elegido es el mismo del fondo
            {
                if (miPincel == Brushes.Black) //si es negro
                    miPincel = Brushes.Yellow; //se lo setea amarillo
                else //si no es negro, se lo setea de ese color
                    miPincel = Brushes.Black;
            }

            if (this.txtTexto.Foreground == miPincel) //si el pincel elegido es el mismo del fondo
            {
                if (miPincel == Brushes.Black) //si es negro
                    miPincel = Brushes.Orange;
                else //si no es negro, se lo setea de ese color
                    miPincel = Brushes.Violet;
            }
            return miPincel;
        }


        private void cambiarTamañoTeclado()
        {
            //if (!(this.WindowState == WindowState.Maximized))
            //{
            miTeclado.Margin = new Thickness(20, 159.805, 3.5, 31.416);
            miTeclado.RenderTransformOrigin = new Point(0.5, 0.5);
            miTeclado.MinHeight = 271;
            miTeclado.MinWidth = 658;
            miTeclado.Height = 278.778;
            miTeclado.Visibility = Visibility.Hidden;
            //}
            //else
            //{
            //    miTeclado.Margin = new Thickness(20, 159.805, 3.5, 31.416);
            //    miTeclado.RenderTransformOrigin = new Point(0.5, 0.5);
            //    miTeclado.MinHeight = 271;
            //    miTeclado.MinWidth = 656.5;
            //    miTeclado.Width = this.Width - 60;
            //}
        }


        private string elegirAyudaSimple(char letra)
        {
            string result = "";

            switch (letra)
            {
                case ',':
                    result = "La coma está ";
                    break;
                case '.':
                    result = "El punto está ";
                    break;
                case '-':
                    result = "El guión se encuentra ";
                    break;
                case ';':
                    result = "El punto y coma se ubica ";
                    break;
                case ':':
                    result = "Los dos puntos se encuentran ";
                    break;
                case '_':
                    result = "El guión bajo está ubicado ";
                    break;
                case '´':
                    result = "El acento agudo está ";
                    break;
                default:
                    result = "La " + auxLetras.traducirCarácter(letra.ToString()) + " está ";
                    break;
            }

            switch (letra)
            {
                case 'q':
                    result += "Arriba de la a. ";
                    break;
                case 'w':
                    result += "Arriba de la ese. ";
                    break;
                case 'e':
                    result += "Arriba de la de. ";
                    break;
                case 'r':
                    result += "Arriba de la efe. ";
                    break;
                case 't':
                    result += "Arriba de la ge. ";
                    break;
                case 'g':
                    result += "A la derecha de la efe. ";
                    break;
                case 'z':
                    result += "Abajo de la a. ";
                    break;
                case 'x':
                    result += "Abajo de la ese. ";
                    break;
                case 'c':
                    result += "Abajo de la de. ";
                    break;
                case 'v':
                    result += "Abajo de la efe. ";
                    break;
                case 'b':
                    result += "Abajo de la ge. ";
                    break;
                case 'y':
                    result += "Arriba de la ache. ";
                    break;
                case 'h':
                    result += "A la izquierda de la jota. ";
                    break;
                case 'n':
                    result += "Abajo de la ache. ";
                    break;
                case 'u':
                    result += "Arriba de la jota. ";
                    break;
                case 'i':
                    result += "Arriba de la ca. ";
                    break;
                case 'o':
                    result += "Arriba de la ele. ";
                    break;
                case 'p':
                    result += "Arriba de la eñe. ";
                    break;
                case 'm':
                    result += "Abajo de la jota. ";
                    break;
                case ',':
                    result += "Abajo de la ca. ";
                    break;
                case '.':
                    result += "Abajo de la ele. ";
                    break;
                case '-':
                    result += "Abajo de la eñe. ";
                    break;
                case ';':
                    result += "Abajo de la ca apretando shift. ";
                    break;
                case ':':
                    result += "Abajo de la ele apretando shift. ";
                    break;
                case '_':
                    result += "Abajo de la eñe apretando shift. ";
                    break;
                case '´':
                    result += "A la derecha de la eñe. ";
                    break;
            }

            if (result == "La " + auxLetras.traducirCarácter(letra.ToString()) + " está ") //si sólo cargó la intro, se la borra
                result = "";

            return result;
        }

        private string elegirAyudaQuéDedoUsar(char letra)
        {
            string mensajeAyuda;
            bool swShift = false, swAlt = false, swManoIzq = false, swManoDer = false, swfilaNum = false, swfilaSup = false, swfilaMedio = false, swfilaInf = false, swEsp = false, swMeñique = false, swAnular = false, swMedio = false, swíndice = false, swPulgar = false, swALaDerecha = false, swALaIzquierda = false ;

            char[] manoDerecha = { 'h', 'j', 'k', 'l', 'ñ', 'y', 'u', 'i', 'o', 'p', '`', '^', '+', '*', '´', '¨', 'ç', 'Ç', 'n', 'm', ',', ';', '.', '-', '_', 'à', 'è', 'ì', 'ò', 'ù', 'â', 'ê', 'î', 'ô', 'û', 'á', 'é', 'í', 'ó', 'ú', 'ü' };
            char[] manoIzquierda = {'f', 'd', 'g', 's', 'a', 'q', 'w', 'e', 'r', 't', 'z', '<', '>', 'x', 'c', 'v', 'b' };
            char[] shiftApretado = { '^', '¨', '*', 'Ç', '>', '!', 'ª', '"', '·', '$', '%', '&', '/', '(', ')', '=', '?', '¿', ';', ':', '_', 'â', 'ê', 'î', 'ô', 'û', 'ü' };
            char[] altApretado = {'\\', '|', '@', '#', '~', '€', '¬', '[', ']', '{', '}' };
            char[] filaNúmeros = {'º', 'ª', '\\', '1', '!', '|', '2', '"', '@', '3', '·', '#', '4', '$', '~', '5', '%', '€', '6', '&', '¬', '7', '/', '8', '(', '9', ')', '0', '=', '\'', '?', '¡', '¿' };
            char[] filaSuperior = {'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', '`', '^', '+', '*', '[', ']', 'à', 'è', 'ì', 'ò', 'ù', 'â', 'ê', 'î', 'ô', 'û' };
            char[] filaCentral = { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'ñ', '´', '¨', '{', 'ç', 'Ç', '}', 'á', 'é', 'í', 'ó', 'ú', 'ü' };
            char[] filaInferior = { '<', '>', 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', ';', '.', ':', '-', '_' };
            char[] espacio = {' '};
            char[] dedoMeñique = { 'a', 'q', 'z', '<', '>', 'p', 'ñ', '-', '_', '`', '^', '[', '+', '*', ']', '´', '¨', '{', 'ç', 'Ç', '}', 'à', 'è', 'ì', 'ò', 'ù', 'â', 'ê', 'î', 'ô', 'û', 'á', 'é', 'í', 'ó', 'ú', 'ü' };
            char[] dedoAnular = {'w', 's', 'x', 'o', 'l', '.', ':' };
            char[] dedoMedio = {'d', 'e', 'c', 'i', 'k', ',', ';' };
            char[] dedoÍndice = { 'f', 'r', 'v', 't', 'g', 'b', 'j', 'u', 'm', 'h', 'y', 'n' };
            char[] dedoPulgar = {' '};
            char[] másALaDerecha = { '`', '^', '+', '*', '´', '¨', 'ç', 'Ç', '[', ']', '{', '}', 't', 'g', 'b', 'à', 'è', 'ì', 'ò', 'ù', 'â', 'ê', 'î', 'ô', 'û', 'á', 'é', 'í', 'ó', 'ú', 'ü' };
            char[] másALaIzquierda = {'h', 'y', 'n', '<', '>' };

            if (manoDerecha.Contains(letra)) swManoDer = true; //las manos
            if (manoIzquierda.Contains(letra)) swManoIzq = true;
            
            if (shiftApretado.Contains(letra)) swShift = true; //shift
            if (altApretado.Contains(letra)) swAlt = true; //alt
            
            if (filaNúmeros.Contains(letra)) swfilaNum = true; //las filas
            if (filaCentral.Contains(letra)) swfilaMedio = true;
            if (filaSuperior.Contains(letra)) swfilaSup = true;
            if (filaInferior.Contains(letra)) swfilaInf = true;
            
            if (espacio.Contains(letra)) swEsp = true; //espacio
            
            if (dedoÍndice.Contains(letra)) swíndice = true; //los dedos
            if (dedoAnular.Contains(letra)) swAnular = true;
            if (dedoMedio.Contains(letra)) swMedio = true;
            if (dedoMeñique.Contains(letra)) swMeñique = true;
            if (dedoPulgar.Contains(letra)) swPulgar = true;

            if (másALaDerecha.Contains(letra)) swALaDerecha = true; //las teclas que hay que estirar los dedos
            if (másALaIzquierda.Contains(letra)) swALaIzquierda = true;

            mensajeAyuda = elegirAyudaSimple(letra);

            if (swfilaNum)
            {
                mensajeAyuda += "Tenés que buscar bien arriba, abajo de las teclas efe, en la ";

                switch (letra)
                {
                    case 'º':
                        mensajeAyuda += "primer";
                        break;
                    case 'ª':
                        mensajeAyuda += "primer";
                        break;
                    case '\\':
                        mensajeAyuda += "primer";
                        break;
                    case '1':
                        mensajeAyuda += "segunda";
                        break;
                    case '!':
                        mensajeAyuda += "segunda";
                        break;
                    case '|':
                        mensajeAyuda += "segunda";
                        break;
                    case '2':
                        mensajeAyuda += "tercer";
                        break;
                    case '"':
                        mensajeAyuda += "tercer";
                        break;
                    case '@':
                        mensajeAyuda += "tercer";
                        break;
                    case '3':
                        mensajeAyuda += "cuarta";
                        break;
                    case '·':
                        mensajeAyuda += "cuarta";
                        break;
                    case '#':
                        mensajeAyuda += "cuarta";
                        break;
                    case '4':
                        mensajeAyuda += "quinta";
                        break;
                    case '$':
                        mensajeAyuda += "quinta";
                        break;
                    case '~':
                        mensajeAyuda += "quinta";
                        break;
                    case '5':
                        mensajeAyuda += "sexta";
                        break;
                    case '%':
                        mensajeAyuda += "sexta";
                        break;
                    case '€':
                        mensajeAyuda += "sexta";
                        break;
                    case '6':
                        mensajeAyuda += "séptima";
                        break;
                    case '&':
                        mensajeAyuda += "séptima";
                        break;
                    case '¬':
                        mensajeAyuda += "séptima";
                        break;
                    case '7':
                        mensajeAyuda += "octava";
                        break;
                    case '/':
                        mensajeAyuda += "octava";
                        break;
                    case '8':
                        mensajeAyuda += "novena";
                        break;
                    case '(':
                        mensajeAyuda += "novena";
                        break;
                    case '9':
                        mensajeAyuda += "décima";
                        break;
                    case ')':
                        mensajeAyuda += "décima";
                        break;
                    case '0':
                        mensajeAyuda += "onceava";
                        break;
                    case '=':
                        mensajeAyuda += "onceava";
                        break;
                    case '\'':
                        mensajeAyuda += "doceava";
                        break;
                    case '?':
                        mensajeAyuda += "doceava";
                        break;
                    case '¡':
                        mensajeAyuda += "treceava";
                        break;
                    case '¿':
                        mensajeAyuda += "treceava";
                        break;
                }

                mensajeAyuda += " tecla, comenzando a contar desde el borde izquierdo";

                if (swShift) mensajeAyuda += " apretando con la otra mano la tecla shift";
                if (swAlt) mensajeAyuda += " apretando con la otra mano la tecla alt";
            }
            else
            {

                mensajeAyuda += "Tenés que usar el dedo ";

                if (swEsp) //si es el espacio
                {
                    if (swPulgar) mensajeAyuda += "pulgar en la tecla larga que está bien abajo";
                }
                else
                {
                    if (swMeñique) mensajeAyuda += "meñique ";
                    if (swAnular) mensajeAyuda += "anular ";
                    if (swMedio) mensajeAyuda += "medio ";
                    if (swíndice) mensajeAyuda += "índice ";
                    if (swPulgar) mensajeAyuda += "pulgar ";

                    if (swManoDer) mensajeAyuda += "de la mano derecha, ";
                    if (swManoIzq) mensajeAyuda += "de la mano izquierda, ";

                    if (swfilaSup) mensajeAyuda += "en la fila de arriba";
                    else if (swfilaMedio) mensajeAyuda += "en la fila del medio";
                    else if (swfilaInf) mensajeAyuda += "en la fila de abajo";

                    if (swALaDerecha) mensajeAyuda += ", bien a la derecha";
                    if (swALaIzquierda) mensajeAyuda += ", bien a la izquierda";

                    if (swShift) mensajeAyuda += ", apretando con la otra mano la tecla shift";
                    if (swAlt) mensajeAyuda += ", apretando con la otra mano la tecla alt";
                }
            }

            if (mensajeAyuda == "") mensajeAyuda = "No sé cómo ayudarte con la tecla que estás buscando";

            return mensajeAyuda;
        }

        private void btnEstadísticas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            abrirEstadísticas();
        }

        private static void abrirEstadísticas()
        {
            VerEstadísticas ventanaEst = new VerEstadísticas();
            ventanaEst.ShowDialog();
        }

	}
}