   M S B A M L   `   `   `  ����M  INormManagementMVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:NormManagementMVVM.ViewModelNormManagementMVVM.ViewModel  5          NormManagementMVVM.App   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    6/   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6E   @	ViewModel*clr-namespace:NormManagementMVVM.ViewModel     5      @d2http://schemas.microsoft.com/expression/blend/2008     6K     �� 	Resources  5      3   -NormManagementMVVM.ViewModel.ViewModelLocator   Locator  GlassButton �� 
TargetType � 	Resources  Key g� 	BeginTime �� 
TargetName �� TargetProperty O� KeyTime O� Value �� CornerRadius	 ��Name
 � RowDefinitions J� Opacity �� RelativeTransform �� ScaleX �� ScaleY �� AngleX �� AngleY �� Angle T� X T� Y � Color � Offset �� EndPoint �� 
StartPoint �� Triggers P� Property P� Value �� 
TargetName �� Property �� Value N� EnterActions  	Timeline1 N� ExitActions  	Timeline2  control-emboss  � SpreadMethod��%(	  &	        &	      7  7  &	       5   
   6%   �� 5   
   " ��6.    5      ���� 5      	Timeline1 .��5      U� $ 00:00:00X�64   $
 glow��6I   $ (UIElement.Opacity) �5   *   .\�5      �� $ 00:00:00.3000000��6/   $ 1 Q�6J   5      5      �� 5!      	Timeline2 .��5#      U� $ 00:00:00X�64   $
 glow��6I   $ (UIElement.Opacity) �5$   *   .\�5&      �� $ 00:00:00.3000000��6/   $ 0 Q�6J   5(      5*      5.      .R�54      �� ���	#FFFFFFFF$��1,1,1,1q�6-   $ 4,4,4,4 ��6G   ._�58      �� $	 border��6   ���	#FF42689E
����  �$��1,1,1,1q�6W   $ 4,4,4,4 ��59      ._�5=      � 
 5?      �� 5A      $��0.507*  �6,   �� 5C      $��0.493*  �6,   .+�5K      �� $
	 glow��6K   $ 0Q�6!   $��Stretch=�6-   $
��Auto��6Y   $��2��6f   $ 4,4,4,4 ��5L      ��5P      
� 5T   "    5X   &   V� 5Z   *   .~�5\   .   �� $ 1.702Q�6=   $ 2.243 Q�6L   �� 5^   .   $ 0Q�6<   $ 0 Q�6G   �� 5`   .   $ 0Q�6>   T� 5b   .   $  -0.368Q�6A   $ -0.152 Q�6M   5d   +   .��5j   &   � $ 	#D58DBDFF��63   $ 0 Q�6E   � 5l   &   $ 	#338DBDFF��63   $ 1 Q�6E   5n   #   5r      �� 5z      $��Center=�6+   $��Center=�6H   $
��Auto��6c   $��2 ��5{      �� 5      $	 shine��6P   $��Stretch=�6!   $��0,0,0,0q�6?   $
��Auto��6_   $ 4,4,0,0 ��5�      ��5�      �� 5�   "   $ 0.494,0.889&�66   $ 0.494,0.028 &�6M   .��5�   &   � $ 	#BBFFFFFF��63   $ 0 Q�6E   � 5�   &   $ 	#55FFFFFF��63   $ 1 Q�6E   5�   #   5�      5�      5�      5�       5�      P� 5�       � �� . . �5�      �� $ shine��60    �@J�Opacity$
 0.4 Q�6C   �� 5�      $ border��63    � �� �	#FF6288BE6G   �� 5�      $
 glow��63    � y�$ Hidden =�6E   5�      P� 5�       � {� .  . �5�      ��  �@J�Opacity$
 0.7 Q�60   5�      P� 5�       �@J�IsMouseOver .  5�      �� 5�      8��  6*    5�      �� 5�      8�� 6*   5�      5�      �� 5�   
   $ 	0.5,1.002&�65   $ 
0.5,-0.002&�6J   $	  Pad=�6b   .��5�      � $ 	#3E4C6EB3��6   $ 0Q�6-   � 5�      $ 	#80EDF4FF��6   $ 1Q�6-   � 5�      $ 	#3F7098DC��6   $ 0.215Q�6-   5�      5�      5�      