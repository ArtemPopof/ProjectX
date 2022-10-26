Shader "Custom/Vertex_Color_Basic_bend"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_SideCurvature("SideCurvature", Float) = 0.004
		_ForwardCurvature("Curvature", Float) = 0.002
	}

		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard //fullforwardshadows

		#pragma surface surf Lambert vertex:vert noforwardadd

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		uniform sampler2D _MainTex;
		uniform float _ForwardCurvature;
		uniform float _SideCurvature;
		uniform bool _Enabled;

		struct Input
		{
			fixed3 color : COLOR;
		};

		fixed4 _Color;

		void vert(inout appdata_full v) {
			float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
			worldSpace.xyz -= _WorldSpaceCameraPos.xyz;
			worldSpace = float4((worldSpace.z * worldSpace.z) * -_SideCurvature * 0, (worldSpace.z * worldSpace.z) * -_ForwardCurvature * 1.8f, 0.0f, 0.0f);

			v.vertex += mul(unity_WorldToObject, worldSpace);
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = IN.color.rgb * _Color;
		}

		ENDCG
	}

		FallBack "Diffuse"
}