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
        float _ParentAngle;
        int _HalfNum;
        float4 _Rotation;
        

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
            float3 vertex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Use the existing Standard shader code to get the texture color and set it to tex variable
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) ;
           
            
           //float currAngle = atan(dot(float3(0, 0, 1), normalize(IN.worldPos - _WorldSpaceCameraPos)));// calculate the angle in radians
            /*float2 uv = IN.uv_MainTex - 0.5; // shift UV coordinates to the center
            float currAngle = atan2(uv.x, uv.y);*/
            float2 projectedPos = normalize(float2(IN.worldPos.z, IN.worldPos.x)); // Project vertex position onto the XZ plane
            float currAngle = atan2(projectedPos.y, projectedPos.x); // Calculate the angle based on the projected position
            if (currAngle < 0.0)
                currAngle += 2.0 * 3.14159; // ensure the angle is in the range [0, 2pi]

            float angleRange = 2.0 * 3.14159 / (360.0 / _Angle); // calculate the angle range for each division based on _Angle
            float startAngle = angleRange * (_HalfNum - 1.0); // calculate the starting angle for the current division
            float endAngle = angleRange * (_HalfNum); // calculate the ending angle for the current division
            bool isInDivision = (startAngle <= currAngle && currAngle <= endAngle);
            if (!isInDivision) {
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
   
