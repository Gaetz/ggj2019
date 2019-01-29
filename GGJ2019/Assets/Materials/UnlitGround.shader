// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/Unlit/UnlitGround" {
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Main Color", Color) = (1,1,1,1)

		_DetailTex("Detail (RGB)", 2D) = "white" {}
	_Strength("Detail Strength", Range(0.0, 1.0)) = 0.2

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
	{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 100

		Lighting Off
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_fog

#include "UnityCG.cginc"
#include "UnityUI.cginc"



		struct appdata_t
	{
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoord2 : TEXCOORD1;
		fixed4 color : COLOR;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoord2 : TEXCOORD1;
		float4 worldPosition : TEXCOORD2;
		fixed4 color : COLOR;
		UNITY_FOG_COORDS(1)
			UNITY_VERTEX_OUTPUT_STEREO
	};

	sampler2D _MainTex;
	sampler2D _DetailTex;
	float4 _MainTex_ST;
	float4 _DetailTex_ST;
	float4 _DetailTex_TexelSize;
	fixed4 _Color;
	fixed _Strength;

	fixed4 _TextureSampleAdd;

	bool _UseClipRect;
	float4 _ClipRect;

	bool _UseAlphaClip;

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.worldPosition = v.vertex;
		o.vertex = UnityObjectToClipPos(o.worldPosition);

		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.texcoord2 = TRANSFORM_TEX(v.texcoord2 , _DetailTex);
		o.color = v.color * _Color;
		UNITY_TRANSFER_FOG(o, o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		fixed4 color = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd) * i.color;
	fixed4 detail = tex2D(_DetailTex, i.texcoord2);
	color.rgb = lerp(color.rgb, color.rgb * detail.rgb, detail.a * _Strength);
	color = color * _Color;
	UNITY_APPLY_FOG(i.fogCoord, color);



	return color;
	}
		ENDCG

	}
	}
}