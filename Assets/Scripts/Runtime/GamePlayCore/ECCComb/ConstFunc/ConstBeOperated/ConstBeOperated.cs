using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class ConstBeOperated
    {
        public static void BeOperated_Transmit(EffEntity owner, ECCWorld world)
        {
            var unitData = owner.GetPortalDataComp();
            var magicWorld = ((MagicWorld) world);
            magicWorld.Transfer(unitData.GetData()).Forget();
        }

        // public async UniTask TransferHouse(int id)
        // {
        //     var magicWorld = ((MagicWorld) EccWorld);
        //     houseModel = ReferencePool.Acquire<InternalHouseModel>();
        //     await houseModel.Initialize(EccWorld, new ChunkHouseData() {HouseId = id, lPos = ConstData.TransmitHousePos, lRot = Quaternion.identity, lScale = Vector3.one});
        //     magicWorld.SetPlayerPQS(ConstData.TransmitHousePos, Quaternion.identity, Vector3.one);
        // }
    }
}