﻿#pragma checksum "..\..\administrarActividades.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "23FC3F84233AB0A92ACF0E13EB09A586"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.239
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Aprender_Teclado;
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
    /// administrarActividades
    /// </summary>
    public partial class administrarActividades : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\administrarActividades.xaml"
        internal Aprender_Teclado.administrarActividades Window;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\administrarActividades.xaml"
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\administrarActividades.xaml"
        internal System.Windows.Controls.ListBox listActividades;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\administrarActividades.xaml"
        internal Aprender_Teclado.etiquetaLink linkModificarActividad;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\administrarActividades.xaml"
        internal Aprender_Teclado.etiquetaLink linkEliminarActividad;
        
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
            System.Uri resourceLocater = new System.Uri("/Aprender-Teclado;component/administraractividades.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\administrarActividades.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Window = ((Aprender_Teclado.administrarActividades)(target));
            
            #line 8 "..\..\administrarActividades.xaml"
            this.Window.KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.listActividades = ((System.Windows.Controls.ListBox)(target));
            return;
            case 4:
            this.linkModificarActividad = ((Aprender_Teclado.etiquetaLink)(target));
            return;
            case 5:
            this.linkEliminarActividad = ((Aprender_Teclado.etiquetaLink)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
