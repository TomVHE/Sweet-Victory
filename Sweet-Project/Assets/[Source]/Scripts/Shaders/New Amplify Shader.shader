// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Test"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Hightones("High tones", Range( 0.1 , 0.9)) = 0.9
		_MidTones("Mid Tones", Range( 0.1 , 0.9)) = 0.6
		_LowTones("Low Tones", Range( 0.1 , 0.9)) = 0.3
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 5.0
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float3 worldNormal;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Hightones;
		uniform float _MidTones;
		uniform float _LowTones;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult9 = dot( ase_worldNormal , ase_worldlightDir );
			float smoothstepResult11 = smoothstep( _Hightones , _Hightones , dotResult9);
			float smoothstepResult13 = smoothstep( _MidTones , _MidTones , dotResult9);
			float blendOpSrc17 = smoothstepResult11;
			float blendOpDest17 = smoothstepResult13;
			float smoothstepResult14 = smoothstep( _LowTones , _LowTones , dotResult9);
			float blendOpSrc18 = smoothstepResult13;
			float blendOpDest18 = smoothstepResult14;
			float4 temp_output_23_0 = ( smoothstepResult11 + ( float4(0.6666667,0.6666667,0.6666667,0) * ( saturate( ( 0.5 - 2.0 * ( blendOpSrc17 - 0.5 ) * ( blendOpDest17 - 0.5 ) ) )) ) + ( ( saturate( ( 0.5 - 2.0 * ( blendOpSrc18 - 0.5 ) * ( blendOpDest18 - 0.5 ) ) )) * float4(0.3333333,0.3333333,0.3333333,0) ) );
			o.Albedo = ( ( -0.2 * i.vertexColor ) + ( ( ( i.vertexColor + ( i.vertexColor * tex2D( _TextureSample0, uv_TextureSample0 ) ) ) * temp_output_23_0 ) + ( ( 1.0 - temp_output_23_0 ) * float4(0.06363474,0.06363474,0.09433961,0) ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
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
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
106;770;1419;677;2174.268;433.4917;1.278838;True;False
Node;AmplifyShaderEditor.WorldNormalVector;8;-3091.479,226.7824;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;7;-3120.465,357.6658;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;15;-2820.28,653.1317;Float;False;Property;_MidTones;Mid Tones;2;0;Create;True;0;0;False;0;0.6;0.6;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2820.28,779.5312;Float;False;Property;_LowTones;Low Tones;3;0;Create;True;0;0;False;0;0.3;0.3;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;9;-2842.15,322.7888;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2820.281,531.5309;Float;False;Property;_Hightones;High tones;1;0;Create;True;0;0;False;0;0.9;0.9;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;11;-2473.078,321.9306;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;14;-2466.68,829.1317;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;13;-2469.881,577.9307;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;19;-2090.683,1047.174;Float;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;0.3333333,0.3333333,0.3333333,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;18;-2093.24,828.4509;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;17;-2093.478,610.5437;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-2087.06,443.3463;Float;False;Constant;_Color1;Color 1;1;0;Create;True;0;0;False;0;0.6666667,0.6666667,0.6666667,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1834.32,-6.110397;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;b2911f93c6c7fa44cb19463401cba19b;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;1;-1718.367,-163.3688;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1655.796,644.6932;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1653.075,896.3774;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1305.58,337.0132;Float;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1437.066,-4.194672;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;25;-968.5636,659.4008;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;26;-984.6092,867.9969;Float;False;Constant;_Color2;Color 2;1;0;Create;True;0;0;False;0;0.06363474,0.06363474,0.09433961,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-1174.69,-145.5598;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-649.6527,793.7847;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-822.7281,218.6365;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1340.764,-404.5417;Float;False;Constant;_Float3;Float 3;1;0;Create;True;0;0;False;0;-0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-422.6384,619.8552;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1125.925,-406.4948;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-608.9858,-265.3083;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;68.48984,-43.12309;Float;False;True;7;Float;ASEMaterialInspector;0;0;Standard;Test;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;8;0
WireConnection;9;1;7;0
WireConnection;11;0;9;0
WireConnection;11;1;12;0
WireConnection;11;2;12;0
WireConnection;14;0;9;0
WireConnection;14;1;16;0
WireConnection;14;2;16;0
WireConnection;13;0;9;0
WireConnection;13;1;15;0
WireConnection;13;2;15;0
WireConnection;18;0;13;0
WireConnection;18;1;14;0
WireConnection;17;0;11;0
WireConnection;17;1;13;0
WireConnection;21;0;20;0
WireConnection;21;1;17;0
WireConnection;22;0;18;0
WireConnection;22;1;19;0
WireConnection;23;0;11;0
WireConnection;23;1;21;0
WireConnection;23;2;22;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;25;0;23;0
WireConnection;5;0;1;0
WireConnection;5;1;3;0
WireConnection;27;0;25;0
WireConnection;27;1;26;0
WireConnection;24;0;5;0
WireConnection;24;1;23;0
WireConnection;28;0;24;0
WireConnection;28;1;27;0
WireConnection;30;0;31;0
WireConnection;30;1;1;0
WireConnection;29;0;30;0
WireConnection;29;1;28;0
WireConnection;0;0;29;0
ASEEND*/
//CHKSM=8522105DDA74190BBC7EC70E7C4B26857A19CF69