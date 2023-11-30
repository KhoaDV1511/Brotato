Shader "UI/DFont" {
	Properties {
		[PerRendererData] _MainTex ("Font Texture", 2D) = "white" {}
		_Color ("Tint", Vector) = (1,1,1,1)
		[Toggle(_OUTLINE_Font_)] _OUTLINE_Font_ ("_OUTLINE_Font_", Float) = 0
		_OutlineWidth ("Outline Width", Float) = 1
		_OutlineColor ("Outline Color", Vector) = (1,1,1,1)
		_AlphaValue ("Out Value", Range(0, 1)) = 0.1
		_LerpValue ("Inner Value", Range(0.1, 0.9)) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}