using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class Main : MonoBehaviour
    {
        async UniTaskVoid Start()
        {
            DontDestroyOnLoad(this);
            Components.SetComponent();
            new AutoBindEvent().AddSystem();
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
            GXGameFrame.Instance.OnDisable();
        }
    }
}