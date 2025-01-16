using System;
using System.Collections.Generic;

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

public class SkinRenderer : MonoBehaviour
{
    enum Types : uint
    {
        localPosx = 0,
        localPosy,
        localPosz,
        localRotx,
        localRoty,
        localRotz,
        localRotw,
        localscalex,
        localscaley,
        localscalez,
        Count,
    }

    class Data
    {
        public string path;
        public AnimationCurve[] curve;
    }

    public SkinnedMeshRenderer renderers;
    public AnimationClip clip;
    public MeshFilter meshFilter;
    public Transform Root;
    private Mesh mesh;
    private Material mat;
    private Transform[] bones;
    private Transform rootBones;
    private Vector3[] vertices;
    private BoneWeight[] boneWeights;
    private Matrix4x4[] bindPose;
    private Matrix4x4[] skinMartixs;
    private float[] trsData;
    public SkinQuality skinQuailty = SkinQuality.Bone1;
    private Data[] bonesData;
    private NativeArray<BoneWeight> boneWeightsNative;
    private NativeArray<Vector3> verticesNativeArray;
    private NativeArray<Vector3> outVerticesArray;

    void Start()
    {
        mesh = Object.Instantiate(renderers.sharedMesh);
        mesh.MarkDynamic();
        meshFilter.mesh = mesh;
        mesh.name = "Mesh";
        vertices = mesh.vertices;
        bindPose = mesh.bindposes;
        boneWeights = mesh.boneWeights;
        mat = renderers.sharedMaterial;
        bones = renderers.bones;
        skinMartixs = new Matrix4x4[bones.Length];
        rootBones = renderers.rootBone ? renderers.rootBone : transform;
        bonesData = new Data[bones.Length];
        trsData = new float[(int) Types.Count];
        boneWeightsNative = new NativeArray<BoneWeight>(boneWeights, Allocator.Persistent);
        verticesNativeArray = new NativeArray<Vector3>(vertices, Allocator.Persistent);
        outVerticesArray = new NativeArray<Vector3>(vertices.Length, Allocator.Persistent);
        Prepare();
    }

    /// <summary>
    /// 准备阶段,在编辑模式下制作数据.
    /// </summary>
    private void Prepare()
    {
        Dictionary<string, Transform> nameDic = new();
        Dictionary<string, Data> values = new();

        Stack<string> stack = new Stack<string>();
        foreach (var bone in bones)
        {
            Transform tempBone = bone;
            if (renderers.rootBone != tempBone)
            {
                stack.Clear();
                string path = "";
                stack.Push(tempBone.name);
                while (tempBone.parent != Root)
                {
                    tempBone = tempBone.parent;
                    stack.Push(tempBone.name);
                }

                foreach (var name in stack)
                {
                    path += $"{name}/";
                }

                nameDic.Add(path.Substring(0, path.Length - 1), bone);
            }
            else
            {
                nameDic.Add(tempBone.name, bone);
            }
        }

        var bingings = AnimationUtility.GetCurveBindings(clip);
        foreach (var bing in bingings)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, bing);
            if (!values.TryGetValue(bing.path, out var data))
            {
                data = new Data();
                data.curve = new AnimationCurve[(int) Types.Count];
                data.path = bing.path;
                values.Add(bing.path, data);
            }

            if (bing.propertyName.Contains("LocalRotation.x"))
            {
                data.curve[(int) Types.localRotx] = curve;
            }
            else if (bing.propertyName.Contains("LocalRotation.y"))
            {
                data.curve[(int) Types.localRoty] = curve;
            }
            else if (bing.propertyName.Contains("LocalRotation.z"))
            {
                data.curve[(int) Types.localRotz] = curve;
            }
            else if (bing.propertyName.Contains("LocalRotation.w"))
            {
                data.curve[(int) Types.localRotw] = curve;
            }

            else if (bing.propertyName.Contains("LocalPosition.x"))
            {
                data.curve[(int) Types.localPosx] = curve;
            }
            else if (bing.propertyName.Contains("LocalPosition.y"))
            {
                data.curve[(int) Types.localPosy] = curve;
            }
            else if (bing.propertyName.Contains("LocalPosition.z"))
            {
                data.curve[(int) Types.localPosz] = curve;
            }

            else if (bing.propertyName.Contains("LocalScale.x"))
            {
                data.curve[(int) Types.localscalex] = curve;
            }
            else if (bing.propertyName.Contains("LocalScale.y"))
            {
                data.curve[(int) Types.localscaley] = curve;
            }
            else if (bing.propertyName.Contains("LocalScale.z"))
            {
                data.curve[(int) Types.localscalez] = curve;
            }
        }

        Dictionary<Transform, int> boneIndex = new();

        for (int i = 0; i < bones.Length; i++)
        {
            boneIndex.Add(bones[i], i);
        }

        foreach (var value in values)
        {
            Transform bone = nameDic[value.Key];
            bonesData[boneIndex[bone]] = value.Value;
        }
    }

    private void BoneCalculate(float time)
    {
        Matrix4x4 worldToRoot = rootBones.worldToLocalMatrix;
        //设置骨骼矩阵
        for (int i = 0; i < bonesData.Length; i++)
        {
            var bonesdata = bonesData[i];
            if (bonesdata == null)
                continue;
            var bone = bones[i];

            trsData[0] = bone.localPosition.x;
            trsData[1] = bone.localPosition.y;
            trsData[2] = bone.localPosition.z;
            trsData[3] = bone.localRotation.x;
            trsData[4] = bone.localRotation.y;
            trsData[5] = bone.localRotation.z;
            trsData[6] = bone.localRotation.w;
            trsData[7] = bone.localScale.x;
            trsData[8] = bone.localScale.y;
            trsData[9] = bone.localScale.z;
            for (int j = 0; j < bonesdata.curve.Length; j++)
            {
                if (bonesdata.curve[j] == null)
                    continue;
                float value = bonesdata.curve[j].Evaluate(time);
                trsData[j] = value;
            }

            bone.localPosition = new Vector3(trsData[0], trsData[1], trsData[2]);
            bone.localRotation = new Quaternion(trsData[3], trsData[4], trsData[5], trsData[6]);
            bone.localScale = new Vector3(trsData[7], trsData[8], trsData[9]);
        }
        for (int i = 0; i < bones.Length; i++)
        {
            skinMartixs[i] = GetSkinMatrix(ref worldToRoot, bones, i);
        }

        NativeArray<Matrix4x4> skinMartix0Native = new NativeArray<Matrix4x4>(vertices.Length, Allocator.TempJob);
        NativeArray<Matrix4x4> skinMartix1Native = new NativeArray<Matrix4x4>(vertices.Length, Allocator.TempJob);
        NativeArray<Matrix4x4> skinMartix2Native = new NativeArray<Matrix4x4>(vertices.Length, Allocator.TempJob);
        NativeArray<Matrix4x4> skinMartix3Native = new NativeArray<Matrix4x4>(vertices.Length, Allocator.TempJob);

        for (int i = 0; i < vertices.Length; i++)
        {
            skinMartix0Native[i] = skinMartixs[boneWeights[i].boneIndex0];
            skinMartix1Native[i] = skinMartixs[boneWeights[i].boneIndex1];
            skinMartix2Native[i] = skinMartixs[boneWeights[i].boneIndex2];
            skinMartix3Native[i] = skinMartixs[boneWeights[i].boneIndex3];
        }
        
        var moveJob = new VertexBlend
        {
            SkinMatrix0 = skinMartix0Native,
            SkinMatrix1 = skinMartix1Native,
            SkinMatrix2 = skinMartix2Native,
            SkinMatrix3 = skinMartix3Native,
            boneWeight = boneWeightsNative,
            inVertices = verticesNativeArray,
            outVertices = outVerticesArray,
        };
        
        var sheduleJobHandle = moveJob.Schedule(vertices.Length,128);
        sheduleJobHandle.Complete();
        mesh.vertices = outVerticesArray.ToArray();
        mesh.UploadMeshData(false);
        skinMartix0Native.Dispose();
        skinMartix1Native.Dispose();
        skinMartix2Native.Dispose();
        skinMartix3Native.Dispose();
    }


    void Update()
    {
        BoneCalculate(Time.realtimeSinceStartup % clip.length);
    }

    Matrix4x4 GetSkinMatrix(ref Matrix4x4 worldToRoot, Transform[] bones, int index)
    {
        Transform bone = bones[index];
        //模型空间 ->(乘bindPose) T/A Pose骨骼空间  ->(乘骨骼的LocalToWorld) 动画计算后的世界空间 ->(乘Root的WorldToLocal) root空间
        return worldToRoot * bone.localToWorldMatrix * bindPose[index];
    }

    struct VertexBlend : IJobParallelFor
    {
        public NativeArray<Matrix4x4> SkinMatrix0;
        public NativeArray<Matrix4x4> SkinMatrix1;
        public NativeArray<Matrix4x4> SkinMatrix2;
        public NativeArray<Matrix4x4> SkinMatrix3;
        public NativeArray<BoneWeight> boneWeight;

        public NativeArray<Vector3> inVertices;
        public NativeArray<Vector3> outVertices;

        [BurstCompile]
        public void Execute(int index)
        {
            Matrix4x4 blendSkin = Matrix4x4.identity;
            BoneWeight bw = boneWeight[index];
            //4根骨骼权重混合
            blendSkin = MatrixWeight(SkinMatrix0[index], bw.weight0);
            blendSkin = MatrixAdd(blendSkin, MatrixWeight(SkinMatrix1[index], bw.weight1));
            blendSkin = MatrixAdd(blendSkin, MatrixWeight(SkinMatrix2[index], bw.weight2));
            blendSkin = MatrixAdd(blendSkin, MatrixWeight(SkinMatrix3[index], bw.weight3));
            outVertices[index] = blendSkin.MultiplyPoint3x4(inVertices[index]);
        }

        //矩阵乘float
        Matrix4x4 MatrixWeight(Matrix4x4 matrix4X4, float weight)
        {
            matrix4X4.m00 *= weight;
            matrix4X4.m01 *= weight;
            matrix4X4.m02 *= weight;
            matrix4X4.m03 *= weight;
            matrix4X4.m10 *= weight;
            matrix4X4.m11 *= weight;
            matrix4X4.m12 *= weight;
            matrix4X4.m13 *= weight;
            matrix4X4.m20 *= weight;
            matrix4X4.m21 *= weight;
            matrix4X4.m22 *= weight;
            matrix4X4.m23 *= weight;
            matrix4X4.m30 *= weight;
            matrix4X4.m31 *= weight;
            matrix4X4.m32 *= weight;
            matrix4X4.m33 *= weight;
            return matrix4X4;
        }

        //矩阵相加
        Matrix4x4 MatrixAdd(Matrix4x4 a, Matrix4x4 b)
        {
            a.m00 += b.m00;
            a.m01 += b.m01;
            a.m02 += b.m02;
            a.m03 += b.m03;
            a.m10 += b.m10;
            a.m11 += b.m11;
            a.m12 += b.m12;
            a.m13 += b.m13;
            a.m20 += b.m20;
            a.m21 += b.m21;
            a.m22 += b.m22;
            a.m23 += b.m23;
            a.m30 += b.m30;
            a.m31 += b.m31;
            a.m32 += b.m32;
            a.m33 += b.m33;
            return a;
        }
    }
}