Shader "Unlit/ColorInversion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Blend ("Blend", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Blend;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);              

                float lum = col.r * 0.3+   col.g * 0.59 + col.b * 0.11; //gray
                //col.rgb = float3(lum, lum, lum);            

                float4 blended = float4(lerp(col.rgb, float3(lum, lum, lum), _Blend), col.a);
                return blended;
            }
            ENDCG
        }
    }
}
