Shader "Custom/VelocityStretch"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Velocity ("Velocity", Vector) = (0, 0, 0)
		_Rotation ("Rotation", Vector) = (0, 0, 0)
		_VelocityRotation ("VelocityRotation", Vector) = (0, 0, 0)
		_Ambient("Ambient", Float) = 0
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

			struct Functions 
			{
				float4x4 identity() 
				{
					return float4x4(1,0,0,0,
						0,1,0,0,
						0,0,1,0,
						0,0,0,1
						);
				}
			};

			float4 _Color;
			float4 _Velocity;
			float4 _Rotation;
			float4 _VelocityRotation;
			float _Ambient;
			float _Intensity;

			v2f vert(appdata v)
			{
				v2f o;
				float4 newVertex = float4(v.vertex.x, v.vertex.y, v.vertex.z * max(length(_Velocity * _Intensity), 1) , 0);
				newVertex.z = (newVertex.z < 0) ? newVertex.z : v.vertex.z;

				_VelocityRotation.x = -_VelocityRotation.x;
				//_Rotation.y = -_Rotation.y;
				//_VelocityRotation += _Rotation;


				// Rotate over Z
				newVertex = float4(
                newVertex.x * cos(_VelocityRotation.z) - newVertex.y * sin(_VelocityRotation.z)
                , newVertex.x * sin(_VelocityRotation.z) + newVertex.y * cos(_VelocityRotation.z)
                , newVertex.z, 1
				 );
				// Rotate over X
				newVertex = float4(
                newVertex.x,
                newVertex.y * cos(_VelocityRotation.x) - newVertex.z * sin(_VelocityRotation.x)
                , newVertex.y * sin(_VelocityRotation.x) + newVertex.z * cos(_VelocityRotation.x)
				, 1);
				// Rotate over Y
				newVertex = float4(
                newVertex.x * cos(_VelocityRotation.y) - newVertex.z * sin(_VelocityRotation.y)
                , newVertex.y
                , newVertex.x * sin(_VelocityRotation.y) + newVertex.z * cos(_VelocityRotation.y)
				, 1);

				// Ignore original object rotation
				Functions f;
				float4x4 mvp = UNITY_MATRIX_MVP;
				float4x4 iden = f.identity();
				//iden._m01 = mvp._m01;
				//iden._m10 = mvp._m10;
				//iden._m00 = 2;
				mvp = mul(mvp, iden);
				//mvp._m10 = 0;
				//mvp._m01 = 0;
				o.vertex = mul(mvp, newVertex);
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
