   M S B A M L   `   `   `  ����  NormManagementMVVM:    4NormManagementMVVM.Windows.WindowEditParameterValues   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   a ]DevExpress.Xpf.Core.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  5   	   Fdxlc;http://schemas.devexpress.com/winfx/2008/xaml/layoutcontrol  5   	     � Title$<  6Изменение значений параметра��5   K   $	��360��6p   $	��520��5   	    � WindowStartupLocation$ CenterOwner=�6    � 
ResizeMode$ CanMinimize=�5   R   .��5      �  � ColumnDefinitions 5      �� 5	      �� 5
      �� 5      .+�5      (  "DevExpress.Xpf.Editors.ListBoxEdit  -   5        $��listBoxAllParams��5         AllowItemHighlighting .    DisplayMember$
 NAME��6K      ValueMember$ VALUE��6`      SelectionMode$ Extended=�5      {�. $��0,32,0,0q�6@   6T   �� 5      ���	#FF6288BE$��1,1,1,1q�6%   $��4��6?   ._�5      �� -   5        $��scrollViewer��5      $
��Auto=�6)   $
��Auto=�6L   .��5   
   �� -   5        $��	rowParams��5       �� Orientation$ Vertical=�6&   $��Stretch=�6=   5      5   	   5      �� 5      -   5        $��	groupBox1��5   #   $��Управление��6   $��1��64   
�������
�������.��5      � 	 � RowDefinitions	 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�5    
   �� -   5        $��btnRight��5          GlassButton#���   6!   $!��Создать группу��6J   $��35��5!   '   $	��115��63   $��0��6?   
 J� Opacity$
 1Q�6L   $��Center=�6X   $��Center=�5"      
�������6.   �� 5#   
   -   5        $��btnGroupBack��5#      #���   6%   $!��Удалить группу��6N   $��Center=�5$      $��Center=�6,   $��35��6c   $	��115��6o   $��1��6{   
�������5%      �� 5&   
   -   5        $��btnLeft��5&      #���   6    $#��Удалить элемент��6I   $��Center=�5'      $��Center=�6,   $��35��6^   $	��115��6j   $��2��6v   
�������5(      �� 5)   
   -   5        $��btnAll��5)      #���   6   $��Перенести всё��6H   $��Center=�5*      $��Center=�6,   $��35��6]   $	��115��6i   $��3��6u   
�������5+      �� 5,   
   -	   5        $��
btnAllBack��5,      #���   6#   $��Удалить всё��6L   $��Center=�5-      $��Center=�6,   $��35��6a   $	��115��6m   $��4��6y   
�������5.      �� 5/   
   -
   5        $��btnOk��5/       �� 	IsDefault . #���   6/   $��OK��6X   $��Center=�50      $��Center=�6,   $��35��6\   $��40��6h   $��5��6s   
�������51      52   	   53      )  #DevExpress.Xpf.Editors.TrackBarEdit  54      -   5        $��trackBarEdit1��54   2   $
��Left=�6   $	��Top=�6G   $	��166��6_   $��26��55      *  $DevExpress.Xpf.Editors.RangeBaseEdit   Maximum$ 1Q�6#   %  DevExpress.Xpf.Editors.BaseEdit   NullText$ 0��6/   6<   57      58      