using System.Collections;
using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class Main : MonoBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            await GXGameFrame.Instance.Start();
            SceneFactory.ChangePlayerScene<CubeScene>();
        }

        // Update is called once per frame
        void Update()
        {
            GXGameFrame.Instance.Update();
        }
    }
}