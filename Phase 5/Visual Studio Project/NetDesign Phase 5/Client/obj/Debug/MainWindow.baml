   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5   	   K GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM8clr-namespace:NetDesign_UC_Lib;assembly=NetDesign_UC_LibNetDesign_UC_Lib 5   	       Client.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Client     5   	   Guc8clr-namespace:NetDesign_UC_Lib;assembly=NetDesign_UC_Lib     5   	   -   5          � Title$  Client��5	   	   $	��700��6   $	��600��6%   .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��3q�6   $��2��6   ��5      �� 5      $��EndPoint Selector��6   $
��Bold"�65   .��5      �� .+�5      '  !NetDesign_UC_Lib.EndPointSelector     	LocalPort 6&   �� *Port+   DestinationIp 6A   �� *
RemoteIp+   DestinationPort 6d   �� *
RemotePort+�� 5      -   5        $��Open Socket��5      $
��Left=�6B   $��3q�6]   $��2q�6h   5      5      �� 5    
   $��1��6   $��2��6    ��5!      �� 5"      $��
Corruption��6   $
��Bold"�6.   .��5$      .  (NetDesign_UC_Lib.PacketCorruptionOptions     packetErrorInfo 6)   �� *PacketErrorInfo+5%      �� 5'   
   $��3q�6   $��2��6   $��2��6+   ��5(      �� 5)      $��
Send Files��6   $
��Bold"�6.   .��5+      #  NetDesign_UC_Lib.TransferFile  -   5          Name$ TransferFileUserControl��5+         SendPercentage 6r   �� *PacketSendPercentage+5,      �� 5.   
   $��3q�6   $��3��6   $��3��6+   ��5/      �� 50      $	��Log��6   $
��Bold"�6'   .��52        NetDesign_UC_Lib.TcpLog  -   5        $	��Log��52      53      56      57      