   M S B A M L   `   `   `  ����K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  5      $    NetDesign_UC_Lib.SettingsPanel   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:NetDesign_UC_Lib     5      -   5        $
��Root��5      .��5      �   � RowDefinitions  5	   
   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Packet Size��6   $��5q�6+   $
��Left=�66   �� 5      $��Center=�6   
������ 
������ ��6k   ��  �� ElementName$ PacketSizeSlider�� �� Path$ Value��� 5      $��Center=�6   $��0,5,5,5q�63   $��Italic$�6D   $��B��6W   5      ]� 5   
   $��1��6   .+�5      �� $
��Left=�6   $��Center=�6.   $��5q�6I   �� 5      �6   �� *	Minimum+$ PacketSizeSlider�� �� 5      $�B��6   5      �� 5      $��Right=�6   $��Center=�6/   $��5q�6J   �� 5       �6   �� *	Maximum+$ PacketSizeSlider�� �� 5!      $�B��6   5"      �� 5$      -   5         ��Name$ PacketSizeSlider��5$       � Minimum$	 512Q�6/    � Maximum$
 2048Q�6=   $	��150��6{   $��5q�6�    � Value 6L   �� *
PacketSize+$
 Root��6�   5%      54      55      