using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class Main : MonoBehaviour
    {
        public static Transform GameObjectLayer;

        async UniTaskVoid Start()
        {
            DontDestroyOnLoad(this);
            GameObjectLayer = transform.Find("GameObjectLayer");
            await GXGameFrame.Instance.Init();
            await Tables.InitializeAsync();
            AutoBindEvent.AddSystem();
            GXGameFrame.Instance.AddFsmComponents(typeof(GameProcess));
        }
    }
}