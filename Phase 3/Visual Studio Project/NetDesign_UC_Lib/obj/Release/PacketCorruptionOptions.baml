   M S B A M L   `   `   `  ���� K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  .    (NetDesign_UC_Lib.PacketCorruptionOptions   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:NetDesign_UC_Lib     -   $
��Root��.���� .+��� $��Corrupt Ack Packet��$��3q�$��Center=�  d� 	IsChecked  �� *AckPacketCorruptionEnabled+ �� ElementName$
 Root��]� $��3q�.+��� $
��Left=�$��Center=�����  �� Path$ Value�$	 CC1�� �� StringFormat$ {0:0%}���� -   $	��CC1�� � Minimum$ 0Q� � Maximum$ 1Q�$��5,0,0,0q� � Value �� *AckPacketCorruptionChance+$
 Root��{��� *AckPacketCorruptionEnabled+$
 Root���� $��Corrupt Data Packet��$��3q�$��Center=�  �� *DataPacketCorruptionEnabled+$
 Root��]� $��3q�.+��� $
��Left=�$��Center=����� $ Value�$	 CC2��$ {0:0%}���� -   $	��CC2��$ 0Q�$ 1Q�$��5,0,0,0q� �� *DataPacketCorruptionChance+$
 Root��{��� *DataPacketCorruptionEnabled+$
 Root��