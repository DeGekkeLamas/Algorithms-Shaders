
Shader "Custom/CellShadeOutlineTransparent"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		_Reflectiveness("Reflectiveness", Float) = 16
		_CellShadeLoops("Cell shade loops", Integer) = 3
		_Emissiveness("Emissiveness", Float) = 0

		_Transparency("Transparency", Range(0.0, 1.0)) = 1
	}
	SubShader
	{
		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		LOD 100
		ZWrite Off

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
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD2;
			};

			float4 _MainTex_ST;
			sampler2D _MainTex;
			float4 _Color;
			float _Reflectiveness;
			int _CellShadeLoops;
			float _Transparency;
			float _Emissiveness;

			output vert(input v)
			{
				output o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				float4 noTranslate = v.normal;
				noTranslate.w = 0;
				float4 mat = mul(UNITY_MATRIX_M, noTranslate);
				o.normal = normalize(mat);
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);

				return o;
			}

			fixed4 frag(output i) : SV_Target
			{
				float4 col;

				col = tex2D(_MainTex, i.uv) * _Color;
				
				col = GetLightingCellshade(col, i.normal, i.worldPos, _Reflectiveness, _CellShadeLoops);

				col.a = 1 * _Transparency;
				col *= 1 + _Emissiveness;

				return col;
			}
			ENDCG
		}
		// cast shadows:
		Pass
		{
			Tags{ "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#pragma vertex VSMain
			#pragma fragment PSMain

			float4 VSMain(float4 vertex:POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			float4 PSMain(float4 vertex:SV_POSITION) : SV_TARGET
			{
				return 0;
			}

			ENDCG
		}
	}
}
