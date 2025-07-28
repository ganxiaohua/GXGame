using System.Collections;
using System.Collections.Generic;
using GameFrame.Runtime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    public class HashSetTest : IContinuousID
    {
        public int ID { get; set; }
    }

    [Test]
    public void NewTestScriptSimplePasses()
    {
        HashSet<HashSetTest> chs = new HashSet<HashSetTest>();
        GXHashSet<HashSetTest> ghs = new GXHashSet<HashSetTest>(10000);
        using (new TimeProfiler("HashSet"))
            for (int i = 0; i < 10000; i++)
            {
                HashSetTest a = new HashSetTest();
                a.ID = i;
                chs.Add(a);
            }

        using (new TimeProfiler("HashSetTest"))
            for (int i = 0; i < 10000; i++)
            {
                HashSetTest a = new HashSetTest();
                a.ID = i;
                chs.Add(a);
            }


        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}