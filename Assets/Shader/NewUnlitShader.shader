Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Transparent ("Transparent", Range(0,1)) = 0.5
        _Angle ("Angle", Range(0, 360)) = 45
        _HalfNum ("Half Number", Range(0, 10)) = 2
    }
    SubShader
    {
         Tags {  "IgnoreProjector" = "True"
            "RenderType" = "Transparent"}
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma Lambert alpha:fade
            #pragma target 3.0
            sampler2D _MainTex;
            fixed4 _Color;
            float _Metallic;
            float _Glossiness;
            float _Transparent;
            float _Angle;
            int _HalfNum;
            float4 _Rotation;
            float _Softness;
            float4 _MainTex_ST;
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : SV_POSITION;
                float3 worldPos: TEXCOORD1;
            };


            /*v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
               // UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }*/
            
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
               // Use the existing Standard shader code to get the texture color and set it to tex variable
                fixed4 tex = tex2D(_MainTex, i.uv);
                float2 projectedPos = normalize(float2(i.worldPos.z, i.worldPos.x)); // Project vertex position onto the XZ plane
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
                fixed4 outputColor;
                outputColor.rgb = tex.rgb * _Color.rgb * tex.a;
                outputColor.a = tex.a;
                 UNITY_APPLY_FOG(i.fogCoord, outputColor);
                // Apply alpha:fade
                fixed alpha = outputColor.a;
                fixed4 c = outputColor * alpha;
                c.a = alpha;
                outputColor = c;
                return outputColor;
            }
            ENDCG
        }
    }
}
