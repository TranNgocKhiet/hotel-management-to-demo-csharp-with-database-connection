   M S B A M L   `   `   `  ����  TranNgocKhietWPF$    TranNgocKhietWPF.BookingDialog   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Booking Room��5   	   $	��250��6   $	��400��6+    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $	��120 �6   �� 5      $��* �6   .+�5   
   �� $��Start Date:��6   $��0��6'   $��0��64   $��Center=�6D   $��	0,0,10,10q�6_   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ StartDatePicker��5      $��0��6.   $��1��6;   $��0,0,0,10q�6K   �� 5   
   $��	End Date:��6   $��1��6%   $��0��62   $��Center=�6B   $��	0,0,10,10q�6]     5   
   -   5        $ EndDatePicker��5      $��1��6,   $��1��69   $��0,0,0,10q�6I   �� 5   
   $��2��6   $��2��6"    �� Orientation$ 
Horizontal=�66   $��Right=�6O   $��0,20,0,0q�6k   .+�5      �� -   5        $��OK��5      $��80��6"   $��0,0,10,0q�6-   6?   �� 5      -   5        $��Cancel��5      $��80��6&   61   5      5      5      