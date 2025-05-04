
Shader "Custom/ProjectileChart"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		_Thickness("Thickness", Float) = 0.05
		_Height("Height", Float) = 0.5
	}
	SubShader
	{
		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		LOD 100
		ZWrite Off

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
				float2 uv : TEXCOORD0;
			};

			struct output
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _MainTex_ST;
			sampler2D _MainTex;
			float4 _Color;
			float _Thickness;
			float _Height;
			float3 _Target;

			output vert(input v)
			{
				output o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}

			fixed4 frag(output i) : SV_Target
			{
				float4 col;
				float size = 30;

				float distance = length(_Target.xz) / size;
				col = float4(i.uv.xy, 1, 1);
				float value = (pow((1 - abs(i.uv.x / distance -.5) * 2), .5) * _Height) + (i.uv.x * _Target.y/size / distance);

				bool isColored = value > i.uv.y - _Thickness && value < i.uv.y + _Thickness;
				col = isColored ? _Color : 0;

				return col;
			}
			ENDCG
		}
	}
}
