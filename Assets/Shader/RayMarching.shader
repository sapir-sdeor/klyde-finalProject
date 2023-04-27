Shader "Custom/RayMarching" {
  Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Opaque"}

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            uniform float _MetaballStrength;
            uniform float _MetaballRadius;
            uniform sampler3D _MetaballSDF;
            uniform float4 _MetaballPosition;

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 worldPosition : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float sampleSDF(float3 position)
            {
                return tex3D(_MetaballSDF, position).r;
            }

            float3 calculateNormal(float3 position)
            {
                float d = 0.001f;
                float3 normal = float3(0, 0, 0);
                normal.x = sampleSDF(position + float3(d, 0, 0)) - sampleSDF(position - float3(d, 0, 0));
                normal.y = sampleSDF(position + float3(0, d, 0)) - sampleSDF(position - float3(0, d, 0));
                normal.z = sampleSDF(position + float3(0, 0, d)) - sampleSDF(position - float3(0, 0, d));
                return normalize(normal);
            }

            float4 frag (v2f i) : SV_Target
            {
                float3 position = i.worldPosition - _MetaballPosition.xyz;
                float distance = sampleSDF(position) * _MetaballStrength;
                float alpha = 1.0 - saturate(distance / (_MetaballRadius * 0.5));
                float4 color = tex2D(_MainTex, float2(i.worldPosition.x, i.worldPosition.y));
                return color * alpha;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
