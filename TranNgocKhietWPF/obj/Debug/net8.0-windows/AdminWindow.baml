   M S B A M L   `   `   `  ����K  GTranNgocKhietWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:TranNgocKhietWPFTranNgocKhietWPF  5   	   "    TranNgocKhietWPF.AdminWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:TranNgocKhietWPF     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ Admin Dashboard��5	   	   $
��1050��5
   	   $	��900��5   	   ���	#FFF9F9FB.��5      � $��20q�6    � ColumnDefinitions 5   
   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� CornerRadius$ 12��5      
������� �� Padding$ 10q�5      ���#DDD$��1q�5      $��0,0,0,20q�5      ._�5       � -   5          �Name$ frMain��5      $��Stretch=�5       $��Stretch=�5!        � NavigationUIVisibility$ Hidden=�5"      5#      �� 5%   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   .+�5&      �� -   5        $��Customer List��5&      $	��180��5'      $��40��6!   	 c� ButtonStyleKey#��["	 5(      ���#8C5DF6
�������$��SemiBold"�5*      5+      �� 5-      -   5        $��Room Info List��5-      $	��180��5.      $��40��6!   #��["	 5/      ���#8C5DF6
�������$��SemiBold"�51      52      �� 54      -   5        $��Booking List��54      $	��180��55      $��40��6!   #��["	 56      ���#8C5DF6
�������$��SemiBold"�58      59      �� 5;      -   5        $��Report��5;      $	��180��5<      $��40��6!   #��["	 5=      ���#8C5DF6
�������$��SemiBold"�5?      5@      �� 5B      -   5        $��Log Out��5B      $	��180��5C      $��40��6!   #��["	 5D      ���#F44336
�������$��SemiBold"�5F      5G      5H      5I      5J      