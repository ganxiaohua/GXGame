using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class Main : MonoBehaviour
    {
        private GameProcessController gameProcessController;
        async UniTaskVoid Start()
        {
            DontDestroyOnLoad(this);
            Components.SetComponent();
            new AutoBindEvent().AddSystem();
            await GXGameFrame.Instance.Start();
           GXGameFrame.Instance.MainScene.AddComponent<GameProcessController>();
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

        private void OnDestroy()
        {
            GXGameFrame.Instance.OnDisable();
        }
    }
}