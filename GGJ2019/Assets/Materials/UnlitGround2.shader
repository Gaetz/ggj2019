﻿// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/Unlit/UnlitGround2" {
	Properties{ _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
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
		SubShader{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 100

		Lighting Off

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata_t
	{
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoord2 : TEXCOORD1;
		fixed4 color : COLOR;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct Input {
		float2 uv_MainTex;
		float4 screenPos;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_FOG_COORDS(1)
			UNITY_VERTEX_OUTPUT_STEREO
	};

	sampler2D _MainTex;

	float4 _MainTex_ST;
	fixed _Cutoff;
	fixed4 _Color;

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.texcoord)* _Color * 2.0;

	clip(col.a - _Cutoff);
	UNITY_APPLY_FOG(i.fogCoord, col);

	float edgeHeight = 0.015;
	return lerp(
		col,
		fixed4(0, 4, 2, 1),
		step(col.a, _Cutoff + edgeHeight)
	);
	}
		ENDCG
	}
	}

}