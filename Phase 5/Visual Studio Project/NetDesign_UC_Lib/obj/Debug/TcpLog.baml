   M S B A M L   `   `   `  ����K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  5          NetDesign_UC_Lib.TcpLog   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:NetDesign_UC_Lib     5      U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullA	matDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      -   5        $
��Root��5        � 	Resources  5	       �� 
TargetType   LogViewerStyle �� Property �� Value K� IsItemsHost �� DataType   NetDesign_UC_Lib.LogEntry � IsSharedSizeScope � ColumnDefinitions�� 5
   
   %]  &	        (�� �     5      *, +)�� 5      " ��6   .�5      ��  � �� 5      �� 5      .R�5      �� ��. .��5   "   �� 5      5      5      �� 5       � �� 5      �� 5      .R�5      $�  . 65   5      5      5      �� 5      "  6   .R�5       �  .  5!      �� 5"      $
��Auto �6+   �� 5#      .+�5&      �� $��2q�6    �� 5'      �6"   �� *
DateTime+ �� 5(      $�>��6"   5)      �� 5+      $��2q�6    $��1��6D   $
��Wrap=�6T   ��6+   �� *	Message+6T   5,      5-      5.      .��51      �  � RowDefinitions 52   
   �� 53      $��* �6   �� 54      $
��Auto �6   .+�57   
   �� #��[  6L   ��6   �� *
LogEntries+	 �� ElementName$
	 Root����58      �� 59      .R�5:      �� ��. .��5;      �� 5<      5=      ��5?      �� 5@      .R�5A      $�  . 6-   5B      5D      �� 5F   
   
 �� Orientation$
 
Horizontal=�6   $��Right=�6.   $��1��6J   .+�5G      !   NetDesign_UC_Lib.IconButton  !   NetDesign_UC_Lib.IconButton   Click$ Clear��6@      Kind$ Clear=�6    � ToolTip$ 	Clear Log��6,   6@     5H      $
 Save��6E   $ ContentSave=�6   $ Save Log��62   6E   5I      5J      5K      