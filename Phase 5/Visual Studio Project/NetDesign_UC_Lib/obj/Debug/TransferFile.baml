   M S B A M L   `   `   `  ����K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  5      #    NetDesign_UC_Lib.TransferFile   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:NetDesign_UC_Lib     5      U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullA	matDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      -   5        $
��Root��5      .��5	      �   � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
      NetDesign_UC_Lib.FileDrop     NetDesign_UC_Lib.FileDrop   FilesDropped$ OnFilesDropped��6   $��3q�67   ]� 5   
   $��1��6   .+�5      ]� $	��Top=�6   .+�5      �� $��Center=�6e   $
��Left=�6�   ��6   �� *SendPercentage+ �� ElementName$
 Root�� �� StringFormat$ {0:0%}��6�   � 5      $��20��6   $��3q�6*    � Minimum$ 0Q�65    � Maximum$ 1Q�6A    � Value 6M   �� *SendPercentage+$
 Root��5      !   NetDesign_UC_Lib.IconButton  5      !   NetDesign_UC_Lib.IconButton   Click$
 Send��6y   $	��Top=�6   $��3q�67   $��Right=�6B      Kind$
 Send=�6Y   	 � ToolTip$	 	Send File��6e   {�6�   �� *SendButtonEnabled+$
 Root��   NetDesign_UC_Lib.FileList  5      $��3q�6[   
   FileDataObjects
 6   �� *FileDataObjects+$
 Root��6[   5      5      5      