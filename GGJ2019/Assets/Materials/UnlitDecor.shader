// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/Unlit/UnlitDecor" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	_Color("Main Color", Color) = (1,1,1,1)
	_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_Detail("Detail", 2D) = "gray" {}
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

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
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