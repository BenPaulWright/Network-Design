   M S B A M L   `   `   `  ����K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  5      .    (NetDesign_UC_Lib.PacketCorruptionOptions   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:NetDesign_UC_Lib     5      -   5        $
��Root��5      .��5      �� .+�5	   
   �� $��Corrupt Ack Packet��6   $��3q�60   $��Center=�6;     d� 	IsChecked  5
      �� *AckPacketCorruptionEnabled+ �� ElementName$
 Root��]� 5   
   $��3q�6   .+�5      �� $
��Left=�6   $��Center=�6.   ��5      ��  �� Path$ Value�$	 CC1�� �� StringFormat$ {0:0%}���� 5      -   5        $	��CC1��5   -    � Minimum$ 0Q�6    � Maximum$ 1Q�6!   $��5,0,0,0q�68    � Value 5      �� *AckPacketCorruptionChance+$
 Root��{�5      �� *AckPacketCorruptionEnabled+$
 Root��5      �� 5   
   $��Corrupt Data Packet��6   $��3q�61   $��Center=�6<     5      �� *DataPacketCorruptionEnabled+$
 Root��]� 5   
   $��3q�6   .+�5      �� $
��Left=�6   $��Center=�6.   ��5      �� $ Value�$	 CC2��$ {0:0%}���� 5      -   5        $	��CC2��5   -   $ 0Q�6   $ 1Q�6!   $��5,0,0,0q�68    5      �� *DataPacketCorruptionChance+$
 Root��{�5      �� *DataPacketCorruptionEnabled+$
 Root��5      5      5      