Shader "Hidden/Custom/Redify"
{
	HLSLINCLUDE

	#include "PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		float _minRed;
		float _maxRed;
		float4 _inputColor;
		float4 _outputColor;
		
		float4 Frag(VaryingsDefault i) : SV_Target
		{
			float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
			float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
			float weight = smoothstep(_minRed, _maxRed, dot(color.rgb, _inputColor.rgb) - luminance);

			color.rgb = lerp(luminance.xxx, color.rgb * _outputColor.rgb, weight); 

			return color;
		}
	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDefault
				#pragma fragment Frag
			ENDHLSL
		}
	}
}
