// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Flames"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
        _MainTex("InputTexture", 2D) = "white" {}

	}

    SubShader
    {
        Blend One Zero

        Pass
        {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

            struct input
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct output
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

            float4      _Color;
            sampler2D _MainTex;

            output vert(input v) 
            {
                output o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                return o;
            }

            float4 frag(output i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float4 color = _Color;

                color = fmod(tex2D(_MainTex,i.uv), fmod(_Time.x,1));

				return color;
            }
            ENDCG
        }
    }
}
