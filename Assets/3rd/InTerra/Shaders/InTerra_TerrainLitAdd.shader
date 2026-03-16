//NOTE! This file is based on Unity file "TerrainLitAdd.shader" which was used as a template for adding all the InTerra features.
Shader "Hidden/InTerra/Lit (Add Pass)"
{
    Properties
    {
        // Layer count is passed down to guide height-blend enable/disable, due
        // to the fact that heigh-based blend will be broken with multipass.
        [HideInInspector] [PerRendererData] _NumLayersCount ("Total Layer Count", Float) = 1.0

        // set by terrain engine
        [HideInInspector] _Control("Control (RGBA)", 2D) = "red" {}
        [HideInInspector] _Splat3("Layer 3 (A)", 2D) = "white" {}
        [HideInInspector] _Splat2("Layer 2 (B)", 2D) = "white" {}
        [HideInInspector] _Splat1("Layer 1 (G)", 2D) = "white" {}
        [HideInInspector] _Splat0("Layer 0 (R)", 2D) = "white" {}
        [HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
        [HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
        [HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
        [HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}
        [HideInInspector][Gamma] _Metallic0("Metallic 0", Range(0.0, 1.0)) = 0.0
        [HideInInspector][Gamma] _Metallic1("Metallic 1", Range(0.0, 1.0)) = 0.0
        [HideInInspector][Gamma] _Metallic2("Metallic 2", Range(0.0, 1.0)) = 0.0
        [HideInInspector][Gamma] _Metallic3("Metallic 3", Range(0.0, 1.0)) = 0.0
        [HideInInspector] _Mask3("Mask 3 (A)", 2D) = "grey" {}
        [HideInInspector] _Mask2("Mask 2 (B)", 2D) = "grey" {}
        [HideInInspector] _Mask1("Mask 1 (G)", 2D) = "grey" {}
        [HideInInspector] _Mask0("Mask 0 (R)", 2D) = "grey" {}
        [HideInInspector] _Smoothness0("Smoothness 0", Range(0.0, 1.0)) = 1.0
        [HideInInspector] _Smoothness1("Smoothness 1", Range(0.0, 1.0)) = 1.0
        [HideInInspector] _Smoothness2("Smoothness 2", Range(0.0, 1.0)) = 1.0
        [HideInInspector] _Smoothness3("Smoothness 3", Range(0.0, 1.0)) = 1.0

        // used in fallback on old cards & base map
        [HideInInspector] _BaseMap("BaseMap (RGB)", 2D) = "white" {}
        [HideInInspector] _BaseColor("Main Color", Color) = (1,1,1,1)

        [HideInInspector] _TerrainHolesTexture("Holes Map (RGB)", 2D) = "white" {}

        //inTerra
        _HT_distance("Distance",  vector) = (3,10,0,25)
        _HT_distance_scale("Scale",   Range(0,0.5)) = 0.25
        _HT_cover("Cover strenght",   Range(0,1)) = 0.6
        _Distance_HeightTransition("Distance Height blending Sharpness ", Range(0,60)) = 10
        _TriplanarSharpness("Triplanar Sharpness",   Range(4,10)) = 9
        [HideInInspector] _TerrainSizeXZPosY("",  Vector) = (0,0,0)
        [HideInInspector] _TriplanarOneToAllSteep("", Float) = 0
    }

    HLSLINCLUDE

    #pragma multi_compile_fragment __ _ALPHATEST_ON

    ENDHLSL

    SubShader
    {
        PackageRequirements { "com.unity.render-pipelines.universal":"[12.1,15.1.3]" }
        Tags { "Queue" = "Geometry-99" "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True"}

        Pass
        {
            Name "TerrainAddLit"
            Tags { "LightMode" = "UniversalForward" }
            

            Blend One One
            HLSLPROGRAM
            #pragma target 3.0

            #pragma vertex SplatmapVert
            #pragma fragment SplatmapFragment

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #pragma multi_compile _ _CLUSTERED_RENDERING

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap
            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            #pragma shader_feature_local_fragment _TERRAIN_BLEND_HEIGHT
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _MASKMAP

            #define TERRAIN_SPLAT_ADDPASS

            // -------------------------------------
            // InTerra Keywords
            #pragma shader_feature_local __ _TERRAIN_MASK_MAPS _TERRAIN_NORMAL_IN_MASK
            #pragma shader_feature_local __ _TERRAIN_TRIPLANAR_ONE _TERRAIN_TRIPLANAR
            #pragma shader_feature_local_fragment _TERRAIN_DISTANCEBLEND
            #pragma shader_feature_local _TERRAIN_PARALLAX

            #define INTERRA_TERRAIN

            #include "InTerra_TerrainLitInput.hlsl"
            #include "InTerra_TerrainLitPasses.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "GBuffer"
            Tags{"LightMode" = "UniversalGBuffer"}

            Blend One One

            HLSLPROGRAM
            #pragma exclude_renderers gles
            #pragma target 3.0
            #pragma vertex SplatmapVert
            #pragma fragment SplatmapFragment

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            //#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            //#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            #pragma multi_compile_fragment _ _LIGHT_LAYERS

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
            #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED

            //#pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap

            #pragma shader_feature_local_fragment _TERRAIN_BLEND_HEIGHT
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _MASKMAP

            #define TERRAIN_SPLAT_ADDPASS 1
            #define TERRAIN_GBUFFER 1

            // -------------------------------------
            // InTerra Keywords
            #pragma shader_feature_local __ _TERRAIN_MASK_MAPS _TERRAIN_NORMAL_IN_MASK
            #pragma shader_feature_local __ _TERRAIN_TRIPLANAR_ONE _TERRAIN_TRIPLANAR
            #pragma shader_feature_local_fragment _TERRAIN_DISTANCEBLEND
            #pragma shader_feature_local _TERRAIN_PARALLAX

            #define INTERRA_TERRAIN

            #include "InTerra_TerrainLitInput.hlsl"
            #include "InTerra_TerrainLitPasses.hlsl"
            ENDHLSL
        }
    }
    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}