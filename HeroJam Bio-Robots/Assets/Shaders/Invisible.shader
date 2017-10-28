// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Invisible" {
	Properties{
		_OutlineColorInvisible("Outline Color", Color) = (0,0,0,0)
		_OutlineInvisible("Outline width", Range(0.0, 0.03)) = 0.75
	}

		CGINCLUDE
#include "UnityCG.cginc"

		struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR;
	};

	uniform float _OutlineInvisible;
	uniform float4 _OutlineColorInvisible;

	v2f vert(appdata v) {
		// just make a copy of incoming vertex data but scaled according to normal direction
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		float2 offset = TransformViewToProjection(norm.xy);

		//o.pos.xy += offset * o.pos.z * _OutlineInvisible;
		//o.color = _OutlineColorInvisible;
		return o;
	}
	ENDCG

		SubShader{
		Tags{ "Queue" = "Transparent" }

		Pass{
		Name "BASE"
		Cull Back
		Blend Zero One

		// uncomment this to hide inner details:
		//Offset -8, -8

		SetTexture[_OutlineColorInvisible]{
		ConstantColor(0,0,0,0)
		Combine constant
	}
	}

		// note that a vertex shader is specified here but its using the one above
		Pass{
		Name "OUTLINE"
		Tags{ "LightMode" = "Always" }
		Cull Front

		// you can choose what kind of blending mode you want for the outline
		//Blend SrcAlpha OneMinusSrcAlpha // Normal
		//Blend One One // Additive
		Blend One OneMinusDstColor // Soft Additive
		//Blend DstColor Zero // Multiplicative
		//Blend DstColor SrcColor // 2x Multiplicative

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		half4 frag(v2f i) :COLOR{
		return i.color;
	}
		ENDCG
	}


	}

		Fallback "Diffuse"
}