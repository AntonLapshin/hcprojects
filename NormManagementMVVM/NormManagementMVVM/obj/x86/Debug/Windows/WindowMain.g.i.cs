﻿#pragma checksum "..\..\..\..\Windows\WindowMain.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0933412F06A7064514C234034FE8B322"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.261
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Xpf.PivotGrid;
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


namespace NormManagement.Windows {
    
    
    /// <summary>
    /// WindowMain
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class WindowMain : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\Windows\WindowMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Menu menuMain;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\Windows\WindowMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Fill;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Windows\WindowMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemExport;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Windows\WindowMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemFieldsList;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Windows\WindowMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemStoreEquip;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Windows\WindowMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemProfile;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Windows\WindowMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemEquipType;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Windows\WindowMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemParameters;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Windows\WindowMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.PivotGrid.PivotGridControl normPivotGridControl;
        
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
            System.Uri resourceLocater = new System.Uri("/NormManagement;component/windows/windowmain.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\WindowMain.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.menuMain = ((System.Windows.Controls.Menu)(target));
            return;
            case 2:
            this.Fill = ((System.Windows.Controls.MenuItem)(target));
            
            #line 23 "..\..\..\..\Windows\WindowMain.xaml"
            this.Fill.Click += new System.Windows.RoutedEventHandler(this.FillClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 25 "..\..\..\..\Windows\WindowMain.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemSummaryClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 26 "..\..\..\..\Windows\WindowMain.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemSummaryTotalClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.menuItemExport = ((System.Windows.Controls.MenuItem)(target));
            
            #line 28 "..\..\..\..\Windows\WindowMain.xaml"
            this.menuItemExport.Click += new System.Windows.RoutedEventHandler(this.MenuItemExportClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.menuItemFieldsList = ((System.Windows.Controls.MenuItem)(target));
            
            #line 29 "..\..\..\..\Windows\WindowMain.xaml"
            this.menuItemFieldsList.Click += new System.Windows.RoutedEventHandler(this.MenuItemFieldsListClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.menuItemStoreEquip = ((System.Windows.Controls.MenuItem)(target));
            
            #line 35 "..\..\..\..\Windows\WindowMain.xaml"
            this.menuItemStoreEquip.Click += new System.Windows.RoutedEventHandler(this.menuItemEquip_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.menuItemProfile = ((System.Windows.Controls.MenuItem)(target));
            
            #line 36 "..\..\..\..\Windows\WindowMain.xaml"
            this.menuItemProfile.Click += new System.Windows.RoutedEventHandler(this.menuItemProfile_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.menuItemEquipType = ((System.Windows.Controls.MenuItem)(target));
            
            #line 37 "..\..\..\..\Windows\WindowMain.xaml"
            this.menuItemEquipType.Click += new System.Windows.RoutedEventHandler(this.menuItemEquipType_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.menuItemParameters = ((System.Windows.Controls.MenuItem)(target));
            
            #line 38 "..\..\..\..\Windows\WindowMain.xaml"
            this.menuItemParameters.Click += new System.Windows.RoutedEventHandler(this.menuItemParameters_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.normPivotGridControl = ((DevExpress.Xpf.PivotGrid.PivotGridControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
