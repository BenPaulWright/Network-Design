   M S B A M L   `   `   `  ���� A  =Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ServerServer  K GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM8clr-namespace:NetDesign_UC_Lib;assembly=NetDesign_UC_LibNetDesign_UC_Lib     Server.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     &localclr-namespace:Server     Guc8clr-namespace:NetDesign_UC_Lib;assembly=NetDesign_UC_Lib     -     � Title$  Server��$	��700��$	��600��.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* � � ColumnDefinitions �� $��* �.+��� $��3q�$��2������ $��EndPoint Selector��$
��Bold"�.���� .+�'  !NetDesign_UC_Lib.EndPointSelector     	LocalPort �� *Port+   DestinationIp �� *
RemoteIp+   DestinationPort �� *
RemotePort+�� -   $��Open Socket��$
��Left=�$��3q�$��2q��� $��1��$��2������ $��
Corruption��$
��Bold"�.��.  (NetDesign_UC_Lib.PacketCorruptionOptions     packetErrorInfo �� *PacketErrorInfo+�� $��3q�$��2��$��2������ $��
Send Files��$
��Bold"�.��#  NetDesign_UC_Lib.TransferFile  -     Name$ TransferFileUserControl��   SendPercentage �� *PacketSendPercentage+�� $��3q�$��3��$��3������ $	��Log��$
��Bold"�.��  NetDesign_UC_Lib.TcpLog  -   $	��Log��