#ifdef INTERRA_OBJECT
    void ObjectIntegration_float(float heightOffset, float3 worldPos, float3 tangentViewDirTerrain, float3 worldViewDir, float3 worldNormal, float3 worldTangent, float3 worldBitangent, float4 terrainNormals, float4 objectAlbedo, float4 objectNormal, float4 objectMask, half3 objectEmission, out half3 albedo, out half3 mixedNormal, out half smoothness, out half metallic, out half occlusion, out half3 emission)
#else
    #include "InTerra_Functions.hlsl"

    void SplatmapMix(float4 uvMainAndLM, float4 uvSplat01, float4 uvSplat23, float3 worldNormal, float3 tangentViewDirTerrain, float3 worldPos, inout half4 splatControl, out half weight, out half4 mixedDiffuse, out half smoothness, out half metallic, out half occlusion, inout half3 mixedNormal)
#endif
{
    #ifdef _LAYERS_ONE
        float2 uvSplat;
        half4 splat, mask;
        #ifdef TRIPLANAR
            float2 uvSplat_front, uvSplat_side;
            half4 splat_front, splat_side, mask_front, mask_side;
        #endif
        #ifdef _TERRAIN_DISTANCEBLEND
            float2 distantUV;
            half4 dSplat, dMask;
            #ifdef TRIPLANAR
                float2 distantUV_front, distantUV_side;
                half4 dSplat_front, dSplat_side, dMask_front, dMask_side;
            #endif
        #endif
        #else        
        float2 uvSplat[_LAYER_COUNT];
        half4 splat[_LAYER_COUNT], mask[_LAYER_COUNT];
        #ifdef TRIPLANAR
            float2 uvSplat_front[_LAYER_COUNT], uvSplat_side[_LAYER_COUNT];
            half4 splat_front[_LAYER_COUNT], splat_side[_LAYER_COUNT], mask_front[_LAYER_COUNT], mask_side[_LAYER_COUNT];
        #endif
        #ifdef _TERRAIN_DISTANCEBLEND
            float2 distantUV[_LAYER_COUNT];
            half4 dSplat[_LAYER_COUNT], dMask[_LAYER_COUNT];
            #ifdef TRIPLANAR
                float2 distantUV_front[_LAYER_COUNT], distantUV_side[_LAYER_COUNT];
                half4 dSplat_front[_LAYER_COUNT], dSplat_side[_LAYER_COUNT], dMask_front[_LAYER_COUNT], dMask_side[_LAYER_COUNT];
            #endif
        #endif
    #endif

    #ifndef INTERRA_OBJECT 
        //In Diffuse remap alphas channels there are the values for parallax mapping, in red channel of _DiffuseRemapOffset there are the values of Layers scales adjust
        //Unity is subtracting the _DiffuseRemapOffset from _DiffuseRemapScale for Terrain shaders, but since the values are used for other purpose than originally there is need to add them back
        _DiffuseRemapScale0 += _DiffuseRemapOffset0;
        _DiffuseRemapScale1 += _DiffuseRemapOffset1;
        #if !defined(_LAYERS_TWO)
            _DiffuseRemapScale2 += _DiffuseRemapOffset2;
            _DiffuseRemapScale3 += _DiffuseRemapOffset3;
        #endif
    #endif

    //====================================================================================
    //--------------------------------- SPLAT MAP CONTROL --------------------------------
    //====================================================================================
    #ifdef INTERRA_OBJECT
        float2 terrainUV = (worldPos.xz - _TerrainPosition.xz) * (1 / _TerrainSize.xz);
        float2 tintUV = terrainUV * _TerrainColorTintTexture_ST.xy + _TerrainColorTintTexture_ST.zw;
        float2 normalTintUV = terrainUV * _TerrainNormalTintTexture_ST.xy + _TerrainNormalTintTexture_ST.zw;

        #ifndef _LAYERS_ONE     
            float2 splatMapUV = (terrainUV * (_Control_TexelSize.zw - 1.0f) + 0.5f) * _Control_TexelSize.xy;
            float4 splatControl = SAMPLE_TEXTURE2D(_Control, sampler_Control, splatMapUV);
        #endif          
    #else
        float2 tintUV = uvMainAndLM.xy *_TerrainColorTintTexture_ST.xy + _TerrainColorTintTexture_ST.zw;
        float2 normalTintUV = uvMainAndLM.xy * _TerrainNormalTintTexture_ST.xy + _TerrainNormalTintTexture_ST.zw;

        weight = dot(splatControl, 1.0h);

        #ifdef TERRAIN_SPLAT_ADDPASS
            clip(weight <= 0.005h ? -1.0h : 1.0h);
        #endif

        #ifndef _TERRAIN_BASEMAP_GEN
            // Normalize weights before lighting and restore weights in final modifier functions so that the overal
            // lighting result can be correctly weighted.
            splatControl /= (weight + HALF_MIN);
        #endif    
    #endif

    float3 tint = SAMPLE_TEXTURE2D(_TerrainColorTintTexture, SamplerState_Linear_Repeat, tintUV).rgb;
    float3 normalTint = UnpackNormalScale(SAMPLE_TEXTURE2D(_TerrainNormalTintTexture, SamplerState_Linear_Repeat, normalTintUV), 1).xyz;

    #ifdef _LAYERS_ONE
        float4 splatControl = 1;
    #endif

    #if defined(_TERRAIN_BLEND_HEIGHT) && !defined(_TERRAIN_BASEMAP_GEN) && !defined(_LAYERS_ONE) 
        splatControl = (splatControl.r + splatControl.g + splatControl.b + splatControl.a == 0.0f ? 1 : splatControl); //this is preventing the black area when more than one pass
    #endif
 
    #if defined(INTERRA_OBJECT) || defined(TRIPLANAR) || defined(_TERRAIN_TRIPLANAR_ONE)
        float3 flipUV = worldNormal.rgb < 0 ? -1 : 1;
        float3  weights = abs(worldNormal.rgb);
        weights = pow(weights, _TriplanarSharpness);
        weights = weights / (weights.x + weights.y + weights.z);

        #ifdef INTERRA_OBJECT
            float weight;
            TriplanarOneToAllSteep(splatControl, (1 - terrainNormals.w), weight);
        #else
            TriplanarOneToAllSteep(splatControl, (1 - weights.y), weight);
        #endif
    #endif  

    #if defined(_LAYERS_TWO)
        splatControl.r = _ControlNumber == 0 ? splatControl.r : _ControlNumber == 1 ? splatControl.g : _ControlNumber == 2 ? splatControl.b : splatControl.a;
        splatControl.g = 1 - splatControl.r;
    #endif

    #if defined(_TERRAIN_DISTANCEBLEND)
        float4 dSplat_control = splatControl;
    #endif

    #ifdef  PARALLAX
        float lod = _MipMapLevel + smoothstep(_MipMapFade.x, _MipMapFade.y, (distance(worldPos, _WorldSpaceCameraPos)));
    #endif

    //================================================================================
    //-------------------------------------- UVs -------------------------------------
    //================================================================================
    #ifdef INTERRA_OBJECT
        half4 mixedDiffuse;
        #if !defined(_OBJECT_TRIPLANAR)
            _SteepDistortion = worldNormal.y > 0.5 ? 0 : (1 - worldNormal.y) * _SteepDistortion;
            UvSplatDistort(uvSplat, worldPos.xz - _TerrainPosition.xz, objectAlbedo.r * _SteepDistortion); 
        #else
            UvSplat(uvSplat, worldPos.xz - _TerrainPosition.xz);
        #endif
    #else
        uvSplat[0] = uvSplat01.xy;
        uvSplat[1] = uvSplat01.zw;
        #if !defined(_LAYERS_TWO)
            uvSplat[2] = uvSplat23.xy;
            uvSplat[3] = uvSplat23.zw;
        #endif
    #endif   
        
    //--------------------- TRIPLANAR UV ------------------------  
    #if defined(TRIPLANAR) && !defined(TERRAIN_SPLAT_ADDPASS)
        #ifdef INTERRA_OBJECT
            float offsetZ = _DisableOffsetY == 1 ? -flipUV.z * worldPos.y : heightOffset * -flipUV.z + (worldPos.z);
            float offsetX = _DisableOffsetY == 1 ? -flipUV.x * worldPos.y : heightOffset * -flipUV.x + (worldPos.x);
              
            UvSplatFront(uvSplat_front, worldPos.x - _TerrainPosition.x, offsetZ - _TerrainPosition.z, flipUV);
            UvSplatSide(uvSplat_side, worldPos.z - _TerrainPosition.z, offsetX - _TerrainPosition.x, flipUV);
        #else
            uvSplat_front[0] = TerrainFrontUV(worldPos, _Splat0_ST, uvSplat[0]);
            uvSplat_side[0] = TerrainSideUV(worldPos, _Splat0_ST, uvSplat[0]);

            #if !defined(_TERRAIN_TRIPLANAR_ONE)
                uvSplat_front[1] = TerrainFrontUV(worldPos, _Splat1_ST, uvSplat[1]);
                uvSplat_side[1] = TerrainSideUV(worldPos, _Splat1_ST, uvSplat[1]);

                #if !defined(_LAYERS_TWO)
                    uvSplat_front[2] = TerrainFrontUV(worldPos, _Splat2_ST, uvSplat[2]);
                    uvSplat_side[2] = TerrainSideUV(worldPos, _Splat2_ST, uvSplat[2]);
                
                    uvSplat_front[3] = TerrainFrontUV(worldPos, _Splat3_ST, uvSplat[3]);
                    uvSplat_side[3] = TerrainSideUV(worldPos, _Splat3_ST, uvSplat[3]);
                #endif
            #endif
        #endif
    #endif

    //-------------------- PARALLAX OFFSET -------------------------                  
    #ifdef _TERRAIN_PARALLAX
        ParallaxUV(uvSplat, tangentViewDirTerrain, lod);
    #endif

    //--------------------- DISTANCE UV ------------------------
    #ifdef _TERRAIN_DISTANCEBLEND
        DistantUV(distantUV, uvSplat);
        #ifdef TRIPLANAR
            #ifdef _TERRAIN_TRIPLANAR_ONE
                distantUV_front[0] = uvSplat_front[0] * (_DiffuseRemapOffset0.r + 1) * _HT_distance_scale;
                distantUV_side[0] = uvSplat_side[0] * (_DiffuseRemapOffset0.r + 1) * _HT_distance_scale;
            #else
                DistantUV(distantUV_front, uvSplat_front);
                DistantUV(distantUV_side, uvSplat_side);
            #endif  
        #endif
    #endif

    //====================================================================================
    //-----------------------------------  MASK MAPS  ------------------------------------
    //====================================================================================
    #if defined(TERRAIN_MASK)
        SampleMask(mask, 0, uvSplat);
        #if defined(TRIPLANAR) && !defined(TERRAIN_SPLAT_ADDPASS)
            #ifdef _TERRAIN_TRIPLANAR_ONE
                SampleMaskTOL(mask_front, mask, uvSplat_front[0]);
                SampleMaskTOL(mask_side, mask, uvSplat_side[0]);
            #else
                SampleMask(mask_front, 0, uvSplat_front);
                SampleMask(mask_side, 0, uvSplat_side);
            #endif 
            MaskWeight(mask, mask_front, mask_side, weights);
        #endif
        #ifdef _TERRAIN_DISTANCEBLEND		
            SampleMask(dMask, 0, distantUV);
            #if defined(TRIPLANAR) && !defined(TERRAIN_SPLAT_ADDPASS)
                #ifdef _TERRAIN_TRIPLANAR_ONE
                    SampleMaskTOL(dMask_front, dMask, distantUV_front[0]);
                    SampleMaskTOL(dMask_side, dMask, distantUV_side[0]);
                #else
                    SampleMask(dMask_front, 0, distantUV_front);
                    SampleMask(dMask_side, 0, distantUV_side);
                #endif
            MaskWeight(dMask, dMask_front, dMask_side, weights);
            #endif
        #endif
    #endif              

    //========================================================================================
    //------------------------------ HEIGHT MAP SPLAT BLENDINGS ------------------------------
    //========================================================================================
    #if defined(_TERRAIN_BLEND_HEIGHT) && !defined(_LAYERS_ONE) && !defined(TERRAIN_SPLAT_ADDPASS)
        #ifdef _TERRAIN_BASEMAP_GEN
            if (_NumLayersCount <= 4)
        #endif
        {
            HeightBlend(mask, splatControl, _HeightTransition);
            #ifdef _TERRAIN_DISTANCEBLEND
                HeightBlend(dMask, dSplat_control, _Distance_HeightTransition);
            #endif
        }
    #endif

    //========================================================================================
    //--------------------------------  ALBEDO & SMOOTHNESS  ---------------------------------
    //========================================================================================
    #ifdef INTERRA_OBJECT 
        half4 defaultAlpha = _TerrainSmoothness;
    #else
        half4 defaultAlpha = float4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3);
    #endif 
    SampleSplat(splat, uvSplat, defaultAlpha, mask);
    #if defined(TRIPLANAR) && !defined(TERRAIN_SPLAT_ADDPASS)
        #ifdef _TERRAIN_TRIPLANAR_ONE
            SampleSplatTOL(splat_front, splat, uvSplat_front[0], defaultAlpha, splatControl, mask[0]);
            SampleSplatTOL(splat_side, splat, uvSplat_side[0], defaultAlpha, splatControl, mask[0]);
        #else
            SampleSplat(splat_front, uvSplat_front, defaultAlpha, mask);
            SampleSplat(splat_side, uvSplat_side, defaultAlpha, mask);
        #endif 
        TriplanarWeight(splat, splat_front, splat_side, weights);
    #endif
   
    SplatWeight(mixedDiffuse, splat, splatControl);

    #ifdef _TERRAIN_DISTANCEBLEND     
        SampleSplat(dSplat, distantUV, defaultAlpha, dMask);
        #if defined(TRIPLANAR) && !defined(TERRAIN_SPLAT_ADDPASS)
            #ifdef _TERRAIN_TRIPLANAR_ONE
                SampleSplatTOL(dSplat_front, dSplat, distantUV_front[0], defaultAlpha, dSplat_control, dMask[0]);
                SampleSplatTOL(dSplat_side, dSplat, distantUV_side[0], defaultAlpha, dSplat_control, dMask[0]);
            #else
                SampleSplat(dSplat_front, distantUV_front, defaultAlpha,  dMask);
                SampleSplat(dSplat_side, distantUV_side, defaultAlpha,  dMask);                
            #endif
            TriplanarWeight(dSplat, dSplat_front, dSplat_side, weights);            
        #endif

        half4 distantDiffuse;
        SplatWeight(distantDiffuse, dSplat, dSplat_control);
            
        float dist = smoothstep(_HT_distance.x, _HT_distance.y, (distance(worldPos, _WorldSpaceCameraPos)));
        distantDiffuse = lerp(mixedDiffuse, distantDiffuse, _HT_cover); 
        #ifdef _TERRAIN_BASEMAP_GEN            
            mixedDiffuse = distantDiffuse;
        #else
            mixedDiffuse = lerp(mixedDiffuse, distantDiffuse, dist);           
        #endif        
    #endif

    #if !defined(TRIPLANAR_TINT) 
        mixedDiffuse.rgb = lerp(mixedDiffuse.rgb, (mixedDiffuse.rgb * tint), _TerrainColorTintStrenght).rgb;
    #endif

    //========================================================================================
    //------------------------------------  NORMAL MAPS   ------------------------------------
    //========================================================================================
    mixedNormal = float3(0, 0, 1);
    #if defined(_NORMALMAP) || defined(_TERRAIN_NORMAL_IN_MASK)
        half4 normalScale;
        float3 wTangent;
        float3 wBTangent;

        #ifdef INTERRA_OBJECT
            normalScale = _TerrainNormalScale;
            wTangent = worldTangent;
            wBTangent = worldBitangent;
        #else
            #ifdef _LAYERS_TWO
                normalScale = half4(_NormalScale0, _NormalScale1, 1, 1);
            #else
                normalScale = half4(_NormalScale0, _NormalScale1, _NormalScale2, _NormalScale3);
            #endif            
        #endif

        #if defined(_TERRAIN_NORMAL_IN_MASK) && defined(TERRAIN_MASK)
            mixedNormal = MaskNormal(mask, splatControl, normalScale);
        #else
            mixedNormal = SampleNormal(uvSplat, splatControl, normalScale);
        #endif

        #ifdef INTERRA_OBJECT
            mixedNormal = WorldTangent(wTangent, wBTangent, mixedNormal);
        #endif

        #if defined(TRIPLANAR) && !defined(TERRAIN_SPLAT_ADDPASS)
            half3 normalFront;
            half3 normalSide;
            #ifdef _TERRAIN_NORMAL_IN_MASK
                #ifdef _TERRAIN_TRIPLANAR_ONE
                    normalFront = MaskNormalTOL(mask_front, mixedNormal, splatControl, normalScale);
                    normalSide = MaskNormalTOL(mask_side, mixedNormal, splatControl, normalScale);
                #else
                    normalFront = MaskNormal(mask_front, splatControl, normalScale);
                    normalSide = MaskNormal(mask_side, splatControl, normalScale);
                #endif 
            #else
                #ifdef _TERRAIN_TRIPLANAR_ONE
                    normalFront = SampleNormalTOL(uvSplat_front, mixedNormal, splatControl, normalScale);
                    normalSide = SampleNormalTOL(uvSplat_side, mixedNormal, splatControl, normalScale);
                #else
                    normalFront = SampleNormal(uvSplat_front, splatControl, normalScale);
                    normalSide = SampleNormal(uvSplat_side, splatControl, normalScale);
                #endif 
            #endif 
            mixedNormal = TriplanarNormal(mixedNormal, wTangent, wBTangent, normalFront, normalSide, weights, flipUV);
        #endif      

        #ifdef _TERRAIN_DISTANCEBLEND	           
            #if !defined(_TERRAIN_NORMAL_IN_MASK)
                half3 dNormal = SampleNormal(distantUV, dSplat_control, normalScale);
            #else
                half3 dNormal = MaskNormal(dMask, dSplat_control, normalScale);
            #endif

            #ifdef INTERRA_OBJECT
                dNormal = WorldTangent(wTangent, wBTangent, dNormal);
            #endif

            #if defined(TRIPLANAR) && !defined(TERRAIN_SPLAT_ADDPASS)
                #ifdef _TERRAIN_NORMAL_IN_MASK
                    #ifdef _TERRAIN_TRIPLANAR_ONE
                        normalFront = MaskNormalTOL(dMask_front, dNormal, dSplat_control, normalScale);
                        normalSide = MaskNormalTOL(dMask_side, dNormal, dSplat_control, normalScale);
                    #else
                        normalFront = MaskNormal(dMask_front, dSplat_control, normalScale);
                        normalSide = MaskNormal(dMask_side, dSplat_control, normalScale);
                    #endif 
                #else
                    #ifdef _TERRAIN_TRIPLANAR_ONE
                        normalFront = SampleNormalTOL(distantUV_front, dNormal, dSplat_control, normalScale);
                        normalSide = SampleNormalTOL(distantUV_side, dNormal, dSplat_control, normalScale);
                    #else
                        normalFront = SampleNormal(distantUV_front, dSplat_control, normalScale);
                        normalSide = SampleNormal(distantUV_side, dSplat_control, normalScale);
                    #endif 
                #endif 
                dNormal = TriplanarNormal(dNormal, wTangent, wBTangent, normalFront, normalSide, weights, flipUV);
            #endif   
            mixedNormal = lerp(mixedNormal, (lerp(mixedNormal, dNormal, _HT_cover)), dist);        
        #endif  

        #if HAS_HALF
            mixedNormal.z += half(0.01);
        #else
            mixedNormal.z += 1e-5f;
        #endif        
    #endif

    #ifdef INTERRA_OBJECT
          normalTint = WorldTangent(wTangent, wBTangent, normalTint);
    #endif
    float normalDist = smoothstep(_TerrainNormalTintDistance.x, _TerrainNormalTintDistance.y, (distance(worldPos, _WorldSpaceCameraPos)));
    mixedNormal = lerp(mixedNormal, BlendNormalRNM(mixedNormal, normalTint), _TerrainNormalTintStrenght * normalDist).rgb;

    //========================================================================================
    //--------------------------------   AMBIENT OCCLUSION   ---------------------------------
    //========================================================================================
    occlusion = 1;
    #if defined(_TERRAIN_MASK_MAPS) || defined(_TERRAIN_NORMAL_IN_MASK)
        occlusion = AmbientOcclusion(mask, splatControl);
        #if defined (_TERRAIN_DISTANCEBLEND)
            half dAo = AmbientOcclusion(dMask, dSplat_control);
            dAo = lerp(occlusion, dAo, _HT_cover);
            occlusion = lerp(occlusion, dAo, dist);
        #endif
    #endif

    //========================================================================================
    //--------------------------------------   METALLIC   ------------------------------------
    //========================================================================================
    #if defined(_TERRAIN_MASK_MAPS) && !defined(_TERRAIN_NORMAL_IN_MASK)
        metallic = MetallicMask(mask, splatControl);
        #if defined (_TERRAIN_DISTANCEBLEND)
            half dMetallic = MetallicMask(dMask, dSplat_control);
            dMetallic = lerp(metallic, dMetallic, _HT_cover);
            metallic = lerp(metallic, dMetallic, dist);
        #endif
    #else
        metallic = Metallic(splatControl);
        #if defined (_TERRAIN_DISTANCEBLEND)
            half dMetallic = Metallic(dSplat_control);
            dMetallic = lerp(metallic, dMetallic, _HT_cover);
            metallic = lerp(metallic, dMetallic, dist);
        #endif
    #endif

    //=======================================================================================
    //==============================|   OBJECT INTEGRATION   |===============================
    //=======================================================================================
    #ifdef INTERRA_OBJECT	
        float steepWeights = _SteepIntersection == 1 ? saturate(worldNormal.y + _Steepness) : 1;
        float intersect1 = smoothstep(_Intersection.y, _Intersection.x, heightOffset) * steepWeights;
        float intersect2 = smoothstep(_Intersection2.y, _Intersection2.x, heightOffset) * (1 - steepWeights);
        float intersection = intersect1 + intersect2;
        float intersectNormal = smoothstep(_NormIntersect.y, _NormIntersect.x, heightOffset);
         
        objectMask.rgba = objectMask.rgba * _MaskMapRemapScale.rgba + _MaskMapRemapOffset.rgba;
        objectAlbedo.a = _HasMask == 1 ? objectMask.a : _Smoothness;
        half objectMetallic = _HasMask == 1 ? objectMask.r : _Metallic;
        half objectAo = _HasMask == 1 ? objectMask.g : _Ao;
        half height = objectMask.b;

        half sSum;
        #ifdef _TERRAIN_BLEND_HEIGHT 
            sSum = lerp(HeightSum(mask, splatControl), 1, intersection);
        #else 	
            sSum = 0.5;
        #endif 

        float2 heightIntersect = (1 / (1 * pow(2, float2(((1 - intersection) * height), (intersection * sSum)) * (-(_Sharpness)))) + 1) * 0.5;
        heightIntersect /= (heightIntersect.r + heightIntersect.g);
                    
        half3 mainNorm = UnpackNormalScale(objectNormal, _BumpScale);

        // avoid risk of NaN when normalizing.
        #if HAS_HALF
            mainNorm.z += half(0.01);
        #else
            mainNorm.z += 1e-5f;
        #endif
        
        #ifdef _OBJECT_DETAIL
            UNITY_BRANCH if (_HasDetailAlbedo > 0)
            {
                half3 dt = SAMPLE_TEXTURE2D(_DetailAlbedoMap, SamplerState_Linear_Repeat, _DetailUV).rgb;
                objectAlbedo.rgb = lerp(objectAlbedo.rgb, half(2.0) * dt, _DetailStrenght).rgb;
            }
            half3 mainNormD = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, SamplerState_Linear_Repeat, _DetailUV), _DetailNormalMapScale);
            mainNorm = (lerp(mainNorm, BlendNormalRNM(mainNorm, mainNormD), _DetailStrenght));
        #endif

        mixedDiffuse = lerp(mixedDiffuse, objectAlbedo, heightIntersect.r);

        float3 terrainNormal = (mixedNormal.z * terrainNormals.xyz);
        terrainNormal.xy = mixedNormal.xy + terrainNormal.xy;

        mixedNormal = lerp(mixedNormal, terrainNormal, intersectNormal);
        mixedNormal = lerp(mixedNormal, mainNorm, heightIntersect.r);

        metallic = lerp(metallic, objectMetallic, heightIntersect.r);
        occlusion = lerp(occlusion, objectAo, heightIntersect.r);

        emission = 0;

        UNITY_BRANCH if (_EmissionEnabled > 0)
        {
            emission = lerp(0, objectEmission, heightIntersect.r);
        }
        albedo = mixedDiffuse.rgb;
    #endif

    smoothness = mixedDiffuse.a;
    //=========================================================================================             
}
