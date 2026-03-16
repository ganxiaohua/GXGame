using System;
using System.Collections.Generic;
using GameFrame.Runtime;
using NUnit.Framework;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public partial class PTests
{
    public class Test
    {
        public Test(int id)
        {
            this.id = id;
        }

        public void OnUpdate(float a, float b)
        {
        }

        public int id;
        public bool show;
    }

    [Test]
    public void ArrayExTest()
    {
        ArrayEx<int> array = new ArrayEx<int>(4, true);
        array[10] = 10;
        Debugger.Log(array[10] + " " + array.Count);
    }

    [Test]
    public unsafe void BulkListTest()
    {
        // var ptr = (byte*) UnsafeUtility.Malloc(1000, 8, Allocator.Persistent);
        // UnsafeUtility.MemClear(ptr, 1000);
    }

    [Test]
    public void StrongList()
    {
        int count = 100000;
        List<CapabilityBase> a = new List<CapabilityBase>(count);
        StrongList<CapabilityBase> b = new StrongList<CapabilityBase>(count);
        JumpIndexArray<CapabilityBase> c = new JumpIndexArray<CapabilityBase>();
        c.Init(count);
        for (int i = 0; i < count; i++)
        {
            var x = new DestroyCapability();
            a.Add(x);
            b.Add(x);
            c.Set(i, x);
        }

        using (new TimeProfiler("List"))
        {
            foreach (var xx in a)
            {
            }
        }

        using (new TimeProfiler("StrongList"))
        {
            foreach (var xx in b)
            {
            }
        }

        using (new TimeProfiler("JumpIndexArray"))
        {
            foreach (var xx in c)
            {
            }
        }
    }

    [Test]
    public void MarkDieList()
    {
        int count = 2;
        MarkDieList<Test> b = new MarkDieList<Test>(count);
        for (int i = 0; i < 10; i++)
        {
            b.Add(new Test(i));
        }

        b.MarkRemoveAt(5);
        b.MarkRemoveAt(6);
        b.MarkRemoveAt(7);
        b.Add(new Test(10));
        b.MarkRemoveAt(2);
        b.Add(new Test(5));
        b.MarkRemoveAt(8);
        b.Remove();
        foreach (var itemTest in b.Data)
        {
            Debug.Log(itemTest.id);
        }
    }

    public unsafe void Span()
    {
        string str = "0123456789";
        var span = str.AsSpan();
        var slice = span.Slice(0, 5);
        Debug.Log(slice.ToString());

        var bytes = new byte[1024];
        var bytesSpan = bytes.AsSpan();
        bytesSpan.Slice(0, 10);
    }

    struct MallocSet
    {
        public Vector2 a;
    }

    class MallocSet2
    {
        public Vector2 a;
    }

    class MallocSet3
    {
        public Vector2 a;
    }

    [Test]
    public unsafe void UnsafeUtilityTest()
    {
        int size = 10000;
        MallocSet2[] ms = new MallocSet2[size];
        List<MallocSet3> mallocSet3S = new List<MallocSet3>();
        for (int i = 0; i < size; i++)
        {
            ms[i] = new MallocSet2();
            mallocSet3S.Add(new MallocSet3());
        }

        using (new TimeProfiler("MallocSet"))
            for (int i = 0; i < size; i++)
            {
                var ptr2 = ms[i];
                var x = ptr2.a;
            }

        int structSize = UnsafeUtility.SizeOf<MallocSet>();
        int alignment = UnsafeUtility.AlignOf<MallocSet>();
        void* ptr = UnsafeUtility.Malloc(structSize * size, alignment, Allocator.Persistent);
        using (new TimeProfiler("MallocSet *"))
            for (int i = 0; i < size; i++)
            {
                MallocSet* ptr2 = (MallocSet*) ptr + i;
                var x = ptr2->a;
            }


        UnsafeUtility.Free(ptr, Allocator.Persistent);
    }
}