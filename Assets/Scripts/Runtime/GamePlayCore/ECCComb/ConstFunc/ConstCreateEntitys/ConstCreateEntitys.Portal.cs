using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class ConstCreateEntitys
    {
        // public static EffEntity CreateResPortal(ECCWorld world, Unit unit, Transform parent, Action<EffEntity, ECCWorld> destroyAction)
        // {
        //     // var item = Tables.Instance.UnitTable.GetOrDefault(unit.UnitId);
        //     // var path = item.Model_Ref.Path;
        //     // var child = world.AddChild();
        //     // child.AddAssetPathComp(path);
        //     // var view = View.Create(typeof(LogicView), child, parent);
        //     // view.LocalPosition = unit.lPos;
        //     // view.LocalRotation = unit.lRot;
        //     // view.scale = unit.lScale;
        //     // child.AddView(view);
        //     // child.Name = path;
        //     // child.AddUnitDataComp(new UnitData() {UnitItem = item, Camp = CampType.Portal, MapUnitIndex = unit.Index});
        //     // world.BindCapability<ViewCapability>(child);
        //     // ActionComp(item, child, world, destroyAction, null, ConstBeOperated.BeOperated_Transmit);
        //     return child;
        // }
    }
}