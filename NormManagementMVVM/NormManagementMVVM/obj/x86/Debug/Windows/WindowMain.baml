   M S B A M L   `   `   `  ����  NormManagementMVVM+    %NormManagementMVVM.Windows.WindowMain   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    69   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   f bDevExpress.Xpf.PivotGrid.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49aBdxpg7http://schemas.devexpress.com/winfx/2008/xaml/pivotgrid  5   	   -   5          � Title$1  +Управление нормативами��5   @   $	��600��6_   $	��800��6l    � WindowState$ 	Maximized=�5   N    � 	Resources 5         PivotGridFilterTemplate �� Path �� Mode�� 5      %I  &	        �� 5      .R�5   
   � .+�5      �� $��..\Images\filter.gif��6   $
��None=�60   $
��Left=�6?   $��Center=�5	      $��	0,-1,2,-1q�6-   �� 5
      $��labelHeader��6   $��0,0,0,0q�6%   $��Right=�5      
���������5
   7   �� $ DisplayText�$ OneWay=�5   C   5      5   	   5      .��5      �  � RowDefinitions 5      �� 5      $��20 �6   6$   �� 5      $��* �6   6#   .+�5      o� -   5        $��menuMain��5      $��0��6   .@�5      m� -   5        $
��Fill��5      $��Разбиение��6   6D   m� 5      $��Параметры��6   .@�5   
   m� -   5        $+��%Промежуточные итоги��5       m� IsCheckable .  m� 	IsChecked .  5      m� 5   
   -   5        $��Общие итоги��5       .  . 6L   5   	   m� 5      -   5        $��menuItemExport��5      $!��Выгрузить в Excel��6'   6B   m� 5      -   5        $��menuItemFieldsList��5      $��Stretch=�6+   $��Stretch=�6I    .  $��22��5   %    .  $��25��6C    J� Opacity$	 0.5Q�6N   	 m� Icon	 5    
   �� 5!      $��..\Images\fields_list.png��6   $��16��65   $��16��6A   $��Center=�6L   $
��Left=�5"      5$   	   m� 5%      $��Справочники��6   .@�5&   
   m� -   5        $��menuItemStoreEquip��5&      $4��.Магазин/Тип оборудования��6-   6O   m� 5'   
   -	   5        $��menuItemProfile��5'      $��Профили��6*   6;   m� 5(   
   -
   5        $��menuItemEquipType��5(      $%��Тип Оборудования��6,   6F   m� 5)   
   -   5        $��menuItemParameters��5)      $��Параметры��6-   6@   5*   	   m� 5+      -   5        $��menuItemRefreshData��5+      $#��Обновить данные��6,   6E   5,      /  )DevExpress.Xpf.PivotGrid.PivotGridControl  5-      -   5        $��normPivotGridControl��5-      $��1��68   
   RowTotalsLocation$	
 Far=�6E   5.      5/      