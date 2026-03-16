using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// BatchRendererGroup 简单示例 - 渲染 1000 个旋转的立方体
/// 使用 BRG 可以高效渲染大量相同材质的物体
/// </summary>
public unsafe class SimpleBRGExample : MonoBehaviour
{
    [Header("渲染设置")]
    public Mesh mesh;

    public Material material;
    public int instanceCount = 1000;

    [Header("动画设置")]
    public float rotationSpeed = 90f;

    public float radius = 50f;

    private BatchRendererGroup _brg;
    private GraphicsBuffer _instanceDataBuffer;
    private BatchID _batchId;
    private BatchMeshID _meshId;
    private BatchMaterialID _materialId;

    // 每个实例的数据: ObjectToWorld 矩阵 (float3x4) + WorldToObject 矩阵 (float3x4)
    private const int MATRIX_SIZE = 48;                // 3 * 4 * 4 bytes
    private const int BUFFER_STRIDE = MATRIX_SIZE * 2; // 两个矩阵

    private NativeArray<float4x4> _matrices;
    private NativeArray<float> _instanceData;

    void Start()
    {
        InitializeBRG();
        CreateInstances();
    }

    void InitializeBRG()
    {
        // 创建 BRG，指定缓冲区大小（单位：float4）
        // 我们需要 instanceCount * (6个float4) 的空间
        int bufferSize = instanceCount * 6;
        _brg = new BatchRendererGroup(this.OnPerformCulling, IntPtr.Zero);

        // 注册 Mesh 和 Material
        _meshId = _brg.RegisterMesh(mesh);
        _materialId = _brg.RegisterMaterial(material);

        // 创建实例数据缓冲区
        _instanceDataBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.Raw,
                bufferSize,
                sizeof(float) * 4 // float4 大小
        );

        // 初始化数据数组
        _matrices = new NativeArray<float4x4>(instanceCount, Allocator.Persistent);
        _instanceData = new NativeArray<float>(instanceCount * 24, Allocator.Persistent); // 6 float4 = 24 floats

        // 创建 Batch
        var batchMetadata = new NativeArray<MetadataValue>(2, Allocator.Temp);

        // ObjectToWorld 矩阵 (float3x4) - 位于偏移 0
        batchMetadata[0] = new MetadataValue
        {
                NameID = Shader.PropertyToID("unity_ObjectToWorld"),
                Value = 0x80000000 | 0, // 最高位为1表示是缓冲区引用，低31位是偏移
        };

        // WorldToObject 矩阵 (float3x4) - 位于偏移 3 (3 个 float4 后)
        batchMetadata[1] = new MetadataValue
        {
                NameID = Shader.PropertyToID("unity_WorldToObject"),
                Value = 0x80000000 | 3, // 偏移 3 个 float4 = 48 bytes
                // float3x4 占用 3 个 float4(12 个 float),所以第二个矩阵从索引 3 开始
        };

        _batchId = _brg.AddBatch(batchMetadata, _instanceDataBuffer.bufferHandle);
        batchMetadata.Dispose();
    }

    void CreateInstances()
    {
        // 初始化所有实例的变换矩阵
        for (int i = 0; i < instanceCount; i++)
        {
            float angle = (i / (float) instanceCount) * math.PI * 2;
            float x = math.cos(angle) * radius;
            float z = math.sin(angle) * radius;

            float3 position = new float3(x, 0, z);
            quaternion rotation = quaternion.Euler(0, angle * Mathf.Rad2Deg, 0);
            float3 scale = new float3(1, 1, 1);

            _matrices[i] = float4x4.TRS(position, rotation, scale);
        }
    }

    void Update()
    {
        UpdateInstanceData();
        UploadDataToGPU();
    }

    void UpdateInstanceData()
    {
        float time = Time.time;

        for (int i = 0; i < instanceCount; i++)
        {
            // 更新旋转
            float angle = (i / (float) instanceCount) * math.PI * 2;
            float x = math.cos(angle + time * 0.5f) * radius;
            float z = math.sin(angle + time * 0.5f) * radius;

            float3 position = new float3(x, math.sin(time + i * 0.1f) * 5f, z);
            quaternion rotation = quaternion.Euler(
                    time * rotationSpeed,
                    angle * Mathf.Rad2Deg + time * rotationSpeed,
                    0
            );

            _matrices[i] = float4x4.TRS(position, rotation, new float3(1, 1, 1));

            // 将矩阵转换为 GPU 格式 (float3x4)
            // 并填充到 instanceData 数组
            WriteMatrixToBuffer(i, _matrices[i]);
        }
    }

    void WriteMatrixToBuffer(int index, float4x4 matrix)
    {
        int baseIndex = index * 24; // 每个实例 24 个 float (6 float4)

        // ObjectToWorld (float3x4) - 转置存储，GPU 按列读取
        // 第 0 列
        _instanceData[baseIndex + 0] = matrix.c0.x;
        _instanceData[baseIndex + 1] = matrix.c1.x;
        _instanceData[baseIndex + 2] = matrix.c2.x;
        _instanceData[baseIndex + 3] = matrix.c3.x;

        // 第 1 列
        _instanceData[baseIndex + 4] = matrix.c0.y;
        _instanceData[baseIndex + 5] = matrix.c1.y;
        _instanceData[baseIndex + 6] = matrix.c2.y;
        _instanceData[baseIndex + 7] = matrix.c3.y;

        // 第 2 列
        _instanceData[baseIndex + 8] = matrix.c0.z;
        _instanceData[baseIndex + 9] = matrix.c1.z;
        _instanceData[baseIndex + 10] = matrix.c2.z;
        _instanceData[baseIndex + 11] = matrix.c3.z;

        // 计算 WorldToObject (逆矩阵)
        float4x4 inverse = math.inverse(matrix);

        // WorldToObject (float3x4)
        _instanceData[baseIndex + 12] = inverse.c0.x;
        _instanceData[baseIndex + 13] = inverse.c1.x;
        _instanceData[baseIndex + 14] = inverse.c2.x;
        _instanceData[baseIndex + 15] = inverse.c3.x;

        _instanceData[baseIndex + 16] = inverse.c0.y;
        _instanceData[baseIndex + 17] = inverse.c1.y;
        _instanceData[baseIndex + 18] = inverse.c2.y;
        _instanceData[baseIndex + 19] = inverse.c3.y;

        _instanceData[baseIndex + 20] = inverse.c0.z;
        _instanceData[baseIndex + 21] = inverse.c1.z;
        _instanceData[baseIndex + 22] = inverse.c2.z;
        _instanceData[baseIndex + 23] = inverse.c3.z;
    }

    void UploadDataToGPU()
    {
        // 将数据上传到 GPU 缓冲区
        _instanceDataBuffer.SetData(_instanceData);
    }

    // BRG 裁剪回调 - 这里简单返回所有实例都可见
    public JobHandle OnPerformCulling(
            BatchRendererGroup rendererGroup,
            BatchCullingContext cullingContext,
            BatchCullingOutput cullingOutput,
            IntPtr userContext)
    {
        // 分配输出缓冲区
        var drawCommands = new BatchCullingOutputDrawCommands();

        // 所有实例都在一个 sub-mesh 中
        drawCommands.drawRangeCount = 1;
        drawCommands.drawRanges = Malloc<BatchDrawRange>(1);
        drawCommands.drawRanges[0] = new BatchDrawRange
        {
                drawCommandsBegin = 0,
                drawCommandsCount = 1,
                filterSettings = new BatchFilterSettings
                {
                        renderingLayerMask = 1,
                        layer = 0,
                        motionMode = MotionVectorGenerationMode.Object,
                        shadowCastingMode = ShadowCastingMode.On,
                        receiveShadows = true,
                        staticShadowCaster = false,
                        allDepthSorted = false
                }
        };

        // 单个绘制命令
        drawCommands.drawCommandCount = 1;
        drawCommands.drawCommands = Malloc<BatchDrawCommand>(1);
        drawCommands.drawCommands[0] = new BatchDrawCommand
        {
                visibleOffset = 0,
                visibleCount = (uint) instanceCount,
                batchID = _batchId,
                materialID = _materialId,
                meshID = _meshId,
                submeshIndex = 0,
                splitVisibilityMask = 0xff,
                flags = BatchDrawCommandFlags.None,
                sortingPosition = 0
        };

        // 可见性列表 - 所有实例都可见 (0 到 instanceCount-1)
        drawCommands.visibleInstanceCount = instanceCount;
        drawCommands.visibleInstances = Malloc<int>(instanceCount);
        for (int i = 0; i < instanceCount; i++)
        {
            drawCommands.visibleInstances[i] = i;
        }

        // 设置输出
        cullingOutput.drawCommands[0] = drawCommands;

        return new JobHandle();
    }

    private static unsafe T* Malloc<T>(int count) where T : unmanaged
    {
        return (T*) UnsafeUtility.Malloc(
                sizeof(T) * count,
                UnsafeUtility.AlignOf<T>(),
                Allocator.TempJob
        );
    }

    void OnDestroy()
    {
        if (_brg != null)
        {
            _brg.RemoveBatch(_batchId);
            _brg.UnregisterMesh(_meshId);
            _brg.UnregisterMaterial(_materialId);
            _brg.Dispose();
        }

        _instanceDataBuffer?.Release();

        if (_matrices.IsCreated) _matrices.Dispose();
        if (_instanceData.IsCreated) _instanceData.Dispose();
    }
}