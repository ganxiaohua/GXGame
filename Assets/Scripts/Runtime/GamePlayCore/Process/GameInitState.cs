using GameFrame.Runtime;
using YooAsset;

namespace GamePlay.Runtime
{
    public class GameInitState : FsmState, IAssetEvent
    {
        private AssetsFsmController assetsFsmController;

        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            assetsFsmController = (AssetsFsmController) GXGameFrame.Instance.AddFsmComponents(typeof(AssetsFsmController));
        }

        public void OnAssetEvent(AssetEventType assetEvent)
        {
            switch (assetEvent)
            {
                case AssetEventType.Succ:
                    fsmController.ChangeState<GameStartState>();
                    break;
            }
        }

        public void OnAssetEvent(DownLoadSize obj)
        {
            assetsFsmController.ChangeState<PackageDownloadState>();
        }

        public void OnAssetEvent(DownloadUpdateData obj)
        {
            //处理正在下载的情况
            // Debugger.Log();
        }
    }
}