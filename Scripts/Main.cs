using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class Main : MonoBehaviour
    {
        async void Start()
        {
            DontDestroyOnLoad(this);
            await GXGameFrame.Instance.Start();
            SceneFactory.ChangePlayerScene<CubeScene>();
        }
        void Update()
        {
            GXGameFrame.Instance.Update();
        }

        private void LateUpdate()
        {
            GXGameFrame.Instance.LateUpdate();
        }
        
        private void FixedUpdate()
        {
            GXGameFrame.Instance.FixedUpdate();
        }

        private void OnDisable()
        {
            // GXGameFrame.Instance.OnDisable();
        }
    }
}