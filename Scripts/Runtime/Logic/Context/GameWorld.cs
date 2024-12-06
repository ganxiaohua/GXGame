using GameFrame;
using UnityEngine;

namespace GXGame.Logic
{
    public class GameWorld : World
    {
        private int count = 3;

        public override void OnInitialize()
        {
            base.OnInitialize();
            EstimateChildsCount(count);
            this.AddSystem<ViewSystem>();
            this.AddSystem<ViewUpdateSystem>();
            this.AddSystem<InputSystem>();
            this.AddSystem<CollisionSystem>();
            this.AddSystem<WorldPosChangeSystem>();
            this.AddSystem<CountDowntSystem>();
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
            
            var palyer = AddChild();
            palyer.Name = $"主角";
            palyer.AddViewType(typeof(Go2DView));
            palyer.AddAssetPath("Player/Prefab/Player");
            palyer.AddWorldPos(new Vector3(-0.5f, 0, 0));
            palyer.AddLocalScale(Vector2.one);
            palyer.AddMoveDirection();
            palyer.AddMoveSpeed(2);
            palyer.AddFaceDirection();
            palyer.AddCollisionBox(CollisionBox.Create(palyer,LayerMask.NameToLayer($"Object")));
            palyer.AddCollisionGroundType(CollisionGroundType.Slide);
            palyer.AddCampComponent(GXGame.Camp.SELF);
            palyer.AddGXInput();
            palyer.AddPlayer();
            palyer.AddHP(10);
            //
            //
            var monster = AddChild();
            monster.Name = "骷髅";
            monster.AddViewType(typeof(Go2DView));
            monster.AddAssetPath("Monster_001/Prefab/Monster_001");
            monster.AddWorldPos(new Vector3(-0.5f, 3.0f, 0));
            monster.AddLocalScale(Vector2.one);
            monster.AddMoveDirection();
            monster.AddMoveSpeed(0.5f);
            monster.AddFaceDirection();
            monster.AddCollisionBox(CollisionBox.Create(monster,LayerMask.NameToLayer($"Object")));
            monster.AddCollisionGroundType(CollisionGroundType.Slide);
            monster.AddCampComponent(GXGame.Camp.ENEMY);
            monster.AddMonster();
            monster.AddHP(10);
            monster.AddBehaviorTreeComponent("MonsterBTO");
            //
            
            // monster = AddChild();
            // monster.Name = "史莱姆";
            // monster.AddViewType(typeof(Go2DView));
            // monster.AddAssetPath("Monster_002/Prefab/Monster_002");
            // monster.AddWorldPos(new Vector3(5.5f, 1, -1));
            // monster.AddMoveDirection();
            // monster.AddMoveSpeed(0.2f);
            // monster.AddFaceDirection();
            // monster.AddCollisionBox(CollisionBox.Create(monster));
            // monster.AddCollisionGroundType(CollisionGroundType.Slide);
            // monster.AddMonster();
        }
    }
}