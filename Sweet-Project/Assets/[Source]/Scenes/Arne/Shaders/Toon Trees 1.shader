// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon Trees"
{
	Properties
	{
		_ShadowColor("Shadow Color", Color) = (1,1,1,0)
		_Bend("Bend", Float) = 0.1
		_PartOne("Part One", Range( 0.1 , 0.9)) = 0.9
		_PartTwo("Part Two", Range( 0.1 , 0.9)) = 0.6
		_PartThree("Part Three", Range( 0.1 , 0.9)) = 0.3941177
		_PartFour("Part Four", Range( 0.1 , 0.9)) = 0.3
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float4 vertexColor : COLOR;
			float3 worldNormal;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _Bend;
		uniform float _PartOne;
		uniform float _PartTwo;
		uniform float _PartThree;
		uniform float _PartFour;
		uniform float4 _ShadowColor;


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float temp_output_223_0 = ( ( ase_vertex3Pos.y * cos( ( ( ( ase_worldPos.x + ase_worldPos.z ) * 1.0 ) + _Time.y ) ) ) * _Bend );
			float4 appendResult224 = (float4(temp_output_223_0 , 0.0 , temp_output_223_0 , 0.0));
			float4 break227 = mul( appendResult224, unity_ObjectToWorld );
			float4 appendResult228 = (float4(break227.x , 0 , break227.z , 0.0));
			float3 rotatedValue230 = RotateAroundAxis( float3( 0,0,0 ), appendResult228.xyz, float3( 0,0,0 ), 1.0 );
			v.vertex.xyz += rotatedValue230;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult129 = dot( ase_worldNormal , ase_worldlightDir );
			float smoothstepResult134 = smoothstep( _PartOne , _PartOne , dotResult129);
			float smoothstepResult135 = smoothstep( _PartTwo , _PartTwo , dotResult129);
			float blendOpSrc139 = smoothstepResult134;
			float blendOpDest139 = smoothstepResult135;
			float smoothstepResult136 = smoothstep( _PartThree , _PartThree , dotResult129);
			float blendOpSrc140 = smoothstepResult135;
			float blendOpDest140 = smoothstepResult136;
			float4 _Color1 = float4(0.5019608,0.5019608,0.5019608,0);
			float smoothstepResult201 = smoothstep( _PartFour , _PartFour , dotResult129);
			float blendOpSrc202 = smoothstepResult136;
			float blendOpDest202 = smoothstepResult201;
			float4 _Color2 = float4(0.2509804,0.2509804,0.2509804,0);
			float smoothstepResult210 = smoothstep( 1.9 , 1.9 , ( 1.0 - dotResult129 ));
			float4 temp_output_145_0 = ( smoothstepResult134 + ( ( saturate( ( 0.5 - 2.0 * ( blendOpSrc139 - 0.5 ) * ( blendOpDest139 - 0.5 ) ) )) * float4(0.7529412,0.7529412,0.7529412,0) ) + ( ( saturate( ( 0.5 - 2.0 * ( blendOpSrc140 - 0.5 ) * ( blendOpDest140 - 0.5 ) ) )) * _Color1 ) + ( ( saturate( ( 0.5 - 2.0 * ( blendOpSrc202 - 0.5 ) * ( blendOpDest202 - 0.5 ) ) )) * _Color2 ) + ( _Color2 * ase_lightAtten ) + ( smoothstepResult210 * _Color1 ) );
			float4 temp_cast_1 = (smoothstepResult210).xxxx;
			float4 blendOpSrc211 = ( 1.0 - temp_output_145_0 );
			float4 blendOpDest211 = temp_cast_1;
			c.rgb = ( ( i.vertexColor * temp_output_145_0 ) + ( ( saturate( ( 0.5 - 2.0 * ( blendOpSrc211 - 0.5 ) * ( blendOpDest211 - 0.5 ) ) )) * _ShadowColor ) ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Albedo = i.vertexColor.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.vertexColor = IN.color;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15600
-40;316;1523;829;1378.865;-1692.793;1;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;213;-3183.418,2038.481;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;215;-2903.527,2157.364;Float;False;Constant;_frequency;frequency;0;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;214;-2877.526,2064.365;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;130;-3995.574,314.3459;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;133;-4043.576,458.3457;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TimeNode;217;-2902.667,2234.581;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-2717.166,2063.361;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;-3802.575,523.3458;Float;False;Property;_PartOne;Part One;2;0;Create;True;0;0;False;0;0.9;0.9;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;203;-3805.512,743.3653;Float;False;Property;_PartFour;Part Four;5;0;Create;True;0;0;False;0;0.3;0.1;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;129;-3739.574,314.3459;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;155;-3803.574,666.3458;Float;False;Property;_PartThree;Part Three;4;0;Create;True;0;0;False;0;0.3941177;0.3;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-3802.574,593.3458;Float;False;Property;_PartTwo;Part Two;3;0;Create;True;0;0;False;0;0.6;0.6;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;218;-2571.583,2069.112;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;220;-2379.269,1875.156;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CosOpNode;219;-2439.269,2087.156;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;201;-3355.33,941.4468;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;134;-3355.575,314.3459;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.9;False;2;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;136;-3355.575,733.3472;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;135;-3355.575,522.3458;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;209;-3347.242,1158.748;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;202;-2986.178,849.9094;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;199;-2602.62,1108.38;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;139;-2987.574,426.3458;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;140;-2987.574,634.3458;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;143;-2667.575,682.3458;Float;False;Constant;_Color1;Color 1;0;0;Create;True;0;0;False;0;0.5019608,0.5019608,0.5019608,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;204;-2675.094,901.2981;Float;False;Constant;_Color2;Color 2;0;0;Create;True;0;0;False;0;0.2509804,0.2509804,0.2509804,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;210;-2859.233,1450.946;Float;True;3;0;FLOAT;0;False;1;FLOAT;1.9;False;2;FLOAT;1.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-2420.269,2315.156;Float;False;Property;_Bend;Bend;1;0;Create;True;0;0;False;0;0.1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;222;-2159.269,2066.156;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;142;-2667.575,474.3458;Float;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;0.7529412,0.7529412,0.7529412,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;-2011.269,1977.156;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;-2445.508,1439.029;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-2427.575,634.3458;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;205;-2422.458,857.8215;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;-2365.126,1098.686;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-2443.575,426.3458;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ObjectToWorldMatrixNode;225;-1942.635,2211.603;Float;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.DynamicAppendNode;224;-1873.205,1927.817;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;-2107.573,314.3459;Float;True;6;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;-1697.635,2110.603;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;157;-1799.274,405.2281;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;227;-1489.009,1982.208;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.VertexColorNode;1;-2028.011,-175.7659;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;211;-1565.701,512.1206;Float;True;Exclusion;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;159;-1601.767,729.1513;Float;False;Property;_ShadowColor;Shadow Color;0;0;Create;True;0;0;False;0;1,1,1,0;0.1836952,0.2561336,0.3018867,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;231;-1677.711,1682.242;Float;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;228;-1233.724,1983.63;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;229;-1237.574,1894.757;Float;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;-1770.929,57.97301;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-1178.994,715.4744;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-3046.78,-344.8264;Float;False;Property;_FresnelStrenght;Fresnel Strenght;6;0;Create;True;0;0;False;0;1;10.8;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;165;-3017.808,-798.0415;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;169;-2224.486,-644.7397;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;160;-973.3711,393.3135;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;170;-2517.485,-811.7398;Float;False;Property;_FresnelColor;Fresnel Color;7;0;Create;True;0;0;False;0;1,0,0,0;0.2499999,0.2499999,0.2499999,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;164;-2782.001,-677.6141;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;161;-177.2779,-304.8548;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;166;-3007.794,-553.9824;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RotateAboutAxisNode;230;-1063.111,1994.12;Float;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;-1640.34,-168.4326;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;62;-298.5634,1539.813;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Toon Trees;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.03;0.1792453,0.06679422,0.1792453,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;214;0;213;1
WireConnection;214;1;213;3
WireConnection;216;0;214;0
WireConnection;216;1;215;0
WireConnection;129;0;130;0
WireConnection;129;1;133;0
WireConnection;218;0;216;0
WireConnection;218;1;217;2
WireConnection;219;0;218;0
WireConnection;201;0;129;0
WireConnection;201;1;203;0
WireConnection;201;2;203;0
WireConnection;134;0;129;0
WireConnection;134;1;153;0
WireConnection;134;2;153;0
WireConnection;136;0;129;0
WireConnection;136;1;155;0
WireConnection;136;2;155;0
WireConnection;135;0;129;0
WireConnection;135;1;154;0
WireConnection;135;2;154;0
WireConnection;209;0;129;0
WireConnection;202;0;136;0
WireConnection;202;1;201;0
WireConnection;139;0;134;0
WireConnection;139;1;135;0
WireConnection;140;0;135;0
WireConnection;140;1;136;0
WireConnection;210;0;209;0
WireConnection;222;0;220;2
WireConnection;222;1;219;0
WireConnection;223;0;222;0
WireConnection;223;1;221;0
WireConnection;212;0;210;0
WireConnection;212;1;143;0
WireConnection;144;0;140;0
WireConnection;144;1;143;0
WireConnection;205;0;202;0
WireConnection;205;1;204;0
WireConnection;200;0;204;0
WireConnection;200;1;199;0
WireConnection;141;0;139;0
WireConnection;141;1;142;0
WireConnection;224;0;223;0
WireConnection;224;2;223;0
WireConnection;145;0;134;0
WireConnection;145;1;141;0
WireConnection;145;2;144;0
WireConnection;145;3;205;0
WireConnection;145;4;200;0
WireConnection;145;5;212;0
WireConnection;226;0;224;0
WireConnection;226;1;225;0
WireConnection;157;0;145;0
WireConnection;227;0;226;0
WireConnection;211;0;157;0
WireConnection;211;1;210;0
WireConnection;228;0;227;0
WireConnection;228;1;231;2
WireConnection;228;2;227;2
WireConnection;146;0;1;0
WireConnection;146;1;145;0
WireConnection;158;0;211;0
WireConnection;158;1;159;0
WireConnection;169;0;170;0
WireConnection;169;1;164;0
WireConnection;160;0;146;0
WireConnection;160;1;158;0
WireConnection;164;0;165;0
WireConnection;164;4;166;0
WireConnection;164;3;168;0
WireConnection;230;1;229;0
WireConnection;230;3;228;0
WireConnection;171;0;169;0
WireConnection;171;1;1;0
WireConnection;62;0;161;0
WireConnection;62;13;160;0
WireConnection;62;11;230;0
ASEEND*/
//CHKSM=F594D49EC15221E4301DFE8AD160B1CD8445FF9F