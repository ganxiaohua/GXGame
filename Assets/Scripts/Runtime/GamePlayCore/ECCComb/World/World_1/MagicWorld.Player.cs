using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class MagicWorld
    {
        private EffEntity camera;
        private EffEntity player;

        private void PlayerInit()
        {
            CreatePlayer();
            CreateCamera();
        }

        public void CreatePlayer()
        {
            var item = Tables.Instance.UnitTable.GetOrDefault(20000);
            player = AddChild();
            player.Name = "Player";
            player.AddAssetPathCompExternal(item.Model_Ref.Path);
            var view = View.Create(typeof(ManView), player, Main.GameObjectLayer);
            view.Position = new Vector3(-15, 0, -20);
            player.AddViewExternal(view);
            player.AddPlayerComp();
            var attr = AttrComp.GetAttrWithConfig(Tables.Instance.AttributeTable.Get(item.Attribute.Value));
            player.AddAttrCompExternal(attr);
            player.AddHPComp(attr.GetHp());
            player.AddUnitDataCompExternal(new UnitData() {UnitItem = item, Camp = CampType.Player});
            player.AddBeAttackFuncCompExternal(new BeAttackFuncData(ConstBeOperated.BeOperated_PlayerStart, ConstBeOperated.BeOperated_PlayerEnd, null));
            ConstCapabilityGroup.PlayerOperatedGroup(this, player);
            ConstCapabilityGroup.BeAttackGroup(this, player);
            BindCapability<ViewCapability>(player);
            BindCapability<PlayerEnterCapability>(player);
            BindCapability<FlyCapability>(player);
            BindCapability<DetectionLogoCapability>(player);
            BindCapability<DieCapability>(player);
            ConstCapabilityGroup.KCCGroup(this, player, new ConstCapabilityGroup.KCCGroupData(
                    ConstData.PlayerRunMoveDefSpeed,
                    ConstData.DefDirectionSpeed, Vector3.down * ConstData.DefGravity, 10, ConstData.PlayerMoveDefSpeedUpMagnification));
        }

        private void CreateCamera()
        {
            camera = AddChild();
            var view = View.Create(typeof(CameraView), camera, Main.GameObjectLayer);
            camera.Name = "CameraRoot";
            camera.AddAssetPathCompExternal("Assets/Res/Prefabs/Enviro/CameraRoot");
            camera.AddViewExternal(view);
            camera.AddCameraWatcherComp(player.ID);
            BindCapability<ViewCapability>(camera);
            BindCapability<CameraEnterCapability>(camera);
        }

        private void SetPlayerPQS(Vector3 pos, Quaternion quaternion, Vector3 scale)
        {
            var view = player.GetView().GetData();
            view.Position = pos;
            view.Rotation = quaternion;
            view.scale = scale;
            var previousGround = player.GetPreviousGroundComp().GetData();
            previousGround.PreviousParent = null;
        }
    }
}