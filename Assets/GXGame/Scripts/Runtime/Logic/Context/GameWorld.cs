using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Logic
{
    public class GameWorld : World
    {
        private int otherCount = 2;
        private int monsterCount = 5000;

        public override void OnInitialize(int totalComponents)
        {
            base.OnInitialize(totalComponents);
            EstimateChildsCount(monsterCount + otherCount);
            this.AddSystem<ViewBaseSystem>();
            this.AddSystem<ViewUpdateSystem>();
            this.AddSystem<CollisionSystem>();
            this.AddSystem<CollisionBehaviorSystem>();
            this.AddSystem<WorldPosChangeSystem>();
            this.AddSystem<CountDowntSystem>();
            this.AddSystem<InputSystem>();
            //最后执行
            this.AddSystem<DestroyBaseSystem>();
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
            palyer.AddWorldPos(new Vector3(5.5f, 0, 0));
            palyer.AddLocalScale(Vector2.one);
            palyer.AddMoveDirection();
            palyer.AddMoveSpeed(2);
            palyer.AddFaceDirection();
            palyer.AddCollisionBox(CollisionBox.Create(palyer, LayerMask.NameToLayer($"Object")));
            palyer.AddCollisionGroundType(CollisionGroundType.Slide);
            palyer.AddCampComponent(GXGame.Camp.SELF);
            palyer.AddGXInput();
            palyer.AddPlayer();
            palyer.AddHP(10);

            for (int i = 0; i < monsterCount; i++)
            {
                var monster = AddChild();
                monster.Name = "骷髅";
                monster.AddViewType(typeof(Go2DView));
                monster.AddAssetPath("Monster_001/Prefab/Monster_001");
                monster.AddWorldPos(new Vector3(-0.5f + i % 50, 3.0f + i / 50, 0));
                monster.AddLocalScale(Vector2.one);
                monster.AddMoveDirection();
                monster.AddMoveSpeed(0.5f);
                monster.AddFaceDirection();
                // monster.AddCollisionBox(CollisionBox.Create(monster, LayerMask.NameToLayer($"Object")));
                // monster.AddCollisionGroundType(CollisionGroundType.Slide);
                monster.AddCampComponent(GXGame.Camp.ENEMY);
                monster.AddMonster();
                monster.AddHP(10);
                monster.AddGXInput();
                // monster.AddBehaviorTreeComponent("MonsterBTO");
            }

            var monster1 = AddChild();
            monster1.Name = "史莱姆";
            monster1.AddViewType(typeof(Go2DView));
            monster1.AddAssetPath("Monster_002/Prefab/Monster_002");
            monster1.AddWorldPos(new Vector3(5.5f, 1, -1));
            monster1.AddLocalScale(Vector2.one);
            monster1.AddMoveDirection();
            monster1.AddMoveSpeed(0f);
            monster1.AddFaceDirection();
            monster1.AddCollisionBox(CollisionBox.Create(monster1, LayerMask.NameToLayer($"Object")));
            monster1.AddCollisionGroundType(CollisionGroundType.Slide);
            monster1.AddMonster();
            monster1.AddCampComponent(GXGame.Camp.ENEMY);
            monster1.AddHP(10);
            monster1.AddBehaviorTreeComponent("MonsterBTO");
        }
    }
}