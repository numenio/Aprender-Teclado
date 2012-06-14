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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para medidor.xaml
	/// </summary>
    public partial class medidor : UserControl
    {
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public double StrokeThickness { get; set; }

        int valorARestar = 13;

        bool swArcoLargo = false;
        //double aciertos, errores;
        
        public medidor()
        {
            this.InitializeComponent();
            //aciertos = misAciertos;
            //errores = misErrores;
            circuloErrores.Visibility = Visibility.Hidden;
        }

        public void actualizarGráfico(double aciertos, double errores, Brush pincel)
        {
            if (this.RenderSize.Width > 0)
            {
                Size radius = new Size(this.RenderSize.Width / 2 - valorARestar, this.RenderSize.Height / 2 - valorARestar);

                Point inicioAciertos = new Point(this.RenderSize.Width / 2, valorARestar);// circuloErrores.Margin.Top); 
                Point finAciertos = calcularCoordenadas(aciertos, errores);
                DrawArc(inicioAciertos, finAciertos, radius, swArcoLargo, pincel);
                circuloErrores.Visibility = Visibility.Visible;
            }
        }

        void DrawArc(Point start, Point end, Size radius, bool swArcoLargo, Brush pincel)
        {
            Canvas miCanvas = new Canvas();
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            geometry.Figures.Add(figure);
            figure.StartPoint = start;

            figure.Segments.Add(new ArcSegment(end, radius, 0, swArcoLargo, SweepDirection.Clockwise, true));
            System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
            myPath.Stroke = pincel;//System.Windows.Media.Brushes.Green;
            myPath.StrokeThickness = 5;
            myPath.Data = geometry;
            gridGráfico.Children.Clear();
            gridGráfico.Children.Add(myPath);

        }

        //private void UserControl_MouseMove(object sender, MouseEventArgs e)
        //{
        //    etiqueta.Text = "x: " + Mouse.GetPosition(UserControl).X + ", y: " + Mouse.GetPosition(UserControl).Y;
        //}

        Point calcularCoordenadas(double aciertos, double errores)
        {
            double total = aciertos + errores;
            double punto = 60 / total;
            double puntosAciertos = aciertos * punto;

            Point coordenadaFinalAciertos =  new Point();

            if (puntosAciertos < 5) //------------> el cero, arriba de todo
                coordenadaFinalAciertos = new Point(this.RenderSize.Width / 2, 15);

            if (puntosAciertos >= 5 && puntosAciertos < 10)
                coordenadaFinalAciertos = new Point(76.3, 18.6);

            if (puntosAciertos >= 10 && puntosAciertos < 15)
                coordenadaFinalAciertos = new Point(94.4, 34.6);

            if (puntosAciertos >= 15 && puntosAciertos < 20) //------------> un cuarto, el 3 del reloj
                coordenadaFinalAciertos = new Point(100, 54.6);

            if (puntosAciertos >= 20 && puntosAciertos < 25)
                coordenadaFinalAciertos = new Point(94.4, 77.6);

            if (puntosAciertos >= 25 && puntosAciertos < 30)
                coordenadaFinalAciertos = new Point(79, 94);

            if (puntosAciertos >= 30 && puntosAciertos < 35)
                coordenadaFinalAciertos = new Point(this.RenderSize.Width / 2, this.RenderSize.Height - valorARestar); //-------------> la mitad, el 6 del reloj

            if (puntosAciertos >= 35 && puntosAciertos < 40)
                coordenadaFinalAciertos = new Point(34.4, 94);

            if (puntosAciertos >= 40 && puntosAciertos < 45)
                coordenadaFinalAciertos = new Point(18.3, 77);

            if (puntosAciertos >= 45 && puntosAciertos < 50) // -----------> tres cuartos, el 9 del reloj
                coordenadaFinalAciertos = new Point(13.4, 54.6);

            if (puntosAciertos >= 50 && puntosAciertos < 55)
                coordenadaFinalAciertos = new Point(19, 34.6);

            if (puntosAciertos >= 55 && puntosAciertos < 59)
                coordenadaFinalAciertos = new Point(35, 18.6);

            if (puntosAciertos >= 59)
                coordenadaFinalAciertos = new Point((this.RenderSize.Width / 2) - 1, valorARestar); //------------> fin, retorno al cero

            if (puntosAciertos >= 30)
                swArcoLargo = true;
            else
                swArcoLargo = false;

            return coordenadaFinalAciertos;
        }


        //double aciertos = 0, errores = 5;
        //private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    aciertos++;
        //    actualizarGráfico(aciertos, errores);
        //}
    }
}