﻿#pragma checksum "..\..\Lecciones.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8D99CF66AD790DB6C16D5B4B205F6CDB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.239
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
    /// Lecciones
    /// </summary>
    public partial class Lecciones : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 5 "..\..\Lecciones.xaml"
        internal Aprender_Teclado.Lecciones Window;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\Lecciones.xaml"
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\Lecciones.xaml"
        internal System.Windows.Controls.ListBox listLecciones;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\Lecciones.xaml"
        internal System.Windows.Controls.Button btnEmpezar;
        
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
            System.Uri resourceLocater = new System.Uri("/Aprender-Teclado;component/lecciones.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Lecciones.xaml"
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
            this.Window = ((Aprender_Teclado.Lecciones)(target));
            
            #line 7 "..\..\Lecciones.xaml"
            this.Window.KeyUp += new System.Windows.Input.KeyEventHandler(this.Window_KeyUp);
            
            #line default
            #line hidden
            
            #line 7 "..\..\Lecciones.xaml"
            this.Window.Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.listLecciones = ((System.Windows.Controls.ListBox)(target));
            
            #line 10 "..\..\Lecciones.xaml"
            this.listLecciones.KeyDown += new System.Windows.Input.KeyEventHandler(this.listLecciones_KeyDown);
            
            #line default
            #line hidden
            
            #line 10 "..\..\Lecciones.xaml"
            this.listLecciones.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.listLecciones_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnEmpezar = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\Lecciones.xaml"
            this.btnEmpezar.Click += new System.Windows.RoutedEventHandler(this.btnEmpezar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
