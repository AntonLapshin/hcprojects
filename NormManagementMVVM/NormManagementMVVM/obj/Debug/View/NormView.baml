   M S B A M L   `   `   `  ����i  eGalaSoft.MvvmLight.Extras.WPF4, Version=3.0.0.19993, Culture=neutral, PublicKeyToken=1673db7d5906b0adoPclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Extras.WPF4GalaSoft.MvvmLight.Command  5   	    NormManagementMVVM&    NormManagementMVVM.View.NormView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   a ]DevExpress.Xpf.Core.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   qGalaSoft_MvvmLight_CommandPclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Extras.WPF4  5   	     � Title$H  BУправление нормативами вместимости��5
   L   $	��600��6w   $	��800��6�    � DataContext 6	   ��  �� Path$
 Main�   Locator �� Source# [  .��5      �  � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      $��25 �6   6*   �� 5      $��5 �6   6)   �� 5      $��* �6   6)   .+�5   
   � -   5        $��
headerGrid��5      $��0��6!   ��5      �� 5       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5      �  � Color$ 	#FF93CAEF��6#    � Offset$ 0Q�65   � 5      $ 	#FF619ED1��6#   $ 1Q�65   5      	 � ColumnDefinitions	 5      �� 5      $��* �6#   �� 5      $��30 �6#   �� 5      $	��300 �6#   .+�5       �� $��Center=�6<   $��Center=�6W   $��0��6t   
�������$��14(�6�   ��6   �� *Head.ProfileContent+6�   �� 5!      $��Stretch=�6   
 � ToolTip$
 Сохранить��63   $��1��6G   $��Stretch=�6W   $��0,0,0,0q�6s   .��5"      �� $��..\Images\ok.ico��6   5#      )  #DevExpress.Xpf.Editors.ComboBoxEdit  5$      -   5        $��
cmbProfile��5$   �   $��Stretch=�6E   $��14(�6c   $��2��6q   )  #DevExpress.Xpf.Editors.TextEditBase   Text$% Выберите профиль��6�   $%
 Выберите профиль��6�   %  DevExpress.Xpf.Editors.BaseEdit   
IsReadOnly .  '  !DevExpress.Xpf.Editors.ButtonEdit   IsTextEditable .  |�.  +  %DevExpress.Xpf.Editors.LookUpEditBase   ItemsSource 6   �� $ Profiles�.  (System.Windows.Interactivity.Interaction   Triggers 5%      /  )System.Windows.Interactivity.EventTrigger  5&         	EventName$ SelectedIndexChanged��6%   .  (System.Windows.Interactivity.TriggerBase   Actions. 5'      /	   )GalaSoft.MvvmLight.Command.EventToCommand	   	  CommandParameter 6D   �� *SelectedItem+ �� ElementName$ 
cmbProfile�� 	  Command 6�   �� $ CommandChangeProfile� �� Mode$ OneWay=�5(      5*      5+      � 5-   
   $��1��6   
������� 63   �� $ Head.Representation�.+�5.      �� $��
Сброс��6f   $��50��6v   $��Stretch=�6�   $
��Left=�6�   $��0q�6�   y�6   �� $ Main.HeadVisibility�# [  ��6�   �� $ CommandReset��� 5/      $��50,0,0,0q�6L   ��6   �� $ RepresentationCells���50      �� 51      .R�52      � 	 53      �� 54   "   $��50 �63   �� 55   "   $��50 �63    57      �� 58   "   $��13 �60   �� 59   "   $��12 �60   .+�5?      	� $��0��6*   $��3��6:    	� 	GroupName$ RepresentationButtons��6K    d� 	IsChecked 6m   �� $ 	IsChecked���5@   "   �� 5A   &   .R�5B   *   d� $��0q�67   $��16(�6C   $	��↓��6Q    6]   �� $ 	IsChecked� �� RelativeSource �� *TemplatedParent+$ TwoWay=�5C   '   5E      �� 5G      $��50��6$    ^� ShowOnDisabled . $��1��6T   $��Center=�6d   $��0��6�   $��3��6�   �� 5H      $	��...��6%   $��Right=�63   
�������$��1��6b   $��1��6r   $��16��6   $��11��6�   $��0q�6�   5I      5J      ��5L      �� 5M      .R�5N      ��  �� Orientation$ 
Horizontal=�6%   5O      5Q      5R      �� 5T   
   -   5        $��scrollViewer��5T      $
��Auto=�6+   $
��Auto=�6N   $��3��5U      ��. .��5V      �� -   5        $��
RowControl��5V   (   $��0��6   ��6:   �� $ 	Head.Rows���5W      �� 5X      .R�5Y      �� $ 
Horizontal=�6%   $��50��6>   .+�5Z      �� $��btnPlus��6S   $��50��6b   $��50��6n   $��0q�6y   ��6%   �� $ CommandAddController�.��5[   "   �� $��..\Images\edit_add.png��6(   $
 Добавить��6H   6]   5\      �� 5]      ��6+   �� $ Cells���5^   "   �� 5_   &   .R�5`   *   �� ��61   �� $ 	CellColor�._�5a   .   � {�63   �� $ 	IsEnabled�	 5b   2   �� 5c   6   $��50 �6G   �� 5d   6   $��50 �6G    5f   2   �� 5g   6   $��12 �6D   �� 5h   6   $��* �6D   �� 5i   6   $��12 �6D   .+�5k   2   �� $��btnParam��69    . $��0��6�   $��3��6�   $��0q�5l   M   
 5k   n   �� $ 	ParamName���6�   �� $ 	ParamName���5l      �� $ CommandEditParameter�
 � Tag 6=   �� 6M   �� 5q   2   $��1��68   $'
 !Удалить разбиение��6H   $��0��6d   $��Right=�6q   $��..\Images\error.ico��6�   y�6�   �� $ RemoveButtonVisibility� 5r   6     5s   :   $ 	MouseDown��6I   . 5t   >   	   6h   �� *Tag+$ btnParam�� 6�   �� $ CommandRemoveController�$ OneWay=�5u   ;   5w   3   �� 5y   2   $��lblParamValues��68   $��50��6N    . $��1��5z      $��Center=�6   $��Center=�6;   $��0��6V   $��3��6c   $��0q�6t   
 5y   ~   �� $ ParamValues���6�   �� $ ParamValues�5z   t   �� 5{   2   $��btnParamValues��69   $��Right=�6O   
�������$��1��6~   $��2��5|      $��11��6   $��11��6'   y�5{   �   �� $ ValuesButtonVisibility���5|   3   �� $  CommandEditParameterValues�5}   /   5~   +   5   '   ��5�   "   �� 5�   &   .R�5�   *   �� $ 
Horizontal=�65   5�   '   5�      � 5�      $��50��6#   $	��401��6/   	 5�   "   �� 5�   &   $��50 �67   �� 5�   &   $��50 �67   �� 5�   &   $��61 �67   �� 5�   &   $��60 �67   �� 5�   &   $��60 �67    5�   "   �� 5�   &   $��25 �64   �� 5�   &   $��* �64   .+�5�   "   �� $��Маркер��6,   $��0��6:   $��0��6G   $��Center=�6W   $��Center=�6r   $��12(�6�    �� Padding$ 0q�6�   $
��Wrap=�6�   �� 5�   "   $��
НеАсс��6,   $��0��69   $��1��6F   $��Center=�6V   $��Center=�6q   $��12(�6�   $ 0q�6�   $
��Wrap=�6�   �� 5�   "   $��Асс��6,   $��0��67   $��2��6D   $��Center=�6T   $��Center=�6o   $��12(�6�   $ 0q�6�   $
��Wrap=�6�   �� 5�   "   $!��НеАсс + АссМакс��6,   $��0��6C   $��3��6P   $��Center=�6`   $��Center=�6{   $��10(�6�   $ 0q�6�   $
��Wrap=�6�   �� 5�   "   $��АссМакс��6,   $��0��6;   $��4��6H   $��Center=�6X   $��Center=�6s   $��10(�6�   $ 0q�6�   $
��Wrap=�6�   %
  DevExpress.Xpf.Editors.SpinEdit
  5�   "   $��	maxColumn��6L   $��1��6/   $��0��6<    
  IsFloatValue .   
  MinValue$ 0X�6�    6r   �� $ 	MaxColumn�6�   
  5�   "   $��delta��6L   $��1��6/   $��1��6<    .  $ 0X�6�    6n   �� $ Delta�6�   
  5�   "   $	��sku��6L   $��1��6/   $��2��6<    .  $ 0X�6�    6l   �� $	 Sku�6�   %  DevExpress.Xpf.Editors.TextEdit  5�   "   $��deltaPlusSku��6L   $��1��6/   $��3��6<    .  6a   �� $ ValueDeltaPlusSku�$ OneWay=�6�     5�   "   $��skuMax��6L   $��1��6/   $��4��6<    .  6Z   �� $ SkuMax�$ OneWay=�6�   5�      5�      5�      5�      5�      �� 5�   
   
���   � J� Opacity$	 0.7Q�6$   $��Stretch=�62   $��Stretch=�6N   $��0��6l   $��4��6y   y�6�   �� $
 Load�._�5�      �� $��Center=�6   $��CEnter=�63   $��Загрузка...��6P   
�������
���   �$ 5q�6�   6�   5�      5�      5�      