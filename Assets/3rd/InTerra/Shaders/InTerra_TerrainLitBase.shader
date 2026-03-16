//NOTE! This file is based on Unity file "TerrainLitBase.shader" which was used as a template for adding all the InTerra features.
Shader "Hidden/InTerra/Lit (Base Pass)"
{
    Properties
    {
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)
        _MainTex("Albedo(RGB), Smoothness(A)", 2D) = "white" {}
        _MetallicTex("Metallic, Occlusion, Splat01", 2D) = "black" {}
        _TriplanarTex("Triplanar Albedo(RGB), Smoothness(A)", 2D) = "black" {}
        _Triplanar_MetallicAO("Metallic, Occlusion", 2D) = "black" {}
        [HideInInspector] _TerrainHolesTexture("Holes Map (RGB)", 2D) = "white" {}
    }

    HLSLINCLUDE

    #pragma multi_compile_fragment __ _ALPHATEST_ON

    ENDHLSL

    SubShader
    {
        PackageRequirements { "com.unity.render-pipelines.universal":"[12.0,15.1.3]" }
        Tags { "Queue" = "Geometry-100" "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True"}
        LOD 200

        // ------------------------------------------------------------------
        //  Forward pass. Shades all light in a single pass. GI + emission + Fog
        Pass
        {
            Name "ForwardLit"
            // Lightmode matches the ShaderPassName set in UniversalPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Pipeline
            Tags{"LightMode" = "UniversalForward"}

            HLSLPROGRAM
            #pragma target 2.0

        // -------------------------------------
        // Material Keywords
        #define _METALLICSPECGLOSSMAP 1
        #define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1

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
        #pragma multi_compile _ _FORWARD_PLUS

        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile_fog
        #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap
        #pragma multi_compile_fragment _ DEBUG_DISPLAY

        #pragma vertex SplatmapVert
        #pragma fragment SplatmapFragment

        #pragma shader_feature_local _NORMALMAP
        #define TERRAIN_SPLAT_BASEPASS 1

        #pragma shader_feature_local __ _TERRAIN_TRIPLANAR_ONE _TERRAIN_TRIPLANAR

        #define INTERRA_TERRAIN 

        #include "InTerra_TerrainLitInput.hlsl"
        #include "InTerra_TerrainLitPasses.hlsl"
        ENDHLSL
    }

    Pass
    {
        Name "ShadowCaster"
        Tags{"LightMode" = "ShadowCaster"}

        ZWrite On
        ColorMask 0

        HLSLPROGRAM
        #pragma target 2.0

        #pragma multi_compile_instancing
        #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap

        #pragma vertex ShadowPassVertex
        #pragma fragment ShadowPassFragment

        #define INTERRA_TERRAIN 

        #include "InTerra_TerrainLitInput.hlsl"
        #include "InTerra_TerrainLitPasses.hlsl"
        ENDHLSL
    }

        // ------------------------------------------------------------------
        //  GBuffer pass. Does GI + emission. All additional lights are done deferred as well as fog
        Pass
        {
            Name "GBuffer"
            Tags{"LightMode" = "UniversalGBuffer"}

            HLSLPROGRAM
            #pragma exclude_renderers gles
            #pragma target 2.0

        // -------------------------------------
        // Material Keywords
        #define _METALLICSPECGLOSSMAP 1
        #define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1

        // -------------------------------------
        // Universal Pipeline keywords
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        //#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        //#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        #pragma multi_compile_fragment _ _LIGHT_LAYERS
        #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS

        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED

        #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap

        #pragma vertex SplatmapVert
        #pragma fragment SplatmapFragment

        #pragma shader_feature_local _NORMALMAP
        #define TERRAIN_SPLAT_BASEPASS 1
        #define TERRAIN_GBUFFER 1

        #define INTERRA_TERRAIN 

        #include "InTerra_TerrainLitInput.hlsl"
        #include "InTerra_TerrainLitPasses.hlsl"

        ENDHLSL
    }

    Pass
    {
        Name "DepthOnly"
        Tags{"LightMode" = "DepthOnly"}

        ZWrite On
        ColorMask 0

        HLSLPROGRAM
        #pragma target 2.0

        #pragma vertex DepthOnlyVertex
        #pragma fragment DepthOnlyFragment

        #pragma multi_compile_instancing
        #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap

        #define INTERRA_TERRAIN 

        #include "InTerra_TerrainLitInput.hlsl"
        #include "InTerra_TerrainLitPasses.hlsl"
        ENDHLSL
    }

    Pass
    {
        Name "DepthNormals"
        Tags{"LightMode" = "DepthNormals"}

        ZWrite On

        HLSLPROGRAM
        #pragma target 2.0

        #pragma vertex DepthNormalOnlyVertex
        #pragma fragment DepthNormalOnlyFragment

        #pragma multi_compile_instancing
        #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
        #pragma shader_feature_local _NORMALMAP

        #include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitDepthNormalsPass.hlsl"
        ENDHLSL
    }

        // This pass it not used during regular rendering, only for lightmap baking.
        Pass
        {
            Name "Meta"
            Tags{"LightMode" = "Meta"}

            Cull Off

            HLSLPROGRAM
            #pragma vertex TerrainVertexMeta
            #pragma fragment TerrainFragmentMeta

            #pragma shader_feature EDITOR_VISUALIZATION
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
            #define _METALLICSPECGLOSSMAP 1
            #define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1

            #include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitMetaPass.hlsl"

            ENDHLSL
        }

        UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
        UsePass "Universal Render Pipeline/Terrain/Lit/SceneSelectionPass"
    }
        FallBack "Hidden/Universal Render Pipeline/FallbackError"
        //CustomEditor "LitShaderGUI"
}
