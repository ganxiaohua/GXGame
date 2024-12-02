using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class Main : MonoBehaviour
    {
        public static Transform ViewLayer;
        public static Transform CollisionLayer;
        async UniTaskVoid Start()
        {
            DontDestroyOnLoad(this);
            ViewLayer = transform.Find("ViewLayer");
            CollisionLayer = transform.Find("CollisionLayer");
            Components.SetComponent();
            new AutoBindEvent().AddSystem();
            await GXGameFrame.Instance.Start();
            GXGameFrame.Instance.RootEntity.AddComponent<GameProcess>();
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
        
        
        private void OnApplicationQuit()
        {
            GXGameFrame.Instance.OnDisable();
        }
    }
}