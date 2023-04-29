Shader "Custom/rayMarchingBlend" {
    Properties {
        _MainTex1 ("Texture1", 2D) = "white" {}
        _MainTex2 ("Texture2", 2D) = "white" {}
        _Cube1Pos ("Cube1 Position", Vector) = (0,0,0,0)
        _Cube1Scale ("Cube1 Scale", Vector) = (1,1,1,1)
        _Cube2Pos ("Cube2 Position", Vector) = (0,0,0,0)
        _Cube2Scale ("Cube2 Scale", Vector) = (1,1,1,1)
        _BlendDistance ("Blend Distance", Range(0, 1)) = 0.1
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

           struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord1 : TEXCOORD1;
            };

           struct v2f {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float4 texcoord1 : TEXCOORD2;
            };

            sampler2D _MainTex1;
            sampler2D _MainTex2;
            float4 _Cube1Pos;
            float4 _Cube1Scale;
            float4 _Cube2Pos;
            float4 _Cube2Scale;
            float _BlendDistance;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.texcoord1 = v.texcoord1;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Sample textures using per-object texture coordinates
                float4 tex1 = tex2D(_MainTex1, i.texcoord1.xy);
                float4 tex2 = tex2D(_MainTex2, i.texcoord1.xy);
                
                // Perform raymarching blend using surface distance
                float3 viewDir = UnityWorldSpaceViewDir(i.worldPos);
                float3 rayOrigin = i.worldPos;
                float3 rayDir = normalize(viewDir);
                float t = 0.0;
                float4 blendColor = float4(0,0,0,0);
                
                // Raymarch through cube 1 and cube 2
                for (int j = 0; j < 50; j++) {
                    float3 p = rayOrigin + rayDir * t;
                    float3 q1 = abs(p - _Cube1Pos.xyz) - _Cube1Scale.xyz * 0.5;
                    float d1 = max(q1.x, max(q1.y, q1.z));
                    float3 q2 = abs(p - _Cube2Pos.xyz) - _Cube2Scale.xyz * 0.5;
                    float d2 = max(q2.x, max(q2.y, q2.z));
                    float blend = smoothstep(0.0, _BlendDistance, min(d1, d2));
                    blendColor += lerp(tex1, tex2, blend) * (1.0 - blendColor.a);
                    if (blend >= 1.0) {
                        break;
                    }
                    t += _BlendDistance * 0.5;
                }
                return blendColor;
}


            ENDCG
        }
    }
}
