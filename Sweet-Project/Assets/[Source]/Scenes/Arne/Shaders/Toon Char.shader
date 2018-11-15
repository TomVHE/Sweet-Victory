// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon Character"
{
	Properties
	{
		_PartOne("Part One", Range( 0.1 , 0.9)) = 0
		_PartTwo("Part Two", Range( 0.1 , 0.9)) = 0
		_PartThree("Part Three", Range( 0.1 , 0.9)) = 0
		_ShadowColor("Shadow Color", Color) = (0,0,0,0)
		_FresnelStrenght("Fresnel Strenght", Range( 0 , 20)) = 1
		_SkinColor("Skin Color", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 viewDir;
			float3 worldNormal;
			INTERNAL_DATA
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

		uniform float4 _SkinColor;
		uniform float _FresnelStrenght;
		uniform float _PartOne;
		uniform float _PartTwo;
		uniform float _PartThree;
		uniform float4 _ShadowColor;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV164 = dot( ase_worldNormal, i.viewDir );
			float fresnelNode164 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV164, _FresnelStrenght ) );
			float4 temp_output_169_0 = ( _SkinColor * fresnelNode164 );
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
			float4 temp_output_145_0 = ( smoothstepResult134 + ( ( saturate( ( 0.5 - 2.0 * ( blendOpSrc139 - 0.5 ) * ( blendOpDest139 - 0.5 ) ) )) * float4(0.66,0.66,0.66,0) ) + ( ( saturate( ( 0.5 - 2.0 * ( blendOpSrc140 - 0.5 ) * ( blendOpDest140 - 0.5 ) ) )) * float4(0.33,0.33,0.33,0) ) );
			c.rgb = ( ( ( temp_output_169_0 + _SkinColor ) * temp_output_145_0 ) + ( ( 1.0 - temp_output_145_0 ) * _ShadowColor ) ).rgb;
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
			float4 temp_output_172_0 = _SkinColor;
			o.Albedo = temp_output_172_0.rgb;
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV164 = dot( ase_worldNormal, i.viewDir );
			float fresnelNode164 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV164, _FresnelStrenght ) );
			float4 temp_output_169_0 = ( _SkinColor * fresnelNode164 );
			o.Emission = temp_output_169_0.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred 

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
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
7;29;1906;1044;3330.891;1177.916;2.80213;True;False
Node;AmplifyShaderEditor.WorldNormalVector;130;-4091.444,193.0079;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;133;-4139.714,338.121;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;155;-3904.615,533.2;Float;False;Property;_PartThree;Part Three;2;0;Create;True;0;0;False;0;0;0.3;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-3905.357,464.9478;Float;False;Property;_PartTwo;Part Two;1;0;Create;True;0;0;False;0;0;0.6;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;129;-3841.099,191.0671;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;-3907.414,396.9435;Float;False;Property;_PartOne;Part One;0;0;Create;True;0;0;False;0;0;0.9;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;135;-3455.634,399.6285;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;134;-3457.743,182.4128;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.9;False;2;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;136;-3455.634,607.8587;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;139;-3082.241,298.3589;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;140;-3079.447,514.4678;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-3495.498,-360.5407;Float;False;Property;_FresnelStrenght;Fresnel Strenght;4;0;Create;True;0;0;False;0;1;3.1;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;143;-2760.028,558.8518;Float;False;Constant;_Color1;Color 1;0;0;Create;True;0;0;False;0;0.33,0.33,0.33,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;165;-3466.526,-813.7557;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;166;-3456.514,-569.6968;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;142;-2761.588,346.0772;Float;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;0.66,0.66,0.66,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-2529.705,514.4892;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-2531.546,300.3425;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;172;-2901.992,-203.5704;Float;False;Property;_SkinColor;Skin Color;5;0;Create;True;0;0;False;0;0,0,0,0;1,0.406,0.371,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;164;-3230.72,-693.3284;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;-2203.704,187.6978;Float;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;169;-2673.204,-660.454;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;-2221.022,-291.0024;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;159;-1921.252,685.5716;Float;False;Property;_ShadowColor;Shadow Color;3;0;Create;True;0;0;False;0;0,0,0,0;0.882353,0.371,0.8901961,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;157;-1826.665,68.28783;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-1590.366,501.2293;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;-1829.395,329.5801;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;174;-1131.007,839.8099;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;175;-909.8711,869.7377;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;160;-1325.578,240.9289;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;161;-177.2779,-304.8548;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightAttenuation;173;-1167.694,755.0248;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;62;0,1.562242;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Toon Character;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.03;0.1792453,0.06679422,0.1792453,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;129;0;130;0
WireConnection;129;1;133;0
WireConnection;135;0;129;0
WireConnection;135;1;154;0
WireConnection;135;2;154;0
WireConnection;134;0;129;0
WireConnection;134;1;153;0
WireConnection;134;2;153;0
WireConnection;136;0;129;0
WireConnection;136;1;155;0
WireConnection;136;2;155;0
WireConnection;139;0;134;0
WireConnection;139;1;135;0
WireConnection;140;0;135;0
WireConnection;140;1;136;0
WireConnection;144;0;140;0
WireConnection;144;1;143;0
WireConnection;141;0;139;0
WireConnection;141;1;142;0
WireConnection;164;0;165;0
WireConnection;164;4;166;0
WireConnection;164;3;168;0
WireConnection;145;0;134;0
WireConnection;145;1;141;0
WireConnection;145;2;144;0
WireConnection;169;0;172;0
WireConnection;169;1;164;0
WireConnection;171;0;169;0
WireConnection;171;1;172;0
WireConnection;157;0;145;0
WireConnection;158;0;157;0
WireConnection;158;1;159;0
WireConnection;146;0;171;0
WireConnection;146;1;145;0
WireConnection;175;0;173;0
WireConnection;175;1;174;0
WireConnection;160;0;146;0
WireConnection;160;1;158;0
WireConnection;62;0;172;0
WireConnection;62;2;169;0
WireConnection;62;13;160;0
ASEEND*/
//CHKSM=DC3A0CD6EC331E2F6C0250CE4EBFADAA27CC565B