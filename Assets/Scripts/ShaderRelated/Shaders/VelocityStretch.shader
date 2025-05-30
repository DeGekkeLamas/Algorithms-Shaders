// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VelocityStretch"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Velocity ("Velocity", Vector) = (0, 0, 0)
		_Rotation ("Rotation", Vector) = (0, 0, 0)
		_VelocityRotation ("VelocityRotation", Vector) = (0, 0, 0)
		_Ambient("Ambient", Float) = 0
		_Intensity("Intensity", Float) = 1
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
			float4 _Rotation;
			float4 _VelocityRotation;
			float _Ambient;
			float _Intensity;

			output vert(input v)
			{
				output o;

				float translate = v.vertex.w;
				v.vertex.w = 0;
				float4 world = mul(UNITY_MATRIX_M, v.vertex);

				float4 velocityDir = normalize(_Velocity);
				float4 reflection = reflect(velocityDir, world) * _Intensity;

				world *= .5/(2*length(reflection)) * length(_Velocity);

				world += mul(UNITY_MATRIX_M, float4(0,0,0,translate));
				o.vertex = mul(UNITY_MATRIX_VP, world);

				return o;
			}

			fixed4 frag(output i) : SV_Target
			{
				fixed4 col = _Color;
				col = col *	(_Ambient + (1 - _Ambient) * saturate(i.normal.y)); // VERY basic lighting. TODO LATER: improve
				return col;
			}
			ENDCG
		}
	}
}
