   M S B A M L   `   `   `  ����K  GAprender-Teclado, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Aprender_TecladoAprender_Teclado  5      *    $Aprender_Teclado.editorDeActividades   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    5      .localclr-namespace:Aprender_Teclado    5      -   5          �Name$  Window��5       � Title$ Editor de Actividades��5      $	��711��5      $	��480��6   
���   � � WindowStartupLocation$ CenterScreen=�6E    � Icon$ input_keyboard.ico��6j    � 
ResizeMode$ CanMinimize=�6�   .��5
      � -   5         �Name$ 
LayoutRoot��5
      $	��700��6   .+�5      �� -   5         ��Name$ txtTextoActividad��5      $��36,131,35,74q�6'    �� TextWrapping$
 Wrap=�6=   $��4��6Q    �� AcceptsReturn . 6^   �� 5      $��21��6   $��36,104,63,0q�6   $
��Wrap=�6/   $	��Top=�6C   ���	#FFE9F50C�� 6s   	 � Language$	 es-es�6w   $m�gEscriba el texto de su actividad, cada nuevo párrafo va a ser una lección diferente para sus alumnos:��6�   6�   �� 5      $
��Left=�6   $��3��6)   $��
193,24,0,0q�64   $
��Wrap=�6H   $��	TextBlock��6\   $	��Top=�6m   $��1��6�   �� 5      -   5        
 ��Name$
 cmbDificultad��5      $��23��6$   $��
210,49,0,0q�60   $	��Top=�6D   $��1��6\   $
��Left=�6i   $	��150��6�   �� 5      -   5        $
 cmbTipo��5      $��23��6   $��
377,49,0,0q�6*   $	��Top=�6>   $
��Left=�6V   $	��149��6q   $��2��6}   �� 5      -   5         ��Name$ 
btnGuardar��5      $��Guardar Actividad��6   $��Right=�6;   $��22��6W   $��	0,0,35,33q�6c   $��Bottom=�6v   $	��124��6�   $��6��6�   6�   �� 5      -   5        $ txtNombreActividad��5      $
��Left=�6(   $��23��6C   $��	36,49,0,0q�6O   $
 Wrap=�6b   $	��Top=�6v   $	��158��6�   $��0��6�   �� 5      $
��Left=�6   $��37��6)   $��36,8,0,0q�65   $
��Wrap=�6G   $	��Top=�6[   $	��158��6s   ���	#FFE9F50C�� 6�   $	 es-es�6�   $(�"Escriba el nombre de su actividad:��6�   6�   �� 5      $��36��6   $��	210,9,0,0q�6   $
��Wrap=�6-   $	��Top=�6A   ���	#FFE9F50C$
��Left=�6p   $	��150��6�   �� 6�   $	 es-es�6�   $*�$Elija la dificultad de su actividad:��6�   6�   �� 5      $��Right=�6   $��36��6*   $��	0,9,173,0q�66   $
��Wrap=�6I   $	��Top=�6]   $	��149��6u   ���	#FFE9F50C�� 6�   $	 es-es�6�   $)�#Elija de qué tipo es su actividad:��6�   6�   #   Aprender_Teclado.etiquetaLink  5      -   5          Name$ linkEliminarActividad��5      #   Aprender_Teclado.etiquetaLink   MouseLeftButtonDown$/ )linkEliminarActividad_MouseLeftButtonDown��6�   $
��Left=�66   $��22��6Q   $��	36,0,0,33q�6]   $��Bottom=�6p   $	��279��6�   $��5��6�   6�   �� 5   
   -	   5        $��txtAutor��5   M   $��23��6   $��Right=�6   $��	0,49,35,0q�6:   $��3��6]   $
 Wrap=�6j   $	��Top=�6~   $	��126��6�   �� 5   
   ���	#FFE9F50C$��37��6+   $
��Left=�67   $��	539,8,0,0q�6R   $
��Wrap=�6e   $	��Top=�6y   $	��126��6�   $#��Escriba su nombre como autor:��6�   6�   5      5      