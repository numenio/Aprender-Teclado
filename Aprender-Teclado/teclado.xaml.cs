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

namespace Aprender_Teclado
{
	/// <summary>
	/// Lógica de interacción para teclado.xaml
	/// </summary>
	public partial class teclado : UserControl
	{	
		public teclado()
		{
			this.InitializeComponent();
			//números
			this._0.texto.Text="0";
			this._1.texto.Text="1";
			this._2.texto.Text="2";
			this._3.texto.Text="3";
			this._4.texto.Text="4";
			this._5.texto.Text="5";
			this._6.texto.Text="6";
			this._7.texto.Text="7";
			this._8.texto.Text="8";
			this._9.texto.Text="9";
			
			//escape y Fs
			this.escape.texto.Text="Esc";
			this.F1.texto.Text="F1";
			this.F2.texto.Text="F2";
			this.F3.texto.Text="F3";
			this.F4.texto.Text="F4";
			this.F5.texto.Text="F5";
			this.F6.texto.Text="F6";
			this.F7.texto.Text="F7";
			this.F8.texto.Text="F8";
			this.F9.texto.Text="F9";
			this.F10.texto.Text="F10";
			this.F11.texto.Text="F11";
			this.F12.texto.Text="F12";

            //letras
            this.a.texto.Text = "a";
            this.b.texto.Text = "b";
            this.c.texto.Text = "c";
            this.d.texto.Text = "d";
            this.e.texto.Text = "e";
            this.f.texto.Text = "f";
            this.g.texto.Text = "g";
            this.h.texto.Text = "h";
            this.i.texto.Text = "i";
            this.j.texto.Text = "j";
            this.k.texto.Text = "k";
            this.l.texto.Text = "l";
            this.m.texto.Text = "m";
            this.n.texto.Text = "n";
			this.ñ.texto.Text = "ñ";
            this.o.texto.Text = "o";
            this.p.texto.Text = "p";
            this.q.texto.Text = "q";
            this.r.texto.Text = "r";
            this.s.texto.Text = "s";
            this.t.texto.Text = "t";
            this.u.texto.Text = "u";
            this.v.texto.Text = "v";
            this.w.texto.Text = "w";
            this.x.texto.Text = "x";
            this.y.texto.Text = "y";
            this.z.texto.Text = "z";
            this.enter.texto.Text = "Ent";
            this.borrar.texto.Text = "Bor";
            this.espacio.texto.Text = "Espacio";
            this.acento.texto.Text = "´";
            this.tab.texto.Text = "Tab";
			this.coma.texto.Text = ",";
			this.punto.texto.Text = ".";
			this.guión.texto.Text = "-";
			this.bloqMay.texto.Text = "May";
			this.shift.texto.Text = "Shift";
			this.shift1.texto.Text = "Shift";
			this.control1.texto.Text = "Ctrl";
			this.control.texto.Text = "Ctrl";
		}
	}
}