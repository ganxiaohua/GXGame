//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

#ifndef WATER_COMMON_INCLUDED
#define WATER_COMMON_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

#if defined(TESSELLATION_ON)
#if (defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE) || defined(SHADER_API_VULKAN) || defined(SHADER_API_METAL) || defined(SHADER_API_PSSL) || defined(SHADER_API_XBOXONE))
#define UNITY_CAN_COMPILE_TESSELLATION
#else
#error [Stylized Water 2] Current graphics API does not support tessellation (only Direct3D 11, OpenGL ES 3.0, OpenGL, Vulkan, Metal, PS4 and Xbox One)
#endif
#endif

//As per the "Shader" section of the documentation, this is primarily used to synchronizing animations in networked applications.
float _CustomTime;
#define TIME_FRAG_INPUT _CustomTime > 0 ? _CustomTime : input.uv.z
#define TIME_VERTEX_OUTPUT _CustomTime > 0 ? _CustomTime : output.uv.z

#define TIME ((TIME_FRAG_INPUT * _Speed) * _Direction.xy)
#define TIME_VERTEX ((TIME_VERTEX_OUTPUT * _Speed) * _Direction.xy)

#define HORIZONTAL_DISPLACEMENT_SCALAR 0.01
#define UP_VECTOR float3(0,1,0)

struct WaterSurface
{
	uint vFace;
	float3 positionWS;
	float3 viewDir;

	float3 vertexNormal;
	float3 waveNormal;	
	half3x3 tangentToWorldMatrix;
	float3 tangentNormal;
	float3 tangentWorldNormal;
	float3 diffuseNormal;
	float3 pointSpotLightNormal;
	
	float3 albedo;
	float3 reflections;
	float3 caustics;
	float3 specular;
	half reflectionMask;
	half reflectionLighting;
	
	float3 offset;
	float slope;
	
	float fog;
	float intersection;
	float foam;

	float alpha;
	float edgeFade;
	float shadowMask;
};


float2 GetSourceUV(float2 uv, float2 wPos, float state) 
{
	float2 output =  lerp(uv, wPos, state);
	//output.x = (int)((output.x / 0.5) + 0.5) * 0.5;
	//output.y = (int)((output.y / 0.5) + 0.5) * 0.5;

	#ifdef _RIVER
	//World-space tiling is useless in this case
	return uv;
	#endif
	
	return output;
}

float4 GetVertexColor(float4 inputColor, float4 mask)
{
	return inputColor * mask;
}

float DepthDistance(float3 wPos, float3 viewPos, float3 normal)
{
	return length((wPos - viewPos) * normal);
}

float2 BoundsToWorldUV(in float3 wPos, in float4 b)
{
	float2 uv = b.xy / b.z + (b.z / (b.z * b.z)) * wPos.xz;

	//TODO: Check if required per URP version
	uv.y = 1 - uv.y;

	return uv;
}

float BoundsEdgeMask(float2 rect)
{
	float2 xz = abs(rect.xy * 14.0) - 6.0;
	float pos = length(max(xz, 0));
	float neg = min(max(xz.x, xz.y), 0);
	return 1-saturate(pos + neg);
}

float4 PackedUV(float2 sourceUV, float2 time, float speed, float subTiling = 0.5, float subSpeed = 0.5)
{
	#if _RIVER
	time.x = 0; //Only move in forward direction
	#endif
	
	float2 uv1 = sourceUV.xy + (time.xy * speed);
	
	#ifndef _RIVER
	//Second UV, 2x larger, twice as slow, in opposite direction
	float2 uv2 = (sourceUV.xy * subTiling) - ((time.xy) * speed * subSpeed);
	#else
	//2x larger, same direction/speed
	float2 uv2 = (sourceUV.xy * subTiling) + (time.xy * speed);
	#endif

	return float4(uv1.xy, uv2.xy);
}

struct SurfaceNormalData
{
	float3 geometryNormalWS;
	float3 pixelNormalWS;
	float lightingStrength;
	float mask;
};

float GetSlope(float3 normalWS, float threshold)
{
	return 1-smoothstep(1.0 - threshold, 1.0, saturate(dot(UP_VECTOR, normalWS)));
}

struct SceneDepth
{
	float raw;
	float linear01;
	float eye;
};

#define FAR_CLIP _ProjectionParams.z
#define NEAR_CLIP _ProjectionParams.y
//Scale linear values to the clipping planes for orthographic projection (unity_OrthoParams.w = 1 = orthographic)
#define DEPTH_SCALAR lerp(1.0, FAR_CLIP - NEAR_CLIP, unity_OrthoParams.w)

//Linear depth difference between scene and current (transparent) geometry pixel
float SurfaceDepth(SceneDepth depth, float4 positionCS)
{
	const float sceneDepth = (unity_OrthoParams.w == 0) ? depth.eye : LinearDepthToEyeDepth(depth.raw);
	const float clipSpaceDepth = (unity_OrthoParams.w == 0) ? LinearEyeDepth(positionCS.z, _ZBufferParams) : LinearDepthToEyeDepth(positionCS.z / positionCS.w);

	return sceneDepth - clipSpaceDepth;
}

//Return depth based on the used technique (buffer, vertex color, baked texture)
SceneDepth SampleDepth(float4 screenPos)
{
	SceneDepth depth = (SceneDepth)0;
	
#ifndef _DISABLE_DEPTH_TEX
	screenPos.xyz /= screenPos.w;

	depth.raw = SampleSceneDepth(screenPos.xy);
	depth.eye = LinearEyeDepth(depth.raw, _ZBufferParams);
	depth.linear01 = Linear01Depth(depth.raw, _ZBufferParams) * DEPTH_SCALAR;
#else
	depth.raw = 1.0;
	depth.eye = 1.0;
	depth.linear01 = 1.0;
#endif

	return depth;
}

float CheckPerspective(float x)
{
	return lerp(x, 1.0, unity_OrthoParams.w);
}

#define ORTHOGRAPHIC_SUPPORT

#if defined(USING_STEREO_MATRICES)
//Will never be used in VR, saves a per-fragment matrix multiplication
#undef ORTHOGRAPHIC_SUPPORT
#endif

//Reconstruct view-space position from depth.
float3 ReconstructViewPos(float4 screenPos, float3 viewDir, SceneDepth sceneDepth)
{
	#if UNITY_REVERSED_Z
	real rawDepth = sceneDepth.raw;
	#else
	// Adjust z to match NDC for OpenGL
	real rawDepth = lerp(UNITY_NEAR_CLIP_VALUE, 1, sceneDepth.raw);
	#endif
	
	#if defined(ORTHOGRAPHIC_SUPPORT)
	//View to world position
	float4 viewPos = float4((screenPos.xy/screenPos.w) * 2.0 - 1.0, rawDepth, 1.0);
	float4x4 viewToWorld = UNITY_MATRIX_I_VP;
	#if UNITY_REVERSED_Z //Wrecked since 7.3.1 "fix" and causes warping, invert second row https://issuetracker.unity3d.com/issues/shadergraph-inverse-view-projection-transformation-matrix-is-not-the-inverse-of-view-projection-transformation-matrix
	//Commit https://github.com/Unity-Technologies/Graphics/pull/374/files
	viewToWorld._12_22_32_42 = -viewToWorld._12_22_32_42;              
	#endif
	float4 viewWorld = mul(viewToWorld, viewPos);
	float3 viewWorldPos = viewWorld.xyz / viewWorld.w;
	#endif

	//Projection to world position
	float3 camPos = GetCurrentViewPosition().xyz;
	float3 worldPos = sceneDepth.eye * (viewDir/screenPos.w) - camPos;
	float3 perspWorldPos = -worldPos;

	#if defined(ORTHOGRAPHIC_SUPPORT)
	return lerp(perspWorldPos, viewWorldPos, unity_OrthoParams.w);
	#else
	return perspWorldPos;
	#endif

}

#define CHROMATIC_OFFSET 2.0

bool _BlurredWaterDepth;
TEXTURE2D_X(_BlurredCameraOpaqueTexture);
SAMPLER(sampler_BlurredCameraOpaqueTexture);
float3 OpaqueTexture(float2 uv)
{
	//return SampleSceneColor(uv).rgb;
	return SAMPLE_TEXTURE2D_X(_BlurredCameraOpaqueTexture, sampler_BlurredCameraOpaqueTexture, uv.xy).rgb;
}

float3 SampleOpaqueTexture(float4 screenPos, float2 offset, half vFace)
{
	//Normalize for perspective projection
	screenPos.xy += offset;
	screenPos.xy /= screenPos.w;
	
	float3 sceneColor = OpaqueTexture(screenPos.xy).rgb;
		
	#if _ADVANCED_SHADING //Chromatic
	float texelOffset = (_ScreenParams.z - 1.0) * CHROMATIC_OFFSET * vFace;
	sceneColor.r = OpaqueTexture(screenPos.xy + float2(texelOffset, 0)).r;
	sceneColor.b = OpaqueTexture(screenPos.xy - float2(texelOffset, 0)).b;
	#endif

	return sceneColor;
}
#endif