   M S B A M L   `   `   `  ����K  GGetLogInfoClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:GetLogInfoClientGetLogInfoClient  5   	   !    GetLogInfoClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:GetLogInfoClient     5   	     � Title$  日志程序客户端��5   	   $	��400��5	   	   $	��500��5
   	   .��5      ]� $��20q�6   .+�5   
   �� $	��Top=�6   .+�5      ]� $��0,0,0,10q�6   .+�5      �� $��包含内容    ：��6   $
��Left=�6-   6E   �� 5      -   5        ��5      �� *IncludeFilteText+ �� Mode$ TwoWay=�.��6h    � 	Resources 5         	HelpBrush j� TileMode �� Opacity j� Stretch j� 
AlignmentX !� Visual��%�   &	        !� 5      $
 None=�68   $	 0.3Q�6H   $
 None=�6V   $
 Left=�6e    5      �� 5   "   $��Italic$�6,   $G��A输入想包含的内容，多条内容以中文分号'；'隔开��6?   5      5      .��6)   ��5      �� 5       �� 
TargetType" ��6    	 �� Triggers	 5      P� 5   "   
 P� Property
 � �� P� Value 6:   Z� . �5   &   ��  �� Property � �� �� Value# [  6C   5   #   P� 5   "   
 � ��$  ��6:   . �5   &   ��  � ��# [  6C   5    #   5"      .��5#   %   5$      5%      ]� 5&      $��0,0,0,10q�6   .+�5'      �� $��不包含内容：��6   $
��Left=�6*   6C   �� 5(      -   5        ��5(      �� *UnIncludeFilteText+$ TwoWay=�.��6l    5)        
HelpBrush2��%�   &	       !� 5*      $
 None=�69   $	 0.3Q�6I   $
 None=�6W   $
 Left=�6f    5+      �� 5,   "   $��Italic$�6,   $J��D输入不想包含的内容，多条内容以中文分号'；'隔开��6?   5.      5/      .��6)   ��50      �� 51      " ��6    	 52      P� 53   "   
 � �� 6:   Z� . �54   &   ��  � ��# [ 6C   55   #   P� 56   "   
 � ��$  ��6:   . �57   &   ��  � ��# [ 6C   58   #   5:      .��5;   %   5<      5=      �� 5>      ��6   �� *LogThemeSource+��5?      �� 5@      .R�5A      ��  �� Orientation$ 
Horizontal=�6%   .+�5B      ��  d� 	IsChecked 6'   �� *	IsChecked+6I   �� 5C      ��6(   �� *Name+6@   5D      5E      ��5G      �� 5H      .R�5I      A�  A� Rows$ 1��6'   $��0,0,0,10q�60   5J      5L      A� 5M      $ 1��6   $��0,0,0,10q�6#   .+�5N      �� -   5        $��ListenButton��5N      $��开始监听��6.   ��6>   �� *ListenStartCommand+{�6e   �� *ListenStartButtonIsEnabled+6�   �� 5O      -   5        $��CancleButton��5O      $��5,0,5,0q�6.   $��取消监听��6?   ��6O   �� *CancleCommand+{�6q   �� *CancleButtonIsEnabled+6�   �� 5P      $��清  空��6   ��6*   �� *CleanCommand+6M   5Q      5R      &   System.Windows.Controls.DataGrid  5S   
   -   5          Name$ DataGrid��5S      $��Bottom=�5T         CanUserAddRows .     
IsReadOnly .    AutoGenerateColumns .  ��5V      �� *CurrentLogPackets+$ TwoWay=� �� SelectedItem 5Y      �� *SelectedItem+$ TwoWay=�   RowStyle 5\      �� 5]      )  #System.Windows.Controls.DataGridRow"  6   .�5^      ��  � ��$ 20��6/   	 5_      P� 5`      
 �@J�IsMouseOver . . �5a      ��  � ��
 �����6;   5b      P� 5c      
 �@ 
IsSelected . . �5d      ��  � ��
 �����6;   5e      5g         Columns 5i      0  *System.Windows.Controls.DataGridTextColumn  5j      ,  &System.Windows.Controls.DataGridColumn   Header$ 名称��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 5k   )   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5l   )   �� *Name+  5m      $ IP��6%   $	 100 5n   )    5o   )   �� *IP+  5p      $ 时间��6%   $	 150 5q   )    5r   )   ��  �� Path$ LogInfo.Time� �� StringFormat$ yyyy-MM-dd HH:mm:ss��  5s      $ 等级��6%   $	 100 5t   )    5u   )   �� $ LogInfo.Level�  5v      $ 堆栈轨迹��6%   $	 200 5w   )    5x   )   �� $ LogInfo.StackTrace�  5y      $ 信息��6%      MinWidth$	 300Q�5z   )      MaxWidth$	 500Q�5{   )    5|   )   �� $ LogInfo.Message�5~      5      5�      