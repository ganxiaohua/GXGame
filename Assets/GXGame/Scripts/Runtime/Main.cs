using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class Main : MonoBehaviour
    {
        public static Transform ViewLayer;
        public static Transform CollisionLayer;
        public static Transform BTOLayer;
        async UniTaskVoid Start()
        {
            DontDestroyOnLoad(this);
            ViewLayer = transform.Find("ViewLayer");
            CollisionLayer = transform.Find("CollisionLayer");
            BTOLayer = transform.Find("BTOLayer");
            Components.SetComponent();
            AutoBindEvent.AddSystem();
            await GXGameFrame.Instance.Init();
            GXGameFrame.Instance.RootEntity.AddComponent<GameProcess>();
        }
    }
}