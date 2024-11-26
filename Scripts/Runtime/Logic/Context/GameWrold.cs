using GameFrame;
using UnityEngine;

namespace GXGame.Logic
{
    public class GameWrold : World
    {
        private int count = 10000;

        public override void Initialize()
        {
            EstimateChildsCount(count + 2);
            base.Initialize();
            this.AddSystem<ViewSystem>();
            this.AddSystem<ViewUpdateSystem>();
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
            map.Name = "Map";
            map.AddViewType(typeof(GoBaseView));
            map.AddAssetPath("Map_BaseMap");
            map.AddWorldPos(Vector3.zero);
        }

        private void CreatePlayer()
        {
            for (int i = 0; i < count; i++)
            {
                var palyer = AddChild();
                palyer.Name = $"角色{i}";
                palyer.AddViewType(typeof(Go2DView));
                palyer.AddAssetPath("Player/Prefab/Player");
                palyer.AddWorldPos(new Vector3(-0.5f + i % 50, i / 50, 0));
                palyer.AddLocalScale(Vector2.one * 0.5f);
                palyer.AddMoveDirection();
                palyer.AddMoveSpeed(1);
                palyer.AddInputDirection();
            }

            var monster = AddChild();
            monster.Name = "怪兽";
            monster.AddViewType(typeof(Go2DView));
            monster.AddAssetPath("Monster_002/Prefab/Monster_002");
            monster.AddWorldPos(new Vector3(5, 0, -1));
            monster.AddMoveDirection();
            monster.AddMoveSpeed(1);
        }
    }
}