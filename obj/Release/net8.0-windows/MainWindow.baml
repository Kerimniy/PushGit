   M S B A M L   `   `   `  ���� B  >PushGit, Version=0.0.1.0, Culture=neutral, PublicKeyToken=null!clr-namespace:PushGitPushGit      PushGit.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:PushGit       � Title$  PushGit��$	��500��$	��800�� � 
ResizeMode$ CanMinimize=����	#FF1F1F1F � Icon$ /pushgit_logo (1) (1).png��.��� ���	#FF1F1F1F.+��� -   $
��Push��$
��Left=�$��586,154,0,0q�$	��Top=�$	��101��$	��160�� J� RenderTransformOrigin$ 0.556,0.941&����	#FF262626���	#FFD6D6D6$��22(� � Cursor$
 Hand���� -    ��Name$ textBox��$
��Left=�$��
46,302,0,0q� �� TextWrapping$
 Wrap=����	#FF262626$	��Top=�$	��700��$	��151��$��14(� �� 	UndoLimit$	 500��$ IBeam�� �� TextAlignment$ Center=����	#FF3D3D3D$��3,3,3,3q����	#FFD6D6D6	 �� SelectionTextBrush	 �	#FF0404CA
 �� 
CaretBrush
 �#000000�� -   $ inputUrl��$
��Left=�$��151,264,0,0q�$
 Wrap=�$	��Top=�$	��595��$ IBeam��$��25�����	#FF262626���	#FF3D3D3D$��3,3,3,3q����	#FFD6D6D6�� $��
Repo's URL��$
��Left=�$��
42,264,0,0q�$	��Top=�$��25��$	��100��$ 0.811,0.256&����	#FFD6D6D6$��14(�$��5,0,0,0q�$��Center=��� -    ��Name$ toggleButtonForce��$
��Left=�$��45��$��
723,27,0,0q����	#FF464646$	��Top=�$��45��$ 0.489,0.445&�$��3�� J� OpacityMask
 ��������	#FF1F1F1F$
 Hand���� $��Force Mode (-f ) push��$
��Left=�$��
563,27,0,0q�$	��Top=����	#FFD6D6D6$��0,0,0,0q�$��Center=�$��16(�$��45��$	��155���� -   $ inpuBranchName��$
��Left=�$��
151,20,0,0q�$
 Wrap=�$��Center=�$	��430��$ IBeam��$��25�����	#FF262626���	#FF3D3D3D$��3,3,3,3q����	#FFD6D6D6�� $��Branch��$
��Left=�$��
42,227,0,0q�$	��Top=�$��25��$	��100��$ 0.811,0.256&����	#FFD6D6D6$��14(�$��5,0,0,0q�$��Center=��� -   $ 
pickFolder��$
��Left=�$��51��$��	10,10,0,0q�
���   �$	��Top=�$��50��$��0��$ ArrowCD������  �� ImageSource$ /folder_icon (1).png���� -   $ toggleButtonAsync��$
��Left=�$��45��$��
497,27,0,0q����	#FF464646$	��Top=�$��46��$ 0.489,0.445&�$��3��
 �����$
 Hand������ $ 
/arr48.png���� $��
Async push��$
��Left=�$��
394,27,0,0q�$	��Top=����	#FFD6D6D6$��0,0,0,0q�$��Center=�$��16(�$��45��$��92���� -   $ toggleButtonAutoPush��$
��Left=�$��45��$��
333,27,0,0q����	#FF464646$	��Top=�$��45��$ 0.489,0.445&�$��3��
 ��������	#FF1F1F1F$
 Hand���� $
��Left=�$��
173,27,0,0q�$
��Wrap=�$+��%Auto Push on           Folder Changes��$	��Top=�$��45��$	��149�����	#FFD5D5D5$��16(��� -	   $ outputFolderTextBox��$
��Left=�$��151,154,0,0q�$
 Wrap=�$	��Top=�$	��430��$ IBeam��$��25�����	#FF262626���	#FF3D3D3D$��3,3,3,3q����	#FFD6D6D6�� $��Your Folder��$
��Left=�$��
46,154,0,0q�$	��Top=�$��25��$	��100��$ 0.811,0.256&����	#FFD6D6D6$��14(�$��0,0,0,0q�$��Center=��� -
   $ inputCommit��$
��Left=�$��151,191,0,0q�$
 Wrap=�$	��Top=�$	��430��$ IBeam��$��25�����	#FF262626���	#FF3D3D3D$��3,3,3,3q����	#FFD6D6D6�� $��Commit��$
��Left=�$��
46,192,0,0q�$	��Top=�$��25��$	��100��$ 0.811,0.256&����	#FFD6D6D6$��14(�$��0,0,0,0q�$��Center=�