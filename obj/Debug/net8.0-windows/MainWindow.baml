   M S B A M L   `   `   `  ����  	WPF_CLock    WPF_CLock.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Clock��5   	   $	��500��6   $	��600��6$   .��5      � ��5	   
   �� 5
       �� ImageSource$ /Resources/BackGround.png��6   6C   .+�5   
   �� -   5        $��CultureTextBlock��5      $�� ��5      $��Right=�5      $	��Top=�5      $��10q�5      
�������$��14(�5      5      �� 5   
   -   5        $��LastApiCallTextBlock��5      $�� ��5      $
��Left=�5      $	��Top=�5      $��10q�5      
�������$��10(�5      5      ]� 5   
   .+�5      o� $��Bottom=�6   
�������
���   �.@�5       m� -   5         m�Name$ LocationMenuItem��5       $��	Locations��65   ��5!      �� 5"      .R�5#      �� 5$      ��5&      �� 5'       �� 
TargetType" m�6*   .R�5(      � .+�5)   "   � $��
PART_Popup��6(   $��Bottom=�5*   (    m� IsSubmenuOpen#��z 5+   (    � AllowsTransparency$
 True��5,   (   $|�False��5-   (   $
��Fade=�5.   (   .��5/   &   �� $	��200��63   $
��Auto=�6C   .��50   *   �� $��SubmenuBorder��61    J� SnapsToDevicePixels$
 True��51   1   #��� ��52   1   #��� ��53   1   $��1q�54   1   ._�55   .   ��  K� IsItemsHost$
 True��69   -  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation$ Cycle=�6L   56   +   57   '   58   #   �� 59   "   $��Header��63   $
��True��5:   3   #��z��5;   3   #��z��5<   3   	 �� HeaderStringFormat
 �� ContentStringFormat#
 z	 5=   3   $��6,1,6,1q�5>   3   5?      5@      5B      m� 5D      $��Format��6   .@�5E      m� -   5        $0��*Toggle date format MM-dd-yyyy / dd-MM-yyyy��5E      6S   m� 5F      -   5        $��Toggle Month format��5F      6<   m� 5G      -   5        $��Toggle 24/12 format��5G      6<   5H      m� 5I      $��Language��6   .@�5J      m� -   5        $#��Switch language / Skift sprog��5J      6F   5K      m� 5L      $��Settings��6   $��Right=�6-   .@�5M      m� -   5        $��API Key��5M      60   5N      5O      � 5P       � RowDefinitions 5Q      �� 5R      $��* �6$   �� 5S      $
��Auto �6$   .+�5U      �� $��0��6   $��Center=�6*   $��Center=�6G   .+�5V      �� -	   5        $��DateTextBox��5V       �� TextAlignment$ Center=�5W      
�������
������ 
���   �$��0q�5[      $��60(�5\       �� 
IsReadOnly$
 True��5]      �� 5^      -
   5        $��
DayTextBox��5^      $ Center=�5_      
�������
������ 
���   �$��0q�5c      $��50(�5d      $
 True��5e      �� 5f      -   5        $��TimeTextBox��5f      $ Center=�5g      
�������
������ 
���   �$��0q�5k      $��80(�5l      $
 True��5m      �� 5n      -   5        $��WeatherTextBox��5n      $ Center=�5o      
�������
������ 
���   �$��0q�5s      $��0,0,0,0q�5t      $��25(�5u      $
 True��5v      5w      �� 5x      -   5        $��NameTextBlock��5x   )   $��1��6   $
��Left=�5y      $��Bottom=�5z      $��10q�5{      
�������$��10(�5}      5~      5      5�      5�      