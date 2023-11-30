Shader "Custom/Outline" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_OutlineWidth ("Outline Width", Float) = 1
		_OutlineColor ("Outline Color", Vector) = (1,1,1,1)
		[Toggle(_OUTLINE_)] _OUTLINE_ ("_OUTLINE_", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}