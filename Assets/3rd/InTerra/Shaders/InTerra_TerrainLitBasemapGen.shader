//NOTE! This file is based on Unity file "TerrainLitBasemapGen.shader" which was used as a template for adding all the InTerra features.
Shader "Hidden/InTerra/Lit (Basemap Gen)"
{
    Properties
    {
        // Layer count is passed down to guide height-blend enable/disable, due
        // to the fact that heigh-based blend will be broken with multipass.
        [HideInInspector] [PerRendererData] _NumLayersCount ("Total Layer Count", Float) = 1.0
        [HideInInspector] _Control("AlphaMap", 2D) = "" {}

        [HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
        [HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
        [HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
        [HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
        [HideInInspector] _Mask3("Mask 3 (A)", 2D) = "grey" {}
        [HideInInspector] _Mask2("Mask 2 (B)", 2D) = "grey" {}
        [HideInInspector] _Mask1("Mask 1 (G)", 2D) = "grey" {}
        [HideInInspector] _Mask0("Mask 0 (R)", 2D) = "grey" {}
        [HideInInspector] [Gamma] _Metallic0 ("Metallic 0", Range(0.0, 1.0)) = 0.0
        [HideInInspector] [Gamma] _Metallic1 ("Metallic 1", Range(0.0, 1.0)) = 0.0
        [HideInInspector] [Gamma] _Metallic2 ("Metallic 2", Range(0.0, 1.0)) = 0.0
        [HideInInspector] [Gamma] _Metallic3 ("Metallic 3", Range(0.0, 1.0)) = 0.0
        [HideInInspector] _Smoothness0 ("Smoothness 0", Range(0.0, 1.0)) = 1.0
        [HideInInspector] _Smoothness1 ("Smoothness 1", Range(0.0, 1.0)) = 1.0
        [HideInInspector] _Smoothness2 ("Smoothness 2", Range(0.0, 1.0)) = 1.0
        [HideInInspector] _Smoothness3 ("Smoothness 3", Range(0.0, 1.0)) = 1.0

        [HideInInspector] _DstBlend("DstBlend", Float) = 0.0
    }

    Subshader
    {

        HLSLINCLUDE
        #pragma target 3.0

        #define _METALLICSPECGLOSSMAP 1
        #define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1
        #define _TERRAIN_BASEMAP_GEN

        #pragma shader_feature_local _TERRAIN_BLEND_HEIGHT
        #pragma shader_feature_local _MASKMAP
        #pragma shader_feature_local __ _TERRAIN_MASK_MAPS _TERRAIN_NORMAL_IN_MASK
        #pragma shader_feature_local __ _TERRAIN_TRIPLANAR_ONE _TERRAIN_TRIPLANAR
        #pragma shader_feature_local _TERRAIN_DISTANCEBLEND
        #pragma shader_feature_local _LAYERS_TWO

        #define INTERRA_TERRAIN 

        #if defined(_TERRAIN_TRIPLANAR) || defined(_TERRAIN_TRIPLANAR_ONE)
            #ifdef _TERRAIN_TRIPLANAR_ONE
                #define TRIPLANAR_TINT
            #endif
            #undef _TERRAIN_TRIPLANAR
            #undef _TERRAIN_TRIPLANAR_ONE
        #endif

        #include "InTerra_TerrainLitInput.hlsl"
        #include "InTerra_TerrainLitPasses.hlsl"

        ENDHLSL

        PackageRequirements { "com.unity.render-pipelines.universal":"[12.1,15.1.3]" }
        Tags {"RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            Tags
            {
                "Name" = "_MainTex"
                "Format" = "ARGB32"
                "Size" = "1"
            }

            ZTest Always Cull Off ZWrite Off
            Blend One [_DstBlend]
            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            Varyings Vert(Attributes IN)
            {
                Varyings output = (Varyings) 0;

                output.clipPos = TransformWorldToHClip(IN.positionOS.xyz);

                // NOTE : This is basically coming from the vertex shader in TerrainLitPasses
                // There are other plenty of other values that the original version computes, but for this
                // pass, we are only interested in a few, so I'm just skipping the rest.
                output.uvMainAndLM.xy = IN.texcoord;
                output.uvSplat01.xy = TRANSFORM_TEX(IN.texcoord, _Splat0);
                output.uvSplat01.zw = TRANSFORM_TEX(IN.texcoord, _Splat1);
                output.uvSplat23.xy = TRANSFORM_TEX(IN.texcoord, _Splat2);
                output.uvSplat23.zw = TRANSFORM_TEX(IN.texcoord, _Splat3);

                return output;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                half3 normalTS = half3(0.0h, 0.0h, 1.0h);
                half4 splatControl;
                half weight;
                half4 mixedDiffuse = 0.0h;
                half4 defaultSmoothness = 0.0h;
                half metallic;
                half occlusion;
                half smoothness;

                float2 splatUV = (IN.uvMainAndLM.xy * (_Control_TexelSize.zw - 1.0f) + 0.5f) * _Control_TexelSize.xy;
                splatControl = SAMPLE_TEXTURE2D(_Control, sampler_Control, splatUV);

                SplatmapMix(IN.uvMainAndLM, IN.uvSplat01, IN.uvSplat23, float3(0, 0, 0), float3(0, 0, 0), IN.positionWS, splatControl, weight, mixedDiffuse, smoothness, metallic, occlusion, normalTS);

                return half4(mixedDiffuse.rgb, smoothness);
            }

            ENDHLSL
        }

        Pass
        {
            Tags
            {
                "Name" = "_MetallicTex"
                "Format" = "RGBA32"
                "Size" = "1/4"
                "EmptyColor" = "FF000000"
            }

            ZTest Always Cull Off ZWrite Off
            Blend One [_DstBlend]

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            Varyings Vert(Attributes IN)
            {
                Varyings output = (Varyings)0;

                output.clipPos = TransformWorldToHClip(IN.positionOS.xyz);

                // This is just like the other in that it is from TerrainLitPasses
                output.uvMainAndLM.xy = IN.texcoord;
                output.uvSplat01.xy = TRANSFORM_TEX(IN.texcoord, _Splat0);
                output.uvSplat01.zw = TRANSFORM_TEX(IN.texcoord, _Splat1);
                output.uvSplat23.xy = TRANSFORM_TEX(IN.texcoord, _Splat2);
                output.uvSplat23.zw = TRANSFORM_TEX(IN.texcoord, _Splat3);

                return output;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                half3 normalTS = half3(0.0h, 0.0h, 1.0h);
                half4 splatControl;
                half weight;
                half4 mixedDiffuse;
                half4 defaultSmoothness;

                half4 masks[4];
                float2 splatUV = (IN.uvMainAndLM.xy * (_Control_TexelSize.zw - 1.0f) + 0.5f) * _Control_TexelSize.xy;
                splatControl = SAMPLE_TEXTURE2D(_Control, sampler_Control, splatUV);

                half metallic;
                half occlusion;
                half smoothness;
                SplatmapMix(IN.uvMainAndLM, IN.uvSplat01, IN.uvSplat23, float3(0, 0, 0), float3(0, 0, 0), IN.positionWS, splatControl, weight, mixedDiffuse, smoothness, metallic, occlusion, normalTS);

                return float4(metallic, occlusion, splatControl.r, 0);
            }
            ENDHLSL
        }

        Pass
        {
            Tags
            {
                "Name" = "_TriplanarTex"
                "Format" = "ARGB32"
                "Size" = "1"
            }

            ZTest Always Cull Off ZWrite Off
            Blend One [_DstBlend]

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            Varyings Vert(Attributes IN)
            {
                Varyings output = (Varyings)0;

                output.clipPos = TransformWorldToHClip(IN.positionOS.xyz);

                // This is just like the other in that it is from TerrainLitPasses
                output.uvMainAndLM.xy = IN.texcoord;
                output.uvSplat01.xy = TRANSFORM_TEX(IN.texcoord, _Splat0);  

                return output;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                half3 normalTS = half3(0.0h, 0.0h, 1.0h);
                half4 splatControl = float4(1, 0, 0, 0);
                half weight;
                half4 mixedDiffuse;
                half4 defaultSmoothness;
                half metallic;
                half occlusion;
                half smoothness;

                SplatmapMix(IN.uvMainAndLM, IN.uvSplat01, IN.uvSplat23, float3(0, 0, 0), float3(0, 0, 0), IN.positionWS, splatControl, weight, mixedDiffuse, smoothness, metallic, occlusion, normalTS);

                return half4(mixedDiffuse.rgb, smoothness);
            }
            ENDHLSL
        }

        Pass
        {
            Tags
            {
                "Name" = "_Triplanar_MetallicAO"
                "Format" = "RGBA32"
                "Size" = "1/4"
            }

            ZTest Always Cull Off ZWrite Off
            Blend One [_DstBlend]

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            Varyings Vert(Attributes IN)
            {
                Varyings output = (Varyings)0;

                output.clipPos = TransformWorldToHClip(IN.positionOS.xyz);

                // This is just like the other in that it is from TerrainLitPasses
                output.uvMainAndLM.xy = IN.texcoord;
                output.uvSplat01.xy = TRANSFORM_TEX(IN.texcoord, _Splat0);

                return output;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                half3 normalTS = half3(0.0h, 0.0h, 1.0h);
                half4 splatControl = float4(1, 0, 0, 0);
                half weight;
                half4 mixedDiffuse;
                half4 defaultSmoothness;
                half metallic;
                half occlusion;
                half smoothness;
                SplatmapMix(IN.uvMainAndLM, IN.uvSplat01, IN.uvSplat23, float3(0, 0, 0), float3(0, 0, 0), IN.positionWS, splatControl, weight, mixedDiffuse, smoothness, metallic, occlusion, normalTS);

                return float4(metallic, occlusion, 0, 0);
            }

            ENDHLSL
        }
    }
}
