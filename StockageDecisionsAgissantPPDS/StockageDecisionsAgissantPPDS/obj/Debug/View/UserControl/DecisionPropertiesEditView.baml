   M S B A M L   `   `   `  ����\  XStockageDecisionsAgissantPPDS, Version=1.1.0.20952, Culture=neutral, PublicKeyToken=nulls>clr-namespace:StockageDecisionsAgissantPPDS.ViewModel.Decision0StockageDecisionsAgissantPPDS.ViewModel.Decision  5      O    IStockageDecisionsAgissantPPDS.View.UserControl.DecisionPropertiesEditView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Sdecision>clr-namespace:StockageDecisionsAgissantPPDS.ViewModel.Decision     5      .��5	      �   � ColumnDefinitions  5
   
   �� 5      $��80 �6   �� 5      $��* �6   $	��500��6)    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   ��    	BoldLabel#��[  6   $��Titre :��63   $��0��6E   $��0��6U   �� 5   
   #��[  6   $��Date :��63   $��0��6D   $��1��6T   �� 5   
   #��[  6   $��Description :��63   $��0��6K   $��2��6[   �� 5   
   #��[  6   $��Lien :��63   $��0��6D   $��3��6T   �� 5   
   $��Stretch=�6   $��1��60   $��0��6@   {�.  ��5      �� *EditedDecision.Titre+ �� Mode$ OneTime=�6F   (  "System.Windows.Controls.DatePicker  5   
   $��Stretch=�6   $��1��63   $��1��6C   {�.     SelectedDate 5      �� *EditedDecision.Date+$ OneTime=�6P   �� 5   
     MultiLineTextBox#��[ 6   $
��auto��6<   $��Stretch=�6J   $	��100��5       $��1��6"   $��2��62   {�.  ��6?   �� *EditedDecision.Description+$ OneTime=�6y   �� 5!   
   $��Stretch=�6   $��1��60   $��3��6@   {�.  ��5"      ��  �� Path$ EditedDecision.Lien�$ OneTime=�6J   �� 5#   
     LabelButton#��[ 6     EDF_Blue_Light#��[ 66   $��4��6c   $��1��6p   $
��13.0(�6�   $��Right=�6�   $��Ouvrir le lien��5$      ��6*   �� *ClickCommand+5%      5&      