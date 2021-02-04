Shader "Custom/Vertex_Color_Basic" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_SideCurvature("SideCurvature", Float) = 0.004
		_ForwardCurvature("Curvature", Float) = 0.002
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard //fullforwardshadows
		
		#pragma surface surf Lambert vertex:vert addshadow
		uniform float _ForwardCurvature;
		uniform float _SideCurvature;
		uniform bool _Enabled;
		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		struct Input 
		{
			fixed3 color : COLOR;
		};

		fixed4 _Color;

		void vert(inout appdata_full v)
		{
			float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
			worldSpace.xyz -= _WorldSpaceCameraPos.xyz;
			worldSpace = float4((worldSpace.z * worldSpace.z) * -_SideCurvature * 0, (worldSpace.z * worldSpace.z) * -_ForwardCurvature * 1.8f, 0.0f, 0.0f);

			v.vertex += mul(unity_WorldToObject, worldSpace);
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = IN.color.rgb* _Color;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
