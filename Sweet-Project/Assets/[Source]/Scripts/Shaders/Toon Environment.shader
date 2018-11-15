// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon Environment"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#pragma target 5.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float4 vertexColor : COLOR;
		};

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = i.vertexColor.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15600
1;742;1918;337;9689.723;4685.728;8.1065;True;True
Node;AmplifyShaderEditor.WorldNormalVector;130;-5419.005,-1868.963;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;205;-3845.888,-1325.488;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;-3788.557,-1084.624;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;-3531.004,-1868.963;Float;True;6;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;157;-3222.707,-1778.081;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;159;-3025.2,-1454.158;Float;False;Property;_ShadowColor;Shadow Color;0;0;Create;True;0;0;False;0;1,1,1,0;0.3315783,0.2354485,0.3490565,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;211;-2989.134,-1671.188;Float;True;Exclusion;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;1;-3451.442,-2359.077;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-2602.427,-1467.835;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;166;-4431.224,-2737.294;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;168;-4470.21,-2528.138;Float;False;Property;_FresnelStrenght;Fresnel Strenght;5;0;Create;True;0;0;False;0;1;10.8;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;164;-4205.43,-2860.926;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;169;-3647.917,-2828.051;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;-3063.773,-2351.744;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;170;-3940.914,-2995.051;Float;False;Property;_FresnelColor;Fresnel Color;6;0;Create;True;0;0;False;0;1,0,0,0;0.2499999,0.2499999,0.2499999,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;165;-4441.238,-2981.353;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-3867.005,-1756.963;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;-3194.362,-2125.337;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;-3868.938,-744.281;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;160;-2396.805,-1789.995;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-3851.005,-1548.963;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;210;-4282.662,-732.364;Float;True;3;0;FLOAT;0;False;1;FLOAT;1.9;False;2;FLOAT;1.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;-5226.006,-1659.963;Float;False;Property;_PartOne;Part One;1;0;Create;True;0;0;False;0;0.9;0.9;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;203;-5228.943,-1439.944;Float;False;Property;_PartFour;Part Four;4;0;Create;True;0;0;False;0;0.3;0.1;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;129;-5163.005,-1868.963;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;155;-5227.005,-1516.963;Float;False;Property;_PartThree;Part Three;3;0;Create;True;0;0;False;0;0.3941177;0.3;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-5226.005,-1589.963;Float;False;Property;_PartTwo;Part Two;2;0;Create;True;0;0;False;0;0.6;0.6;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;134;-4779.005,-1868.963;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.9;False;2;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;135;-4779.005,-1660.963;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;201;-4778.76,-1241.863;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;213;-209.5453,-157.6993;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;209;-4770.672,-1024.562;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;140;-4411.004,-1548.963;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;143;-4091.004,-1500.963;Float;False;Constant;_Color1;Color 1;0;0;Create;True;0;0;False;0;0.5019608,0.5019608,0.5019608,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;139;-4411.004,-1756.963;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;199;-4026.049,-1074.93;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;202;-4409.608,-1333.4;Float;True;Exclusion;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;204;-4098.523,-1282.012;Float;False;Constant;_Color2;Color 2;0;0;Create;True;0;0;False;0;0.2509804,0.2509804,0.2509804,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;142;-4091.004,-1708.963;Float;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;0.7529412,0.7529412,0.7529412,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;133;-5467.007,-1724.963;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SmoothstepOpNode;136;-4779.005,-1449.962;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;62;0,1.562242;Float;False;True;7;Float;ASEMaterialInspector;0;0;Standard;Toon Environment;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.03;0.1792453,0.06679422,0.1792453,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;205;0;202;0
WireConnection;205;1;204;0
WireConnection;200;0;204;0
WireConnection;200;1;199;0
WireConnection;145;0;134;0
WireConnection;145;1;141;0
WireConnection;145;2;144;0
WireConnection;145;3;205;0
WireConnection;145;4;200;0
WireConnection;145;5;212;0
WireConnection;157;0;145;0
WireConnection;211;0;157;0
WireConnection;211;1;210;0
WireConnection;158;0;211;0
WireConnection;158;1;159;0
WireConnection;164;0;165;0
WireConnection;164;4;166;0
WireConnection;164;3;168;0
WireConnection;169;0;170;0
WireConnection;169;1;164;0
WireConnection;171;0;169;0
WireConnection;171;1;1;0
WireConnection;141;0;139;0
WireConnection;141;1;142;0
WireConnection;146;0;1;0
WireConnection;146;1;145;0
WireConnection;212;0;210;0
WireConnection;212;1;143;0
WireConnection;160;0;146;0
WireConnection;160;1;158;0
WireConnection;144;0;140;0
WireConnection;144;1;143;0
WireConnection;210;0;209;0
WireConnection;129;0;130;0
WireConnection;129;1;133;0
WireConnection;134;0;129;0
WireConnection;134;1;153;0
WireConnection;134;2;153;0
WireConnection;135;0;129;0
WireConnection;135;1;154;0
WireConnection;135;2;154;0
WireConnection;201;0;129;0
WireConnection;201;1;203;0
WireConnection;201;2;203;0
WireConnection;209;0;129;0
WireConnection;140;0;135;0
WireConnection;140;1;136;0
WireConnection;139;0;134;0
WireConnection;139;1;135;0
WireConnection;202;0;136;0
WireConnection;202;1;201;0
WireConnection;136;0;129;0
WireConnection;136;1;155;0
WireConnection;136;2;155;0
WireConnection;62;0;213;0
ASEEND*/
//CHKSM=85CEFF58B48A95E96A76742BDBF61AF55B454BA4