using System;
using GameFrame.Runtime;
using GamePlay.Runtime.MapData;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class ConstCreateEntitys
    {
        public static void CreateHarvest(ECCWorld world, Item item, int count, Vector3 pos)
        {
            var child = world.AddChild();
            var be = View.Create(typeof(EffEntityView), child, Main.GameObjectLayer);
            string path = "Assets/Res/Prefabs/Makings/Stone_1";
            if (item.Unit_Ref != null)
                path = item.Unit_Ref.Model_Ref.Path;
            child.Name = path;
            child.AddAssetPathCompExternal(path);
            child.AddViewExternal(be);
            be.Position = pos;
            be.Rotation = Quaternion.identity;
            child.AddHarvestStateComp(HarvestState.Jump);
            child.AddItemCompExternal(item);
            child.AddItemCountComp(count);
            world.BindCapability<ViewCapability>(child);
            world.BindCapability<JumpoutHarvestCapability>(child);
            world.BindCapability<HarvestAbsorbCapability>(child);
            world.BindCapability<ShowItemQualityCapability>(child);
        }

        public static EffEntity CreateUnitForMapEditor(ECCWorld world, Unit unit, Transform parent, Action<EffEntity, ECCWorld> destroyAction)
        {
            var item = Tables.Instance.UnitTable.GetOrDefault(unit.UnitId).GetItem();
            if (item != null)
            {
                return item.ItemType switch
                {
                        ItemType.Monster => CreateMonsterUnitForMapEditor(world, unit, parent, destroyAction),
                        _ => CreateMapUnit(world, unit, parent, destroyAction, null)
                };
            }
            else
            {
                return CreateMapUnit(world, unit, parent, destroyAction, null);
            }
        }

        public static EffEntity CreateMapUnit(ECCWorld world, Unit unit, Transform parent, Action<EffEntity, ECCWorld> destroyAction, Action<EffEntity, ECCWorld> commonUseAction)
        {
            var item = Tables.Instance.UnitTable.GetOrDefault(unit.UnitId);
            var path = item.Model_Ref.Path;
            var child = world.AddChild();
            child.AddAssetPathCompExternal(path);
            var view = View.Create(typeof(LogicView), child, parent);
            view.LocalPosition = unit.lPos;
            view.LocalRotation = unit.lRot;
            view.scale = unit.lScale;
            child.AddViewExternal(view);
            child.Name = path;
            child.AddUnitDataCompExternal(new UnitData() {UnitItem = item, Camp = CampType.Res, MapUnitIndex = unit.Index});
            world.BindCapability<ViewCapability>(child);
            ActionComp(item, child, world, destroyAction, null, commonUseAction);
            return child;
        }

        public static EffEntity CreateMapPortalUnit(ECCWorld world, PortalUnit unit, Transform parent, Action<EffEntity, ECCWorld> destroyAction, Action<EffEntity, ECCWorld> commonUseAction)
        {
            var item = Tables.Instance.UnitTable.GetOrDefault(unit.UnitId);
            var path = item.Model_Ref.Path;
            var child = world.AddChild();
            child.AddAssetPathCompExternal(path);
            var view = View.Create(typeof(LogicView), child, parent);
            view.LocalPosition = unit.lPos;
            view.LocalRotation = unit.lRot;
            view.scale = unit.lScale;
            child.AddViewExternal(view);
            child.Name = path;
            child.AddPortalDataCompExternal(unit);
            child.AddUnitDataCompExternal(new UnitData() {UnitItem = item, Camp = CampType.Res, MapUnitIndex = unit.Index});
            world.BindCapability<ViewCapability>(child);
            ActionComp(item, child, world, destroyAction, null, commonUseAction);
            return child;
        }

        /// <summary>
        /// 创建手持抛投物
        /// </summary>
        /// <param name="world"></param>
        /// <param name="item"></param>
        /// <param name="holder"></param>
        /// <param name="parent"></param>
        /// <param name="destroyAction"></param>
        /// <returns></returns>
        public static EffEntity CreateHandheld(ECCWorld world, Item iteminfo, EffEntity holder, Action<EffEntity, ECCWorld> destroyAction)
        {
            var item = iteminfo.Unit_Ref;
            var path = item.Model_Ref.Path;
            var child = world.AddChild();
            child.AddAssetPathCompExternal(path);
            var view = View.Create(typeof(LogicView), child, Main.GameObjectLayer);
            var holderView = holder.GetView().GetData();
            var x = holderView.Rotation * Vector3.forward;
            view.Position = holderView.Position + x + new Vector3(0, holder.GetColliderLogicComp().GetLogicData().Height + 0.1f, 0);
            view.Rotation = holderView.Rotation;
            view.scale = holderView.scale;
            child.AddViewExternal(view);
            child.Name = path;
            child.AddUnitDataCompExternal(new UnitData() {UnitItem = item, Camp = CampType.Res});
            world.BindCapability<ViewCapability>(child);
            ConstCapabilityGroup.KCCGroup(world, child, new ConstCapabilityGroup.KCCGroupData(
                    10,
                    ConstData.DefDirectionSpeed, Vector3.down * 20, 15, ConstData.PlayerMoveDefSpeedUpMagnification));
            // x.y = 1;
            child.SetMoveDirectionComp(x);
            child.AddOperatedDetectionFilterComp(new DetectionFilter() {Camp = CampType.CropLand, EntityId = holder.ID});
            child.AddTimeOutDestroyComp(Time.realtimeSinceStartup + 10);
            world.BindCapability<TimeOutDestroyCapability>(child);
            world.BindCapability<OperatedDetectionCapability>(child);
            world.BindCapability<UpdateCollisionDataCapability>(child);
            world.BindCapability<CollisionDealWithCapabilty>(child);
            return child;
        }


        public static EffEntity CreateCropLand(ECCWorld world, AreaCropLand croplandData)
        {
            var land = world.AddChild();
            land.AddAssetPathCompExternal("Assets/Res/Prefabs/Cropland/Cropland");
            land.Name = "Cropland";
            var view = View.Create(typeof(CropLandView), land, Main.GameObjectLayer, croplandData);
            land.AddViewExternal(view);
            land.AddUnitDataCompExternal(new UnitData() {Camp = CampType.CropLand});
            world.BindCapability<ViewCapability>(land);
            return land;
        }

        /// <summary>
        /// 被攻击或者被使用的回调
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="child"></param>
        /// <param name="world"></param>
        /// <param name="destroyAction"></param>
        /// <param name="pickRetainAction"></param>
        private static void ActionComp(UnitItem unit, EffEntity child, ECCWorld world,
                Action<EffEntity, ECCWorld> destroyAction,
                Action<EffEntity, ECCWorld> pickRetainAction,
                Action<EffEntity, ECCWorld> commonUseAction
        )
        {
            if (unit.Attribute_Ref != null)
            {
                var attr = AttrComp.GetAttrWithConfig(unit.Attribute_Ref);
                child.AddAttrCompExternal(attr);
                child.AddHPComp(attr.GetHp());
            }

            for (int i = 0; i < unit.BeOperated.Length; i++)
            {
                if (unit.BeOperated[i].OpType == UnitOpType.Attack)
                {
                    ConstCapabilityGroup.BeAttackGroup(world, child);
                    child.AddOrSetBeAttackFuncCompExternal(new BeAttackFuncData(ConstBeOperated.BeAttack_Sundries, null, destroyAction));
                }
                else if (unit.BeOperated[i].OpType == UnitOpType.Use)
                {
                    ConstCapabilityGroup.BeUse(world, child);
                    if (commonUseAction != null)
                    {
                        AddUseFunc(child, i, commonUseAction, unit.BeOperated.Length);
                    }

                    if (unit.BeOperated[i].ResultType == ResultUnitOpType.PickRetain)
                    {
                        AddUseFunc(child, i, pickRetainAction, unit.BeOperated.Length);
                    }
                    else if (unit.BeOperated[i].ResultType == ResultUnitOpType.Destroy)
                    {
                        AddUseFunc(child, i, destroyAction, unit.BeOperated.Length);
                    }
                }
            }
        }

        private static void AddUseFunc(EffEntity effEntity, int index, Action<EffEntity, ECCWorld> action, int count)
        {
            var ben = effEntity.GetBeUseFuncCompWithHave();
            if (!ben.have)
            {
                var actionFun = new Action<EffEntity, ECCWorld>[count];
                actionFun[index] = action;
                effEntity.AddBeUseFuncCompExternal(actionFun);
            }
            else
            {
                ben.data.GetData()[index] = action;
            }
        }
    }
}