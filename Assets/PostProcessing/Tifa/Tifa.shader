Shader "Custom/Tifa"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
        LOD 100

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_ST;
        uniform half4 _MainTex_TexelSize;
        
        float _rotationAmount;


        CBUFFER_END

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert (appdata v)
            {
                v2f o;
                VertexPositionInputs  PositionInputs = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = PositionInputs.positionCS;   
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                float s = sin(_rotationAmount);
                float c = cos(_rotationAmount);
                float4x4 m = float4x4(
                    c,-s, 0, 0,
                    s, c, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                    );

                float aspectRatio = _ScreenParams.x / _ScreenParams.y;
            
                float2 newUV = o.uv - 0.5;
                float2 rotation = float2(aspectRatio, 1 / aspectRatio);
                rotation = mul(m, rotation); 
                o.uv = mul(m,newUV) * rotation;
                o.uv += 0.5;

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float aspectRatio = _ScreenParams.x / _ScreenParams.y;
                float2 translatedCoords = i.uv - 0.5;
                float2 invertedCoords = float2(translatedCoords.y * (1 / aspectRatio) , translatedCoords.x * aspectRatio);
                
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                return color;
            }
            ENDHLSL
        }
    }
}