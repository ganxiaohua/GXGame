using System.Collections;
using System.Collections.Generic;
using GameFrame;
using UnityEngine;

public class ReferenceDelete : MonoBehaviour
{
    // Start is called before the first frame update
    class Iref : IReference
    {
        public void Clear()
        {
            Debugger.Log("清理");
        }
    }

    private Iref x;

   async  void  Start()
   {
       await GXGameFrame.Instance.Start();
        x = ReferencePool.Acquire<Iref>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ReferencePool.Release(x);
        }
        GXGameFrame.Instance.Update();
    }
}