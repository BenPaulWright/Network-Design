   M S B A M L   `   `   `  ���� K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  #    NetDesign_UC_Lib.TransferFile   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:NetDesign_UC_Lib     U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullA	matDesign1http://materialdesigninxaml.net/winfx/xaml/themes  -   $
��Root��.���   � RowDefinitions  �� $
��Auto ��� $��* �.+�   NetDesign_UC_Lib.FileDrop     NetDesign_UC_Lib.FileDrop   FilesDropped$ OnFilesDropped��$��3q�]� $��1��.+�]� $	��Top=�.+��� $��Center=�$
��Left=����� *SendPercentage+ �� ElementName$
 Root�� �� StringFormat$ {0:0%}��� $��20��$��3q� � Minimum$ 0Q� � Maximum$ 1Q� � Value �� *SendPercentage+$
 Root��!   NetDesign_UC_Lib.IconButton  !   NetDesign_UC_Lib.IconButton   Click$
 Send��$	��Top=�$��3q�$��Right=�   Kind$
 Send=�	 � ToolTip$	 	Send File��{��� *SendButtonEnabled+$
 Root��   NetDesign_UC_Lib.FileList  $��3q�
   FileDataObjects
 �� *FileDataObjects+$
 Root��