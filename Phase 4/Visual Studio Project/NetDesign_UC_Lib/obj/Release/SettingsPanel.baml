   M S B A M L   `   `   `  ���� K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  $    NetDesign_UC_Lib.SettingsPanel   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:NetDesign_UC_Lib     -   $
��Root��.���   � RowDefinitions  �� $
��Auto ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $
��Auto ��� $��* �.+���  �� Orientation$ 
Horizontal=�.+��� $��Packet Size��$��5q�$
��Left=��� $��Center=�
������ 
������ ����  �� ElementName$ PacketSizeSlider�� �� Path$ Value��� $��Center=�$��0,5,5,5q�$��Italic$�$��B��]� $��1��.+��� $
��Left=�$��Center=�$��5q��� ��� *	Minimum+$ PacketSizeSlider�� �� $�B���� $��Right=�$��Center=�$��5q��� ��� *	Maximum+$ PacketSizeSlider�� �� $�B���� -    ��Name$ PacketSizeSlider�� � Minimum$	 512Q� � Maximum$
 2048Q�$	��150��$��5q� � Value �� *
PacketSize+$
 Root��