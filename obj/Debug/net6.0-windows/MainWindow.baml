   M S B A M L   `   `   `  ����>  :PZ2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PZ2PZ2  5
   	       PZ2.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5	   	   #localclr-namespace:PZ2     5
   	     � Title$  PZ2_Radunovic_Vuk��5   	   $	��800��6#   $	��800��60   ���#424242 � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5         
btn_efekat �� 
TargetType �� Property �� Value �� 
TargetType	 �� Triggers
 P� Property P� Value��%�  &	        �� 5   
   " ��6#   .�5      ��  �@�OverridesDefaultStyle$
 True��66   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $��	btn_okvir��6!   $��0q�62   ._�5      �� $��Center=�6/   $��Center=�6L   5      	 5      P� 5      
 �@J�IsMouseOver$
 True��6=   . �5    "   ��  �@J�Opacity$	 0.7Q�6<   5!      5#      5%      5&      5*      .��5,      � -   5        $��MainGrid��5,      $	��800��6   $	��800��6(   .+�5.   
   �� -   5         ��Name$ btn_exit��5.      #��[  5/      $��42��50      $��42��6   $��754,4,4,754q�6(   
������ .��53      �� $#��Assets\Menu\btn_menu_exit.png��6   54      �� 56   
   -   5        $ btn_hide��56      #��[  57      $��42��58      $��42��6   
������ $��702,6,56,752q�5:   (   .��5;      �� $#��Assets\Menu\btn_menu_hide.png��6   5<      �� 5>   
   -   5        $ btn_show��5>      #��[  5?      $��42��5@      $��42��6   
������ $��649,3,109,755q�5B   (   .��5C      �� $#��Assets\Menu\btn_menu_show.png��6   5D      �� 5F   
   -   5        $ btn_color_pipe��5F      #��[  5G      $��42��5H      $��42��6   
������ $��3,4,755,754q�5J   .   .��5K      �� $)��#Assets\Menu\btn_menu_color_pipe.png��6   5L      �� 5N   
   -   5        $ btn_color_switch��5N      #��[  5O      $��42��5P      $��42��6   
������ $��59,4,699,754q�5R   0   .��5S      �� $+��%Assets\Menu\btn_menu_color_switch.png��6   5T      '� 5X   
   -   5        $��MainViewPort��5Y      $
}�True��5X      $��0,70,0,0q�5^   8    '� Camera 5b      =� 5c      -   5        
mainCamera�� � Position$ 
1 -650 8501�5c   8    � LookDirection$ 0,3.5,-55�6P    =� FieldOfView$ 60Q�6k   x�5j      g� 5l      -	   5        mainLightSource�� g� Content 5p      �� 5q      -
   5        dirLightMain��5q   '   5v      g� 5x       5z      h� 5{      -   5        model3dGroup��.��5|      �  � Geometry 5�   "   k� 5�   &    k� 	Positions5 �    v�� 8�� v������# 8����#�� k� TriangleIndices �     k� TextureCoordinates �   5�   1    � Material 5�   "   u� 5�   &    u� Brush 5�   *   �� 5�   .    �� ImageSource$! Assets\Maps\map_primary.jpg��69   5�   '   5�      5�       g� 	Transform 5�      Y� 5�      .�5�      �� -   5        	scale�� �� ScaleX$ 1Q�5�   >    �� ScaleY$ 1Q�6I    �� ScaleZ$ 1Q�6T    �� CenterX$ 1Q�6_    �� CenterY$ 0Q�6k    �� CenterZ$ 1Q�6w   �� 5�        �� CenterX$  1Q�60   ! �� CenterZ$! 1Q�6<   " �� CenterY$" 0Q�6H   # �� Rotation# 5�   "   �� 5�   &   -   5        rotateX��$ �� Axis$$ 1 0 05�5�   O   5�      �� 5�      $  1Q�60   $! 1Q�6<   $" 1Q�6H   # 5�   "   �� 5�   &   -   5        rotateY��$$ 0 1 05�5�   M   5�      S� 5�      -   5        	translate��% S� OffsetX$% 0Q�5�   F   & S� OffsetY$& 0Q�6R   ' S� OffsetZ$' 0Q�6^   5�      5�      5�      5�      5�      