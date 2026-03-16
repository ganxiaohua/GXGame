using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class ConstCreateEntitys
    {
        public static void CreateJianQi(ECCWorld world, EffEntity Owner)
        {
            // var view = Owner.GetView().Value;
            // var rot = view.Rotation.normalized;
            // var pos = view.Position + rot * Vector3.forward + new Vector3(0, 1.5f, 0);
            // var child = world.AddChild();
            // child.Name = "Assets/Res/Prefabs/Skill/JianQi";
            // child.AddAssetPathComp("JianQi");
            // var be = View.Create(typeof(LogicView), child, Main.GameObjectLayer);
            // be.Position = pos;
            // be.Rotation = rot;
            // child.AddView(be);
            // child.AddMoveSpeedComp(25);
            // //世界基础组件
            // child.AddMoveDirectionComp(view.transform.forward);
            // child.AddAttrComp(AttrComp.GetAttrWithConfig(Tables.Instance.AttributeTable.Get(5)));
            // child.AddTimeOutDestroyComp(Time.realtimeSinceStartup + 3);
            // child.AddOperateCollisionLayerComp(ConstLayer.AllOperatedLayer());
            // child.AddOperatedFuncComp(ConstOperatedFunc.OnOperated_Player);
            // child.AddOperatedDetectionFilterComp(new DetectionFilter() {Camp = Owner.GetUnitDataComp().Value.Camp});
            // world.BindCapability<ViewCapability>(child);
            // world.BindCapability<CommonMoveCapability>(child);
            // world.BindCapability<OperatedStart_AlwaysDetectionCapability>(child);
            // world.BindCapability<OperatedDetectionCapability>(child);
            // world.BindCapability<OperatedExecuteCapability>(child);
            // world.BindCapability<TimeOutDestroyCapability>(child);
        }
    }
}