Shader "Custom/Kalideoscope"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NumSlices ("Number of Slices", Range(1, 20)) = 3
    }
 
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Opaque"}
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Declare the angle variable as a uniform
            uniform float angle;
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float _NumSlices;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                float2 p = i.uv - 0.5;
                float r = length(p);
                float angle = atan2(p.y, p.x);
                
 
                // Divide the circle into slices
                angle = angle / (2.0 * 3.14159265359 / _NumSlices);
                angle = angle - floor(angle);
 
                // Reflect and rotate the texture
                float2 reflection = abs((angle - r) % 1.0 - 0.5) * 2.0;
                float2 rotated = float2(cos(angle), sin(angle)) * r;
                fixed4 texel = tex2D(_MainTex, rotated + 0.5);
 
                // Apply the reflection
                if (angle > 0.5) {
                    texel *= reflection.x;
                } else {
                    texel *= reflection.y;
                }
 
                return texel;
            }
            ENDCG
        }
    }
}
