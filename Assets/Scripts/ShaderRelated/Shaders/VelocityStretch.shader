// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VelocityStretch"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Velocity ("Velocity", Vector) = (0, 0, 0)
		_Ambient("Ambient", Float) = 0
		_Intensity("Intensity", Float) = 1
		_Radius("Radius", FLoat) = 1
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
			float _Ambient;
			float _Intensity;
			float _Radius;

			output vert(input v)
			{
				const float pi = 3.14159;
				output o;

				float translate = v.vertex.w;
				v.vertex.w = 0;
				float4 world = mul(UNITY_MATRIX_M, v.vertex);

				_Velocity *= _Intensity;
				float4 velocityDir = normalize(_Velocity);
				float4 reflection = reflect(velocityDir, world);

				float x = length(reflection/_Radius)/_Radius;
				world *= pow( ( sin( (x+.5)*pi)+1)/2, 1 ) * length(_Velocity) + 1;

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
