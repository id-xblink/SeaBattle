#pragma checksum "..\..\AuthW.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "11DE0022059B7A6B4070059BCF4D22166D1F4382"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
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
using System.Windows.Shell;
using WpfSeaTest;


namespace WpfSeaTest {
    
    
    /// <summary>
    /// AuthW
    /// </summary>
    public partial class AuthW : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\AuthW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TBlockSwap;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\AuthW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TBLogin;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\AuthW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PBPass;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\AuthW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PBRepPass;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\AuthW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BGo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfSeaTest;component/authw.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AuthW.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TBlockSwap = ((System.Windows.Controls.Label)(target));
            
            #line 18 "..\..\AuthW.xaml"
            this.TBlockSwap.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.TBlockSwap_PreviewMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TBLogin = ((System.Windows.Controls.TextBox)(target));
            
            #line 21 "..\..\AuthW.xaml"
            this.TBLogin.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.Field_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PBPass = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 22 "..\..\AuthW.xaml"
            this.PBPass.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.Field_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.PBRepPass = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 23 "..\..\AuthW.xaml"
            this.PBRepPass.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.Field_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BGo = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\AuthW.xaml"
            this.BGo.Click += new System.Windows.RoutedEventHandler(this.BGo_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

