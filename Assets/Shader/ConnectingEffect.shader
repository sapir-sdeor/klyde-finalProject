Shader "Custom/ConnectingEffect"
{
    Properties
    {
        _MainTex1 ("Texture1", 2D) = "white" {}
        _MainTex2 ("Texture2", 2D) = "white" {}
        _Distance ("Distance", Range(0, 1)) = 0.5
    }
 
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };
 
            struct v2f
            {
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex1;
            sampler2D _MainTex2;
            float _Distance;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1 = v.uv1;
                o.uv2 = v.uv2;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the textures for each object
                fixed4 texel1 = tex2D(_MainTex1, i.uv1);
                fixed4 texel2 = tex2D(_MainTex2, i.uv2);
 
                // Calculate the distance between the two objects
                float distance = length(i.uv1 - i.uv2);
 
                // Blend the textures based on the distance
                fixed4 finalTexel = lerp(texel1, texel2, distance / _Distance);
 
                // Output the final color
                return finalTexel;
            }
            ENDCG
        }
    }
}
