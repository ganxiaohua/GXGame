//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

TEXTURE2D(_IntersectionNoise);
SAMPLER(sampler_IntersectionNoise);
TEXTURE2D(_DepthTex);


CBUFFER_START(UnityPerMaterial)
	float4 _ShallowColor;
	float4 _BaseColor;

	//float _Smoothness;
	//float _Metallic;

	float4 _IntersectionColor;
	float _DepthVertical;
	float _DepthHorizontal;
	float _DepthExp;
	float _WorldSpaceUV;
	float _NormalTiling;
	float _NormalSpeed;
	half _NormalStrength;

	half2 _DistanceNormalsFadeDist;
	half _DistanceNormalsTiling;

	half _TranslucencyStrength;
	half _TranslucencyExp;
	half _TranslucencyCurvatureMask;
	half _TranslucencyReflectionMask;
	half _EdgeFade;
	float _WaveSpeed;
	float4 _HorizonColor;
	half _HorizonDistance;
	float _SparkleIntensity;
	half _SparkleSize;
	half _SunReflectionDistortion;
	half _SunReflectionSize;
	float _SunReflectionStrength;
	float _PointSpotLightReflectionStrength;
	half _PointSpotLightReflectionSize;
	half _PointSpotLightReflectionDistortion;
	float _ReflectionDistortion;
	float _ReflectionBlur;
	float _ReflectionFresnel;
	float _ReflectionStrength;
	half _ReflectionLighting;
	half _PlanarReflectionsEnabled;
	half _ShadowStrength;
	float2 _Direction;
	float _Speed;

	half _SlopeStretching;
	half _SlopeSpeed;
	half _SlopeThreshold;
	half _SlopeFoam;

	//Foam
	float _FoamTiling;
	float4 _FoamColor;
	float _FoamSpeed;
	half _FoamSize;
	half _FoamWaveMask;
	half _FoamWaveMaskExp;

	//Intersection
	half _IntersectionSource;
	half _IntersectionLength;
	half _IntersectionFalloff;
	half _IntersectionTiling;
	half _IntersectionRippleDist;
	half _IntersectionRippleStrength;
	half _IntersectionClipping;
	float _IntersectionSpeed;

	//Waves
	half _WaveHeight;
	half _WaveNormalStr;
	float _WaveDistance;
	half2 _WaveFadeDistance;
	float _WaveSteepness;
	uint _WaveCount;
	half4 _WaveDirection;

	half _ShoreLineWaveStr;
	half _ShoreLineWaveDistance;
	half _ShoreLineLength;

	//Underwater
	half _CausticsBrightness;
	float _CausticsTiling;
	half _CausticsSpeed;
	half _RefractionStrength;
	half _CausticsDistortion;

	half _UnderwaterSurfaceSmoothness;
	half _UnderwaterRefractionOffset;

	half _VertexColorDepth;
	half _VertexColorWaveFlattening;
	half _VertexColorFoam;

	half _WaveTint;
#ifdef TESSELLATION_ON	
	float _TessValue;
	float _TessMin;
	float _TessMax;
#endif
CBUFFER_END