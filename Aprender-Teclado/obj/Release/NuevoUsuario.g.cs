﻿#pragma checksum "..\..\NuevoUsuario.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "500AB1BF4F1D6A4C20E6AD19D927F7DF"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.235
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Aprender_Teclado {
    
    
    /// <summary>
    /// NuevoUsuario
    /// </summary>
    public partial class NuevoUsuario : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 5 "..\..\NuevoUsuario.xaml"
        internal Aprender_Teclado.NuevoUsuario Window;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\NuevoUsuario.xaml"
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\NuevoUsuario.xaml"
        internal System.Windows.Controls.TextBox txtNombre;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\NuevoUsuario.xaml"
        internal System.Windows.Controls.Button btnAceptar;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\NuevoUsuario.xaml"
        internal System.Windows.Controls.Button btnCancelar;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\NuevoUsuario.xaml"
        internal System.Windows.Controls.TextBox txtApellido;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Aprender-Teclado;component/nuevousuario.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\NuevoUsuario.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Window = ((Aprender_Teclado.NuevoUsuario)(target));
            
            #line 7 "..\..\NuevoUsuario.xaml"
            this.Window.KeyUp += new System.Windows.Input.KeyEventHandler(this.Window_KeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.txtNombre = ((System.Windows.Controls.TextBox)(target));
            
            #line 10 "..\..\NuevoUsuario.xaml"
            this.txtNombre.GotFocus += new System.Windows.RoutedEventHandler(this.txtNombre_GotFocus);
            
            #line default
            #line hidden
            
            #line 10 "..\..\NuevoUsuario.xaml"
            this.txtNombre.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtNombre_KeyUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnAceptar = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\NuevoUsuario.xaml"
            this.btnAceptar.Click += new System.Windows.RoutedEventHandler(this.btnAceptar_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnCancelar = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\NuevoUsuario.xaml"
            this.btnCancelar.Click += new System.Windows.RoutedEventHandler(this.btnCancelar_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtApellido = ((System.Windows.Controls.TextBox)(target));
            
            #line 14 "..\..\NuevoUsuario.xaml"
            this.txtApellido.GotFocus += new System.Windows.RoutedEventHandler(this.txtApellido_GotFocus);
            
            #line default
            #line hidden
            
            #line 14 "..\..\NuevoUsuario.xaml"
            this.txtApellido.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtApellido_KeyUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

