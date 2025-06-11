// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Energy"
{
	Properties
	{
		_T_MF_Tile_01("T_MF_Tile_01", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Emmision("Emmision", Range( 0 , 5)) = 1
		_Refract("Refract %", Range( 0 , 5)) = 0.05
		_Intensity("Intensity", Range( 0 , 2)) = 0.05
		_T_MF_Tile_02("T_MF_Tile_02", 2D) = "white" {}
		_T_MF_Energy("T_MF_Energy", 2D) = "white" {}
		_Color("Color", Vector) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _T_MF_Energy;
		uniform sampler2D _T_MF_Tile_02;
		uniform float _Refract;
		uniform sampler2D _T_MF_Tile_01;
		uniform sampler2D _TextureSample0;
		uniform float _Intensity;
		uniform float _Emmision;
		uniform float3 _Color;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord16 = i.uv_texcoord * float2( 0.7,1 );
			float2 panner17 = ( 1.0 * _Time.y * float2( -1,-1 ) + uv_TexCoord16);
			float cos38 = cos( 0.25 * _Time.y );
			float sin38 = sin( 0.25 * _Time.y );
			float2 rotator38 = mul( i.uv_texcoord - float2( 1,0 ) , float2x2( cos38 , -sin38 , sin38 , cos38 )) + float2( 1,0 );
			float cos39 = cos( 0.25 * _Time.y );
			float sin39 = sin( 0.25 * _Time.y );
			float2 rotator39 = mul( i.uv_texcoord - float2( 0,1 ) , float2x2( cos39 , -sin39 , sin39 , cos39 )) + float2( 0,1 );
			float4 temp_output_36_0 = ( tex2D( _T_MF_Energy, ( i.uv_texcoord + ( tex2D( _T_MF_Tile_02, panner17 ).g * _Refract ) ) ) * ( ( tex2D( _T_MF_Tile_01, rotator38 ).r * tex2D( _TextureSample0, rotator39 ) ) * _Intensity ) );
			o.Emission = ( ( ( temp_output_36_0 * _Emmision ) * i.vertexColor ) * float4( _Color , 0.0 ) ).rgb;
			float4 clampResult26 = clamp( ( ( temp_output_36_0 * 1.0 ) * i.vertexColor.a ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			o.Alpha = clampResult26.r;
		}

		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1927;89;1435;933;3911.215;728.3468;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-3681.018,-554.4074;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.7,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;17;-3278.431,-556.3663;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-3314.873,133.4782;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;39;-2929.046,286.2578;Float;False;3;0;FLOAT2;2,2;False;1;FLOAT2;0,1;False;2;FLOAT;0.25;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-2997.647,-346.2721;Float;True;Property;_Refract;Refract %;4;0;Create;True;0;0;False;0;0.05;0.21;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;38;-2937.738,-54.71837;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;0.25;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;18;-3014.792,-567.1903;Float;True;Property;_T_MF_Tile_02;T_MF_Tile_02;6;0;Create;True;0;0;False;0;ad13c9e62fa036c4c9c3d6bf7c4ae085;ad13c9e62fa036c4c9c3d6bf7c4ae085;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-2641.059,-528.235;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;34;-2875.516,-840.339;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-2717.23,250.6721;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;734619842a111f446acff9bdf1cdfd45;734619842a111f446acff9bdf1cdfd45;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-2712.761,-54.35533;Float;True;Property;_T_MF_Tile_01;T_MF_Tile_01;0;0;Create;True;0;0;False;0;734619842a111f446acff9bdf1cdfd45;734619842a111f446acff9bdf1cdfd45;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-2316.506,37.01316;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-2329.29,303.0712;Float;True;Property;_Intensity;Intensity;5;0;Create;True;0;0;False;0;0.05;1.77;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-2225.372,-638.6709;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1842.527,35.68402;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;35;-1925.687,-628.0992;Float;True;Property;_T_MF_Energy;T_MF_Energy;7;0;Create;True;0;0;False;0;83f0ac1088c1ff247b6ede329376e4f1;2197c5ae00cf0ce489f1a726cadfb6e5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1511.067,-240.7857;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1119.602,-105.8769;Float;True;Property;_Emmision;Emmision;3;0;Create;True;0;0;False;0;1;4.5;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1369.646,498.2549;Float;True;Constant;_Opacity;Opacity;3;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;22;-963.4019,170.2887;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1014.394,385.1193;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-810.2589,-351.9654;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-526.9664,-204.7693;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;53;-411.8506,-452.6426;Float;False;Property;_Color;Color;8;0;Create;True;0;0;False;0;1,1,1;3,1.5,0.85;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-637.7333,335.8645;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-236.601,-290.2388;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;26;-287.5992,299.4065;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Effects/Energy;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;16;0
WireConnection;39;0;43;0
WireConnection;38;0;43;0
WireConnection;18;1;17;0
WireConnection;37;0;18;2
WireConnection;37;1;19;0
WireConnection;12;1;39;0
WireConnection;11;1;38;0
WireConnection;44;0;11;1
WireConnection;44;1;12;0
WireConnection;33;0;34;0
WireConnection;33;1;37;0
WireConnection;45;0;44;0
WireConnection;45;1;46;0
WireConnection;35;1;33;0
WireConnection;36;0;35;0
WireConnection;36;1;45;0
WireConnection;50;0;36;0
WireConnection;50;1;51;0
WireConnection;47;0;36;0
WireConnection;47;1;48;0
WireConnection;49;0;47;0
WireConnection;49;1;22;0
WireConnection;52;0;50;0
WireConnection;52;1;22;4
WireConnection;54;0;49;0
WireConnection;54;1;53;0
WireConnection;26;0;52;0
WireConnection;0;2;54;0
WireConnection;0;9;26;0
ASEEND*/
//CHKSM=F7A7436CB9CDACD47C99F3B20D7F78CCBA6B94E5