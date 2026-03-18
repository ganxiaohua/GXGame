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
           var succ = await GXGameFrame.Instance.Init();
           if (!succ)
               return;
           succ =  await Tables.InitializeAsync();
           if (!succ)
               return;
            AutoBindEvent.AddSystem();
            GXGameFrame.Instance.AddFsmComponents(typeof(GameProcess));
        }
    }
}