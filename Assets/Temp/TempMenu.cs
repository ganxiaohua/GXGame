using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public static class TempMenu 
{

    [MenuItem("TempMenu/So")]
    public static unsafe void CreateMenuSO()
    {
       //  TestData a= ScriptableObject.CreateInstance<TestData>();
       // var data = TestDataEditor.CreateData();
       // data[3] = new TestDataInput() {Data1 = new Vector3(100, 100, 100)};
       // a.TestDataInputs = data;
       // AssetDatabase.CreateAsset(a, "Assets/Temp/SO/SoData.asset");
       // int sizeInBytes = data.Length * sizeof(TestDataInput);
       // var sbytes = StructArrayToBytes(data);
       //  File.WriteAllBytes("Assets/Temp/SO/SoData.bytes",sbytes);
       //  AssetDatabase.Refresh();

        var bytes = BytesToStructArray("Assets/Temp/SO/SoData.bytes");
    }
    
    public static unsafe byte[] StructArrayToBytes(TestDataInput[] structArray)
    {
        int structSize = Marshal.SizeOf<TestDataInput>();
        byte[] byteArray = new byte[structArray.Length * structSize];

        fixed (byte* ptr = byteArray)
        {
            for (int i = 0; i < structArray.Length; i++)
            {
                // 将每个结构体复制到字节数组中
                Marshal.StructureToPtr(structArray[i], new IntPtr(ptr + i * structSize), false);
            }
        }
        return byteArray;
    }
    
    public static unsafe TestDataInput[] BytesToStructArray(string filePath)
    {
    
        byte[] byteArray = File.ReadAllBytes(filePath);


        int structSize = Marshal.SizeOf<TestDataInput>();
        int structCount = byteArray.Length / structSize;


        TestDataInput[] structArray = new TestDataInput[structCount];

 
        fixed (byte* ptr = byteArray)
        {
            for (int i = 0; i < structCount; i++)
            {
                IntPtr structPtr = new IntPtr(ptr + i * structSize);
                structArray[i] = Marshal.PtrToStructure<TestDataInput>(structPtr);
            }
        }

        return structArray;
    }
}
