   M S B A M L   `   `   `  ����  NormManagementMVVM2    ,NormManagementMVVM.Controls.NormativeControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      a ]DevExpress.Xpf.Core.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  5      .��5      �   � RowDefinitions  5      �� 5	      $��50 �6   6$   �� 5
      $��* �6   6#   .+�5      �� -   5        $��profileGrpBox��5   9   ���	#FF6288BE$��Профиль: ��6&   $��0��6N   $��Stretch=�6[   .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5        $��paramProfileLabel��5      $��Параметры:��6)   $��Center=�6>   $��0��6Y   ���	#FF6288BE5   )   �� 5      -   5        $��
buttonSave��5      $��Right=�6+    � ToolTip$ Сохранить��6G   $��1��6\      GlassButton#���   5      
�������.��5      �� $��..\Images\ok.ico��6   5      )  #DevExpress.Xpf.Editors.ComboBoxEdit  5   
   -   5        $��
cmbProfile��5   G   $��Right=�6   $��2��67   )  #DevExpress.Xpf.Editors.TextEditBase   Text$% Выберите профиль��5   J   %  DevExpress.Xpf.Editors.BaseEdit   
IsReadOnly .  '  !DevExpress.Xpf.Editors.ButtonEdit   IsTextEditable .  |�.  5   /   5   	   5       �� 5(      -   5        $��scrollViewer��5(      $
��Auto=�6'   $
��Auto=�6J   $��1��5)      ��. .��5*      �� -   5        $��	normPanel��5*       �� Orientation$ Vertical=�6$   $��Stretch=�6;   $��Stretch=�6W   6w   5+      5-      5.      