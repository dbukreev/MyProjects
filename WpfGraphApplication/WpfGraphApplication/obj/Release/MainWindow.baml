   M S B A M L   `   `   `  ����   WpfGraphApplication$    WpfGraphApplication.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Draw Graph Application��$	��586��$	��586��$	��586��$	��678��$	��678��$	��678�� � Icon$ Resources/Icon.ico��.��� -   $��MyGrid�����	#FFEAE7E7.+��� -   $��	MyCanvasY��$
��Left=�$	��526��$��	120,0,0,0q�$	��Top=�$��41�����#3CB371�� -   $��	MyCanvasX��$
��Left=�$��25��$��161,501,0,0q�$	��Top=�$	��509�����#3CB371�� -   $��MyCanvas��$��Right=�$	��500��$	��Top=�$	��500��
�������}�. �� -   $��MyStatusBar��$
��Left=�$��25��$��Bottom=�$	��670�����	#FF2EA3C5$��0,0,0,-4q�.@��� $	��396��.���� -   $��TextBlockStatus��$��Статус��
��������� �� .���� -   $��TextBlockPoint��
�������X: Y:�� -    ��Name$ ClearButton�� � ToolTip$% Очистить полотно��$
��Left=�$��29��$��	65,10,0,0q�$	��Top=�$��35�����	#FFF0E9E9 J� RenderTransformOrigin$ -0.24,-0.263&�.���� .+��� $��Resources/Clear.ico��$��25��$��30���� -	   $ 	AddButton��$A ;Добавить файлы точечных функций��$
��Left=�$��	16,53,0,0q�$	��Top=�$��35��.���� .+��� $��Resources/Add.ico��$��25��$��30���� -
   $��DeleteButton��$= 7Удалить все загруженные файлы��$
��Left=�$��	65,53,0,0q�$	��Top=�$��35��.���� .+��� $��Resources/Delete.ico��$��25��$��30���� -   $��	MyListBox��$
��Left=�$��97��$��	16,96,0,0q�$	��Top=�$��84�� �� SelectionMode$ Extended=� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$
 Code������ �� -   $ 
DrawButton��$: 4Построить выбранные функции��$
��Left=�$��	16,10,0,0q�$	��Top=�$��35��.���� .+��� $��Resources/Draw.ico��$��25��$��30���� -   $��RevertButton��$Y SПостроить симетрию по X для выбранных функций��$
��Left=�$��
16,224,0,0q�$	��Top=�$��35��.���� .+��� $��Resources/Log.png��$��31��$��35���� -   $��UnionButton��$� �Объединить выбранные точечные функции, для одинаковых аргументов усреднить��$
��Left=�$��
65,224,0,0q�$	��Top=�$��35��$��35��.���� .+��� $��Resources/Union.png��$��30��$��30��