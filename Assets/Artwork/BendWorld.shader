Shader "AbbySoft/BendWorld"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SideCurvature("SideCurvature", Float) = 0.004
        _ForwardCurvature("Curvature", Float) = 0.002
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert addshadow
        uniform sampler2D _MainTex;
        uniform float _ForwardCurvature;
        uniform float _SideCurvature;
        uniform bool _Enabled;

        struct Input
        {
            float2 uv_MainTex;
		};

        void vert(inout appdata_full v)
        {
            float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
            worldSpace.xyz -= _WorldSpaceCameraPos.xyz;
            //worldSpace = float4( (worldSpace.z * worldSpace.z) * -_SideCurvature, (worldSpace.z * worldSpace.z) * -_ForwardCurvature, 0.0f, 0.0f);

            //v.vertex += mul(unity_WorldToObject, worldSpace);
		}

        void surf(Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
		}
        ENDCG
    }
    FallBack "Mobile/Diffuse"
}
