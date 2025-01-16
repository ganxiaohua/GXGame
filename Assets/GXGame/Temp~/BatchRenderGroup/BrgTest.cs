using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

struct Test
{
    public int id;
}
public class BrgTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        unsafe
        {
            var testIns = (Test*)UnsafeUtility.Malloc(
                UnsafeUtility.SizeOf<Test>() * 1,
                UnsafeUtility.AlignOf<Test>(),
                Allocator.TempJob);
;            var testIns2 = testIns;
            testIns2->id = 15;
            Debug.Log(testIns->id);
        }
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
