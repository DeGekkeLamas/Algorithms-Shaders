// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VelocityStretch"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Velocity ("Velocity", Vector) = (0, 0, 0)
		_Reflectiveness("Reflectiveness", Float) = 16
		_CellShadeLoops("Cell shade loops", Integer) = 3
		_Emissiveness("Emissiveness", Float) = 0
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
			#include "UnityLightingCommon.cginc"
			#include "Assets\Shaders\Lighting.hlsl"

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
				float4 worldPos : TEXCOORD2;
			};
			
			sampler2D _MainTex;
			float4 _Color;
			float4 _Velocity;
			float _Reflectiveness;
			int _CellShadeLoops;
			float _Emissiveness;
			float _Intensity;

			output vert(input v)
			{
				output o;
				o.uv = v.uv;

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

				// Stuff for lighting
				float4 noTranslate = v.normal;
				noTranslate.w = 0;
				float4 mat = mul(UNITY_MATRIX_M, noTranslate);
				o.normal = normalize(mat);
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);

				return o;
			}

			fixed4 frag(output i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				
				col = GetLightingCellshade(col, i.normal, i.worldPos, _Reflectiveness, _CellShadeLoops);
				col *= 1 + _Emissiveness;

				return col;
			}
			ENDCG
		}
	}
}
