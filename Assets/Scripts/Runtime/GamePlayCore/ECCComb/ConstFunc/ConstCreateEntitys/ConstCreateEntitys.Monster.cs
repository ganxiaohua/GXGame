using System;
using GameFrame.Runtime;
using GamePlay.Runtime.MapData;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class ConstCreateEntitys
    {
        private static EffEntity CreateMonsterUnitForMapEditor(ECCWorld world, Unit unit, Transform parent, Action<EffEntity, ECCWorld> destroyAction)
        {
            int monsterTableId = unit.UnitId;
            var monster = Tables.Instance.UnitTable.GetOrDefault(monsterTableId);
            var child = world.AddChild();
            child.Name = monster.Model_Ref.Path;
            child.AddAssetPathCompExternal(monster.Model_Ref.Path);
            var view = View.Create(typeof(MonsterView), child, Main.GameObjectLayer);
            view.Position = unit.lPos;
            child.AddViewExternal(view);
            child.AddOrSetBehaviorTreeCompExternal(monster.BehaviorTable_Ref.BehaviorName);
            var attr = AttrComp.GetAttrWithConfig(monster.Attribute_Ref);
            child.AddHPComp(attr.GetHp());
            child.AddAttrCompExternal(attr);
            child.AddUnitDataCompExternal(new UnitData() {UnitItem = monster, Camp = CampType.Monster,MapUnitIndex = unit.Index});
            child.AddBeAttackFuncCompExternal(new BeAttackFuncData(ConstBeOperated.BeOperated_EnemyStart, ConstBeOperated.BeOperated_EnemyEnd, destroyAction));
            ConstCapabilityGroup.KCCGroup(world, child, new ConstCapabilityGroup.KCCGroupData(
                    3,
                    ConstData.DefDirectionSpeed, Vector3.down * ConstData.DefGravity, 10, ConstData.PlayerMoveDefSpeedUpMagnification));
            world.BindCapability<ViewCapability>(child);
            world.BindCapability<BehaviorTreeCapability>(child);
            ConstCapabilityGroup.MonsterOperatedGroup(world, child);
            ConstCapabilityGroup.BeAttackGroup(world, child);
            return child;
        }
    }
}