using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class Main : MonoBehaviour
    {
        private IEntity hot;

        async UniTaskVoid Start()
        {
            DontDestroyOnLoad(this);
            Components.SetComponent();
            await GXGameFrame.Instance.Start();
            CubeScene x = SceneFactory.ChangePlayerScene<CubeScene>();
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