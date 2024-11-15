using GameFrame;
using UnityEngine;

namespace GXGame.Logic
{
    public class GameWrold : World
    {
        public override void Initialize()
        {
            base.Initialize();
            this.AddSystem<ViewSystem>();
            this.AddSystem<InputSystem>();
            this.AddSystem<WorldPosChangeSystem>();
            this.AddSystem<WorldDirChangeSystem>();
            //最后执行
            this.AddSystem<DestroySystem>();
            CreateMap();
            CreatePlayer();
        }

        private void CreateMap()
        {
            var map = AddChild();
            map.AddViewType(typeof(GoBaseView));
            map.AddAssetPath("Map");
            map.AddWorldPos(Vector3.zero);
        }

        private void CreatePlayer()
        {
            var palyer = AddChild();
            palyer.AddViewType(typeof(Go2DView));
            palyer.AddAssetPath("Assets/GXGame/Art/Runtime/Role/Player/Prefab/Player.prefab");
            palyer.AddWorldPos(new Vector3(-0.5f,0,0));
            palyer.AddMoveDirection();
            palyer.AddMoveSpeed(1);
            palyer.AddInputDirection();

            var monster = AddChild();
            monster.AddViewType(typeof(Go2DView));
            monster.AddAssetPath("Assets/GXGame/Art/Runtime/Role/Monster_002/Prefab/Monster_002.prefab");
            monster.AddWorldPos(new Vector3(5,0,-1));
            monster.AddMoveDirection();
            monster.AddMoveSpeed(1);
        }
    }
}