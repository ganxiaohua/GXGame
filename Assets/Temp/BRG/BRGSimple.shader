Shader "Custom/BRGSimple"
{
    Properties
    {
        _BaseColor ("基础颜色", Color) = (1,1,1,1)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "BRGForward"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma enable_d3d11_debug_symbols
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : NORMAL;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
            CBUFFER_END
            
            // BRG 通过 GPU 实例化提供这些
            // UNITY_INSTANCING_BUFFER_START(Props)
            // UNITY_INSTANCING_BUFFER_END(Props)

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                
                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS);
                
                output.positionCS = posInputs.positionCS;
                output.normalWS = normalInputs.normalWS;
                
                // 根据实例 ID 生成颜色变化
                float instanceID = unity_InstanceID;
                output.color = float4(
                    frac(instanceID * 0.1),
                    frac(instanceID * 0.05),
                    frac(instanceID * 0.02),
                    1
                );
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                
                float3 normalWS = normalize(input.normalWS);
                float NdotL = saturate(dot(normalWS, _MainLightPosition.xyz));
                
                half3 ambient = SampleSH(normalWS);
                half3 lighting = ambient + _MainLightColor.rgb * NdotL;
                
                return half4(input.color.rgb * lighting, 1);
            }
            ENDHLSL
        }
    }
}