   M S B A M L   `   `   `  ����  NormManagementMVVM5    /NormManagementMVVM.View.EditParameterValuesView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   a ]DevExpress.Xpf.Core.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  5   	   $	��360��5   W   $	��520��5   	     � WindowStartupLocation$  CenterOwner=�6    � 
ResizeMode$ CanMinimize=�69    � DataContext 5   	   ��  �� Path$ ParameterValues�   Locator �� Source# [   � Title 5	   	   �� $ ParameterValues.Title�# [  .��5
      �  � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      .+�5   
   (  "DevExpress.Xpf.Editors.ListBoxEdit  -   5        $��listBoxAllParams��5         AllowItemHighlighting .    SelectionMode$ Extended=�5      {�. $��3,50,3,3q�6=   	   ItemsSource	 5   O   �� $ 
ValuesLeft�

 � Tag
 6w   �� 5      �� 5   
   
�������$��1,1,1,1q�6%   $��4��6?   ._�5      �� -   5        $��scrollViewer��5      $
��Auto=�6/   $
��Auto=�6R   .��5        $��3,3,3,3q�6K   	 6"   �� $ GroupsRight�   ItemTemplate 5      �� 5      .R�5        -   5        $��Stretch=�5   .   $ Extended=�6L   	 6e   �� 
 6}   �� $ ParameterValues�# [  5       5!      5#      5$      5%      �� 5'   
   -   5        $��	groupBox1��5'   '   $��Управление��6   $��1��68   
�������
�������.��5(      �  � RowDefinitions 5)      �� 5*      �� 5+      �� 5,      �� 5-      �� 5.      �� 5/      .+�52      �� -   5        $��btnRight��52        GlassButton#���  6)   $!��Создать группу��6R   $��35��53      $	��115��6   $��0��6*    J� Opacity$ 1Q�67   $��Center=�6C   $��Center=�54      
���������52   k   �� *CommandMoveRight+54   .   �� 56      -   5        $��btnGroupBack��56      #���  6-   $!��Удалить группу��6V   $��Center=�57      $��Center=�6,   $��35��6J   $	��115��6V   $��1��6b   
���������56   o   �� *CommandMoveLeftGroup+58      �� 5:      -   5        $��btnLeft��5:      #���  6(   $#��Удалить элемент��6Q   $��Center=�5;      $��Center=�6,   $��35��6I   $	��115��6U   $��2��6a   
���������5:   k   �� *CommandMoveLeft+5<      �� 5>      -   5        $��btnAll��5>      #���  6'   $��Перенести всё��6P   $��Center=�5?      $��Center=�6,   $��35��6I   $	��115��6U   $��3��6a   
���������5>   h   �� *CommandMoveRightAll+5@      �� 5B      -	   5        $��
btnAllBack��5B      #���  6+   $��Удалить всё��6T   $��Center=�5C      $��Center=�6,   $��35��6I   $	��115��6U   $��4��6a   
���������5B   j   �� *CommandMoveLeftAll+5D      �� 5F      -
   5        $��btnOk��5F       �� 	IsDefault . #���  67   $��OK��6`   $��Center=�5G      $��Center=�6,   $��35��6I   $��40��6U   $��5��6`   
���������5F   m   �� *CommandProcess+5H      5I      5J      �� 5K   
   $��Сортировка��6   $��	0,1,0,297q�6%   $��Center=�68   6W   �� 5L   
   $��	Назв.��6   $��13,22,115,276q�6    $��Center=�67   6V   �� 5M   
   $��Код��6   $��113,22,25,276q�6   $��Center=�65   6T   )  #DevExpress.Xpf.Editors.TrackBarEdit  5N   
   -   5        $��trackBarSort��5N   6   $
��Left=�6   $	��Top=�6J   $��50��6b   $��26��5O      *  $DevExpress.Xpf.Editors.RangeBaseEdit   Maximum$ 1Q�6#   %  DevExpress.Xpf.Editors.BaseEdit   NullText$ 0��6/   $��	59,23,0,0q�6i      Value 6<   �� $ SortType� �� Mode$ TwoWay=�6i   5P      5Q      