   M S B A M L   `   `   `  ����K  GNetDesign_UC_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:NetDesign_UC_LibNetDesign_UC_Lib  5      '    !NetDesign_UC_Lib.EndPointSelector   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:NetDesign_UC_Lib     5      -   5          >�Name$
  Root��5   3   .��5      �  � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Ip��6   $��3q�6   �� 5   
   $��3q�5      $��1��6   ��5      �� *AvailableIps+ �� ElementName$
 Root�� �� SelectedItem 5      �� *Ip+$
 Root��5      �� 5   
   $
��Port��6   $��3q�6    $��1��6+   �� 5   
   $��3q�5      $��1��6   $��1��6*   ��5      �� *Port+$
 Root��5   *   5      5      