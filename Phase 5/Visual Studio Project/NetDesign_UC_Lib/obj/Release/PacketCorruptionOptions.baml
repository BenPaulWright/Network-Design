   M S B A M L   `   `   `  ���� K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  .    (NetDesign_UC_Lib.PacketCorruptionOptions   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:NetDesign_UC_Lib     -   $
��Root��.���� .+��� $��Corrupt Ack Packets��$��3q�$��Center=�  d� 	IsChecked  �� *&$packetErrorInfo.AckCorruptionEnabled+ �� ElementName$
 Root��]� $��3q�.+��� $
��Left=�$��Center=�
������ 
������ ����  �� Path$ Value�$	 CC1�� �� StringFormat$ F2��{��� *&$packetErrorInfo.AckCorruptionEnabled+$
 Root���� -   $	��CC1�� � Minimum$ 0Q� � Maximum$ 1Q�$��5,0,0,0q� �� TickFrequency$
 0.01Q� �� IsSnapToTickEnabled .  � Value �� *%#packetErrorInfo.AckCorruptionChance+$
 Root��{��� *&$packetErrorInfo.AckCorruptionEnabled+$
 Root���� $��Corrupt Data Packets��$��3q�$��Center=�  �� *'%packetErrorInfo.DataCorruptionEnabled+$
 Root��]� $��3q�.+��� $
��Left=�$��Center=�
������ 
������ ���� $ Value�$	 CC2��$ F2��	 �� UpdateSourceTrigger$	 PropertyChanged=�{��� *'%packetErrorInfo.DataCorruptionEnabled+$
 Root���� -   $	��CC2��$ 0Q�$ 1Q�$��5,0,0,0q�$
 0.01Q� .  �� *&$packetErrorInfo.DataCorruptionChance+$
 Root��{��� *'%packetErrorInfo.DataCorruptionEnabled+$
 Root���� $��Drop Ack Packets��$��3q�$��Center=�  �� * packetErrorInfo.AckDropEnabled+$
 Root��]� $��3q�.+��� $
��Left=�$��Center=�
������ 
������ ���� $ Value�$	 CC3��$ F2��{��� * packetErrorInfo.AckDropEnabled+$
 Root���� -   $	��CC3��$ 0Q�$ 1Q�$��5,0,0,0q�$
 0.01Q� .  �� *packetErrorInfo.AckDropChance+$
 Root��{��� * packetErrorInfo.AckDropEnabled+$
 Root���� $��Drop Data Packets��$��3q�$��Center=�  �� *!packetErrorInfo.DataDropEnabled+$
 Root��]� $��3q�.+��� $
��Left=�$��Center=�
������ 
������ ���� $ Value�$	 CC4��$ F2��{��� *!packetErrorInfo.DataDropEnabled+$
 Root���� -   $	��CC4��$ 0Q�$ 1Q�$��5,0,0,0q�$
 0.01Q� .  �� * packetErrorInfo.DataDropChance+$
 Root��{��� *!packetErrorInfo.DataDropEnabled+$
 Root��