using GameFrame;
using UnityEngine;

namespace GXGame.Logic
{
    public class GameWorld : World
    {
        private int count = 1;

        public override void OnInitialize()
        {
            base.OnInitialize();
            EstimateChildsCount(count + 2);
            this.AddSystem<ViewSystem>();
            this.AddSystem<ViewUpdateSystem>();
            this.AddSystem<InputSystem>();
            this.AddSystem<CollisionSystem>();
            this.AddSystem<WorldPosChangeSystem>();
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
                // if (i % 2 == 0)
                    palyer.AddAssetPath("Player/Prefab/Player");
                // else
                //     palyer.AddAssetPath("Monster_001/Prefab/Monster_001");
                palyer.AddWorldPos(new Vector3(-0.5f + i % 50, i / 50, 0));
                palyer.AddLocalScale(Vector2.one * 0.5f);
                palyer.AddMoveDirection();
                palyer.AddMoveSpeed(1);
                palyer.AddFaceDirection();
                palyer.AddCollisionBox(CollisionBox.Create(palyer));
                palyer.AddCollisionGroundType(CollisionGroundType.Slide);
                palyer.AddGXInput();
            }
            
            var monster = AddChild();
            monster.Name = "骷髅";
            monster.AddViewType(typeof(Go2DView));
            monster.AddAssetPath("Monster_001/Prefab/Monster_001");
            monster.AddWorldPos(new Vector3(-0.5f,3.0f, 0));
            monster.AddLocalScale(Vector2.one * 0.5f);
            monster.AddMoveDirection();
            monster.AddMoveSpeed(1);
            monster.AddFaceDirection();
            monster.AddCollisionBox(CollisionBox.Create(monster));
            monster.AddCollisionGroundType(CollisionGroundType.Slide);

             monster = AddChild();
            monster.Name = "史莱姆";
            monster.AddViewType(typeof(Go2DView));
            monster.AddAssetPath("Monster_002/Prefab/Monster_002");
            monster.AddWorldPos(new Vector3(5, 0, -1));
            monster.AddMoveDirection();
            monster.AddMoveSpeed(1);
            monster.AddFaceDirection();
            monster.AddCollisionBox(CollisionBox.Create(monster));
        }
    }
}