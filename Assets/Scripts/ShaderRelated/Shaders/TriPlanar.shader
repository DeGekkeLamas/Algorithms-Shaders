
Shader "Custom/Triplanar"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Reflectiveness("Reflectiveness", Float) = 16
		_CellShadeLoops("Cell shade loops", Integer) = 3
		_BlendStrength("Blend strength", Float) = 1
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

			struct input
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
                float3 coords : TEXCOORD1;
			};

			struct output
			{
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
                float3 coords : TEXCOORD1;
			};

			float4 _MainTex_ST;
			sampler2D _MainTex;
			float _Reflectiveness;
			int _CellShadeLoops;
			float _BlendStrength;

			output vert(input v)
			{
				output o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// In this case, we pass the *world normal* to the fragment shader:
				//  (This will be explained in Bootcamp 7)

				float4 noTranslate = v.normal;
				noTranslate.w = 0;
				float4 mat = mul(UNITY_MATRIX_M, noTranslate);
				o.normal = normalize(mat);

				o.coords =  mul(UNITY_MATRIX_M, v.vertex);

				return o;
			}

			fixed4 frag(output i) : SV_Target
			{
				float4 col;

				float4 colX = tex2D(_MainTex, i.coords.yz / _MainTex_ST.xy);
				float4 colY = tex2D(_MainTex, i.coords.xz / _MainTex_ST.xy);
				float4 colZ = tex2D(_MainTex, i.coords.xy / _MainTex_ST.xy);
				// blendWeight blends the color if multple sampled colors have values
				float3 blendWeight = pow( abs(i.normal), _BlendStrength);
				// ensures total sum of the components in blendWeight is 1
				blendWeight /= dot(blendWeight, 1);
				
				col = colX * blendWeight.x + colY * blendWeight.y + colZ * blendWeight.z;

				
				// Lighting
				float4 lightDir = _WorldSpaceLightPos0;
				float4 lightColor = _LightColor0;
				float4 ambientColor = (float4(1,1,1,1) - lightColor);
				float3 cameraDir = _WorldSpaceCameraPos;
				// Diffuse
				float diffuse = saturate(dot(lightDir, i.normal));
				// Specular
				float3 reflectionDir = normalize(cameraDir - i.coords);
				float4 reflection = reflect(float4(-reflectionDir, 0), normalize(i.normal));
				float specular = dot(reflection, lightDir);
				specular = pow( specular, _Reflectiveness);
				float4 specular4 = saturate( float4(specular.xxx, 0) * lightColor );
				diffuse += specular4;
				// Cell shade conversion
				diffuse = ceil(diffuse * _CellShadeLoops + pow(_CellShadeLoops, -1)) / _CellShadeLoops;
				 
				col *= (diffuse + ambientColor) * lightColor;

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
