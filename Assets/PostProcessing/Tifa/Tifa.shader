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
        float _density;
        float _zoomAmount;




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

                return o;
            }

            float2 rotate(float2 uv, float rotationAmount)
            {
                float s = sin(rotationAmount);
                float c = cos(rotationAmount);
                float4x4 m = float4x4(
                    c,-s, 0, 0,
                    s, c, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                );

                float aspectRatio = _ScreenParams.x / _ScreenParams.y;
            
                float2 newUV = uv - 0.5;
                newUV.x *= aspectRatio;
                newUV = mul(m,newUV);
                newUV.x /= aspectRatio;
                newUV += 0.5;

                return newUV;
            }

            float2 zoomIn(float2 uv, float zoomAmount)
            {
                float4x4 m = float4x4(
                    1 / zoomAmount, 0, 0, 0,
                    0, 1 / zoomAmount, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                );

                float aspectRatio = _ScreenParams.x / _ScreenParams.y;

                float2 newUV = uv - 0.5;
                newUV.x *= aspectRatio;
                newUV = mul(m,newUV);
                newUV.x /= aspectRatio;
                newUV += 0.5;

                return newUV;
            }

            half4 frag (v2f i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                
                for (float r = 0; r < _rotationAmount; r += _density)
                {
                    const float pi = 3.141592653589793238462;
                    float scale = 1.0 + r / (2 * pi) * (_zoomAmount - 1);
                    const float2 rotation = rotate(i.uv, r);
                    const float2 zoom = zoomIn(rotation, scale);
                    color = color * 0.5 + 0.5 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, zoom);
                }
                
                return color;
            }
            ENDHLSL
        }
    }
}