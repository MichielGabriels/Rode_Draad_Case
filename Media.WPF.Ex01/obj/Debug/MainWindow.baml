   M S B A M L   `   `   `  ����I  EMedia.WPF.Ex01, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Media.WPF.Ex01Media.WPF.Ex01  5   	       Media.WPF.Ex01.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Media.WPF.Ex01     5   	   -   5          � Title$  Media Player��5   	   $	��440��6   $	��440��6.   $	��525��6;   $	��525��6J    � 	Resources 5
       �� 
TargetType   BaseButtonCustom �� BasedOn �� Property �� Value��%_   '��       7   �� 5   
   " ��6   8   .�5      ��  � ��$ 3q�6'   63   5      5      .��5      ]� .+�5   
   o� $	��Top=�6    o� 
IsMainMenu . .@�5      m� $��_File��6   .@�5      m� -   5        $��_Close��5      6+   5      m� 5      $��_Help��6   .@�5      m� $��_About��6   5      5      �� 5   
   -   5        $��Bottom=�5      .@�5      �� -   5        $��musicTabItem��5      $��Music��6*   .��5      �  � ColumnDefinitions 5      �� 5      $��* �6+   $	��100��65   �� 5       $	��300 �6+   $	��300��67    � RowDefinitions 5"      �� 5#      $	��265��6(   �� 5$      $
��Auto �6(   �� 5%      .+�5(      �� $��0��6#   $��3��63   $��5��6D   
�������6N   �� 5*      -   5        	 ��Name$	 musicListBox��5*      $��0��64   $��0��6D   $��Stretch=�6Q   $��Stretch=�6o   $��10q�6�   ��5+      �� 5,      .R�5-   "   �� 
 �� Orientation$
 
Horizontal=�6-   .+�5.   &   �� ��60   ��  �� Path$ Singer��� 5/   &   $	�� - ��60   �� 50   &   ��60   �� $ Title�51   #   52      54      �� 56      $
 Vertical=�6!   $��0��68   $��1��6H   .+�57      �� $��20(�6    $
��Bold"�6.   $��Center=�6A   $��0,10,0,0q�6^   $	��Top=�6p   .��58      �� $��	Underline{�6(   $��Playlist��6D   59      5:      �� 5<      -   5        $	 playlistListBox��5<      $��0��67   $��2��6G   $��Stretch=�6T   $��Stretch=�6r   $��10q�6�   ��5=      �� 5>      .R�5?   "   �� $
 
Horizontal=�6-   .+�5@   &   �� ��60   �� $ Singer��� 5A   &   $	�� - ��60   �� 5B   &   ��60   �� $ Title�5C   #   5D      5F      �� 5H      $
 Vertical=�6!   $��2��68   $��0��6H   $��Center=�6U   $	��275��6r   .+�5I      �� $��20(�6    $
��Bold"�6.   $��Center=�6A   $��0,10,0,0q�6^   $	��Top=�6p   .��5J      �� $��	Underline{�6(   $��Selected Song��6D   5K      �� 5M      $
 
Horizontal=�6%   $��15, 10, 0, 0q�6>   .+�5N      ��  �� Target 6$   ��  �� ElementName$ textBoxSinger��.��6Q   Singer6Y   �� 5O      -   5         ��Name$ textBoxSinger��5O   &   $	��190��6=   $��18��6I   $��20, 0, 0, 0q�6U   5P      �� 5R      $
 
Horizontal=�6%   $��15, 10, 0, 0q�6>   .+�5S      ��  6$   �� $ textBoxSongTitle��.��6T   Title6[   �� 5T      -   5        $ textBoxSongTitle��5T   &   $	��190��6@   $��18��6L   $��32, 0, 0, 0q�6X   5U      �� 5W      $
 
Horizontal=�6%   $��20, 10, 0, 0q�6>   .+�5X      �� -	   5         ��Name$ buttonAddMusicFile��5X   %   $	��100��6]   .��6i   Add musicfile6x   �� 5Y      -
   5         ��Name$ checkBoxMusicFilePresent��5Y   '   $��Center=�6I   $��10, 0, 0, 0q�6d   {�.  .��6�   Music File Present6�   5Z      �� 5\      $
 
Horizontal=�6%   $��35, 10, 0, 0q�6>   .+�5]      �� -   5        $ buttonSaveSong��5]   %   $��
5, 0, 0, 0q�6=   $��60��6Q   .��6z   Save6�   �� 5^      -   5        $ buttonDeleteSong��5^   %   $��10, 0, 0, 0q�6?   $��60��6T   {�.  .��6�   Delete6�   �� 5_      -   5        $ buttonCancelSong��5_   %   $��10, 0, 0, 0q�6?   $��60��6T   .��6z   Cancel6�   5`      �� 5b      $��0, 85, 0, 0q�6%   $
��Left=�6:   $	��275��6U   .+�5c      �� -   5        $ buttonAddToPlaylist��5c   %   $	��160��6B   {�.  $��Add To Playlist��6�   5d      5e      �� 5g      $
 Vertical=�6!   $��2��68   $��2��6H   $��
0, 0, 0, 5q�6U   $��Bottom=�6i   $��Center=�6�   .+�5h      �� -   5         ��Name$ labelNowPlaying��5h       $��Center=�69   .��6V   Now playing:6d   �� 5j      $
 
Horizontal=�6%   $��0, 5, 0, 10q�6>   $��Center=�6S   .+�5k      �  5l   "   �� 5m   &   �� 5n   &   �� 5o   &   �� 5p   &   �� 5q   &   .+�5t   "   �� -   5        $ 
buttonPlay��5t   )   $��0��6=   $��40��6M   $��
5, 0, 0, 0q�6X   {�.  .��6�   Play6�   �� 5u   "   -   5        $ buttonPause��5u   )   $��1��6>   $��40��6N   $��
5, 0, 0, 0q�6Y   {�.  .��6�   Pause6�   �� 5v   "   -   5        $ 
buttonNext��5v   )   $��2��6=   $��40��6M   $��
5, 0, 0, 0q�6X   {�.  .��6�   Next6�   �� 5w   "   -   5        $ 
buttonStop��5w   )   $��3��6=   $��40��6M   $��
5, 0, 0, 0q�6X   {�.  .��6�   Stop6�   �� 5y   "   -   5         ��Name$ sliderVolume��5y   )   $��4��6?   $��Center=�6O   $��60��6j   $��
5, 0, 0, 0q�6u   {�.   � Minimum$ 0Q�6�    � Maximum$ 1Q�6�   6�   5z      5{      5|      5}      5~      �� 5      -   5        $��moviesTabItem��5      $��Movies��6+   .��5�      �  5�      �� 5�      $��* �6+   $	��100��65   �� 5�      $	��300 �6+   $	��300��67    5�      �� 5�      �� 5�      $
��Auto �6(   .+�5�      �� $��0��6#   $��2��63   $��5��6D   
�������6N   �� 5�      -   5        $	 movieListBox��5�      $��0��64   $��2��6D   $��Stretch=�6U   $��Stretch=�6s   $��10q�6�   ��5�      �� 5�      .R�5�   "   �� $
 
Horizontal=�6-   .+�5�   &   �� ��60   �� $ Title��� 5�   &   $	�� - ��60   �� 5�   &   ��60   �� $ Director�5�   #   5�      5�      �� 5�      $
 Vertical=�6!   $��2��68   $��0��6H   $��Center=�6U   $	��275��6r   .+�5�      �� $��20(�6    $
��Bold"�6.   $��Center=�6A   $��0,10,0,0q�6^   $	��Top=�6p   .��5�      �� $��	Underline{�6(   $��Selected Movie��6D   5�      �� 5�      $
 
Horizontal=�6%   $��15, 10, 0, 0q�6>   .+�5�      ��  6$   �� $ textBoxDirector��.��6S   
Director6]   �� 5�      -   5        $ textBoxDirector��5�   &   $	��190��6?   $��18��6K   $��11, 0, 0, 0q�6W   5�      �� 5�      $
 
Horizontal=�6%   $��15, 10, 0, 0q�6>   .+�5�      ��  6$   �� $ textBoxMovieTitle��.��6U   Title6\   �� 5�      -   5        $ textBoxMovieTitle��5�   &   $	��190��6A   $��18��6M   $��32, 0, 0, 0q�6Y   5�      �� 5�      $
 
Horizontal=�6%   $��20, 10, 0, 0q�6>   .+�5�      �� -   5        $ buttonAddMovieFile��5�   %   $	��100��6]   .��6i   Add moviefile6x   �� 5�      -   5        $ checkBoxMovieFilePresent��5�   '   $��Center=�6I   $��10, 0, 0, 0q�6d   {�.  .��6�   Movie File Present6�   5�      �� 5�      $
 
Horizontal=�6%   $��35, 10, 0, 0q�6>   .+�5�      �� -   5        $ buttonSaveMovie��5�   %   $��
5, 0, 0, 0q�6>   $��60��6R   .��6{   Save6�   �� 5�      -   5        $ buttonDeleteMovie��5�   %   $��10, 0, 0, 0q�6@   $��60��6U   {�.  .��6�   Delete6�   �� 5�      -   5        $ buttonCancelMovie��5�   %   $��10, 0, 0, 0q�6@   $��60��6U   .��6{   Cancel6�   5�      5�      �� 5�      $
 Vertical=�6!   $��2��68   $��1��6H   $��0, 0, 0, 10q�6U   $��Bottom=�6j   $��Center=�6�   .+�5�      �� -   5        $ buttonPlayMovie��5�   !   $��70��6:   $��0, 60, 0, 0q�6E   {�.  .��6�   
Play Movie6�   5�      5�      5�      5�      5�      5�      