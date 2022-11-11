Shader "Unlit/BasicLighting"
{
    Properties
    {

        //_MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1,1,1,0) //white color
        //_Gloss ("Gloss", Float) = 1
        _Gloss("Gloss", Float) = 32
        _GlossStrength("GlossStrength", Float) = 0.5
    }

    SubShader
    {

        //Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f //interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;

            };

            //declarations

            //sampler2D _MainTex;
            //float4 _MainTex_ST;

            float4 _BaseColor;
            float _Gloss;
            float _GlossStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv0 = v.uv0;
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex); //from local space to world space
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.clipSpacePos = UnityObjectToClipPos(v.vertex);

                //o.vertex = TransformObjectToHClip(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
 
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {

                //ambient lighting
                float ambientStrength = 0.1;
                float3 ambient = ambientStrength * _LightColor0;

                //return float4((ambient)*_BaseColor,1);


                //diffuse lighting
                float3 diffuse = float3(0, 0, 0);
                float3 lightDir0 = normalize(_WorldSpaceLightPos0.xyz - i.worldPos); //vector from fragment towards light source
                float diff = max(dot(normalize(i.normal), lightDir0),0);
                float3 diffuse0 = diff * _LightColor0;

                //return float4((ambient+diffuse0)*_BaseColor,1);


                //specular
                //float specularStrength = 0.5f;
                float specularStrength = _GlossStrength;
                //float specularShinyness = 32;
                float specularShinyness = _Gloss;

                float3 viewDir0 = normalize(_WorldSpaceCameraPos - i.worldPos);

                float3 reflectDir0 = reflect(-lightDir0, normalize(i.normal));

                float spec = pow(max(dot(viewDir0, reflectDir0), 0.0), specularShinyness);
                float3 specular = specularStrength * spec * _LightColor0;

                return float4((specular + ambient + diffuse0) * _BaseColor, 1);
        
            }

            ENDCG

        }
    }
}
