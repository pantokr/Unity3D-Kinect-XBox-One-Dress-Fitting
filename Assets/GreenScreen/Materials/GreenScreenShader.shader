// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "DX11/GreenScreenShader" {
	Properties{
	_MainTex("Base (RGB)", 2D) = "white" {}
	}

		SubShader{

		Pass {

		CGPROGRAM
		#pragma target 5.0

		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		Texture2D _MainTex;

		Texture2D _SubTex;

		sampler sampler_MainTex;

		float4 _MainTex_ST;

		struct vs_input {
			float4 pos : POSITION;
			float2 tex : TEXCOORD0;
		};

		StructuredBuffer<float2> depthCoordinates;
		StructuredBuffer<float> bodyIndexBuffer;

		struct ps_input {
			float4 pos : SV_POSITION;
			float2 tex : TEXCOORD0;
		};

		ps_input vert(vs_input v)
		{
			ps_input o;
			o.pos = UnityObjectToClipPos(v.pos);
			o.tex = v.tex;

			// Flip x texture coordinate to mimic mirror.
			o.tex.x = v.tex.x;
			o.tex.y = 1 - v.tex.y;
			return o;
		}
		float4 frag(ps_input i, in uint id : SV_InstanceID) : COLOR
		{
			float4 o;

			int colorWidth = (int)(i.tex.x * (float)1920);
			int colorHeight = (int)(i.tex.y * (float)1080);
			int colorIndex = (int)(colorWidth + colorHeight * (float)1920);

			o = _SubTex.Sample(sampler_MainTex, i.tex);

			if ((!isinf(depthCoordinates[colorIndex].x) && !isnan(depthCoordinates[colorIndex].x) && depthCoordinates[colorIndex].x != 0) ||
				!isinf(depthCoordinates[colorIndex].y) && !isnan(depthCoordinates[colorIndex].y) && depthCoordinates[colorIndex].y != 0)
			{
				// We have valid depth data coordinates from our coordinate mapper.  Find player mask from corresponding depth points.
				float player = bodyIndexBuffer[(int)depthCoordinates[colorIndex].x + (int)(depthCoordinates[colorIndex].y * 512)];
				if (player != 255)
				{
					o = _MainTex.Sample(sampler_MainTex, i.tex);
				}
			}
			return o;
		}

		ENDCG

		}
	}

		Fallback Off
}