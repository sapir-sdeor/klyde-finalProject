Shader "Custom/Kalid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Range(0, 10)) = 1.0
        _Angle ("Angle", Range(0, 360)) = 0.0
        _Segments ("Segments", Range(1, 20)) = 6
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input {
            float2 uv_MainTex;
        };

        uniform sampler2D _MainTex;
        uniform float _Scale;
        uniform float _Angle;
        uniform float _Segments;
        uniform float4 _Color;

        void surf (Input IN, inout SurfaceOutput o) {
            float2 p = IN.uv_MainTex - 0.5; 
            float r = length(p);
            float theta = atan2(p.y, p.x);

            // Rotate the texture coordinates by the angle
            theta += radians(_Angle);

            // Reflect the texture coordinates based on the number of segments
            float segmentAngle = 2.0 * 3.14159 / _Segments;
            float segmentIndex = floor(theta / segmentAngle);
            float reflectAngle = (segmentIndex + 0.5) * segmentAngle;
            theta = 2.0 * reflectAngle - theta;

            // Scale the texture coordinates
            p = r * float2(cos(theta), sin(theta)) * _Scale;

            o.Albedo = tex2D(_MainTex, p + 0.5);
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
