   M S B A M L   `   `   `  ����   AssortmentManagement6    0AssortmentManagement.Windows.WindowAuthorization   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -   $��windowAuthorization��  � Title$M  GУправление ассортиментом. Авторизация��$	��206��$	��400�� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � AllowsTransparency .  � Icon$. (/AssortmentManagement;component/icon.ico����Z� ��Z� .���  � RowDefinitions �� $��25 ��� $��25 ��� $��25 ��� $��45 ��� $��44 ��� $��3* � � ColumnDefinitions �� $	��200 ��� $��* �.+��� -   $��
rectangle1��$��2��$��4��$��0,0,0,0q����	#FF5F5F5F �� RadiusX$ 5�� �� RadiusY$ 5������ 	 �� EndPoint$	 0.5,1&�
 �� 
StartPoint$
 0.5,0&�.���  � Color$ 	#FFEBEBEB�� � Offset$ 0Q�� $ 	#FFBABABA��$ 1Q��� -   $��
labelLogin��$%��Имя пользователя��$��0��$��1��$��Right=��� -   $��labelPassword��$��Пароль��$��0��$��2��$��Right=��� -   $��	listLogin��$��1��$��1��$	��150��$
��Left=�$��22��G� -   $��txtPassword��$��1��$��2��$	��150��$
��Left=�$��22���� -   $��	btnSubmit��$��1��$��3��$��
Войти��$	��Top=�$
��Left=�$��75��$��22��$��0,3,0,0q��� -   $��image1��$��0,3,5,2q�$6��0/AssortmentManagement;component/Images/close.png��$��16��$��1��$��Right=��� -	   $��labelStatus��$��3��$r��lПодождите, идёт инициализация глобальных временных таблиц��$��Right=�$��Center=�$	��190��$��40��$
��Wrap=�$y�Hidden=�$��WordEllipsis=� �� TextAlignment$ Right=�$��0,5,5,0q����	#FF3B3A3A � Triggers :�  :� RoutedEvent$ TextBlock.Loaded��.[��� .���� .��W�  �� 
TargetName$ labelStatus�� �� TargetProperty$ Opacity� W� From$ 1X�	 W� To$	 0.2X� g� Duration$ 0:0:1D� g� AutoReverse .  g� RepeatBehavior$ Forever���� -
   $��	imageLogo��$��3��$��64��$��64��$
��Left=�$	��Top=�$6��0/AssortmentManagement;component/Images/rubik.png��$��8,8,0,0q��� -   $��labelVersion��$��Версия 12.08.2011��$��0,0,0,0q�$��3��$��0,0,5,5q�$��11(����	#FF383535$��1��$��Right=�$��95��$��14��$��Bottom=�$��Calibri)�$
��Bold"��� $��4��$��2��$	��276��$��44��$��Center=�$:��4/AssortmentManagement;component/Images/main_logo.png���� -   $��	labelName��$!��Рабочий сервер��$��0,0,0,0q�$��
59,10,52,1q�$��11(�
���   �$��14��$��Calibri)�$y�Visible=�