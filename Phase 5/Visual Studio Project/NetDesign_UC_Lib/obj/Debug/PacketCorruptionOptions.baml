   M S B A M L   `   `   `  ����K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  5      .    (NetDesign_UC_Lib.PacketCorruptionOptions   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:NetDesign_UC_Lib     5      -   5        $
��Root��5      .��5      �� .+�5	   
   �� $��Corrupt Ack Packets��6   $��3q�61   $��Center=�6<     d� 	IsChecked  5
      �� *&$packetErrorInfo.AckCorruptionEnabled+ �� ElementName$
 Root��]� 5   
   $��3q�6   .+�5      �� $
��Left=�6   $��Center=�6,   
������ 
������ ��5      ��  �� Path$ Value�$	 CC1�� �� StringFormat$ F2��{�5      �� *&$packetErrorInfo.AckCorruptionEnabled+$
 Root���� 5      -   5        $	��CC1��5   -    � Minimum$ 0Q�6    � Maximum$ 1Q�6!   $��5,0,0,0q�68    �� TickFrequency$
 0.01Q�5       �� IsSnapToTickEnabled .  � Value 5      �� *%#packetErrorInfo.AckCorruptionChance+$
 Root��{�5      �� *&$packetErrorInfo.AckCorruptionEnabled+$
 Root��5      �� 5   
   $��Corrupt Data Packets��6   $��3q�62   $��Center=�6=     5      �� *'%packetErrorInfo.DataCorruptionEnabled+$
 Root��]� 5   
   $��3q�6   .+�5      �� $
��Left=�6   $��Center=�6,   
������ 
������ ��5      �� $ Value�$	 CC2��$ F2��	 �� UpdateSourceTrigger$	 PropertyChanged=�{�5      �� *'%packetErrorInfo.DataCorruptionEnabled+$
 Root���� 5      -   5        $	��CC2��5   -   $ 0Q�6   $ 1Q�6!   $��5,0,0,0q�68   $
 0.01Q�5       .  5      �� *&$packetErrorInfo.DataCorruptionChance+$
 Root��{�5       �� *'%packetErrorInfo.DataCorruptionEnabled+$
 Root��5!      �� 5#   
   $��Drop Ack Packets��6   $��3q�6.   $��Center=�69     5$      �� * packetErrorInfo.AckDropEnabled+$
 Root��]� 5%   
   $��3q�6   .+�5&      �� $
��Left=�6   $��Center=�6,   
������ 
������ ��5(      �� $ Value�$	 CC3��$ F2��{�5)      �� * packetErrorInfo.AckDropEnabled+$
 Root���� 5*      -   5        $	��CC3��5*   -   $ 0Q�6   $ 1Q�6!   $��5,0,0,0q�68   $
 0.01Q�5+       .  5,      �� *packetErrorInfo.AckDropChance+$
 Root��{�5-      �� * packetErrorInfo.AckDropEnabled+$
 Root��5.      �� 50   
   $��Drop Data Packets��6   $��3q�6/   $��Center=�6:     51      �� *!packetErrorInfo.DataDropEnabled+$
 Root��]� 52   
   $��3q�6   .+�53      �� $
��Left=�6   $��Center=�6,   
������ 
������ ��55      �� $ Value�$	 CC4��$ F2��{�56      �� *!packetErrorInfo.DataDropEnabled+$
 Root���� 57      -   5        $	��CC4��57   -   $ 0Q�6   $ 1Q�6!   $��5,0,0,0q�68   $
 0.01Q�58       .  59      �� * packetErrorInfo.DataDropChance+$
 Root��{�5:      �� *!packetErrorInfo.DataDropEnabled+$
 Root��5;      5<      5=      