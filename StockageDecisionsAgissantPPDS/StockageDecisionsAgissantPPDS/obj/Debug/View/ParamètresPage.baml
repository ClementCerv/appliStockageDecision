   M S B A M L   `   `   `  ����\  XStockageDecisionsAgissantPPDS, Version=1.1.0.20952, Culture=neutral, PublicKeyToken=null[2clr-namespace:StockageDecisionsAgissantPPDS.Design$StockageDecisionsAgissantPPDS.Design  5      S ONormacode.Wpf.Converters, Version=1.0.6.0, Culture=neutral, PublicKeyToken=nullsOclr-namespace:Normacode.Wpf.Converters.String;assembly=Normacode.Wpf.ConvertersNormacode.Wpf.Converters.String 5      M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089/$clr-namespace:System;assembly=SystemSystem 5      8    2StockageDecisionsAgissantPPDS.View.ParamètresPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Edesign2clr-namespace:StockageDecisionsAgissantPPDS.Design     5      bstringOclr-namespace:Normacode.Wpf.Converters.String;assembly=Normacode.Wpf.Converters     5      7system$clr-namespace:System;assembly=System     5        � 	Resources  5      :  4Normacode.Wpf.Converters.String.UriToStringConverter   UriToStringConverter;  5Normacode.Wpf.Converters.String.CharToStringConverter  CharToStringConverter  	BoldLabel �� BasedOn �� 
TargetType �� Property �� Value��%�   &	        &	      '�� &     7    5   
   6&     5   
   6'   �� 5   
   8   " ��65   .�5      ��  � ��$ 0,5,0,0q�6'   5      5      .��5      �� .+�5      �� $��15,20,15,15q�6   {�6&   �� *SettingsEnabled+.+�5   
   �� $��
Domaines :��6   �� 5   
     RadioButtonList#��[ 5      ��5      �� *PersisterDomainesSourceValues+ �� Mode$ OneTime=� �� SelectedItem 5      �� *PersisterDomainesSource+�� 5    
   $��Décisions :��6   �� 5!   
   #��[ 5"      ��5!      �� *PersisterTypeValues+$ OneTime=� 5#      �� *PersisterType+�� 5%   
   $��
Lien RSS :��6   {�6%   �� *IsRSSUsed.Value+�� 5&   
   {�6   �� *IsRSSUsed.Value+��68   �� *PersisterRssUri+ �� ValidatesOnExceptions .  �� 	Converter# [  	 �� UpdateSourceTrigger$	 	LostFocus=�  System.UriKind
   RelativeOrAbsolute �� ConverterParameter# Z
 �� 5,   
   $��Chemin du fichier CSV :��6   {�62   �� *IsCsvUsed.Value+�� 5-   
   {�6   �� *IsCsvUsed.Value+��68   �� *PersisterCsvPath+ . $	 	LostFocus=��� 5/   
   $��Separateur :��6   {�6'   �� *IsCsvUsed.Value+�� 50   
    �� 	MaxLength$ 1��51      $��30��6    ��50      �� *CsvSeparator+# [ {�51   +   �� *IsCsvUsed.Value+52      �� 53   
     LabelButtonBlue#��[ 6   $��11(�6:   $��Ouvrir les logs��6H   $��Right=�54      ��6-   �� *LogFileCommand+55      56      