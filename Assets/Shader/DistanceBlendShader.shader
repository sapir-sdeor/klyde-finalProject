Shader "Custom/DistanceBlendShader"
{
   Properties {
        _MainTex1 ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _Distance ("Distance", Range(0.0, 1.0)) = 0.5
    }

    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float dist : TEXCOORD1;
            };

            sampler2D _MainTex1;
            sampler2D _MainTex2;
            float _Distance;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy * 0.5 + 0.5;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float dist = distance(i.uv, float2(0.5, 0.5));
                dist = (dist - _Distance) / (1.0 - _Distance);

                fixed4 c1 = tex2D(_MainTex1, i.uv);
                fixed4 c2 = tex2D(_MainTex2, i.uv);
                fixed4 color = lerp(c1, c2, smoothstep(0.0, 1.0, dist));

                return color;
            }
            ENDCG
        }
    }
}
