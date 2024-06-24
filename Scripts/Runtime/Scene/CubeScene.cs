using Cysharp.Threading.Tasks;
using FairyGUI;
using GameFrame;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GXGame
{
    public class CubeScene : Entity, IScene, IStartSystem, IUpdateSystem
    {
        public void Start()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            await AssetManager.Instance.LoadSceneAsync("Assets/GXGame/Scenes/CubeScene.unity");
            AddComponent<CubeConText>();
            UIManager.Instance.OpenUI(typeof(UICardListWindow));
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        public override void Clear()
        {
            AssetManager.Instance.DecrementReferenceCount("Assets/GXGame/Scenes/CubeScene.unity");
            base.Clear();
        }
    }
}