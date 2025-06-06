// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VelocityStretch"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Velocity ("Velocity", Vector) = (0, 0, 0)
		_Intensity("Intensity", Float) = 1
		_Radius("Radius", Float) = 1
		_Sharpness("Sharpness", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct input
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct output
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
			};

			float4 _Color;
			float4 _Velocity;
			float _Intensity;
			float _Radius;
			float _Sharpness;

			output vert(input v)
			{
				const float pi = 3.14159;
				output o;

				// rotated, scaled, but no translation
				float translate = v.vertex.w;
				v.vertex.w = 0;
				float4 world = mul(UNITY_MATRIX_M, v.vertex); 

				_Velocity *= _Intensity;
				float4 velocityDir = float4(0,0,0,0);
				float velocityLength = length(_Velocity);
				if (velocityLength != 0) 
				{
					velocityDir = _Velocity / velocityLength;
				}

				// Using Section 5.2.2 of gamemath book:
				// vector projection formula:
				float4 parallel = dot(world,velocityDir) * _Velocity; // w=0
				float4 perpendicular = world-parallel; // w=0

				world = parallel * (1+_Intensity) + perpendicular / sqrt(1+_Intensity);

				// add world position of GO
				world += mul(UNITY_MATRIX_M, float4(0,0,0,translate)); 
				o.vertex = mul(UNITY_MATRIX_VP, world);

				return o;
			}

			fixed4 frag(output i) : SV_Target
			{
				fixed4 col = _Color;
				return col;
			}
			ENDCG
		}
	}
}
