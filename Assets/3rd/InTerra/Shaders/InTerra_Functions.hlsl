#ifdef INTERRA_OBJECT
    #if defined(_TERRAIN_MASK_MAPS) || defined(_TERRAIN_NORMAL_IN_MASK) || defined(_TERRAIN_BLEND_HEIGHT) || defined(_TERRAIN_PARALLAX)
        #define TERRAIN_MASK
    #endif

    #if defined (_OBJECT_TRIPLANAR) || defined (_TERRAIN_TRIPLANAR_ONE)
        #define TRIPLANAR
    #endif

    #if defined(_TERRAIN_PARALLAX) || defined(_OBJECT_PARALLAX) 
        #define PARALLAX
    #endif
#endif

#ifdef _LAYERS_ONE
    #define _LAYER_COUNT 1
#else
    #ifdef _LAYERS_TWO
        #define _LAYER_COUNT 2
    #else
        #define _LAYER_COUNT 4
    #endif
#endif

//==========================================================================================
//======================================   FUNCTIONS   =====================================
//==========================================================================================
float2 ObjectFrontUV(float posOffset, half4 splatUV, float offsetZ)
{
    return  float2((posOffset + splatUV.z) / splatUV.x, (offsetZ + splatUV.w) / splatUV.y);
}

float2 ObjectSideUV(float posOffset, half4 splatUV, float offsetX)
{
    return  float2((offsetX + splatUV.z) / splatUV.x, (posOffset + splatUV.w) / splatUV.y);
}

half3 WorldTangent(float3 wTangent, float3 wBTangent, half3 mixedNormal)
{
    mixedNormal.xy = mul(float2x2(wTangent.xz, wBTangent.xz), mixedNormal.xy);
    return  half3(mixedNormal);
}

half2 HeightBlendTwoTextures(float2 splat, float2 heights, half sharpness)
{
    splat *= (1 / (1 * pow(2, heights * (-(sharpness)))) + 1) * 0.5;
    splat /= (splat.r + splat.g);

    return  splat;
}

half3 UnpackNormalGAWithScale(half4 packednormal, float scale)
{
    half3 normal;
    normal.xy = (packednormal.wy * 2 - 1) * scale;
    normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));

    return normal;
}


void TriplanarOneToAllSteep(in out float4 splat_control, float weightY, in out half splatWeight)
{   
    if (_TriplanarOneToAllSteep == 1)
    {
       #if !defined(TERRAIN_SPLAT_ADDPASS) 
            splat_control = float4(saturate(splat_control.r + weightY), saturate(splat_control.gba - weightY));
            splatWeight = saturate(splatWeight + weightY);
        #else
            splat_control = float4(saturate(splat_control.rgba - weightY));
           splatWeight = saturate(splatWeight - weightY);
        #endif
    }
}  

half3 TriplanarNormal(half3 normal, half3 tangent, half3 bTangent, half3 normal_front, half3 normal_side, float3 weights, half3 flipUV)
{
    #ifdef INTERRA_OBJECT
        normal_front.y *= -flipUV.z;
        normal_front.xy = mul(float2x2(tangent.xy, bTangent.xy), normal_front.xy);

        normal_side.x *= -flipUV.x;
        normal_side.xy = mul(float2x2(tangent.yz, bTangent.yz), normal_side.xy);
    #else
         normal_front.y *= -flipUV.z;
         normal_side.xy = normal_side.yx; //this is needed because the uv was rotated
         normal_side.x *= -flipUV.x;
    #endif

    return half3 (normal * weights.y + normal_front * weights.z + normal_side * weights.x);
}

#if defined (PARALLAX)

    #define MipMapLod(i, lod) float(_MipMapLevel + (lod * log2(max(_Mask##i##_TexelSize.z, _Mask##i##_TexelSize.w)) + 1))

    float GetParallaxHeight(texture2D maskT, sampler maskS, float2 uv, float2 offset, float lod)
    {
        return SAMPLE_TEXTURE2D_LOD(maskT, maskS, float2(uv + offset), lod).b;
    }

    //this function is based on Parallax Occlusion Mapping from Shader Graph URP
    float2 ParallaxOffset(texture2D maskT, sampler maskS, int numSteps, float amplitude, float2 uv, float3 tangentViewDir, float affineSteps, float lod)
    {    
        float2 offset = 0;

        if (numSteps > 0)
        {
            float3 viewDir = float3(tangentViewDir.xy * amplitude * -0.01, tangentViewDir.z);
            float stepSize = (1.0 / numSteps);

            float2 texOffsetPerStep = stepSize * viewDir.xy;

            // Do a first step before the loop to init all value correctly
            float2 texOffsetCurrent = float2(0.0, 0.0); 
            float prevHeight = GetParallaxHeight(maskT, maskS, uv, texOffsetCurrent, lod);
            texOffsetCurrent += texOffsetPerStep;
            float currHeight = GetParallaxHeight(maskT, maskS, uv, texOffsetCurrent, lod);
            float rayHeight = 1.0 - stepSize; // Start at top less one sample

            for (int stepIndex = 0; stepIndex < numSteps; ++stepIndex)
            {
                // Have we found a height below our ray height ? then we have an intersection
                if (currHeight > rayHeight)
                    break; // end the loop

                prevHeight = currHeight;
                rayHeight -= stepSize;
                texOffsetCurrent += texOffsetPerStep;

                currHeight = GetParallaxHeight(maskT, maskS, uv, texOffsetCurrent, lod);
            }

            if (affineSteps <= 1)
            {
                float delta0 = currHeight - rayHeight;
                float delta1 = (rayHeight + stepSize) - prevHeight;
                float ratio = delta0 / (delta0 + delta1);
                offset = texOffsetCurrent - ratio * texOffsetPerStep;

                currHeight = GetParallaxHeight(maskT, maskS, uv, texOffsetCurrent, lod);
            }
            else
            {
                float pt0 = rayHeight + stepSize;
                float pt1 = rayHeight;
                float delta0 = pt0 - prevHeight;
                float delta1 = pt1 - currHeight;

                float delta;

               // Secant method to affine the search
                // Ref: Faster Relief Mapping Using the Secant Method - Eric Risser
               for (int i = 0; i < affineSteps; ++i)
                {
                    // intersectionHeight is the height [0..1] for the intersection between view ray and heightfield line
                    float intersectionHeight = (pt0 * delta1 - pt1 * delta0) / (delta1 - delta0);
                    // Retrieve offset require to find this intersectionHeight
                    offset = (1 - intersectionHeight) * texOffsetPerStep * numSteps;

                    currHeight = GetParallaxHeight(maskT, maskS, uv, offset, lod);

                    delta = intersectionHeight - currHeight;

                    if (abs(delta) <= 0.01)
                        break;

                    // intersectionHeight < currHeight => new lower bounds
                    if (delta < 0.0)
                    {
                        delta1 = delta;
                        pt1 = intersectionHeight;
                    }
                    else
                    {
                        delta0 = delta;
                        pt0 = intersectionHeight;
                    }
                }
            }
        }  
        return offset;
    }
#endif


//=========================================================================================
//--------------------------------------   ONE LAYER   ------------------------------------
//=========================================================================================
#ifdef _LAYERS_ONE
    #ifdef INTERRA_OBJECT
        void UvSplat(out float2 uvSplat, float2 posOffset)
        {
            uvSplat = ((posOffset + _SplatUV0.zw) / _SplatUV0.xy);
        }

        void UvSplatDistort(out float2 uvSplat, float2 posOffset, half distortion)
        {
            uvSplat = ((posOffset + (_SplatUV0.zw + distortion)) / _SplatUV0.xy).xy;
        }

        void DistantUV(out float2 distantUV, float2 uvSplat)
        {
            distantUV = uvSplat * (_DiffuseRemapOffset0.r + 1) * _HT_distance_scale;
        }

        void UvSplatFront(out float2 uvSplat, float worldPos, float offset, float3 flip)
        {
            #ifdef PARALLAX
                offset += ((_DiffuseRemapScale0.w * 0.004 * _SplatUV0.x) * -flip.z);
            #endif
            uvSplat = (ObjectFrontUV(worldPos, _SplatUV0, offset));
        }

        void UvSplatSide(out float2 uvSplat, float worldPos, float offset, float3 flip)
        {
            #ifdef PARALLAX
                offset += ((_DiffuseRemapScale0.w * 0.004 * _SplatUV0.y) * -flip.x);
            #endif
            uvSplat = ObjectSideUV(worldPos, _SplatUV0, offset);       
        }
    #endif

    void SampleMask(out half4 mask, half4 hasMask, float2 uv)
    {
        mask = SAMPLE_TEXTURE2D(_Mask0, sampler_Mask0, uv);

        #ifdef _TERRAIN_NORMAL_IN_MASK
            mask.rb = mask.rb * _MaskMapRemapScale0.gb + _MaskMapRemapOffset0.gb;
        #else
            mask.rgba = mask.rgba * _MaskMapRemapScale0.rgba + _MaskMapRemapOffset0.rgba;
        #endif
    }

    void SampleSplat(out half4 splat, float2 uv, half defaultAlpha, half4 mask)
    {
        splat = SAMPLE_TEXTURE2D(_Splat0, sampler_Splat0, uv.xy) * half4(_DiffuseRemapScale0.xyz, 1);

        #if defined(_TERRAIN_MASK_MAPS) && !defined(_TERRAIN_NORMAL_IN_MASK)
            splat.a = mask.a;
        #else
            splat.a = splat.a * defaultAlpha.r;
        #endif
    }

    #if defined(_NORMALMAP)
        half3 SampleNormal(float2 uv, float splat_control, half4 normalScale)
        {
        
            half3 nrm = half(0.0);
            nrm = UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal0, sampler_Normal0, uv), normalScale.x);

            // avoid risk of NaN when normalizing.
            #if HAS_HALF
                nrm.z += half(0.01);
            #else
                nrm.z += 1e-5f;
            #endif

            return  normalize(nrm.xyz);   
        }
    #endif

    void SplatWeight(out half4 mixedDiffuse, half4 splat, float4 splat_control)
    {
        mixedDiffuse = splat;
    }

    void TriplanarWeight(inout half4 mask, half4 mask_front, half4 mask_side, float3 weights)
    {
        mask = (mask * weights.y) + (mask_front * weights.z) + (mask_side * weights.x);
    }

    void MaskWeight(inout half4 mask, float4 mask_front, float4 mask_side, float3 weights)
    {
        mask = (mask * weights.y) + (mask_front * weights.z) + (mask_side * weights.x);
    }

    #ifdef _TERRAIN_NORMAL_IN_MASK 
        half3 MaskNormal(float4 mask, float4 splat_control, half4 normalScale)
        {
            return  half3(normalize(UnpackNormalGAWithScale(mask, normalScale.x)));
        }        
    #endif

    #ifdef TERRAIN_MASK
        half AmbientOcclusion(half4 mask, half4 splat_control)
        {
            #ifdef _TERRAIN_NORMAL_IN_MASK
                return  mask.r;
            #else
                return  mask.g;
            #endif  
        }

        half MetallicMask(half4 mask, half4 splat_control)
        {
            return  mask.r;
        }
    #endif  

    half Metallic(half4 splat_control)
    {
        return  _TerrainMetallic.x;
    }

    half HeightSum(half4 mask, half4 splat_control)
    {
        return half(mask.b);      
    }

    #ifdef PARALLAX
        void ParallaxUV(inout float2 uv, float3 tangentViewDir, float lod)
        {
            uv += ParallaxOffset(_Mask0, sampler_Mask0, _DiffuseRemapOffset0.w, _DiffuseRemapScale0.w, uv, tangentViewDir, _ParallaxAffineStepsTerrain, MipMapLod(0, lod));
        }
    #endif 

#endif


//=========================================================================================
//--------------------------------------   TWO LAYERS   -----------------------------------
//=========================================================================================
#ifdef _LAYERS_TWO
    #ifdef INTERRA_OBJECT
        void UvSplat(out float2 uvSplat[2], float2 posOffset)
        {
            uvSplat[0] = (posOffset + _SplatUV0.zw) / _SplatUV0.xy;
            uvSplat[1] = (posOffset + _SplatUV1.zw) / _SplatUV1.xy;
        }

        void UvSplatDistort(out float2 uvSplat[2], float2 posOffset, half distortion)
        {
            uvSplat[0] = ((posOffset + (_SplatUV0.zw + distortion)) / _SplatUV0.xy).xy;
            uvSplat[1] = ((posOffset + (_SplatUV1.zw + distortion)) / _SplatUV1.xy).xy;
        }

        void UvSplatFront(out float2 uvSplat[2], float worldPos, float offset, float3 flip)
        {
            #ifdef PARALLAX
                uvSplat[0] = ObjectFrontUV(worldPos, _SplatUV0, offset + (_DiffuseRemapScale0.w * 0.004 * _SplatUV0.x) * -flip.z);
                uvSplat[1] = ObjectFrontUV(worldPos, _SplatUV1, offset + (_DiffuseRemapScale1.w * 0.004 * _SplatUV1.x) * -flip.z);
            #else
                uvSplat[0] = ObjectFrontUV(worldPos, _SplatUV0, offset);
                uvSplat[1] = ObjectFrontUV(worldPos, _SplatUV1, offset);
            #endif
        }

        void UvSplatSide(out float2 uvSplat[2], float worldPos, float offset, float3 flip)
        {
            #ifdef PARALLAX
                uvSplat[0] = ObjectSideUV(worldPos, _SplatUV0, offset + (_DiffuseRemapScale0.w * 0.004 * _SplatUV0.y) * -flip.x);
                uvSplat[1] = ObjectSideUV(worldPos, _SplatUV1, offset + (_DiffuseRemapScale1.w * 0.004 * _SplatUV1.y) * -flip.x);
            #else
                uvSplat[0] = ObjectSideUV(worldPos, _SplatUV0, offset);
                uvSplat[1] = ObjectSideUV(worldPos, _SplatUV1, offset);
            #endif
        }
    #endif

    void DistantUV(out float2 distantUV[2], float2 uvSplat[2])
    {
        distantUV[0] = uvSplat[0] * (_DiffuseRemapOffset0.r + 1) * _HT_distance_scale;
        distantUV[1] = uvSplat[1] * (_DiffuseRemapOffset1.r + 1) * _HT_distance_scale;
    }

    #ifdef TERRAIN_MASK
        void SampleMask(out half4 masks[2], half4 hasMask, float2 uv[2])
        {
            masks[0] = 0.5h;
            masks[1] = 0.5h;

            masks[0] = SAMPLE_TEXTURE2D(_Mask0, sampler_Mask0, uv[0]);
            masks[1] = SAMPLE_TEXTURE2D(_Mask1, sampler_Mask0, uv[1]);

            #ifdef _TERRAIN_NORMAL_IN_MASK
                masks[0].rb = masks[0].rb * _MaskMapRemapScale0.gb + _MaskMapRemapOffset0.gb;
                masks[1].rb = masks[1].rb * _MaskMapRemapScale1.gb + _MaskMapRemapOffset1.gb;
            #else
                masks[0].rgba = masks[0].rgba * _MaskMapRemapScale0.rgba + _MaskMapRemapOffset0.rgba;
                masks[1].rgba = masks[1].rgba * _MaskMapRemapScale1.rgba + _MaskMapRemapOffset1.rgba;
            #endif
        }
    #endif

    #ifdef _TERRAIN_BLEND_HEIGHT
        void  HeightBlend(half4 mask[2], inout float4 splat_control, half sharpness)
        {
            half2 height = half2(mask[0].b, mask[1].b);

            splat_control.rg *= (1 / (1 * pow(2, (height + splat_control.rg) * (-(sharpness)))) + 1) * 0.5;
            splat_control.rg /= (splat_control.r + splat_control.g);
        }
    #endif

    void SampleSplat(out half4 splat[2], float2 uv[2], half4 defaultAlpha, half4 mask[2])
    {
        splat[0] = SAMPLE_TEXTURE2D(_Splat0, sampler_Splat0, uv[0]) * half4(_DiffuseRemapScale0.xyz, 1);
        splat[1] = SAMPLE_TEXTURE2D(_Splat1, sampler_Splat0, uv[1]) * half4(_DiffuseRemapScale1.xyz, 1);

        #if defined(_TERRAIN_MASK_MAPS) && !defined(_TERRAIN_NORMAL_IN_MASK)
            splat[0].a = mask[0].a;
            splat[1].a = mask[1].a;
        #else
            splat[0].a *= defaultAlpha.r;
            splat[1].a *= defaultAlpha.g;
        #endif
    }

    #if defined(_NORMALMAP)
        half3 SampleNormal(float2 uv[2], float4 splatControl, half4 normalScale)
        {

            half3 nrm = half(0.0);
            nrm += splatControl.r * UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal0, sampler_Normal0, uv[0]), normalScale.r);
            nrm += splatControl.g * UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal1, sampler_Normal0, uv[1]), normalScale.g);

            // avoid risk of NaN when normalizing.
            #if HAS_HALF
                nrm.z += half(0.01);
            #else
                nrm.z += 1e-5f;
            #endif

            return  normalize(nrm.xyz);

        }
    #endif

    void MaskWeight(inout half4 mask[2], half4 mask_front[2], half4 mask_side[2], float3 weights)
    {
        for (int i = 0; i < 2; ++i)
        {
            mask[i] = (mask[i] * weights.y) + (mask_front[i] * weights.z) + (mask_side[i] * weights.x);
        }
    }

    void SplatWeight(out half4 mixedDiffuse, half4 splat[2], float4 splat_control)
    {
        mixedDiffuse = splat[0] * splat_control.r + splat[1] * splat_control.g;
    }

    void TriplanarWeight(inout half4 mask[2], half4 mask_front[2], half4 mask_side[2], float3 weights)
    {
        for (int i = 0; i < 2; ++i)
        {
            mask[i] = (mask[i] * weights.y) + (mask_front[i] * weights.z) + (mask_side[i] * weights.x);
        }
    }

    #ifdef _TERRAIN_NORMAL_IN_MASK 
        half3 MaskNormal(half4 mask[2], float4 splatControl, half4 normalScale)
        {
            half3 normal;
            normal = UnpackNormalScale(mask[0], normalScale.x) * splatControl.r;
            normal += UnpackNormalGAWithScale(mask[1], normalScale.y) * splatControl.g;

            return  normalize(normal);
        }

        half3 MaskNormalTOL(half4 mask[2], half3 noTriplanarNormal, float4 splat_control, half4 normalScale)
        {
            half3 normal;
            normal = UnpackNormalGAWithScale(mask[0], normalScale.x) * splat_control.r;
            normal *= splat_control.r;
            noTriplanarNormal *= splat_control.g;
            normal += noTriplanarNormal;
            return  normal;
        }
    #endif

    #ifdef TERRAIN_MASK
        half AmbientOcclusion(half4 mask[2], half4 splat_control)
        {
            #ifdef _TERRAIN_NORMAL_IN_MASK
                half2 ao = half2(mask[0].r, mask[1].r);
            #else
                half2 ao = half2(mask[0].g, mask[1].g);
            #endif  

            return  half(dot(splat_control.rg, half2(ao.r, ao.g)));
        }

        half MetallicMask(half4 mask[2], half4 splat_control)
        {
            return  dot(splat_control.rg, half2(mask[0].r, mask[1].r));
        }
    #endif  

    half Metallic(half4 splat_control)
    {
        #ifdef INTERRA_OBJECT
            return dot(splat_control.rg, _TerrainMetallic.xy);
        #else
            return dot(splat_control.rg, half2(_Metallic0, _Metallic1));
        #endif           
    }

    half HeightSum(half4 mask[2], half4 splat_control)
    {
        return half(dot(splat_control.rg, half2(mask[0].b, mask[1].b)));      
    }

#ifdef PARALLAX
    void ParallaxUV(inout float2 uv[2], float3 tangentViewDir, float lod)
    {
        uv[0] += ParallaxOffset(_Mask0, sampler_Mask0, _DiffuseRemapOffset0.w, _DiffuseRemapScale0.w, uv[0], tangentViewDir, _ParallaxAffineStepsTerrain, MipMapLod(0, lod));
        uv[1] += ParallaxOffset(_Mask1, sampler_Mask0, _DiffuseRemapOffset1.w, _DiffuseRemapScale1.w, uv[1], tangentViewDir, _ParallaxAffineStepsTerrain, MipMapLod(1, lod));
    }
#endif 

    #if defined(_NORMALMAP) && !defined(_TERRAIN_NORMAL_IN_MASK) && !defined(TERRAIN_BASEGEN)
        half3 SampleNormalTOL(float2 uv[2], half3 noTriplanarNormal, float4 splat_control, half4 normalScale)
        {
            half3 normal = UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal0, sampler_Normal0, uv[0]), normalScale.x);

            normal *= splat_control.r;
            noTriplanarNormal *= splat_control.g;
            normal += noTriplanarNormal;

            return normal;
        }
    #endif

    #ifndef TERRAIN_BASEGEN
        void SampleSplatTOL(out half4 splat[2], half4 noTriplanarSplat[2], float2 uv, half defaultAlpha, float4 splat_control, half4 mask)
        {
            splat[0] = SAMPLE_TEXTURE2D(_Splat0, sampler_Splat0, uv) * half4(_DiffuseRemapScale0.xyz, 1);

            #if defined(_TERRAIN_MASK_MAPS) && !defined(_TERRAIN_NORMAL_IN_MASK)
                splat[0].a = mask.a;
            #else
                splat[0].a *= defaultAlpha.r;
            #endif 

            splat[1] = noTriplanarSplat[1];
        }

        #if defined(TERRAIN_MASK)
            void SampleMaskTOL(out half4 mask[2], half4 noTriplanarMask[2], float2 uv)
            {
                mask[0] = SAMPLE_TEXTURE2D(_Mask0, sampler_Mask0, uv);

                #ifdef _TERRAIN_NORMAL_IN_MASK
                    mask[0].rb = mask[0].rb * _MaskMapRemapScale0.gb + _MaskMapRemapOffset0.gb;
                #else
                    mask[0].rgba = mask[0].rgba * _MaskMapRemapScale0.rgba + _MaskMapRemapOffset0.rgba;
                #endif

                mask[1] = noTriplanarMask[1];
            }
        #endif 
    #endif
#endif



//=========================================================================================
//--------------------------------------   ONE PASS   -------------------------------------
//=========================================================================================
#if !defined(_LAYERS_ONE) && !defined(_LAYERS_TWO)
    #ifdef INTERRA_OBJECT
        void UvSplat(out float2 uvSplat[4], float2 posOffset)
        {
            uvSplat[0] = (posOffset + _SplatUV0.zw) / _SplatUV0.xy;
            uvSplat[1] = (posOffset + _SplatUV1.zw) / _SplatUV1.xy;
            uvSplat[2] = (posOffset + _SplatUV2.zw) / _SplatUV2.xy;
            uvSplat[3] = (posOffset + _SplatUV3.zw) / _SplatUV3.xy;
        }

        void UvSplatDistort(out float2 uvSplat[4], float2 posOffset, half distortion)
        {
            uvSplat[0] = ((posOffset + (_SplatUV0.zw + distortion)) / _SplatUV0.xy).xy;
            uvSplat[1] = ((posOffset + (_SplatUV1.zw + distortion)) / _SplatUV1.xy).xy;
            uvSplat[2] = ((posOffset + (_SplatUV2.zw + distortion)) / _SplatUV2.xy).xy;
            uvSplat[3] = ((posOffset + (_SplatUV3.zw + distortion)) / _SplatUV3.xy).xy;
        }

        void UvSplatFront (out float2 uvSplat[4], float worldPos, float offset, float3 flip)
        {
            #ifdef PARALLAX
                uvSplat[0] = ObjectFrontUV(worldPos, _SplatUV0, offset + (_DiffuseRemapScale0.w * 0.004 * _SplatUV0.x) * -flip.z);
                uvSplat[1] = ObjectFrontUV(worldPos, _SplatUV1, offset + (_DiffuseRemapScale1.w * 0.004 * _SplatUV1.x) * -flip.z);
                uvSplat[2] = ObjectFrontUV(worldPos, _SplatUV2, offset + (_DiffuseRemapScale2.w * 0.004 * _SplatUV2.x) * -flip.z);
                uvSplat[3] = ObjectFrontUV(worldPos, _SplatUV3, offset + (_DiffuseRemapScale3.w * 0.004 * _SplatUV3.x) * -flip.z);
            #else
                uvSplat[0] = ObjectFrontUV(worldPos, _SplatUV0, offset);
                uvSplat[1] = ObjectFrontUV(worldPos, _SplatUV1, offset);
                uvSplat[2] = ObjectFrontUV(worldPos, _SplatUV2, offset);
                uvSplat[3] = ObjectFrontUV(worldPos, _SplatUV3, offset);
            #endif
        }

        void UvSplatSide(out float2 uvSplat[4], float worldPos, float offset, float3 flip)
        {
            #ifdef PARALLAX
                uvSplat[0] = ObjectSideUV(worldPos, _SplatUV0, offset + (_DiffuseRemapScale0.w * 0.004 * _SplatUV0.y) * -flip.x);
                uvSplat[1] = ObjectSideUV(worldPos, _SplatUV1, offset + (_DiffuseRemapScale1.w * 0.004 * _SplatUV1.y) * -flip.x);
                uvSplat[2] = ObjectSideUV(worldPos, _SplatUV2, offset + (_DiffuseRemapScale2.w * 0.004 * _SplatUV2.y) * -flip.x);
                uvSplat[3] = ObjectSideUV(worldPos, _SplatUV3, offset + (_DiffuseRemapScale3.w * 0.004 * _SplatUV3.y) * -flip.x);
            #else
                uvSplat[0] = ObjectSideUV(worldPos, _SplatUV0, offset);
                uvSplat[1] = ObjectSideUV(worldPos, _SplatUV1, offset);
                uvSplat[2] = ObjectSideUV(worldPos, _SplatUV2, offset);
                uvSplat[3] = ObjectSideUV(worldPos, _SplatUV3, offset);
            #endif
        }
    #endif

    void DistantUV(out float2 distantUV[4], float2 uvSplat[4])
    {
        distantUV[0] = uvSplat[0] * (_DiffuseRemapOffset0.r + 1) * _HT_distance_scale;
        distantUV[1] = uvSplat[1] * (_DiffuseRemapOffset1.r + 1) * _HT_distance_scale;
        distantUV[2] = uvSplat[2] * (_DiffuseRemapOffset2.r + 1) * _HT_distance_scale;
        distantUV[3] = uvSplat[3] * (_DiffuseRemapOffset3.r + 1) * _HT_distance_scale;
    }

    #ifdef TERRAIN_MASK
        void SampleMask(out half4 masks[4], half4 hasMask, float2 uv[4]) 
        {
            masks[0] = 0.5h;
            masks[1] = 0.5h;
            masks[2] = 0.5h;
            masks[3] = 0.5h;

            #ifdef TERRAIN_MASK
                masks[0] = SAMPLE_TEXTURE2D(_Mask0, sampler_Mask0, uv[0]);
                masks[1] = SAMPLE_TEXTURE2D(_Mask1, sampler_Mask0, uv[1]);
                masks[2] = SAMPLE_TEXTURE2D(_Mask2, sampler_Mask0, uv[2]);
                masks[3] = SAMPLE_TEXTURE2D(_Mask3, sampler_Mask0, uv[3]);
            #endif

            #ifdef _TERRAIN_NORMAL_IN_MASK
                masks[0].rb = masks[0].rb * _MaskMapRemapScale0.gb + _MaskMapRemapOffset0.gb;
                masks[1].rb = masks[1].rb * _MaskMapRemapScale1.gb + _MaskMapRemapOffset1.gb;
                masks[2].rb = masks[2].rb * _MaskMapRemapScale2.gb + _MaskMapRemapOffset2.gb;
                masks[3].rb = masks[3].rb * _MaskMapRemapScale3.gb + _MaskMapRemapOffset3.gb;
            #else
                masks[0].rgba = masks[0].rgba * _MaskMapRemapScale0.rgba + _MaskMapRemapOffset0.rgba;
                masks[1].rgba = masks[1].rgba * _MaskMapRemapScale1.rgba + _MaskMapRemapOffset1.rgba;
                masks[2].rgba = masks[2].rgba * _MaskMapRemapScale2.rgba + _MaskMapRemapOffset2.rgba;
                masks[3].rgba = masks[3].rgba * _MaskMapRemapScale3.rgba + _MaskMapRemapOffset3.rgba;
            #endif
        }
    #endif

    #ifdef _TERRAIN_BLEND_HEIGHT
        void HeightBlend(half4 mask[4], inout float4 splat_control, float sharpness)
        {
            half4 height = half4 (mask[0].b, mask[1].b, mask[2].b, mask[3].b);
           
            splat_control.rgba *= (1 / (1 * pow(2, (height + splat_control.rgba) * (-(sharpness)))) + 1) * 0.5;
            splat_control.rgba /= (splat_control.r + splat_control.g + splat_control.b + splat_control.a);
        }
    #endif

    void SampleSplat(out half4 splat[4], float2 uv[4], half4 defaultAlpha, half4 mask[4])
    {
        splat[0] = SAMPLE_TEXTURE2D(_Splat0, sampler_Splat0, uv[0]) * half4(_DiffuseRemapScale0.xyz, 1);
        splat[1] = SAMPLE_TEXTURE2D(_Splat1, sampler_Splat0, uv[1]) * half4(_DiffuseRemapScale1.xyz, 1);
        splat[2] = SAMPLE_TEXTURE2D(_Splat2, sampler_Splat0, uv[2]) * half4(_DiffuseRemapScale2.xyz, 1);
        splat[3] = SAMPLE_TEXTURE2D(_Splat3, sampler_Splat0, uv[3]) * half4(_DiffuseRemapScale3.xyz, 1);

        #if defined(_TERRAIN_MASK_MAPS) && !defined(_TERRAIN_NORMAL_IN_MASK)
            splat[0].a = mask[0].a;
            splat[1].a = mask[1].a;
            splat[2].a = mask[2].a;
            splat[3].a = mask[3].a;
        #else
            splat[0].a *= defaultAlpha.r;
            splat[1].a *= defaultAlpha.g;
            splat[2].a *= defaultAlpha.b;
            splat[3].a *= defaultAlpha.a;
        #endif
    }

    #if defined(_NORMALMAP)
        half3 SampleNormal(float2 uv[4], float4 splatControl, half4 normalScale)
        {        
            half3 nrm = half(0.0);
            nrm += splatControl.r * UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal0, sampler_Normal0, uv[0]), normalScale.r);
            nrm += splatControl.g * UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal1, sampler_Normal0, uv[1]), normalScale.g);
            nrm += splatControl.b * UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal2, sampler_Normal0, uv[2]), normalScale.b);
            nrm += splatControl.a * UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal3, sampler_Normal0, uv[3]), normalScale.a);

            // avoid risk of NaN when normalizing.
            #if HAS_HALF
                nrm.z += half(0.01);
            #else
                nrm.z += 1e-5f; 
            #endif

            return  normalize(nrm.xyz);        
        }
    #endif

    void MaskWeight(inout half4 mask[4], half4 mask_front[4], half4 mask_side[4], float3 weights)
    {
        for (int i = 0; i < 4; ++i)
        {
            mask[i] = (mask[i] * weights.y) + (mask_front[i] * weights.z) + (mask_side[i] * weights.x);
        }
    }

    void SplatWeight(out half4 mixedDiffuse, half4 splat[4], float4 splat_control)
    {
        mixedDiffuse = splat[0] * splat_control.r + splat[1] * splat_control.g + splat[2] * splat_control.b + splat[3] * splat_control.a;
    }

    void TriplanarWeight(inout half4 mask[4], half4 mask_front[4], half4 mask_side[4], float3 weights)
    {
        for (int i = 0; i < 4; ++i)
        {
            mask[i] = (mask[i] * weights.y) + (mask_front[i] * weights.z) + (mask_side[i] * weights.x);
        }
    }

    #ifdef _TERRAIN_NORMAL_IN_MASK 
        half3 MaskNormal(half4 mask[4], float4 splatControl, half4 normalScale)
        {
            half3 normal;
            normal  = UnpackNormalGAWithScale(mask[0], normalScale.x) * splatControl.r;
            normal += UnpackNormalGAWithScale(mask[1], normalScale.y) * splatControl.g;
            normal += UnpackNormalGAWithScale(mask[2], normalScale.z) * splatControl.b;
            normal += UnpackNormalGAWithScale(mask[3], normalScale.w) * splatControl.a;
            return  normalize(normal);
        }

        half3 MaskNormalTOL(half4 mask[4], half3 noTriplanarNormal, float4 splat_control, half4 normalScale)
        {
            half3 normal;
            normal = UnpackNormalGAWithScale(mask[0], normalScale.x) * splat_control.r;
            normal *= splat_control.r;
            noTriplanarNormal *= splat_control.g + splat_control.b + splat_control.a;
            normal += noTriplanarNormal;
            return  normal;
        }
    #endif

    #ifdef TERRAIN_MASK
        half AmbientOcclusion(half4 mask[4], half4 splat_control)
        {
            #ifdef _TERRAIN_NORMAL_IN_MASK
                half4 ao = half4(mask[0].r, mask[1].r, mask[2].r, mask[3].r);
            #else
                half4 ao = half4(mask[0].g, mask[1].g, mask[2].g, mask[3].g);
            #endif  
               
            return  half(dot(splat_control, ao));
        }

        half MetallicMask(half4 mask[4], half4 splat_control)
        {       
            return dot(splat_control, half4(mask[0].r, mask[1].r, mask[2].r, mask[3].r));
        } 
    #endif  

    half Metallic(half4 splat_control)
    {
        #ifndef INTERRA_OBJECT 
            return dot(splat_control, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));
        #else
            return dot(splat_control, _TerrainMetallic);
        #endif         
    }

    half HeightSum(half4 mask[4], half4 splat_control)
    {        
        return half(dot(splat_control, half4(mask[0].b, mask[1].b, mask[2].b, mask[3].b)));

    }

    #if defined(_NORMALMAP) && !defined(_TERRAIN_NORMAL_IN_MASK) && !defined(TERRAIN_BASEGEN)
        half3 SampleNormalTOL(float2 uv[4], half3 noTriplanarNormal, float4 splat_control, half4 normalScale)
        {
            half3 normal = UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal0, sampler_Normal0, uv[0]), normalScale.x);
                
            normal *= splat_control.r;
            noTriplanarNormal *= splat_control.g + splat_control.b + splat_control.a;
            normal += noTriplanarNormal;

            return normal;
        }
    #endif

    #ifdef PARALLAX
        void ParallaxUV(inout float2 uv[4], float3 tangentViewDir, float lod)
        {
            uv[0] += ParallaxOffset(_Mask0, sampler_Mask0, _DiffuseRemapOffset0.w, _DiffuseRemapScale0.w, uv[0], tangentViewDir, _ParallaxAffineStepsTerrain, MipMapLod(0, lod));
            uv[1] += ParallaxOffset(_Mask1, sampler_Mask0, _DiffuseRemapOffset1.w, _DiffuseRemapScale1.w, uv[1], tangentViewDir, _ParallaxAffineStepsTerrain, MipMapLod(1, lod));
            uv[2] += ParallaxOffset(_Mask2, sampler_Mask0, _DiffuseRemapOffset2.w, _DiffuseRemapScale2.w, uv[2], tangentViewDir, _ParallaxAffineStepsTerrain, MipMapLod(2, lod));
            uv[3] += ParallaxOffset(_Mask3, sampler_Mask0, _DiffuseRemapOffset3.w, _DiffuseRemapScale3.w, uv[3], tangentViewDir, _ParallaxAffineStepsTerrain, MipMapLod(3, lod));
        }
    #endif 

    #ifndef TERRAIN_BASEGEN
        void SampleSplatTOL(out half4 splat[4], half4 noTriplanarSplat[4], float2 uv, half4 defaultAlpha, float4 splat_control, half4 mask)
        {
            splat[0] = SAMPLE_TEXTURE2D(_Splat0, sampler_Splat0, uv) * half4(_DiffuseRemapScale0.xyz, 1);

            #if defined(_TERRAIN_MASK_MAPS) && !defined(_TERRAIN_NORMAL_IN_MASK)
                splat[0].a = mask.a;
            #else
                splat[0].a *= defaultAlpha.r;
            #endif 

            splat[1] = noTriplanarSplat[1];
            splat[2] = noTriplanarSplat[2];
            splat[3] = noTriplanarSplat[3];
        }

        #if defined(TERRAIN_MASK)
            void SampleMaskTOL(out half4 mask[4], half4 noTriplanarMask[4], float2 uv)
            {
                mask[0] = SAMPLE_TEXTURE2D(_Mask0, sampler_Mask0, uv);
                        
                #ifdef _TERRAIN_NORMAL_IN_MASK
                    mask[0].rb = mask[0].rb * _MaskMapRemapScale0.gb + _MaskMapRemapOffset0.gb;
                #else
                    mask[0].rgba = mask[0].rgba * _MaskMapRemapScale0.rgba + _MaskMapRemapOffset0.rgba;
                #endif

                mask[1] = noTriplanarMask[1];
                mask[2] = noTriplanarMask[2];
                mask[3] = noTriplanarMask[3];
            }
        #endif 
    #endif
#endif