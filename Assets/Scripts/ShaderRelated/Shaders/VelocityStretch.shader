Shader "Custom/VelocityStretch"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Velocity ("Velocity", Vector) = (0, 0, 0)
		_Rotation ("Rotation", Vector) = (0, 0, 0)
		_VelocityRotation ("Position", Vector) = (0, 0, 0)
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
				static const float PI = 3.14159265f;
				float PingPongAngle(float value) 
				{
					return (abs(fmod(value + 180, 360)-180) / 180);
				}

				float PingPongVelocity(float value) 
				{
					return 90 - 90 * value;
				}

				float GetAngleBetweenVectors(float4 dir1, float4 dir2)
				{
					float _angle = acos( dot(dir2, dir1) / (length(dir2) * length(dir1) ) ) * (180 / PI );
					return _angle;
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
				float4 newVertex = float4(v.vertex.x, v.vertex.y, v.vertex.z * max(length(_Velocity * _Intensity), 1) , 1);
				newVertex.z = (newVertex.z < 0) ? newVertex.z : v.vertex.z;

				Functions f;
				_Rotation = _VelocityRotation;
				_Rotation.x = -_Rotation.x;


				// Rotate over Z
				newVertex = float4(
                newVertex.x * cos(_Rotation.z) - newVertex.y * sin(_Rotation.z)
                , newVertex.x * sin(_Rotation.z) + newVertex.y * cos(_Rotation.z)
                , newVertex.z, 1
				 );
				// Rotate over X
				newVertex = float4(
                newVertex.x,
                newVertex.y * cos(_Rotation.x) - newVertex.z * sin(_Rotation.x)
                , newVertex.y * sin(_Rotation.x) + newVertex.z * cos(_Rotation.x)
				, 1);
				// Rotate over Y
				newVertex = float4(
                newVertex.x * cos(_Rotation.y) - newVertex.z * sin(_Rotation.y)
                , newVertex.y
                , newVertex.x * sin(_Rotation.y) + newVertex.z * cos(_Rotation.y)
				, 1);


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
