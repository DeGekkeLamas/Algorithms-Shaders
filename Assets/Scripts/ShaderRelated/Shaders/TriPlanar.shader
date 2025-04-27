
Shader "Custom/Triplanar"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Ambient("Ambient", Float) = 0
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

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				half3 objNormal : TEXCOORD0;
                float3 coords : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
				half3 objNormal : TEXCOORD0;
                float3 coords : TEXCOORD1;
			};

			float4 _MainTex_ST;
			sampler2D _MainTex;
			float _Ambient;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// In this case, we pass the *world normal* to the fragment shader:
				//  (This will be explained in Bootcamp 7)

				float4 noTranslate = v.normal;
				noTranslate.w = 0;
				float4 mat = mul(UNITY_MATRIX_M, noTranslate);
				o.normal = normalize(mat);

				o.coords =  mul(UNITY_MATRIX_M, v.vertex);
				o.objNormal = v.normal;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 col;

				float4 colX = tex2D(_MainTex, i.coords.yz / _MainTex_ST.y);
				float4 colY = tex2D(_MainTex, i.coords.xz / _MainTex_ST.xy);
				float4 colZ = tex2D(_MainTex, i.coords.xy / _MainTex_ST.x);
				float3 blendWeight = abs(i.objNormal);
				blendWeight /= dot(blendWeight, 1);

				col = colX * blendWeight.x + colY * blendWeight.y + colZ * blendWeight.z;

				//Lighting
				float4 lightDir = _WorldSpaceLightPos0;
				float4 lightColor = _LightColor0;
				float4 ambientColor = (float4(1,1,1,1) - lightColor) * _Ambient;
				float3 cameraDir = normalize(_WorldSpaceCameraPos);

				float diffuse = saturate(dot(lightDir, i.normal));

				//float cartoonLoops = 3;
				//diffuse = ceil(diffuse * cartoonLoops) / cartoonLoops;
				float reflection = saturate( lightDir - dot(2*dot(lightDir, cameraDir), cameraDir) );

				col *= diffuse * lightColor + ambientColor;

				return col;
			}
			ENDCG
		}
	}
}
