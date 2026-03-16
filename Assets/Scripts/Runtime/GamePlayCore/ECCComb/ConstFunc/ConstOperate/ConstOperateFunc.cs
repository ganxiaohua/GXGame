using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static class ConstOperateFunc
    {
        public static void PlayerUserItem(EffEntity owner, ECCWorld world)
        {
            var itemInfo = BagData.Instance.GetCurPocketInfo();
            var view = owner.GetView().GetData();
            VirtualCollision vc = new VirtualCollision();
            vc.Rot = view.Rotation.normalized;
            vc.Pos = view.Position + vc.Rot * Vector3.forward;
            vc.Size = Vector3.one;
            vc.Layer = ConstLayer.OperatedLayer;
            OperatedType type = OperatedType.手操;
            int itemId = 0;
            if (itemInfo != null && itemInfo.Item.Unit_Ref != null)
            {
                type = itemInfo.Item.Unit_Ref.OperatedItemType;
                itemId = itemInfo.Item.Id;
            }

            owner.AddOrSetOperatedDetectionComp(new OperatedDetectionData()
            {
                    OperatorCount = 1,
                    OperatedType = type,
                    Itemid = itemId
            });
            owner.AddOrSetCollisionDetectionDataComp(vc);
        }

        public static void PlayerThrowItem(EffEntity owner, ECCWorld world)
        {
            var itemInfo = BagData.Instance.GetCurPocketInfo();
            if (itemInfo == null || itemInfo.Item.Unit_Ref == null)
                return;
            var entity = ConstCreateEntitys.CreateHandheld(world, itemInfo.Item, owner, null);
            BagData.Instance.SubItem(itemInfo.Item.Id, 1);
        }
    }
}