/* Configuration: UnityFog */

//Set this to value to 1 through Shader.SetGlobalFloat to temporarily disable fog for water
float _WaterFogDisabled;

//Authors of third-party fog solutions can reach out to have their method integrated here

#ifdef SCPostEffects
//Macros normally used for cross-RP compatibility
#define LINEAR_DEPTH(depth) Linear01Depth(depth, _ZBufferParams)
#endif

//Executed in vertex stage
float CalculateFogFactor(float3 positionCS) {
	return ComputeFogFactor(positionCS.z);
}

//Fragment stage. Note: Screen position passed here is not normalized (divided by w-component)
void ApplyFog(inout float3 color, float fogFactor, float4 screenPos, float3 positionWS, float vFace) 
{
	float3 foggedColor = color;
	
#ifdef UnityFog
	foggedColor = MixFog(color.rgb, fogFactor);
#endif

#ifdef Colorful
	if(_DensityParams.x > 0) foggedColor.rgb = ApplyFog(color.rgb, fogFactor, positionWS, screenPos.xy / screenPos.w);
#endif
	
#ifdef Enviro
	foggedColor.rgb = TransparentFog(float4(color.rgb, 1.0), positionWS, screenPos.xy / screenPos.w, fogFactor).rgb;
#endif

#ifdef Enviro3
	foggedColor.rgb = ApplyFogAndVolumetricLights(color.rgb, screenPos.xy / screenPos.w, positionWS, 0);
#endif
	
#ifdef Azure
	foggedColor.rgb = ApplyAzureFog(float4(color.rgb, 1.0), positionWS).rgb;
#endif

#ifdef AtmosphericHeightFog
	float4 fogParams = GetAtmosphericHeightFog(positionWS.xyz);
	foggedColor.rgb = lerp(color.rgb, fogParams.rgb, fogParams.a);
#endif

#ifdef SCPostEffects
	//Distance or height fog enabled
	if(_DistanceParams.z == 1 || _DistanceParams.w == 1)
	{
		//The screen position input is used when the fog color source is set to "Skybox". If never in use, you can set it to 0
		float4 fogColor = ComputeTransparentFog(positionWS, screenPos.xy);

		//The alpha channel will hold the density of the fog, use it as a lerp factor
		foggedColor.rgb = lerp(fogColor.rgb, foggedColor.rgb, fogColor.a);
	}
#endif

#ifdef COZY
	foggedColor = BlendStylizedFog(positionWS, float4(color.rgb, 1.0)).rgb;
#endif

#ifdef Buto
	foggedColor = ButoFogBlend(screenPos.xy / screenPos.w, color.rgb);
#endif

	#ifndef UnityFog
	//Allow fog to be disabled for water globally by setting the value through script
	foggedColor = lerp(foggedColor, color, _WaterFogDisabled);
	#endif
	
	//Fog only applies to the front faces, otherwise affects underwater rendering
	color.rgb = lerp(color.rgb, foggedColor.rgb, vFace);
}
