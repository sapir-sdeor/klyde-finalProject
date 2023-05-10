Shader "Custom/KaleidoscopeMovementEffect" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _KaleidoscopeAngle ("Kaleidoscope Angle", Range(0,360)) = 0
        _KaleidoscopeSpeed ("Kaleidoscope Speed", Range(0,20)) = 1
        _KaleidoscopeMovement ("Kaleidoscope Movement", Vector) = (1,0,0,0)
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard

        // sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
        };

        uniform sampler2D _MainTex;
        uniform float _KaleidoscopeMovement;
        uniform float _KaleidoscopeSpeed ;
        uniform float _KaleidoscopeAngle;
        uniform float4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            float2 kaleidoscopeUV = IN.uv_MainTex;
            kaleidoscopeUV -= 0.5; // center the coordinates around (0,0)
            float angle = _KaleidoscopeAngle * 3.141592653589793238 / 180.0; // convert angle to radians
            float2 rotatedUV = float2(
                kaleidoscopeUV.x * cos(angle) - kaleidoscopeUV.y * sin(angle),
                kaleidoscopeUV.x * sin(angle) + kaleidoscopeUV.y * cos(angle)
            );
            float t = _Time * _KaleidoscopeSpeed;
            float2 offset = float2(
                sin(t) * _KaleidoscopeMovement.x + cos(t) * _KaleidoscopeMovement.y,
                cos(t) * _KaleidoscopeMovement.x - sin(t) * _KaleidoscopeMovement.y
            );
            rotatedUV += offset;
            o.Albedo = tex2D(_MainTex, rotatedUV + 0.5).rgb;
            o.Alpha = tex2D(_MainTex, rotatedUV + 0.5).a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
