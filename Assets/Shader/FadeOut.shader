Shader "Custom/FadeOut"
{
     Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _FadeSpeed ("Fade Speed", Range(0, 1)) = 0.5
        _StartTime ("Time", Range(0, 1)) = 0
    }
    
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend srcAlpha OneMinusSrcAlpha
        LOD 100
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float time;
            float _StartTime = 0;
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos: TEXCOORD1;
            };
            
            sampler2D _MainTex;
            float _FadeSpeed;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                float xPos = i.worldPos.x;
                float elapsed = _StartTime;
                if (xPos < 0)
                {
                    float t = saturate(elapsed * _FadeSpeed);
                    col.a = lerp(1, 0, t);
                    if (col.a == 0)
                    {
                        _StartTime = 0;
                    }
                }
                else if (xPos >= 0 && col.a < 1)
                {
                    float t = saturate(elapsed * _FadeSpeed);
                    col.a = lerp(0, 1, t);
                     if (col.a == 1)
                    {
                        _StartTime = 0;
                    }
                }
                return col;
/*              
                time = 0;
                time += unity_DeltaTime;
                float xPos = i.worldPos.x;
                
                if (i.worldPos.x < 0)
                {
                    float t = time * _FadeSpeed;
                    col.a = col.a * t;
                    //float t = time * _FadeSpeed;
                    //col.a = lerp(1, 0, t);
                }
                else
                {
                    float t = time / _FadeSpeed;
                    col.a = col.a * t;
                 //   float t = time / _FadeSpeed;
                  //  col.a = lerp(1, 0, t);
                }
                if (col.a <= 0)
                {
                    col.a = 0;
                    time = 0;
                }
                return col;*/
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
