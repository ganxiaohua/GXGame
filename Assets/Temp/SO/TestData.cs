using System;
using UnityEngine;


[Serializable]
public struct TestDataInput
{
    public Vector3 Data1;
}

[CreateAssetMenu(menuName = "Temp/SO",fileName = "SO")]
public class TestData : ScriptableObject
{
    [SerializeField]
    public TestDataInput[] TestDataInputs;
}

public static class TestDataEditor
{
    public static TestDataInput[] CreateData()
    {
        TestDataInput[] data = new TestDataInput[100000];
        return data;
    }
}