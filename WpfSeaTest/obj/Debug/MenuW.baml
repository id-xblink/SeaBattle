   M S B A M L   `   `   `  ����D  @SeaBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:WpfSeaTest
WpfSeaTest  5   	       WpfSeaTest.MenuW   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:WpfSeaTest     5   	   U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  	SeaBattle��5	   	   $	��420��6   $	��380��6(    � WindowStartupLocation$ CenterScreen=�64    � 
ResizeMode$ NoResize=�6Y   .��5
      �  � RowDefinitions 5   
   �� 5      $	��115 �6   �� 5      �� 5      �� 5      �� 5      �� 5      .+�5   
   �� -   5        $	��100��5      $	��100��6   $��Assets/logo.ico��6)   $��0 10 0 5q�6B   6T   �� 5   
   -   5         ��Name$ BOnline��5      $��1��6   $	��250��6E   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 Wifi=�6*   $
��auto��66   $
��auto��6C   $
��10 0q�6Q   �� 5      $��По сети��6   $��Center=�6+   $��15(�6F   $��0 0 10 0q�6T   5      5      �� 5   
   -   5        $ BSolo��5      $��2��6   $	��250��6A   .��5      �� $ 
Horizontal=�6   .+�5        $ ComputerClassic=�6*   $
��auto��6A   $
��auto��6N   $
��10 0q�6\   �� 5      $'��!Против компьютера��6   $��Center=�65   $��15(�6P   $��0 0 10 0q�6^   5      5      �� 5    
   -   5        $
 BDuo��5       $��3��6   $	��250��6?   .��5!      �� $ 
Horizontal=�6   .+�5"        $ Keyboard=�6*   $
��auto��6:   $
��auto��6G   $
��10 0q�6U   �� 5#      $*��$На одном компьютере��6   $��Center=�67   $��15(�6R   $��0 0 10 0q�6`   5$      5%      �� 5&   
   -   5        $ BProfile��5&      $��4��6   $	��250��6G   .��5'      �� $ 
Horizontal=�6   .+�5(        $ Account=�6*   $
��auto��69   $
��auto��6F   $
��10 0q�6T   �� 5)      $��Профиль��6   $��Center=�6+   $��15(�6F   $��0 0 10 0q�6T   5*      5+      �� 5,   
   -   5        $ BExit��5,      $��5��6   $	��250��6A   .��5-      �� $ 
Horizontal=�6   .+�5.        $ 	ExitToApp=�6*   $
��auto��6;   $
��auto��6H   $
��10 0q�6V   �� 5/      $��
Выход��6   $��Center=�6)   $��15(�6D   $��0 0 10 0q�6R   50      51      52      53      