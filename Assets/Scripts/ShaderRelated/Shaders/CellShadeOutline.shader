
Shader "Custom/CellShadeOutline"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		_Reflectiveness("Reflectiveness", Float) = 16
		_CellShadeLoops("Cell shade loops", Integer) = 3
		_Emissiveness("Emissiveness", Float) = 0

		//_SrcBlendAlpha("__SrcBlendAlpha", Float) = 0
		_Transparency("Transparency", Range(0.0, 1.0)) = 1
	}
	SubShader
	{
		// Tags {
		// 	"RenderType" = "Transparent"
		// 	"Queue" = "Transparent"
		// 	}
		// Blend SrcAlpha OneMinusSrcAlpha
		// Cull Back
		LOD 100
		//ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

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

				col = tex2D(_MainTex, i.uv / _MainTex_ST.xy + _MainTex_ST.zw) * _Color;

				// Lighting
				float4 lightDir = _WorldSpaceLightPos0;
				float4 lightColor = _LightColor0;
				float4 ambientColor = (float4(1,1,1,1) - lightColor);
				float3 cameraDir = _WorldSpaceCameraPos;
				// Diffuse
				float diffuse = saturate(dot(lightDir, i.normal));
				// Specular
				float3 reflectionDir = normalize(cameraDir - i.worldPos);
				float4 reflection = reflect(float4(-reflectionDir, 0), normalize(i.normal));
				float specular = dot(reflection, lightDir);
				specular = pow( specular, _Reflectiveness);
				float4 specular4 = saturate( float4(specular.xxx, 0) * lightColor );
				diffuse += specular4;
				// Cell shade conversion
				diffuse = ceil(diffuse * _CellShadeLoops + pow(_CellShadeLoops, -1)) / _CellShadeLoops;
				 
				col *= (diffuse + ambientColor) * lightColor;
				col.a = 1 * _Transparency;
				col *= 1 + _Emissiveness;

				//float4 depth = mul(i.vertex, UNITY_MATRIX_MV);
				//depth.w = 1;
				//depth = sign(depth);
				//return depth;

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
