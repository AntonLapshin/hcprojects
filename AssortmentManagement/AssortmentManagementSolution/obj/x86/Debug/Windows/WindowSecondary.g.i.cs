﻿#pragma checksum "..\..\..\..\Windows\WindowSecondary.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "791F856F63427C2B24C12FFE59977E07"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.1
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using AssortmentManagement.Controls;
using AssortmentManagement.Converters.PivotGridControlModified;
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
    /// WindowSecondary
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class WindowSecondary : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 50 "..\..\..\..\Windows\WindowSecondary.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Menu menuMain;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Windows\WindowSecondary.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemNum;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\Windows\WindowSecondary.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemFieldsList;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\Windows\WindowSecondary.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemsAdd;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\Windows\WindowSecondary.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemUndo;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\Windows\WindowSecondary.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuItemRedo;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\..\Windows\WindowSecondary.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AssortmentManagement.Controls.PivotGridControlModified _pivotGridControl2;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\Windows\WindowSecondary.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridInfo;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\Windows\WindowSecondary.xaml"
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
            System.Uri resourceLocater = new System.Uri("/AssortmentManagement;component/windows/windowsecondary.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\WindowSecondary.xaml"
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
            
            #line 51 "..\..\..\..\Windows\WindowSecondary.xaml"
            this.menuItemNum.Click += new System.Windows.RoutedEventHandler(this.MenuItemFieldsListClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 57 "..\..\..\..\Windows\WindowSecondary.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DocumentSaveHandler);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 58 "..\..\..\..\Windows\WindowSecondary.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DocumentDescription);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 59 "..\..\..\..\Windows\WindowSecondary.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemAssortmentAddClick);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 62 "..\..\..\..\Windows\WindowSecondary.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemSummaryClick);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 63 "..\..\..\..\Windows\WindowSecondary.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemSummaryTotalClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.menuItemFieldsList = ((System.Windows.Controls.MenuItem)(target));
            
            #line 65 "..\..\..\..\Windows\WindowSecondary.xaml"
            this.menuItemFieldsList.Click += new System.Windows.RoutedEventHandler(this.MenuItemFieldsListClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.menuItemsAdd = ((System.Windows.Controls.MenuItem)(target));
            
            #line 71 "..\..\..\..\Windows\WindowSecondary.xaml"
            this.menuItemsAdd.Click += new System.Windows.RoutedEventHandler(this.MenuItemItemsAddClick);
            
            #line default
            #line hidden
            return;
            case 10:
            this.menuItemUndo = ((System.Windows.Controls.MenuItem)(target));
            
            #line 76 "..\..\..\..\Windows\WindowSecondary.xaml"
            this.menuItemUndo.Click += new System.Windows.RoutedEventHandler(this.MenuItemUndoClick);
            
            #line default
            #line hidden
            return;
            case 11:
            this.menuItemRedo = ((System.Windows.Controls.MenuItem)(target));
            
            #line 81 "..\..\..\..\Windows\WindowSecondary.xaml"
            this.menuItemRedo.Click += new System.Windows.RoutedEventHandler(this.MenuItemRedoClick);
            
            #line default
            #line hidden
            return;
            case 12:
            this._pivotGridControl2 = ((AssortmentManagement.Controls.PivotGridControlModified)(target));
            return;
            case 13:
            this.gridInfo = ((System.Windows.Controls.Grid)(target));
            return;
            case 14:
            
            #line 91 "..\..\..\..\Windows\WindowSecondary.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.GridInfoMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 15:
            this.labelInfo = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

