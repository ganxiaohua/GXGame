using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;
using Assert = GameFrame.Runtime.Assert;

namespace GamePlay.Runtime
{
    public class OperatedStart_PlayerAccumulateCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.PlayerAccumulate;

        private EffEntity CropLand;
        private List<Vector2Int> towards;
        private DynamicMesh dynamicMesh;
        private DefaultAssetReference reference;
        private Material logoMaterial;
        private Vector2Int oldPoint;

        protected override void OnInit()
        {
            towards = ListPool<Vector2Int>.Get();
            towards.Add(Vector2Int.up);
            towards.Add(Vector2Int.right);
            towards.Add(Vector2Int.down);
            towards.Add(Vector2Int.left);
            oldPoint = new Vector2Int(-1, -1);
            reference = new DefaultAssetReference();
            Filter(ComponentsID<EnterOparatedComp>.TID, ComponentsID<OperatedObjectComp>.TID);
        }

        public override bool ShouldActivate()
        {
            bool ac = false;
            var comp = Owner.GetEnterOparatedCompWithHave();
            if (comp.have && comp.data.Value.IsLongTouch)
            {
                var itemInfo = BagData.Instance.GetCurPocketInfo();
                ac = itemInfo != null && itemInfo.Item.AccumulatePower.Length != 0;
            }

            return ac && Owner.HasComponent<OperatedObjectComp>();
        }

        public override bool ShouldDeactivate()
        {
            var comp = Owner.GetEnterOparatedComp();
            return !Owner.HasComponent(ComponentsID<OperatedObjectComp>.TID) || !comp.Value.IsLongTouch;
        }

        public override void OnActivated()
        {
            base.OnActivated();
            Owner.GetCapabilityComponent().Block(CapabilityTags.Tag_OpExcute, this);
            LoadMat().Forget();
            if (!Owner.HasComponent<AccumulateCropIndex>())
                Owner.AddAccumulateCropIndexExternal(64);
        }

        public override void OnDeactivated()
        {
            Owner.GetCapabilityComponent().UnBlock(CapabilityTags.Tag_OpExcute, this);
            CropLand = null;
            dynamicMesh?.SetAction(false);
            base.OnDeactivated();
        }


        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            if (logoMaterial)
                找到农田();
        }

        public override void Dispose()
        {
            reference.Dispose();
            logoMaterial = null;
            dynamicMesh?.Dispose();
            ListPool<Vector2Int>.Release(towards);
            towards = null;
        }

        private async UniTask LoadMat()
        {
            if (logoMaterial == null)
                logoMaterial = await AssetManager.Instance.LoadAsync<Material>("Assets/Res/_Common/Materials/Select", reference);
        }


        private void 找到农田()
        {
            var operatedFuncComp = Owner.GetOperatedFuncComp();
            Assert.IsNotNull(operatedFuncComp, $"{Owner.Name} 没有碰撞回馈函数");
            var targets = Owner.GetOperatedObjectComp().GetData();
            if (targets == null || targets.Count == 0)
                return;
            for (int i = targets.Count - 1; i >= 0; i--)
            {
                if (ConstCamp.IsCropLand(targets[i]))
                {
                    CropLand = targets[i];
                    break;
                }
            }

            //TODO:不用每一帧都变,数值发生修改了在变
            找到可以蓄力的点位();
        }

        private void 找到可以蓄力的点位()
        {
            if (CropLand == null)
            {
                dynamicMesh?.SetAction(false);
                return;
            }

            var ownerView = Owner.GetView().GetData();
            var cropLandView = (CropLandView) CropLand.GetView().GetData();
            var data = cropLandView.areaCropLand.croplandData;
            var operatedDetection = Owner.GetOperatedDetectionComp().Value;
            var collisionDetectionDataComp = Owner.GetCollisionDetectionDataComp().Value;
            var vp = data.Data.WorldToCell(collisionDetectionDataComp.Pos);
            if (vp == oldPoint)
                return;
            oldPoint = vp;
            var itemInfo = BagData.Instance.GetCurPocketInfo();
            var size = itemInfo.Item.AccumulatePower;
            int powerSize = size[0] * size[1];
            List<int> showIndex = ListPool<int>.Get();
            for (int i = 0; i < powerSize; i++)
            {
                showIndex.Add(i);
            }

            var dir = collisionDetectionDataComp.Pos - ownerView.Position;
            var vect = GetClosestDirection(new Vector2(dir.x, dir.z));
            int gridwidth = 0;
            int gridheght = 0;
            if (vect.x != 0)
            {
                gridwidth = size[1];
                gridheght = size[0];
            }
            else
            {
                gridwidth = size[0];
                gridheght = size[1];
            }

            dynamicMesh ??= new DynamicMesh(gridwidth, gridheght, data.Data.CellSize, logoMaterial, Main.GameObjectLayer);
            dynamicMesh.SetAction(true);
            dynamicMesh.SetGirdeHeight(gridwidth, gridheght);
            dynamicMesh.GenerateCombinedMesh(showIndex);
            int offsetX = 0, offsetY = 0;
            if (vect.y == 1)
            {
                offsetX = -gridwidth / 2;
            }
            else if (vect.y == -1)
            {
                offsetX = -gridwidth / 2;
                offsetY = -gridheght + 1;
            }

            if (vect.x == 1)
            {
                offsetY = -gridheght / 2;
            }
            else if (vect.x == -1)
            {
                offsetX = -gridwidth + 1;
                offsetY = -gridheght / 2;
            }

            var p = oldPoint + new Vector2Int(offsetX, offsetY);

            var cellWorldPos = data.Data.CellToWolrd(p);
            cellWorldPos += new Vector3(0, 0.01f, 0);
            dynamicMesh.SetWorldPRS(cellWorldPos, Quaternion.identity, Vector3.one);

            int cropLandSize = data.Data.GirdArea.x * data.Data.GirdArea.y;
            ListPool<int>.Release(showIndex);
            var need = Owner.GetAccumulateCropIndex().GetData();
            need.Clear();
            for (int i = 0; i < gridheght; i++)
            {
                for (int j = 0; j < gridwidth; j++)
                {
                    var newP = new Vector2Int(p.x + j, p.y + i);
                    var index = data.Data.Cell2Index(newP);
                    if (index < 0 || index >= cropLandSize)
                    {
                        continue;
                    }

                    need.Add(index);
                }
            }
        }

        private Vector2Int GetClosestDirection(Vector2 vector)
        {
            if (vector == Vector2.zero)
            {
                return towards[0];
            }

            Vector2 vectorNormalized = vector.normalized;
            List<float> list = ListPool<float>.Get();
            list.Add(Vector2.Dot(vectorNormalized, towards[0]));
            list.Add(Vector2.Dot(vectorNormalized, towards[1]));
            list.Add(Vector2.Dot(vectorNormalized, towards[2]));
            list.Add(Vector2.Dot(vectorNormalized, towards[3]));
            int maxIndex = 0;
            float max = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                    maxIndex = i;
                }
            }

            return towards[maxIndex];
        }
    }
}