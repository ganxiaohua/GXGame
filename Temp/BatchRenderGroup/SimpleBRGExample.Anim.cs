using System;
using System.Collections.Generic;
using System.Security.Claims;
using Cysharp.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public partial class SimpleBRGExample : MonoBehaviour
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

    class ClipData
    {
        public string path; //这里运动的部位
        public AnimationCurve[] curve; //这里是这个部位 pos xyz rot xyzw scale xyz 的数据, 下标就是types
    }


    public SkinnedMeshRenderer renderers;
    public AnimationClip clip;
    public Transform Root;
    private Transform[] bones;
    private Transform rootBones;
    private NativeArray<Matrix4x4> bindPose;
    private Vector3[] vertices;

    private BoneWeight[] boneWeights;

    // private Matrix4x4[] bindPose;
    private float[] trsData;
    private ClipData[] bonesData;


    void AnimatorStart()
    {
        vertices = mesh.vertices;
        boneWeights = mesh.boneWeights;
        bones = renderers.bones;
        rootBones = renderers.rootBone ? renderers.rootBone : transform;
        bonesData = new ClipData[bones.Length];
        trsData = new float[(int) Types.Count];
        bindPose = new NativeArray<Matrix4x4>(mesh.bindposes.Length*kNumInstances, Allocator.Persistent);
        for (int i = 0; i < kNumInstances*mesh.bindposes.Length; i++)
        {
            bindPose[i] = mesh.bindposes[i % mesh.bindposes.Length];
        }
        Prepare();
        SetUV12();
    }


    private void SetUV12()
    {
        Vector4[] uv1 = new Vector4[vertices.Length];
        Vector4[] uv2 = new Vector4[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            ref var bw = ref boneWeights[i];
            uv1[i] = new Vector4(bw.boneIndex0, bw.weight0, bw.boneIndex1, bw.weight1);
            uv2[i] = new Vector4(bw.boneIndex2, bw.weight2, bw.boneIndex3, bw.weight3);
        }

        mesh.SetUVs(1, uv1);
        mesh.SetUVs(2, uv2);
    }

    /// <summary>
    /// 准备阶段,在编辑模式下制作数据.
    /// </summary>
    private void Prepare()
    {
        Dictionary<string, Transform> nameDic = new();
        Dictionary<string, ClipData> values = new();

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
                data = new ClipData();
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

    private void BoneCalculate()
    {
        float time = Time.realtimeSinceStartup % clip.length;
        Matrix4x4 worldToRoot = rootBones.worldToLocalMatrix;
        int count = bones.Length * kNumInstances;
        NativeArray<Vector3> pos = new NativeArray<Vector3>(count, Allocator.TempJob);
        NativeArray<Quaternion> rot = new NativeArray<Quaternion>(count, Allocator.TempJob);
        NativeArray<Vector3> scale = new NativeArray<Vector3>(count, Allocator.TempJob);
        NativeArray<Matrix4x4> skinMartixs = new NativeArray<Matrix4x4>(count, Allocator.TempJob);
        NativeArray<Matrix4x4> structuredSkinMartixs = new NativeArray<Matrix4x4>(count, Allocator.TempJob);
        //设置骨骼矩阵
        for (int z = 0; z < kNumInstances; z++)
        {
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

                pos[z * kNumInstances + i] = new Vector3(trsData[0], trsData[1], trsData[2]);
                rot[z * kNumInstances + i] = new Quaternion(trsData[3], trsData[4], trsData[5], trsData[6]);
                scale[z * kNumInstances + i] = new Vector3(trsData[7], trsData[8], trsData[9]);
            }
        }

        var job = new Matrix4x4Calculate
        {
            skinMartixs = skinMartixs,
            worldToRoot = worldToRoot,
            bindPose = bindPose,
            pos = pos,
            rot = rot,
            scale = scale,
        };
        var jobHandle = job.Schedule(count, 128);
        jobHandle.Complete();

        int boneLength = bones.Length;
        for (int i = 0; i < skinMartixs.Length; i++)
        {
            int manIndex = i / boneLength;
            int boneindex = i % boneLength;
            structuredSkinMartixs[boneindex * kNumInstances + manIndex] = skinMartixs[i];
        }
        
        uint byteSkinMartixs = byteAddressColor + kSizeOfFloat4 * (uint) kNumInstances;
        for (int i = 0; i < structuredSkinMartixs.Length; i += kNumInstances)
        {
            m_InstanceData.SetData(structuredSkinMartixs, i, (int) (byteSkinMartixs / kSizeOfMatrix), kNumInstances);
            byteSkinMartixs += (uint) (kSizeOfMatrix * kNumInstances);
        }

        pos.Dispose();
        rot.Dispose();
        scale.Dispose();
        skinMartixs.Dispose();
        structuredSkinMartixs.Dispose();
    }

    void AnimationUpdate()
    {
        BoneCalculate();
    }

    
    [BurstCompile]
    partial struct Matrix4x4Calculate : IJobParallelFor
    {
        public NativeArray<Matrix4x4> skinMartixs;
        public Matrix4x4 worldToRoot;
        public NativeArray<Matrix4x4> bindPose;
        public NativeArray<Vector3> pos;
        public NativeArray<Vector3> scale;
        public NativeArray<Quaternion> rot;

        [BurstCompile]
        public void Execute(int index)
        {
            Matrix4x4 bonesMatrix = Matrix4x4.Translate(pos[index]) * Matrix4x4.Rotate(rot[index]) * Matrix4x4.Scale(scale[index]);
            skinMartixs[index] = worldToRoot * bonesMatrix * bindPose[index ];
        }
    }
}