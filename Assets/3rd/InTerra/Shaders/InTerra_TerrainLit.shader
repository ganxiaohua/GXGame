// NOTE! This file is based on Unity file "TerrainLit.shader" which was used as template for adding all the InTerra features.
Shader "InTerra/URP/Terrain (Lit with Features)"
{
    Properties
    {
        [HideInInspector] [ToggleUI] _EnableHeightBlend("EnableHeightBlend", Float) = 0.0
        _HeightTransition("Height Transition", Range(0, 60.0)) = 0.0
        // Layer count is passed down to guide height-blend enable/disable, due
        // to the fact that heigh-based blend will be broken with multipass.
        [HideInInspector] [PerRendererData] _NumLayersCount ("Total Layer Count", Float) = 1.0

        // set by terrain engine
        [HideInInspector] _Control("Control (RGBA)", 2D) = "red" {}
        [HideInInspector] _Splat3("Layer 3 (A)", 2D) = "grey" {}
        [HideInInspector] _Splat2("Layer 2 (B)", 2D) = "grey" {}
        [HideInInspector] _Splat1("Layer 1 (G)", 2D) = "grey" {}
        [HideInInspector] _Splat0("Layer 0 (R)", 2D) = "grey" {}
        [HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
        [HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
        [HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
        [HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}
        [HideInInspector] _Mask3("Mask 3 (A)", 2D) = "grey" {}
        [HideInInspector] _Mask2("Mask 2 (B)", 2D) = "grey" {}
        [HideInInspector] _Mask1("Mask 1 (G)", 2D) = "grey" {}
        [HideInInspector] _Mask0("Mask 0 (R)", 2D) = "grey" {}
        [HideInInspector][Gamma] _Metallic0("Metallic 0", Range(0.0, 1.0)) = 0.0
        [HideInInspector][Gamma] _Metallic1("Metallic 1", Range(0.0, 1.0)) = 0.0
        [HideInInspector][Gamma] _Metallic2("Metallic 2", Range(0.0, 1.0)) = 0.0
        [HideInInspector][Gamma] _Metallic3("Metallic 3", Range(0.0, 1.0)) = 0.0
        [HideInInspector] _Smoothness0("Smoothness 0", Range(0.0, 1.0)) = 0.5
        [HideInInspector] _Smoothness1("Smoothness 1", Range(0.0, 1.0)) = 0.5
        [HideInInspector] _Smoothness2("Smoothness 2", Range(0.0, 1.0)) = 0.5
        [HideInInspector] _Smoothness3("Smoothness 3", Range(0.0, 1.0)) = 0.5

        // used in fallback on old cards & base map
        [HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "grey" {}
        [HideInInspector] _BaseColor("Main Color", Color) = (1,1,1,1)
        [HideInInspector] _TriplanarTex("Triplanar Albedo(RGB), Smoothness(A)", 2D) = "black" {}
        [HideInInspector] _Triplanar_MetallicAO("Metallic, Occlusion", 2D) = "black" {}

        [HideInInspector] _TerrainHolesTexture("Holes Map (RGB)", 2D) = "white" {}

        [ToggleUI] _EnableInstancedPerPixelNormal("Enable Instanced per-pixel normal", Float) = 1.0

        //inTerra
        _HT_distance("Distance",  vector) = (3,10,0,25)
        _HT_distance_scale("Scale",   Range(0,0.5)) = 0.25
        _HT_cover("Cover strenght",   Range(0,1)) = 0.6
        _Distance_HeightTransition("Distance Height blending Sharpness ", Range(0,60)) = 10
        _TriplanarSharpness("Triplanar Sharpness",   Range(4,10)) = 9
        _ParallaxAffineStepsTerrain("", Float) = 3
        _MipMapFade("Parallax MipMap fade",  vector) = (3,15,0,35)
        _MipMapLevel("Parallax MipMap level", Float) = 0
        _TriplanarOneToAllSteep("", Float) = 0
        _TerrainColorTintTexture("Color Tint Texture", 2D) = "white" {}
        _TerrainColorTintStrenght("Color Tint Strenght", Range(0, 1)) = 0
        _TerrainNormalTintTexture("Additional Normal Texture", 2D) = "bump" {}
        _TerrainNormalTintStrenght("Additional Normal Strenght", Range(0, 1)) = 0.0
        _TerrainNormalTintDistance("Additional Normal Distance",  vector) = (3,10,0,25)
        [HideInInspector] _TerrainSizeXZPosY("",  Vector) = (0,0,0)
    }

    HLSLINCLUDE

    #pragma multi_compile_fragment __ _ALPHATEST_ON

    ENDHLSL

    SubShader
    {
        PackageRequirements { "com.unity.render-pipelines.universal":"[12.0,17.0]" }
        Tags { "Queue" = "Geometry-100" "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "False" "TerrainCompatible" = "True"}

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            HLSLPROGRAM
            #pragma target 3.0

            #pragma vertex SplatmapVert
            #pragma fragment SplatmapFragment

            #define _METALLICSPECGLOSSMAP 1
            #define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
            #pragma multi_compile _ _CLUSTERED_RENDERING            
            #pragma multi_compile _ _FORWARD_PLUS

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            #pragma multi_compile_instancing
            #pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap

            #pragma shader_feature_local_fragment _TERRAIN_BLEND_HEIGHT
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _MASKMAP

            // -------------------------------------
            // InTerra Keywords
            #pragma shader_feature_local __ _TERRAIN_MASK_MAPS _TERRAIN_NORMAL_IN_MASK
            #pragma shader_feature_local __ _TERRAIN_TRIPLANAR_ONE _TERRAIN_TRIPLANAR      
            #pragma shader_feature_local_fragment _TERRAIN_DISTANCEBLEND
            #pragma shader_feature_local _TERRAIN_PARALLAX

            #pragma shader_feature_local_fragment _LAYERS_TWO

            #define INTERRA_TERRAIN

            #pragma shader_feature_local_fragment UNITY_2022_2_OR_NEWER

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

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap

            // -------------------------------------
            // Universal Pipeline keywords

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #define INTERRA_TERRAIN

            #include "InTerra_TerrainLitInput.hlsl"
            #include "InTerra_TerrainLitPasses.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "GBuffer"
            Tags{"LightMode" = "UniversalGBuffer"}

            HLSLPROGRAM
            #pragma exclude_renderers gles
            #pragma target 3.0
            #pragma vertex SplatmapVert
            #pragma fragment SplatmapFragment

            #define _METALLICSPECGLOSSMAP 1
            #define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            //#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            //#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_LAYERS

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
            #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED

            //#pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap

            #pragma shader_feature_local_fragment _TERRAIN_BLEND_HEIGHT
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _MASKMAP
            #define TERRAIN_GBUFFER 1

            // -------------------------------------
            // InTerra Keywords
            #pragma shader_feature_local __ _TERRAIN_MASK_MAPS _TERRAIN_NORMAL_IN_MASK
            #pragma shader_feature_local __ _TERRAIN_TRIPLANAR_ONE _TERRAIN_TRIPLANAR
            #pragma shader_feature_local_fragment _TERRAIN_DISTANCEBLEND
            #pragma shader_feature_local _TERRAIN_PARALLAX

            #pragma shader_feature_local_fragment _LAYERS_TWO

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

        // This pass is used when drawing to a _CameraNormalsTexture texture
        Pass
        {
            Name "DepthNormals"
            Tags{"LightMode" = "DepthNormals"}

            ZWrite On

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex DepthNormalOnlyVertex
            #pragma fragment DepthNormalOnlyFragment

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _TERRAIN_NORMAL_IN_MASK
            #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap

            #pragma shader_feature_local_fragment UNITY_2022_2_OR_NEWER

            #include "InTerra_TerrainLitInput.hlsl"
            #include "InTerra_TerrainLitDepthNormalsPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "SceneSelectionPass"
            Tags { "LightMode" = "SceneSelectionPass" }

            HLSLPROGRAM
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap

            #define SCENESELECTIONPASS
            #define INTERRA_TERRAIN

            #include "InTerra_TerrainLitInput.hlsl"
            #include "InTerra_TerrainLitPasses.hlsl"
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

            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
            #pragma shader_feature EDITOR_VISUALIZATION
            #define _METALLICSPECGLOSSMAP 1
            #define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1

            #define INTERRA_TERRAIN

            #include "InTerra_TerrainLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitMetaPass.hlsl"
            ENDHLSL
        }

        UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
    }
    Dependency "AddPassShader" = "Hidden/InTerra/Lit (Add Pass)"
    Dependency "BaseMapShader" = "Hidden/InTerra/Lit (Base Pass)"
    Dependency "BaseMapGenShader" = "Hidden/InTerra/Lit (Basemap Gen)"

    CustomEditor "InTerra.InTerra_TerrainShaderGUI"

    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}
