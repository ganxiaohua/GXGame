#ifndef UNIVERSAL_TERRAIN_LIT_INPUT_INCLUDED
#define UNIVERSAL_TERRAIN_LIT_INPUT_INCLUDED

#if defined(_TERRAIN_MASK_MAPS) || defined(_TERRAIN_NORMAL_IN_MASK) || defined(_TERRAIN_BLEND_HEIGHT) || defined(_TERRAIN_PARALLAX)
    #define TERRAIN_MASK
#endif

#if (defined(_TERRAIN_TRIPLANAR) || defined(_OBJECT_TRIPLANAR) || defined(_TERRAIN_TRIPLANAR_ONE)) && !defined(_TERRAIN_BASEMAP_GEN)
    #define TRIPLANAR
#endif

#if defined(_TERRAIN_NORMAL_IN_MASK)
    #undef _NORMALMAP
    #define _NORMALMAP
#endif

#if defined(_TERRAIN_PARALLAX)
    #define PARALLAX
#endif
 
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
    float4 _MainTex_ST;
    half4 _BaseColor;
    half _Cutoff;
CBUFFER_END

#define _Surface 0.0 // Terrain is always opaque

CBUFFER_START(_Terrain)
    half _NormalScale0, _NormalScale1, _NormalScale2, _NormalScale3;
    half _Metallic0, _Metallic1, _Metallic2, _Metallic3;
    half _Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3;
    half4 _DiffuseRemapScale0, _DiffuseRemapScale1, _DiffuseRemapScale2, _DiffuseRemapScale3;
    half4 _DiffuseRemapOffset0, _DiffuseRemapOffset1, _DiffuseRemapOffset2, _DiffuseRemapOffset3;
    half4 _MaskMapRemapOffset0, _MaskMapRemapOffset1, _MaskMapRemapOffset2, _MaskMapRemapOffset3;
    half4 _MaskMapRemapScale0, _MaskMapRemapScale1, _MaskMapRemapScale2, _MaskMapRemapScale3;
    float4 _Mask0_TexelSize, _Mask1_TexelSize, _Mask2_TexelSize, _Mask3_TexelSize;

    float4 _Control_ST;
    float4 _Control_TexelSize;
    half _DiffuseHasAlpha0, _DiffuseHasAlpha1, _DiffuseHasAlpha2, _DiffuseHasAlpha3;
    half _LayerHasMask0, _LayerHasMask1, _LayerHasMask2, _LayerHasMask3;
    half4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST;
    half _HeightTransition;
    half _NumLayersCount;

    #ifdef UNITY_INSTANCING_ENABLED
        float4 _TerrainHeightmapRecipSize;   // float4(1.0f/width, 1.0f/height, 1.0f/(width-1), 1.0f/(height-1))
        float4 _TerrainHeightmapScale;       // float4(hmScale.x, hmScale.y / (float)(kMaxHeight), hmScale.z, 0.0f)
    #endif
    #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
    #endif

    //InTerra
    half4 _HT_distance, _MipMapFade;
    float _HT_distance_scale, _HT_cover, _MipMapLevel;
    half _Distance_Height_blending, _Distance_HeightTransition, _TriplanarOneToAllSteep, _TriplanarSharpness;
    half4 _TerrainSize;
    float3 _TerrainSizeXZPosY;   
    half _ControlNumber;
    half _ParallaxAffineStepsTerrain;
    float _TerrainColorTintStrenght;
    float _TerrainNormalTintStrenght;
    float4 _TerrainColorTintTexture_ST;
    float4 _TerrainNormalTintTexture_ST;
    float4 _TerrainNormalTintDistance;

CBUFFER_END


TEXTURE2D(_Control);    SAMPLER(sampler_Control);
TEXTURE2D(_Splat0);     SAMPLER(sampler_Splat0);
TEXTURE2D(_Splat1);
TEXTURE2D(_Splat2);
TEXTURE2D(_Splat3);

#if defined(_NORMALMAP) || !defined(_TERRAIN_NORMAL_IN_MASK)
    TEXTURE2D(_Normal0);     SAMPLER(sampler_Normal0); 
    TEXTURE2D(_Normal1);
    TEXTURE2D(_Normal2);
    TEXTURE2D(_Normal3);
#endif

#ifdef TERRAIN_MASK
    TEXTURE2D(_Mask0);      SAMPLER(sampler_Mask0);
    TEXTURE2D(_Mask1);
    TEXTURE2D(_Mask2);
    TEXTURE2D(_Mask3);
#endif


    TEXTURE2D(_MainTex);        SAMPLER(sampler_MainTex); 
#ifdef TERRAIN_SPLAT_BASEPASS
    TEXTURE2D(_SpecGlossMap);   SAMPLER(sampler_SpecGlossMap);
    TEXTURE2D(_MetallicTex);    SAMPLER(sampler_MetallicTex);
    TEXTURE2D(_TriplanarTex);   SAMPLER(sampler_TriplanarTex);
    TEXTURE2D(_Triplanar_MetallicAO);   SAMPLER(sampler_Triplanar_MetallicAO);
#endif
TEXTURE2D(_TerrainColorTintTexture); 
TEXTURE2D(_TerrainNormalTintTexture); SAMPLER(SamplerState_Linear_Repeat);


#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
#endif

#if defined(UNITY_INSTANCING_ENABLED) || defined(TRIPLANAR)
    TEXTURE2D(_TerrainHeightmapTexture);
    TEXTURE2D(_TerrainNormalmapTexture);
    SAMPLER(sampler_TerrainNormalmapTexture);
#endif

UNITY_INSTANCING_BUFFER_START(Terrain)
UNITY_DEFINE_INSTANCED_PROP(float4, _TerrainPatchInstanceData)  // float4(xBase, yBase, skipScale, ~)
UNITY_INSTANCING_BUFFER_END(Terrain)

#ifdef _ALPHATEST_ON
    TEXTURE2D(_TerrainHolesTexture);
    SAMPLER(sampler_TerrainHolesTexture);

    void ClipHoles(float2 uv)
    {
        float hole = SAMPLE_TEXTURE2D(_TerrainHolesTexture, sampler_TerrainHolesTexture, uv).r;
        clip(hole == 0.0f ? -1 : 1);
    }
#endif

#ifdef TERRAIN_SPLAT_BASEPASS
    half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
    {
        half4 specGloss;
        specGloss = SAMPLE_TEXTURE2D(_MetallicTex, sampler_MetallicTex, uv);
        specGloss.a = albedoAlpha;
        return specGloss;
    }

    inline void InitializeStandardLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
    {
        outSurfaceData = (SurfaceData)0;
        half4 albedoSmoothness = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
        outSurfaceData.alpha = 1;

        half4 specGloss = SampleMetallicSpecGloss(uv, albedoSmoothness.a);
        outSurfaceData.albedo = albedoSmoothness.rgb;

        outSurfaceData.metallic = specGloss.r;
        outSurfaceData.specular = half3(0.0h, 0.0h, 0.0h);

        outSurfaceData.smoothness = specGloss.a;
        outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));
        outSurfaceData.occlusion = 1;
        outSurfaceData.emission = 0;
    }
#endif

void TerrainInstancing(inout float4 positionOS, inout float3 normal, inout float2 uv)
{
#ifdef UNITY_INSTANCING_ENABLED
    float2 patchVertex = positionOS.xy;
    float4 instanceData = UNITY_ACCESS_INSTANCED_PROP(Terrain, _TerrainPatchInstanceData);

    float2 sampleCoords = (patchVertex.xy + instanceData.xy) * instanceData.z; // (xy + float2(xBase,yBase)) * skipScale
    float height = UnpackHeightmap(_TerrainHeightmapTexture.Load(int3(sampleCoords, 0)));

    positionOS.xz = sampleCoords * _TerrainHeightmapScale.xz;
    positionOS.y = height * _TerrainHeightmapScale.y;

#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
    normal = float3(0, 1, 0);
#else
    normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb * 2 - 1;
#endif
    uv = sampleCoords * _TerrainHeightmapRecipSize.zw;
    #ifdef TRIPLANAR
        normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb * 2 - 1;
    #endif
#endif


}

void TerrainInstancing(inout float4 positionOS, inout float3 normal)
{
    float2 uv = { 0, 0 };
    TerrainInstancing(positionOS, normal, uv);
}
#endif
