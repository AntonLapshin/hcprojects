﻿#pragma checksum "..\..\..\..\Windows\WindowBase.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "857E31229D905DD315AFBB09A166E07C"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.269
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using AssortmentManagement;
using AssortmentManagement.Controls;
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


namespace AssortmentManagement.Windows {
    
    
    /// <summary>
    /// WindowBase
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class WindowBase : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 65 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Menu menuMain;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemNum;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemRegularDocCreate;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemOperativeDocCreate;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemConsumedItemDocCreate;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemRegister;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemFieldsList;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AssortmentManagement.Controls.PivotGridControlModified _pivotGridControl1;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridInfo;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\..\Windows\WindowBase.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelInfo;
        
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
            System.Uri resourceLocater = new System.Uri("/AssortmentManagement;component/windows/windowbase.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\WindowBase.xaml"
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
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.menuMain = ((System.Windows.Controls.Menu)(target));
            return;
            case 2:
            this.menuItemNum = ((System.Windows.Controls.MenuItem)(target));
            
            #line 66 "..\..\..\..\Windows\WindowBase.xaml"
            this.menuItemNum.Click += new System.Windows.RoutedEventHandler(this.MenuItemFieldsListClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.menuItemRegularDocCreate = ((System.Windows.Controls.MenuItem)(target));
            
            #line 73 "..\..\..\..\Windows\WindowBase.xaml"
            this.menuItemRegularDocCreate.Click += new System.Windows.RoutedEventHandler(this.MenuItemRegularDocCreateClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.menuItemOperativeDocCreate = ((System.Windows.Controls.MenuItem)(target));
            
            #line 74 "..\..\..\..\Windows\WindowBase.xaml"
            this.menuItemOperativeDocCreate.Click += new System.Windows.RoutedEventHandler(this.MenuItemOperativeDocCreateClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.menuItemConsumedItemDocCreate = ((System.Windows.Controls.MenuItem)(target));
            
            #line 75 "..\..\..\..\Windows\WindowBase.xaml"
            this.menuItemConsumedItemDocCreate.Click += new System.Windows.RoutedEventHandler(this.MenuItemExpendMaterialDocClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.menuItemRegister = ((System.Windows.Controls.MenuItem)(target));
            
            #line 77 "..\..\..\..\Windows\WindowBase.xaml"
            this.menuItemRegister.Click += new System.Windows.RoutedEventHandler(this.MenuItemRegisterClick);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 80 "..\..\..\..\Windows\WindowBase.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemSummaryClick);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 81 "..\..\..\..\Windows\WindowBase.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemSummaryTotalClick);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 84 "..\..\..\..\Windows\WindowBase.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemFiltersItems);
            
            #line default
            #line hidden
            return;
            case 10:
            this.menuItemFieldsList = ((System.Windows.Controls.MenuItem)(target));
            
            #line 86 "..\..\..\..\Windows\WindowBase.xaml"
            this.menuItemFieldsList.Click += new System.Windows.RoutedEventHandler(this.MenuItemFieldsListClick);
            
            #line default
            #line hidden
            return;
            case 11:
            this._pivotGridControl1 = ((AssortmentManagement.Controls.PivotGridControlModified)(target));
            return;
            case 12:
            this.gridInfo = ((System.Windows.Controls.Grid)(target));
            return;
            case 13:
            
            #line 98 "..\..\..\..\Windows\WindowBase.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.GridInfoMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 14:
            this.labelInfo = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

