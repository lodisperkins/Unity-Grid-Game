Shader "Unlit/MyFirstShader"
{
	Properties
	{
		_Color("Main Color",Color) = (255,1,0,0)
		_SpecColor("Spec Color",Color) = (1,1,1,1)
		_Emission ("Emmisive Colors", Color) = (0,0,0,0)
		_Glossiness("Smoothness", Range(0.000000,1.000000)) = 0.500000
		_Shininess ("Shininess", Range(0.01,1)) = 0.7
	}
	SubShader
	{
		
		Pass
		{
			Material 
			{
				Diffuse[_Color]
				Ambient[_Color]
				Shininess[_Shininess]
				Specular[_SpecColor]
				Emission[_Emission]
			}
		Lighting On
		}
	}
}
