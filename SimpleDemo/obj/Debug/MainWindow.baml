   M S B A M L   `   `   `  ����E  ASimpleDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SimpleDemo
SimpleDemo  5      Z VSharpDX.Mathematics, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b4dcf0f35e5521f1>2clr-namespace:SharpDX;assembly=SharpDX.MathematicsSharpDX 5	      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5
          SimpleDemo.MainWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ` \HelixToolkit.Wpf.SharpDX, Version=2.12.0.0, Culture=neutral, PublicKeyToken=52aa3500039caf0d-hx$http://helix-toolkit.org/wpf/SharpDX  5      $localclr-namespace:SimpleDemo  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      =sdxm2clr-namespace:SharpDX;assembly=SharpDX.Mathematics  5	      0sys&clr-namespace:System;assembly=mscorlib  5
        � Title$  SharpDX in WPF��5      $
��1280��5      $	��720��5       � DataContext 5         SimpleDemo.MainViewModel  5   
    � 	Resources 5      7  1HelixToolkit.Wpf.SharpDX.RenderTechniqueConverter   RenderTechniqueConverter��%   &	          5   
   6&   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   o� $��0��6   $	��Top=�6   ��5      �� 5       �� Opacity$	 0.5Q�6"    �� Color$ 
WhiteSmoke��60   .@�5       m� $
��View��6   .@�5!      m�  m� Command$% hx:ViewportCommands.ZoomExtents��6   $��Zoom extents��6E   m� 5"      $��Up Dir X��6:    6   �� *
UpXCommand+6:   m� 5#      $��Up Dir Y��6:    6   �� *
UpYCommand+6:   m� 5$      $��Up Dir Z��6:    6   �� *
UpZCommand+6:   5%      5&      *  $HelixToolkit.Wpf.SharpDX.Viewport3DX  5(   
   -   5          Name$ view1��5)      $��1��5+         BackgroundColor$ Black��5,      &	   CoordinateSystemLabelForeground$	 #434343��5.      
   UseDefaultGestures
 .    ShowViewCube .     ShowCoordinateSystem .    	TextBrush �	#FFFEFFF6   EnableD2DRendering .     EnableSwapChainRendering .    	FXAALevel$	 Low=�58         EnableDeferredRendering .  }�.     Title 5*      �� *Title+   Camera 5-      �� *Camera+   EffectsManager 50      �� *EffectsManager+   ModelUpDirection 51      �� *UpDirection+   SubTitle 54      �� *
SubTitle+ J� InputBindings 5=      �� 5?      
 �� Key$ B��6    �� Command$" hx:ViewportCommands.BackView��6%   �� 5@      $ G��6   $# hx:ViewportCommands.FrontView��6%   �� 5A      $ T��6   $! hx:ViewportCommands.TopView��6%   �� 5B      $ V��6   $$ hx:ViewportCommands.BottomView��6%   �� 5C      $ G��6   $" hx:ViewportCommands.LeftView��6%   �� 5D      $ H��6   $# hx:ViewportCommands.RightView��6%   �� 5E      $% hx:ViewportCommands.ZoomExtents��6    �� Gesture$ 	Control+E��6G   d� 5F      $  hx:ViewportCommands.Rotate��6    d� Gesture$ 
RightClicka�6D   d� 5G      $ hx:ViewportCommands.Zoom��6   $ MiddleClicka�6B   d� 5H      $ hx:ViewportCommands.Pan��6   $ 	LeftClicka�6A      Items. 5J      -  'HelixToolkit.Wpf.SharpDX.AmbientLight3D  &   HelixToolkit.Wpf.SharpDX.Light3D   Color 6    �� *AmbientLightColor+1  +HelixToolkit.Wpf.SharpDX.DirectionalLight3D  5K         	Direction 6$   �� *Camera.LookDirection+ 6O   �� *DirectionalLightColor+0  *HelixToolkit.Wpf.SharpDX.ScreenQuadModel3D  5L         Texture 6#   �� *BackgroundTexture+2  ,HelixToolkit.Wpf.SharpDX.LineGeometryModel3D  5N      -   5           Name$
  grid��5O      !   	Thickness$	! 0.5Q�5P      (	  "HelixToolkit.Wpf.SharpDX.Element3D" 	  IsHitTestVisible" .  # �� 	IsEnabled# .  .
  (HelixToolkit.Wpf.SharpDX.GeometryModel3D$ 
  IsDepthClipEnabled$ .  % 
  IsMultisampleEnabled% .  & 
  IsScissorEnabled& .  ' 
  Geometry' 5Q      �� *Grid+( 	  	Transform( 5R      �� *GridTransform+)   Color) 5S      �� *	GridColor+6�     5U      -   5        $  arrows��5V      $! 1Q�5W      " .  # .  $ .  % .  & .  ' 5X      �� *Arrow+( 5Y      �� *ArrowTransform+) 5Z      �� *
ArrowColor+6�   3  -HelixToolkit.Wpf.SharpDX.PointGeometryModel3D  5\      -   5        *  Name$* points��5]      $ .  # .  " .  % .  & .  +   Size$+ 1.5,1��5a   �   ,   	FixedSize, . ' 5^      �� *Points+( 5_      �� *PointsTransform+-   Color- 5`      �� *PointsColor+5a   �   5c      �� 5d   
   $��1��6   $��1��6"   .+�5f      9� $	��150��5g      $��10q�5h      $��Right=�5i      $	��Top=�5j      
�������$��
Instancing��5l      . 9� 
IsExpanded. . ��5n      �� 5o      $
 0.25Q�6&   $ 
WhiteSmoke��65   .��5s      �� .+�5t      �� $��4q�6    / d� 	IsChecked/ 6+   �� *IsRendering+0 �� ElementName$0 arrows��.��6a   Render Arrows5v      �� 5w      $��4q�6   / 6*   �� *IsRendering+$0 points��.��6`   Render Points5y      �� 5z      $��4q�6   / 6*   �� *IsRendering+$
0 grid��.��6^   Render Grid5|      �� 5}      $��4q�6    $��10(�6+   
�������
���   �$��81��6e   1 J� Opacity$
1 0.25Q�6q   Instrukcje:�� 5~   &   BLABLA5�      5�      5�      5�      �� 5�   
   $��2��6   $��Bottom=�6!   ��5�      �� 5�      $	 0.5Q�6"   $ 
WhiteSmoke��60   .@�5�      �� .��5�      �� $
��Left=�5�      $	��Top=�5�      ��5�      �� *	FrameRate+$0 view1��2 �� StringFormat$2 D3D11 - {0:0.00} FPS��5�      �� 5�      �� 5�      .��5�      �� ��6   �� *Camera.Position+$2 Position: {0:0.0}��5�      �� 5�      �� 5�      .��5�      �� ��6   �� *Camera.LookDirection+$2 LookDirection: {0:0.0}��5�      �� 5�      �� 5�      .��5�      �� ��6   �� *Camera.UpDirection+$2 UpDirection: {0:0.0}��5�      �� 5�      �� 5�      .��5�      �� ��6   �� *Items.Count+$0 view1��$2 Children: {0}��5�      5�      5�      5�      