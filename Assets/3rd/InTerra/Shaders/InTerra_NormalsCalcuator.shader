Shader "Hidden/InTerra/CalculateNormal"
{
    Properties
    { 
        _TerrainHeightmapTexture("Texture", 2D) = "red" {}
        _HeightmapScale("hs", Vector) = (0,0,0)
    }

    SubShader
    {
        PackageRequirements { "com.unity.render-pipelines.universal":"[12.1,15.1.3]" }
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"   

            TEXTURE2D(_TerrainHeightmapTexture);    SAMPLER(sampler_TerrainHeightmapTexture);
            float4 _TerrainHeightmapTexture_TexelSize;
            float3 _HeightmapScale;

            struct Attributes
            {
                float4 positionOS   : POSITION; 
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv : TEXCOORD0;
            };            

            Varyings vert(Attributes IN)
            {
                Varyings OUT; 
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.texcoord;
                return OUT;
            }
           
            half4 frag(Varyings IN) : SV_Target
            {
                float2 hmUV = IN.uv;
                float hm = UnpackHeightmap(SAMPLE_TEXTURE2D_LOD(_TerrainHeightmapTexture, sampler_TerrainHeightmapTexture, hmUV, 0)).r;
                float2 ts = float2(_TerrainHeightmapTexture_TexelSize.x, _TerrainHeightmapTexture_TexelSize.y);
                float hsX = _HeightmapScale.y / _HeightmapScale.x;
                float hsZ = _HeightmapScale.y / _HeightmapScale.z;

                float height[4];
                float3 norm;

                height[0] = UnpackHeightmap(SAMPLE_TEXTURE2D_LOD(_TerrainHeightmapTexture, sampler_TerrainHeightmapTexture, hmUV + float2(ts * float2(0, -1)), 0)).r * hsZ;
                height[1] = UnpackHeightmap(SAMPLE_TEXTURE2D_LOD(_TerrainHeightmapTexture, sampler_TerrainHeightmapTexture, hmUV + float2(ts * float2(-1, 0)), 0)).r * hsX;
                height[2] = UnpackHeightmap(SAMPLE_TEXTURE2D_LOD(_TerrainHeightmapTexture, sampler_TerrainHeightmapTexture, hmUV + float2(ts * float2(1, 0)), 0)).r * hsX;
                height[3] = UnpackHeightmap(SAMPLE_TEXTURE2D_LOD(_TerrainHeightmapTexture, sampler_TerrainHeightmapTexture, hmUV + float2(ts * float2(0, 1)), 0)).r * hsZ;

                norm.x = height[1] - height[2];
                norm.z = height[0] - height[3];
                norm.y = 1;

                return   float4 ((normalize(norm) + 1) / 2, hm);
            }
            ENDHLSL
        }
    }
}
