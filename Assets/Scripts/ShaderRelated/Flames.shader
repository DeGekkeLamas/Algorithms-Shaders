Shader "CustomRenderTexture/Flames"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
        _MainTex("InputText", 2D) = "white" {}

	}

    SubShader
    {
        Blend One Zero

        Pass
        {
            Name "Flames"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            sampler2D _MainTex;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
                float2 center = float2(0.5, 0.5);
                float4 color = _Color;

                color = fmod(tex2D(_MainTex,uv), fmod(_Time.x,1));

				return color;
            }
            ENDCG
        }
    }
}
