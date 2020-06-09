// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "NewWater"
{
	Properties
	{
		_liquid_n("liquid_n", 2D) = "bump" {}
		_Color2("Color 2", Color) = (0.5960785,0.1882353,0.1924017,0)
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_Wave2Speed("Wave2Speed", Vector) = (-0.08,-0.06,0,0)
		_wave1Speed("wave1Speed", Vector) = (0,-0.05,0,0)
		_Wave2Tile("Wave2Tile", Vector) = (12,24,0,0)
		_Wave1Tile("Wave1Tile", Vector) = (8,16,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _liquid_n;
		uniform float2 _wave1Speed;
		uniform float2 _Wave1Tile;
		uniform sampler2D _TextureSample0;
		uniform float2 _Wave2Speed;
		uniform float2 _Wave2Tile;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float4 _Color2;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord8 = i.uv_texcoord * _Wave1Tile;
			float2 panner4 = ( _Time.y * _wave1Speed + uv_TexCoord8);
			float2 uv_TexCoord19 = i.uv_texcoord * _Wave2Tile;
			float2 panner21 = ( _Time.y * _Wave2Speed + uv_TexCoord19);
			o.Normal = BlendNormals( UnpackNormal( tex2D( _liquid_n, panner4 ) ) , UnpackNormal( tex2D( _TextureSample0, panner21 ) ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 screenColor16 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_grabScreenPos.xy/ase_grabScreenPos.w);
			o.Albedo = ( screenColor16 * _Color2 ).rgb;
			float4 color14 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 lerpResult15 = lerp( color14 , float4( 0,0,0,0 ) , float4( 0,0,0,0 ));
			o.Smoothness = lerpResult15.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17900
83;526;1630;621;1286.979;450.939;1.3;True;False
Node;AmplifyShaderEditor.Vector2Node;17;-1790.113,378.4644;Inherit;False;Property;_Wave2Tile;Wave2Tile;6;0;Create;True;0;0;False;0;12,24;12,24;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;13;-1715.553,-325.2003;Inherit;False;Property;_Wave1Tile;Wave1Tile;7;0;Create;True;0;0;False;0;8,16;8,16;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;10;-1390.857,127.3019;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;20;-1676.448,656.3506;Inherit;False;Property;_Wave2Speed;Wave2Speed;4;0;Create;True;0;0;False;0;-0.08,-0.06;-0.08,-0.06;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-1496.248,366.5386;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;9;-1869.843,38.15615;Inherit;True;Property;_wave1Speed;wave1Speed;5;0;Create;True;0;0;False;0;0,-0.05;0,-0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;18;-1381.188,780.2037;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1436.66,-236.1031;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;4;-1182.587,12.70362;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;21;-1160.672,580.9313;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenColorNode;16;-420.6438,-332.0271;Inherit;False;Global;_GrabScreen0;Grab Screen 0;2;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-879.1765,-42.98869;Inherit;True;Property;_liquid_n;liquid_n;1;0;Create;True;0;0;False;0;-1;1d21f0e46fef03b4eb740bc6c3718627;1d21f0e46fef03b4eb740bc6c3718627;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-925.827,282.893;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;-1;1d21f0e46fef03b4eb740bc6c3718627;1d21f0e46fef03b4eb740bc6c3718627;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;14;-517.3018,354.1723;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;26;-435.4551,-122.7677;Inherit;False;Property;_Color2;Color 2;2;0;Create;True;0;0;False;0;0.5960785,0.1882353,0.1924017,0;0.5960785,0.1882353,0.1924017,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;23;-518.7664,77.39904;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-40.27869,-222.1389;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;1;288.3383,-182.0335;Inherit;False;313;505;Comment;1;0;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;15;-152.541,330.9736;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;332.0979,-130.7854;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;NewWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;17;0
WireConnection;8;0;13;0
WireConnection;4;0;8;0
WireConnection;4;2;9;0
WireConnection;4;1;10;0
WireConnection;21;0;19;0
WireConnection;21;2;20;0
WireConnection;21;1;18;0
WireConnection;6;1;4;0
WireConnection;22;1;21;0
WireConnection;23;0;6;0
WireConnection;23;1;22;0
WireConnection;28;0;16;0
WireConnection;28;1;26;0
WireConnection;15;0;14;0
WireConnection;0;0;28;0
WireConnection;0;1;23;0
WireConnection;0;4;15;0
ASEEND*/
//CHKSM=15EBDF7F836ABB8F39CCDA565D69F4C29F3AE67B