   M S B A M L   `   `   `  ����M  IReverseShellServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ReverseShellServerReverseShellServer  5   	   #    ReverseShellServer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:ReverseShellServer     5   	     � Title$  TCP Server Console��5   	   $��555.6��6$   $��1013.2��63   
�������.��5	      � -   5         �Name$
 grid��5	       � ShowGridLines .  � RowDefinitions 5
   
   �� 5      $	��20* �6   �� 5      $��40 �6    � ColumnDefinitions 5   
   �� 5      $��3* �6   �� 5      .+�5   
   �� $��10,10,10.2,5q�6   .��5      �� -   5         ��Name$ OutputConsole��5      ���	#FF2E2E2E���	#FFF3F3F3$��	Fira Code)�6[   $��5,10,5,6q�6r   $��> Awaiting Connection ...��6�    �� 	MaxLength$ 30��6�    �� TextWrapping$
 Wrap=�6�    �� 
IsReadOnly . 6�   5      �� 5   
   $��10,10,9.8,10q�6   
�������$
��Auto=�6>   $��2��6n   $��1��6�   .+�5      	� -   5        	 	�Name$	 CmdRadio��5      $	��cmd��6,   6:   	� 5      -   5        $	 PowerShellRadio��5      $��
PowerShell��63   6H   �� 5      -   5        
 ��Name$
 ClientNumLabel��5      $��Connected Clients: 0��6,   �� 5      $��4��6   $��	0,0,0.2,0q�6#   �� 5      -   5         ��Name$ 
ClientList��5      $	��312��6+   $��	0,0,0.2,0q�68    J� RenderTransformOrigin$ 0.5,0.5&�6K   ��5      �� 5       .R�5!      ��  �� Orientation$ 
Horizontal=�6%   .+�5"      �� -   5        ��5"   (   �� 69   5#      5$      5&      5'      ]� 5)   
   $��2��6   ���	#FF313131$��	10,0,10,0q�6<   $��1��6O   $��36��6\   $	��Top=�6h   .+�5*      �� $��18��6   $��$��6   
�������$��0q�6>   $��Center=�6I   $��30��6k   �� 5+      -   5        $ InputConsole��5+      $��0,0,5,0q�6,   $
 Wrap=�6=   $	��5,0q�6g   $��	Fira Code)�6u   $��30��6�   
�������$��Center=�6�   ���	#FF797979 �� SelectionBrush
 ����� �� 
CaretBrush
 �������6Q   Z� 6B  5,      �� 5-   
   -	   5         ��Name$ 
SendButton��5-      $
��Send��6%   $
��Left=�64   $��10,0,0,0q�6O   $��1��6a   $	��101��6n   $��1��6�   $��Center=�6�   5/      50      