   M S B A M L   `   `   `  ����D  @SeaBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:WpfSeaTest
WpfSeaTest  5   	       WpfSeaTest.LeaderW   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:WpfSeaTest     5   	   -   5          � Title$  	SeaBattle��5   	   $	��450��6   $	��400��6(    � WindowStartupLocation$ CenterOwner=�6K    � 
ResizeMode$ NoResize=�6o   .��5	      �  � RowDefinitions 5
   
   �� 5      $	��115 �6   �� 5      $��40 �6   �� 5      �� 5      $
��auto �6   .+�5   
   �� -   5        $��0��5      $	��100��6   $	��100��6)   $��Assets/logo.ico��66   $��0 10 0 5q�6O   6a   � 5   
   $��1��6   $��5q�6    5      �� 5      $��30 �6    .+�5      � $��0 0 16 0q�6    � ColumnDefinitions 5      �� 5      $
��auto �6'   �� 5      �� 5      .+�5      �� $��0��6   $��
Место��6,   $��16(�69   $
��Bold"�6G   $��Center=�6Y   $��Center=�6v   $
��10 0q�6�   �� 5       $��1��6   $��
Логин��6,   $��16(�69   $
��Bold"�6G   $��Center=�6Y   $��Center=�6v   $
��10 0q�6�   �� 5!      $��2��6   $��Очки��6,   $��16(�68   $
��Bold"�6F   $��Center=�6X   $��Center=�6u   $
��10 0q�6�   5"      5#      �� 5%   
   $��2��6   $��Visible=�6$   $��5q�6J   .��5&      � -   5         �Name$ GList��5&      5'      �� 5)   
   -   5         ��Name$ BBack��5)      $��3��6   $��
Назад��6-   $��15q�6=   $��Center=�6I   $��Center=�6f   6�   5*      5+      