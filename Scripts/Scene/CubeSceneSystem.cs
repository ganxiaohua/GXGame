using System;
using System.Collections.Generic;
using GameFrame;
using UnityEngine.SceneManagement;

namespace GXGame
{
    public static class CubeSceneSystem
    {
        [SystemBind]
        public class CubeSceneStartSystem : StartSystem<CubeScene>
        {
            protected override async void Start(CubeScene self)
            {
                await AssetManager.Instance.LoadSceneAsync("Assets/GXGame/Scenes/CubeScene.unity");
            }
        }

        [SystemBind]
        public class CubeSceneShowSystem : ShowSystem<CubeScene>
        {
            protected override void Show(CubeScene self)
            {
            }
        }

        [SystemBind]
        public class CubeSceneHideSystem : HideSystem<CubeScene>
        {
            protected override void Hide(CubeScene self)
            {
            }
        }

        [SystemBind]
        public class CubeSceneUpdateSystem : UpdateSystem<CubeScene>
        {
            protected override void Update(CubeScene self, float elapseSeconds, float realElapseSeconds)
            {
            }
        }

        [SystemBind]
        public class CubeSceneClearSystem : ClearSystem<CubeScene>
        {
            protected override void Clear(CubeScene self)
            {
            }
        }
    }
}