Shader "Custom/BackgroundMountains" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}



	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed2 _uv = IN.uv_MainTex;
			_uv.y = _uv.y * .56;

			fixed4 c = tex2D (_MainTex, _uv);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
