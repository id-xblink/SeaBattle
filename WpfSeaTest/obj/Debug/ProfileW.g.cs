#pragma checksum "..\..\ProfileW.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7B3450CBEF51C6797DBE3D3D1E8AE8ABAAA7B9F9"
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
    /// ProfileW
    /// </summary>
    public partial class ProfileW : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 41 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DPStart;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DPEnd;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CBSolo;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CBDuo;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CBOnline;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CBVictory;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CBDefeat;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BLeader;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BExit;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BAll;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BOne;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\ProfileW.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DGShow;
        
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
            System.Uri resourceLocater = new System.Uri("/SeaBattle;component/profilew.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ProfileW.xaml"
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
            
            #line 9 "..\..\ProfileW.xaml"
            ((WpfSeaTest.ProfileW)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DPStart = ((System.Windows.Controls.DatePicker)(target));
            
            #line 41 "..\..\ProfileW.xaml"
            this.DPStart.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.DatePicker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.DPEnd = ((System.Windows.Controls.DatePicker)(target));
            
            #line 42 "..\..\ProfileW.xaml"
            this.DPEnd.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.DatePicker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.CBSolo = ((System.Windows.Controls.CheckBox)(target));
            
            #line 59 "..\..\ProfileW.xaml"
            this.CBSolo.Click += new System.Windows.RoutedEventHandler(this.CheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.CBDuo = ((System.Windows.Controls.CheckBox)(target));
            
            #line 60 "..\..\ProfileW.xaml"
            this.CBDuo.Click += new System.Windows.RoutedEventHandler(this.CheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.CBOnline = ((System.Windows.Controls.CheckBox)(target));
            
            #line 61 "..\..\ProfileW.xaml"
            this.CBOnline.Click += new System.Windows.RoutedEventHandler(this.CheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CBVictory = ((System.Windows.Controls.CheckBox)(target));
            
            #line 77 "..\..\ProfileW.xaml"
            this.CBVictory.Click += new System.Windows.RoutedEventHandler(this.CheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CBDefeat = ((System.Windows.Controls.CheckBox)(target));
            
            #line 78 "..\..\ProfileW.xaml"
            this.CBDefeat.Click += new System.Windows.RoutedEventHandler(this.CheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.BLeader = ((System.Windows.Controls.Button)(target));
            
            #line 93 "..\..\ProfileW.xaml"
            this.BLeader.Click += new System.Windows.RoutedEventHandler(this.BLeader_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.BExit = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\ProfileW.xaml"
            this.BExit.Click += new System.Windows.RoutedEventHandler(this.BExit_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.BAll = ((System.Windows.Controls.Button)(target));
            
            #line 95 "..\..\ProfileW.xaml"
            this.BAll.Click += new System.Windows.RoutedEventHandler(this.BAll_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.BOne = ((System.Windows.Controls.Button)(target));
            
            #line 96 "..\..\ProfileW.xaml"
            this.BOne.Click += new System.Windows.RoutedEventHandler(this.BOne_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.DGShow = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

