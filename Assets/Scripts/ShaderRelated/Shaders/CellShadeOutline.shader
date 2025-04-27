
Shader "Custom/CellShadeOutline"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Ambient("Ambient", Float) = 0.5
		_Color("Color", Color) = (0,0,0,0)
		_CellShadeLoops("Cell shade loops", Integer) = 3

		_Transparency("Transparency", Range(0.0, 1.0)) = 0
	}
	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back

		Tags {
			"RenderType" = "Opaque"
			"Queue" = "Transparent"
			}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			float4 _MainTex_ST;
			sampler2D _MainTex;
			float _Ambient;
			float4 _Color;
			int _CellShadeLoops;
			float _Transparency;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				float4 noTranslate = v.normal;
				noTranslate.w = 0;
				float4 mat = mul(UNITY_MATRIX_M, noTranslate);
				o.normal = normalize(mat);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 col;

				col = tex2D(_MainTex, i.uv / _MainTex_ST.xy + _MainTex_ST.zw) * _Color;

				//Lighting
				float4 lightDir = _WorldSpaceLightPos0;
				float4 lightColor = _LightColor0;
				float4 ambientColor = (float4(1,1,1,1) - lightColor) * _Ambient;
				float3 cameraDir = normalize(_WorldSpaceCameraPos);

				float diffuse = saturate(dot(lightDir, i.normal));

				diffuse = ceil(diffuse * _CellShadeLoops) / _CellShadeLoops;
				float reflection = saturate( lightDir - dot(2*dot(lightDir, cameraDir), cameraDir) );

				col *= diffuse * lightColor + ambientColor;
				col.a = 1 * _Transparency;

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
