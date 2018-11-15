Shader "Custom/Shockwave"
{
	Properties
	{
		_Color("Tint", Color) = (1,1,1,1)
		_RefPow("Refraction Power", Range(0,128)) = 10
		_Rim("Rim Power", Range(0,16)) = 4
		_Size("Shockwave Size", Float) = 1
	}
		SubShader
	{
		GrabPass {"_Ref"}

		Tags { "RenderType" = "CustomType" "Queue" = "Transparent+1" }
		LOD 100

			CGPROGRAM
			#pragma surface surf Lambert fullforwardshadows alpha
			#pragma vertex vert

			#include "UnityCG.cginc"

			sampler2D _Ref;
			fixed4 _Ref_TexelSize;

			struct Input
			{
				float4 screenPos;
				float3 viewDir;
			};

			half _RefPow;
			half _Rim;
			half _Size;
			fixed4 _Color;

			void vert(inout appdata_full v) {
				v.vertex.xyz += v.vertex.xyz * (_Size - 1);
			}

			void surf(in Input i, inout SurfaceOutput o)
			{
				float rim = 1.0 - saturate(dot(normalize(i.viewDir), normalize(o.Normal)));

				float2 offset = o.Normal * (_RefPow * 4 * pow(rim, _Rim)) * _Ref_TexelSize;
				i.screenPos.xy = i.screenPos.z * offset + i.screenPos.xy;

				fixed4 col = tex2Dproj(_Ref, UNITY_PROJ_COORD(i.screenPos)) * _Color;
				o.Emission = col;
				o.Alpha = _Color.a;
			}
			ENDCG

	}
}