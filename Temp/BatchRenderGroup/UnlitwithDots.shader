Shader "Universal Render Pipeline/Custom/UnlitWithDotsInstancing"
{
    Properties
    {
        _BaseMap ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Colour", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline" "Queue"="Geometry"
        }

        Pass
        {
            Name "Forward"
            Tags
            {
                "LightMode"="UniversalForward"
            }

            Cull Back

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5
            #pragma vertex UnlitPassVertex
            #pragma fragment UnlitPassFragment
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile  DOTS_INSTANCING_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 uv1 :TEXCOORD1;
                float4 uv2 :TEXCOORD2;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 uv1 :TEXCOORD1;
                float4 uv2 :TEXCOORD2;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4x4 _Bone1;
                float4x4 _Bone2;
                float4x4 _Bone3;
                float4x4 _Bone4;
                float4x4 _Bone5;
                float4x4 _Bone6;
                float4x4 _Bone7;
                float4x4 _Bone8;
                float4x4 _Bone9;
                float4x4 _Bone10;
                float4x4 _Bone11;
                float4x4 _Bone12;
                float4x4 _Bone13;
                float4x4 _Bone14;
                float4x4 _Bone15;
                float4x4 _Bone16;
                float4x4 _Bone17;
                float4x4 _Bone18;
                float4x4 _Bone19;
                float4x4 _Bone20;
                float4x4 _Bone21;
                float4x4 _Bone22;
                float4x4 _Bone23;
                float4x4 _Bone24;
                float4x4 _Bone25;
                float4x4 _Bone26;
                float4x4 _Bone27;
                float4x4 _Bone28;
                float4x4 _Bone29;
                float4x4 _Bone30;
                float4x4 _Bone31;
                float4x4 _Bone32;
                float4x4 _Bone33;
                float4x4 _Bone34;
                float4x4 _Bone35;
                float4x4 _Bone36;
                float4x4 _Bone37;
                float4x4 _Bone38;
                float4x4 _Bone39;
                float4x4 _Bone40;
                float4x4 _Bone41;
                float4x4 _Bone42;
                float4x4 _Bone43;
                float4x4 _Bone44;
                float4x4 _Bone45;
                float4x4 _Bone46;
                float4x4 _Bone47;
                float4x4 _Bone48;
                float4x4 _Bone49;
                float4x4 _Bone50;
                float4x4 _Bone51;
                float4x4 _Bone52;
                float4x4 _Bone53;
                float4x4 _Bone54;
                float4x4 _Bone55;
                float4x4 _Bone56;
                float4x4 _Bone57;
                float4x4 _Bone58;
                float4x4 _Bone59;
                float4x4 _Bone60;
                float4x4 _Bone61;
                float4x4 _Bone62;
                float4x4 _Bone63;
                float4x4 _Bone64;
                float4x4 _Bone65;
            CBUFFER_END

            #ifdef UNITY_DOTS_INSTANCING_ENABLED
            UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
                UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone1)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone2)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone3)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone4)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone5)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone6)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone7)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone8)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone9)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone10)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone11)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone12)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone13)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone14)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone15)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone16)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone17)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone18)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone19)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone20)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone21)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone22)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone23)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone24)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone25)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone26)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone27)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone28)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone29)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone30)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone31)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone32)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone33)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone34)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone35)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone36)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone37)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone38)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone39)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone40)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone41)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone42)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone43)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone44)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone45)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone46)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone47)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone48)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone49)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone50)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone51)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone52)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone53)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone54)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone55)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone56)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone57)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone58)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone59)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone60)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone61)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone62)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone63)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone64)
                UNITY_DOTS_INSTANCED_PROP(float4x4, _Bone65)
            UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)


            #define _BaseColor UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _BaseColor)
            #define _Bone1 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone1)
            #define _Bone2 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone2)
            #define _Bone3 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone3)
            #define _Bone4 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone4)
            #define _Bone5 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone5)
            #define _Bone6 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone6)
            #define _Bone7 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone7)
            #define _Bone8 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone8)
            #define _Bone9 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone9)
            #define _Bone10 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone10)
            #define _Bone11 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone11)
            #define _Bone12 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone12)
            #define _Bone13 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone13)
            #define _Bone14 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone14)
            #define _Bone15 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone15)
            #define _Bone16 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone16)
            #define _Bone17 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone17)
            #define _Bone18 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone18)
            #define _Bone19 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone19)
            #define _Bone20 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone20)
            #define _Bone21 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone21)
            #define _Bone22 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone22)
            #define _Bone23 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone23)
            #define _Bone24 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone24)
            #define _Bone25 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone25)
            #define _Bone26 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone26)
            #define _Bone27 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone27)
            #define _Bone28 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone28)
            #define _Bone29 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone29)
            #define _Bone30 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone30)
            #define _Bone31 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone31)
            #define _Bone32 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone32)
            #define _Bone33 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone33)
            #define _Bone34 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone34)
            #define _Bone35 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone35)
            #define _Bone36 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone36)
            #define _Bone37 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone37)
            #define _Bone38 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone38)
            #define _Bone39 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone39)
            #define _Bone40 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone40)
            #define _Bone41 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone41)
            #define _Bone42 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone42)
            #define _Bone43 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone43)
            #define _Bone44 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone44)
            #define _Bone45 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone45)
            #define _Bone46 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone46)
            #define _Bone47 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone47)
            #define _Bone48 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone48)
            #define _Bone49 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone49)
            #define _Bone50 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone50)
            #define _Bone51 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone51)
            #define _Bone52 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone52)
            #define _Bone53 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone53)
            #define _Bone54 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone54)
            #define _Bone55 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone55)
            #define _Bone56 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone56)
            #define _Bone57 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone57)
            #define _Bone58 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone58)
            #define _Bone59 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone59)
            #define _Bone60 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone60)
            #define _Bone61 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone61)
            #define _Bone62 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone62)
            #define _Bone63 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone63)
            #define _Bone64 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone64)
            #define _Bone65 UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4x4, _Bone65)
            #endif


            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);


            float4x4 LoadDOTSInstancedDataOverridden_float4x4(uint metadata)
            {
                uint address = ComputeDOTSInstanceDataAddressOverridden(metadata, 4 * 16);
                float4 p1 = asfloat(DOTSInstanceData_Load4(address + 0 * 16));
                float4 p2 = asfloat(DOTSInstanceData_Load4(address + 1 * 16));
                float4 p3 = asfloat(DOTSInstanceData_Load4(address + 2 * 16));
                float4 p4 = asfloat(DOTSInstanceData_Load4(address + 3 * 16));
                return float4x4(
                    p1.x, p2.x, p3.x, p4.x,
                    p1.y, p2.y, p3.y, p4.y,
                    p1.z, p2.z, p3.z, p4.z,
                    p1.w, p2.w, p3.w, p4.w);
            }

            //矩阵乘float
            float4x4 MatrixWeight(float4x4 matrix4X4, float weight)
            {
                return matrix4X4 * weight;
            }

            //矩阵相加
            float4x4 MatrixAdd(float4x4 a, float4x4 b)
            {
                return a + b;
            }

            float4 MultiplyPoint3x4(float4x4 boneSkine, float3 vPos)
            {
                float4 vector3;
                vector3.x = (boneSkine[0][0] * vPos.x + boneSkine[0][1] * vPos.y + boneSkine[0][2] * vPos.z) + boneSkine
                    [0][3];
                vector3.y = (boneSkine[1][0] * vPos.x + boneSkine[1][1] * vPos.y + boneSkine[1][2] * vPos.z) + boneSkine
                    [1][3];
                vector3.z = (boneSkine[2][0] * vPos.x + boneSkine[2][1] * vPos.y + boneSkine[2][2] * vPos.z) + boneSkine
                    [2][3];
                return vector3;
            }

            float4 Transform(Attributes input)
            {
                float4x4 Bones[65];
                Bones[0] = _Bone1;
                Bones[1] = _Bone2;
                Bones[2] = _Bone3;
                Bones[3] = _Bone4;
                Bones[4] = _Bone5;
                Bones[5] = _Bone6;
                Bones[6] = _Bone7;
                Bones[7] = _Bone8;
                Bones[8] = _Bone9;
                Bones[9] = _Bone10;
                Bones[10] = _Bone11;
                Bones[11] = _Bone12;
                Bones[12] = _Bone13;
                Bones[13] = _Bone14;
                Bones[14] = _Bone15;
                Bones[15] = _Bone16;
                Bones[16] = _Bone17;
                Bones[17] = _Bone18;
                Bones[18] = _Bone19;
                Bones[19] = _Bone20;
                Bones[20] = _Bone21;
                Bones[21] = _Bone22;
                Bones[22] = _Bone23;
                Bones[23] = _Bone24;
                Bones[24] = _Bone25;
                Bones[25] = _Bone26;
                Bones[26] = _Bone27;
                Bones[27] = _Bone28;
                Bones[28] = _Bone29;
                Bones[29] = _Bone30;
                Bones[30] = _Bone31;
                Bones[31] = _Bone32;
                Bones[32] = _Bone33;
                Bones[33] = _Bone34;
                Bones[34] = _Bone35;
                Bones[35] = _Bone36;
                Bones[36] = _Bone37;
                Bones[37] = _Bone38;
                Bones[38] = _Bone39;
                Bones[39] = _Bone40;
                Bones[40] = _Bone41;
                Bones[41] = _Bone42;
                Bones[42] = _Bone43;
                Bones[43] = _Bone44;
                Bones[44] = _Bone45;
                Bones[45] = _Bone46;
                Bones[46] = _Bone47;
                Bones[47] = _Bone48;
                Bones[48] = _Bone49;
                Bones[49] = _Bone50;
                Bones[50] = _Bone51;
                Bones[51] = _Bone52;
                Bones[52] = _Bone53;
                Bones[53] = _Bone54;
                Bones[54] = _Bone55;
                Bones[55] = _Bone56;
                Bones[56] = _Bone57;
                Bones[57] = _Bone58;
                Bones[58] = _Bone59;
                Bones[59] = _Bone60;
                Bones[60] = _Bone61;
                Bones[61] = _Bone62;
                Bones[62] = _Bone63;
                Bones[63] = _Bone64;
                Bones[64] = _Bone65;
                half boneIndex0 = input.uv1[0];
                half boneWeight0 = input.uv1[1];

                float4x4 Bone = Bones[boneIndex0];
                float4x4 bianhuan = MatrixWeight(Bone, boneWeight0);
                return MultiplyPoint3x4(bianhuan, input.positionOS);
            }


            Varyings UnlitPassVertex(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                // const VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                const VertexPositionInputs positionInputs = GetVertexPositionInputs(Transform(input).xyz);
                output.positionCS = positionInputs.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.color = input.color;
                return output;
            }

            half4 UnlitPassFragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                return _BaseColor;
            }
            ENDHLSL
        }
    }
}