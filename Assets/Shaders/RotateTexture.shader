Shader "Custom/RotateTexture" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Default" "RenderType" = "Opaque" }
		Lighting On
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
	};

	float _RotationSpeed;
	void vert(inout appdata_full v) {
		float2x2 rotationMatrix = float2x2(0, 1, 1, 0);
		v.texcoord.xy = mul(v.texcoord.xy, rotationMatrix);
	}

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}