   M S B A M L   `   `   `  ���� K  GAprender-Teclado, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Aprender_TecladoAprender_Teclado  *    $Aprender_Teclado.editorDeActividades   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    .localclr-namespace:Aprender_Teclado    -     �Name$  Window�� � Title$ Editor de Actividades��$	��711��$	��480��
���   � � WindowStartupLocation$ CenterScreen=� � Icon$ input_keyboard.ico��.��� -    �Name$ 
LayoutRoot��$	��700��.+��� -    ��Name$ txtTextoActividad��$��36,131,35,74q� �� TextWrapping$
 Wrap=�$��4�� �� AcceptsReturn . �� $��21��$��36,104,63,0q�$
��Wrap=�$	��Top=����	#FFE9F50C��  � Language$ es-es�$m�gEscriba el texto de su actividad, cada nuevo párrafo va a ser una lección diferente para sus alumnos:���� $
��Left=�$��3��$��
193,24,0,0q�$
��Wrap=�$��	TextBlock��$	��Top=�$��1���� -   	 ��Name$	 cmbDificultad��$��23��$��
210,49,0,0q�$	��Top=�$��1��$
��Left=�$	��150���� -   $	 cmbTipo��$��23��$��
377,49,0,0q�$	��Top=�$
��Left=�$	��149��$��2���� -   
 ��Name$
 
btnGuardar��$��Guardar Actividad��$��Right=�$��22��$��	0,0,35,33q�$��Bottom=�$	��124��$��6���� -   $ txtNombreActividad��$
��Left=�$��23��$��	36,49,0,0q�$
 Wrap=�$	��Top=�$	��158��$��0���� $
��Left=�$��37��$��36,8,0,0q�$
��Wrap=�$	��Top=�$	��158�����	#FFE9F50C�� $ es-es�$(�"Escriba el nombre de su actividad:���� $��36��$��	210,9,0,0q�$
��Wrap=�$	��Top=����	#FFE9F50C$
��Left=�$	��150���� $ es-es�$*�$Elija la dificultad de su actividad:���� $��Right=�$��36��$��	0,9,173,0q�$
��Wrap=�$	��Top=�$	��149�����	#FFE9F50C�� $ es-es�$)�#Elija de qué tipo es su actividad:��#   Aprender_Teclado.etiquetaLink  -     Name$ linkEliminarActividad��#   Aprender_Teclado.etiquetaLink   MouseLeftButtonDown$/ )linkEliminarActividad_MouseLeftButtonDown��$
��Left=�$��22��$��	36,0,0,33q�$��Bottom=�$	��279��$��5���� -	   $��txtAutor��$��23��$��Right=�$��	0,49,35,0q�$��3��$
 Wrap=�$	��Top=�$	��126���� ���	#FFE9F50C$��37��$
��Left=�$��	539,8,0,0q�$
��Wrap=�$	��Top=�$	��126��$#��Escriba su nombre como autor:��