//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

#if !defined(PIPELINE_INCLUDED)
#define PIPELINE_INCLUDED
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

#if !defined(SAMPLERS_DEFINED) && !defined(UNITY_CORE_SAMPLERS_INCLUDED) && !defined(UNITY_CORE_BLIT_INCLUDED) && (!defined(POST_PROCESSING) && UNITY_VERSION >= 202320)
#define SAMPLERS_DEFINED
SamplerState sampler_LinearClamp;
SamplerState sampler_PointClamp;
SamplerState sampler_LinearRepeat;
#endif

#ifndef _DISABLE_DEPTH_TEX
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#endif

#if _REFRACTION || UNDERWATER_ENABLED
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
#endif

// Deprecated in URP 11+ https://github.com/Unity-Technologies/Graphics/pull/2529. Keep function for backwards compatibility
// Compute Normalized Device Coordinate here (this is normally done in GetVertexPositionInputs, but clip and world-space coords are done manually already)
#if UNITY_VERSION >= 202110 && !defined(UNITY_SHADER_VARIABLES_FUNCTIONS_DEPRECATED_INCLUDED)
float4 ComputeScreenPos(float4 positionCS)
{
	return ComputeNormalizedDeviceCoordinates(positionCS);
}
#endif
#endif

#if UNITY_VERSION <= 202010 //Unity 2019 (URP v7)
//Not available in older versions
float3 GetCurrentViewPosition()
{
	return _WorldSpaceCameraPos.xyz;
}

float3 GetWorldSpaceViewDir(float3 positionWS)
{
	return normalize(GetCurrentViewPosition() - positionWS);
}
#endif

#if UNITY_VERSION < 202120 //Unity 2020.3 (URP 10)
//Already declared in ShaderVariablesFunctions.hlsl
float LinearDepthToEyeDepth(float rawDepth)
{
	#if UNITY_REVERSED_Z
	return _ProjectionParams.z - (_ProjectionParams.z - _ProjectionParams.y) * rawDepth;
	#else
	return _ProjectionParams.y + (_ProjectionParams.z - _ProjectionParams.y) * rawDepth;
	#endif
}
#endif

#if UNITY_VERSION <= 202130
#define LIGHT_LOOP_BEGIN(lightCount) \
for (uint lightIndex = 0; lightIndex < pixelLightCount; ++lightIndex) { \
if (lightIndex >= (uint)lightCount) break;

#define LIGHT_LOOP_END }
#endif