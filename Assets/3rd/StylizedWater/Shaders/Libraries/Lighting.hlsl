//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

#include "Common.hlsl"

#if !_UNLIT
#define LIT
#endif

#define SPECULAR_POWER_RCP 0.01562 // 1.0/32
#define AIR_RI 1.000293

//Schlick's BRDF fresnel
float ReflectionFresnel(float3 worldNormal, float3 viewDir, float exponent)
{
	float cosTheta = saturate(dot(worldNormal, viewDir));	
	return pow(max(0.0, AIR_RI - cosTheta), exponent);
}

TEXTURE2D_X(_PlanarReflection);
SAMPLER(sampler_PlanarReflection);

float3 SampleReflections(float3 reflectionVector, float smoothness, float mask, float4 screenPos, float3 positionWS, float3 normal, float3 viewDir, float2 pixelOffset)
{
	#if !_RIVER || UNITY_VERSION >= 202220
	screenPos.xy += pixelOffset.xy * lerp(1.0, 0.1, unity_OrthoParams.w);
	screenPos /= screenPos.w;
	#endif
	
	#if UNITY_VERSION >= 202220
	float3 probe = saturate(GlossyEnvironmentReflection(reflectionVector, positionWS, smoothness, 1.0, screenPos.xy)).rgb;
	#elif UNITY_VERSION >= 202120
	float3 probe = saturate(GlossyEnvironmentReflection(reflectionVector, positionWS, smoothness, 1.0)).rgb;
	#else
	float3 probe = saturate(GlossyEnvironmentReflection(reflectionVector, smoothness, 1.0)).rgb;
	#endif

	#if !_RIVER //Planar reflections are pointless on curved surfaces, skip	
	float4 planarReflections = SAMPLE_TEXTURE2D_X_LOD(_PlanarReflection, sampler_PlanarReflection, screenPos.xy, 0);
	//Terrain add-pass can output negative alpha values. Clamp as a safeguard against this
	planarReflections.a = saturate(planarReflections.a);
	
	return lerp(probe, planarReflections.rgb, planarReflections.a * mask);
	#else
	return probe;
	#endif
}

//Reusable for every light
struct TranslucencyData
{
	float3 subsurfaceColor;
	float3 lightColor;
	float3 lightDir;
	float3 viewDir;
	float3 normal;
	float curvature;
	float mask; //Actually the 'thickness'
	float strength;
	float exponent;

};

TranslucencyData PopulateTranslucencyData(float3 subsurfaceColor, float3 lightDir, float3 lightColor, float3 viewDir, float3 WorldNormal, float3 worldTangentNormal, float mask, float strength, float exponent, float offet)
{
	TranslucencyData d = (TranslucencyData)0;
	d.subsurfaceColor = subsurfaceColor;
	d.lightColor = lightColor;
	d.lightDir = lightDir;
	
	#if _ADVANCED_SHADING
	d.normal = lerp(WorldNormal, worldTangentNormal, 0.2);
	#else
	d.normal = WorldNormal;
	#endif
	d.curvature = offet;
	d.mask = mask; //Shadows, foam, intersection, etc
	d.strength = strength;
	d.viewDir = viewDir;
	d.exponent = exponent;

	return d;
}

//Single channel overlay
float BlendOverlay(float a, float b)
{
	return (b < 0.5) ? 2.0 * a * b : 1.0 - 2.0 * (1.0 - a) * (1.0 - b);
}

//RGB overlay
float3 BlendOverlay(float3 a, float3 b)
{
	return float3(BlendOverlay(a.r, b.r), BlendOverlay(a.g, b.g), BlendOverlay(a.b, b.b));
}

void ApplyTranslucency(float3 subsurfaceColor, float3 lightDir, float3 lightColor, float3 viewDir, float3 normal, float mask, float strength, float exponent, float offset, inout float3 emission)
{
	#if _TRANSLUCENCY

	float attenuation = 1;

	//Perturb the light vector towards the normal. Pushes the effect towards the rim of the surface
	const float3 lightHalfVec = normalize(lightDir + (normal * offset));
	//Coefficient describing how much the surface orientation is between the camera and the direction of/to the light  
	float VdotL = saturate(dot(-viewDir, lightHalfVec));
	//Exponentiate to tighten the falloff
	VdotL = saturate(pow(VdotL, exponent));

	#if _ADVANCED_SHADING
	//Fade the effect out as the sun approaches the horizon (80 to 90 degrees)
	half sunAngle = saturate(dot(float3(0, 1, 0), lightDir));
	half angleMask = saturate(sunAngle * 10); /* 1.0/0.10 = 10 */
	VdotL *= angleMask;
	#endif

	float lightIntensity = length(lightColor);
	attenuation = VdotL * mask;

#if _ADVANCED_SHADING
	//Modulate with light color to better match dynamic lighting conditions
	subsurfaceColor = BlendOverlay(lightColor, subsurfaceColor);
	subsurfaceColor *= lightIntensity * strength;
	
	emission += subsurfaceColor * attenuation;
#else //Simple shading
	subsurfaceColor *= lightIntensity * strength;
	
	emission += lerp(emission, subsurfaceColor, attenuation);
#endif

	#endif
}

void ApplyTranslucency(TranslucencyData translucencyData, inout float3 emission)
{
	ApplyTranslucency(translucencyData.subsurfaceColor, translucencyData.lightDir, translucencyData.lightColor, translucencyData.viewDir, translucencyData.normal, translucencyData.mask, translucencyData.strength, translucencyData.exponent, translucencyData.curvature, emission);
}

void AdjustShadowStrength(inout Light light, float strength, float vFace)
{
	light.shadowAttenuation = saturate(light.shadowAttenuation + (1.0 - (strength * vFace)));
}

//In URP light intensity is pre-multiplied with the HDR color, extract via magnitude of color "vector"
float GetLightIntensity(Light light)
{
	return length(light.color);
}

//Specular Blinn-phong reflection in world-space
float3 SpecularReflection(Light light, float3 viewDirectionWS, float3 geometryNormalWS, float3 normalWS, float perturbation, float exponent, float intensity)
{
	//Blend between geometry/wave normals and normals from normal map (aka distortion)
	normalWS = lerp(geometryNormalWS, normalWS, perturbation);

	const float3 halfVec = SafeNormalize(light.direction + viewDirectionWS + (normalWS * perturbation));
	half NdotH = saturate(dot(geometryNormalWS, halfVec));

	float specular = pow(NdotH, exponent);
	
	//Attenuation includes shadows, if available
	const float3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);

	//Mask reflection by surfaces "visible" by the light only
	float viewFactor = saturate(dot(geometryNormalWS, light.direction));

	#if _ADVANCED_SHADING
	//Create a linear gradient a little before the cutoff point, in order to maintain HDR values properly
	viewFactor = smoothstep(0.0, 0.15, viewFactor);
	#endif

	float3 specColor = attenuatedLightColor * specular * intensity * viewFactor;
	
	#if UNITY_COLORSPACE_GAMMA
	specColor = LinearToSRGB(specColor);
	#endif

	return specColor;
}

//Based on UniversalFragmentBlinnPhong (no BRDF)
float3 ApplyLighting(inout SurfaceData surfaceData, inout float3 sceneColor, Light mainLight, InputData inputData, WaterSurface water, TranslucencyData translucencyData, float shadowStrength, float vFace)
{
	ApplyTranslucency(translucencyData, surfaceData.emission.rgb);

	#if _CAUSTICS
	float causticsAttentuation = 1;
	#endif
	
#ifdef LIT
	#if _CAUSTICS
	causticsAttentuation = GetLightIntensity(mainLight) * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
	#endif
	
	//Allow shadow strength to be overridden.
	AdjustShadowStrength(mainLight, shadowStrength, vFace);
	
	half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);

	MixRealtimeAndBakedGI(mainLight, water.diffuseNormal, inputData.bakedGI, shadowStrength.xxxx);

	/*
	//PBR shading
	BRDFData brdfData;
	InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

	half3 diffuseColor = GlobalIllumination(brdfData, inputData.bakedGI, shadowStrength, inputData.water.diffuseNormal, inputData.viewDirectionWS);
	diffuseColor += LightingPhysicallyBased(brdfData, mainLight, water.diffuseNormal, inputData.viewDirectionWS);
	*/

	half3 diffuseColor = inputData.bakedGI + LightingLambert(attenuatedLightColor, mainLight.direction, water.diffuseNormal);
	
#if _ADDITIONAL_LIGHTS //Per pixel lights
	#ifndef _SPECULARHIGHLIGHTS_OFF
	half specularPower = (_PointSpotLightReflectionSize * SPECULAR_POWER_RCP);
	specularPower = lerp(8.0, 1.0, _PointSpotLightReflectionSize) * _PointSpotLightReflectionStrength;
	#endif
	
	uint pixelLightCount = GetAdditionalLightsCount();
	#if _LIGHT_LAYERS && UNITY_VERSION >= 202220
	uint meshRenderingLayers = GetMeshRenderingLayer();
	#endif
	
	LIGHT_LOOP_BEGIN(pixelLightCount)
		#if UNITY_VERSION >= 202110 //URP 11+
		Light light = GetAdditionalLight(lightIndex, inputData.positionWS, shadowStrength.xxxx);	
		#else
		Light light = GetAdditionalLight(lightIndex, inputData.positionWS);
		#endif

		#if _LIGHT_LAYERS && UNITY_VERSION >= 202220
		if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
		#endif
		{
			#if _ADVANCED_SHADING
			#if _CAUSTICS && !_LIGHT_COOKIES //Actually want to skip this when using cookies. Since they can be used for caustics instead
			//Light attenuation adds caustics, mask by shadows
			causticsAttentuation += GetLightIntensity(light) * (light.distanceAttenuation * light.shadowAttenuation) * _PointSpotLightReflectionStrength * (1-water.fog);
			#endif
			
			#if _TRANSLUCENCY
			//Keep settings from main light pass, override these
			translucencyData.lightDir = light.direction;
			translucencyData.lightColor = light.color * light.distanceAttenuation;
			translucencyData.strength *= light.shadowAttenuation;
				
			ApplyTranslucency(translucencyData, surfaceData.emission.rgb);
			#endif
			#endif

			#if UNITY_VERSION >= 202110 && _ADDITIONAL_LIGHT_SHADOWS //URP 11+
			AdjustShadowStrength(light, shadowStrength, vFace);
			#endif

			half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
			diffuseColor += LightingLambert(attenuatedLightColor, light.direction, water.diffuseNormal);

			#ifndef _SPECULARHIGHLIGHTS_OFF
			//Note: View direction fetched again using the function that takes orthographic projection into account
			surfaceData.specular += SpecularReflection(light, normalize(GetWorldSpaceViewDir(inputData.positionWS)), water.waveNormal, water.tangentWorldNormal, _PointSpotLightReflectionDistortion, lerp(4096, 64, _PointSpotLightReflectionSize), specularPower);
		#endif
	}
	LIGHT_LOOP_END
#endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX //Previous calculated in vertex stage
	diffuseColor += inputData.vertexLighting;
#endif

#else //Unlit
	const half3 diffuseColor = 1;
#endif

	#if _CAUSTICS
	surfaceData.emission.rgb += water.caustics * causticsAttentuation;
	#endif

	float3 color = (surfaceData.albedo.rgb * diffuseColor) + surfaceData.emission.rgb + surfaceData.specular;
	
	#ifndef _ENVIRONMENTREFLECTIONS_OFF
	//Reflections blend in on top of everything
	color = lerp(color, water.reflections.rgb, water.reflectionMask * water.reflectionLighting);
	sceneColor = lerp(sceneColor, water.reflections.rgb, water.reflectionMask * water.reflectionLighting);
	#endif

	#if _REFRACTION
	//Ensure the same effects are applied to the underwater scene color. Otherwise not visible on clear water
	sceneColor += surfaceData.emission.rgb + surfaceData.specular;
	#endif
	
	//Debug
	//return float4(surfaceData.emission.rgb, 1.0);	

	
	return color;
}