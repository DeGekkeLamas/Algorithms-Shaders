Shader "Custom/VelocityStretch"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Velocity ("Velocity", Vector) = (0, 0, 0)
		_Ambient("Ambient", Float) = 0
		_Intensity("Intnsity", Float) = .5
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

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
			};

			float4 _Color;
			float4 _Velocity;
			float _Ambient;
			float _Intensity;

			v2f vert(appdata v)
			{
				v2f o;
				float4 newVertex = v.vertex * (_Velocity + float4(1,1,1,1) );
				o.vertex = UnityObjectToClipPos(newVertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = _Color;
				col = col *	(_Ambient + (1 - _Ambient) * saturate(i.normal.y)); // VERY basic lighting. TODO LATER: improve
				return col;
			}
			ENDCG
		}
	}
}
