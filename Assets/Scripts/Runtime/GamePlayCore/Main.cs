using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class Main : MonoBehaviour
    {
        public static Transform GameObjectLayer;

        void Start()
        {
            GXGameFrame.Instance.Init();
            DontDestroyOnLoad(this);
            GameObjectLayer = transform.Find("GameObjectLayer");
            AutoBindEvent.AddSystem();
            GXGameFrame.Instance.AddFsmComponents(typeof(GameProcess));
        }
    }
}