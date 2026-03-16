using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public static class ConstDie
    {
        public static void Crop(EffEntity owner, ECCWorld world)
        {
            var dependEntity = owner.GetDependEntityComp().GetData();
            var landView = (CropLandView) dependEntity.GetView().GetData();
            landView.areaCropLand.RemoveCrop(owner.GetView().GetData().Position);
        }
    }
}