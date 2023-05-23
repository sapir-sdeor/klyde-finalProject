Shader "Custom/combineMask"
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Transparent ("Transparent", Range(0,1)) = 0.5
        _Angle ("Angle", Range(0, 360)) = 45
        _HalfNum ("Half Number", Range(0, 10)) = 2
        _CurrAngle("CurrAngle", Range(0, 360)) = 45
    }
    SubShader {
        Tags {  "IgnoreProjector" = "True"
            "RenderType" = "Transparent"}
        
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade addshadow
        sampler2D _MainTex;
        fixed4 _Color;
        float _Metallic;
        float _Glossiness;
        float _Transparent;
        float _Angle;
        float _CurrAngle;
        int _HalfNum;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Use the existing Standard shader code to get the texture color and set it to tex variable
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) ;
          
            float currAngle = acos(dot(float3(0, 0, 1), normalize(IN.worldPos)));
            if (IN.worldPos.x < 0)
            {
                currAngle = 360 - currAngle;
            }
            // float currAngle = acos(dot(float3(0, 0, 1), normalize(IN.worldPos)));
            // currAngle += _CurrAngle;
            if (!(_Angle * (_HalfNum - 1) <= currAngle && currAngle <= _Angle * _HalfNum))
            {
                tex.a = _Transparent;
            }

            
            // Set the surface properties using the existing Standard shader code
            o.Albedo = tex.rgb * _Color.rgb * tex.a;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = tex.a;
        }
        ENDCG
    }
    FallBack "Standard"
}
   
