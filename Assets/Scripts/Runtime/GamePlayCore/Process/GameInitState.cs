using System.Collections.Generic;
using GameFrame.Runtime;
using YooAsset;

namespace GamePlay.Runtime
{
    public class GameInitState : FsmState, IAssetEvent
    {
        private AssetsPrepareFsmController assetsPrepareFsmController;
        private int totalDownloadCount;
        private long totalDownloadBytes;
        private List<ResourceDownloaderOperation> resourceDownloaderOperationList;
        private int assetsPrepareFsmControllerCount;
        private List<FsmController> fsmControllers;
        private Dictionary<string, DownloadUpdateData> downloadUpdateDataDic;


        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            fsmControllers = new List<FsmController>(4);
            resourceDownloaderOperationList = new List<ResourceDownloaderOperation>(4);
            downloadUpdateDataDic = new Dictionary<string, DownloadUpdateData>(4);
            YooAssets.Initialize();
            totalDownloadCount = 0;
            totalDownloadBytes = 0;
            resourceDownloaderOperationList.Clear();
            var datas = YooConst.PackageSettings;
            foreach (var t in datas)
            {
                var fsm = (AssetsPrepareFsmController) GXGameFrame.Instance.AddFsmChild(typeof(AssetsPrepareFsmController));
                fsm.SetData("packageName", t.Name);
                fsm.SetData("playMode", t.PlayMode);
                fsm.Start();
                fsmControllers.Add(fsm); }
        }

        public override void OnExit()
        {
            base.OnExit();
            foreach (var fsm in fsmControllers)
            {
                GXGameFrame.Instance.RemoveFsmChild(fsm);
            }
        }

        public void OnAssetEvent(AssetEventType assetEvent, object obj)
        {
            switch (assetEvent)
            {
                case AssetEventType.InitFail:
                    break;
                case AssetEventType.LoadVersionFail:
                    break;
                case AssetEventType.PackageManifestFail:
                    break;
                case AssetEventType.PackageDownloaderNoMsg:
                    CheckDownAssets();
                    break;
                case AssetEventType.PackageDownloaderMsgGetOver:
                    CheckDownAssets();
                    break;
                case AssetEventType.Succ:
                    Next();
                    break;
            }
            
        }
        
        public void OnAssetEvent(ResourceDownloaderOperation obj)
        {
            totalDownloadCount += obj.TotalDownloadCount;
            totalDownloadBytes += obj.TotalDownloadBytes;
            resourceDownloaderOperationList.Add(obj);
        }
        
        public void OnAssetEvent(DownloadUpdateData obj)
        {
            downloadUpdateDataDic[obj.PackageName] = obj;
            var curDownloadCount = 0;
            long curDownloadBytes = 0;
            foreach (var item in downloadUpdateDataDic)
            {
                curDownloadCount += item.Value.TotalDownloadCount;
                curDownloadBytes += item.Value.TotalDownloadBytes;
            }
            Debugger.Log($"已经下载了:{curDownloadCount}/{totalDownloadCount}  {curDownloadBytes}/{totalDownloadBytes}");
        }
        
        private void CheckDownAssets()
        {
            assetsPrepareFsmControllerCount++;
            if (assetsPrepareFsmControllerCount != YooConst.PackageSettings.Count) return;
            if (resourceDownloaderOperationList.Count == 0)
            {
                Next();
                return;
            }

            foreach (var t in resourceDownloaderOperationList)
            {
                SetData("packageName", t.PackageName);
                SetData("downloader", t);
                var fsm = (AssetsDownLoadFsmController) GXGameFrame.Instance.AddFsmChild(typeof(AssetsDownLoadFsmController));
                fsm.Start();
                fsmControllers.Add(fsm);
            }
        }
        
        private void Next()
        {
            fsmController.ChangeState<GameStartState>();
        }
    }
}