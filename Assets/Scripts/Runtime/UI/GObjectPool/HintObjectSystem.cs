using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using GameFrame.Runtime.Runtime.Timer;

namespace GamePlay.Runtime
{
    public class HintObjectSystem : Singleton<HintObjectSystem>
    {
        private ObjectPool<HintObject> objectPool;
        private Queue<ItemInfo> strList;
        private WaitTime waitTime;
        private bool isSqawn;

        public HintObjectSystem()
        {
            strList = new Queue<ItemInfo>();
            objectPool = GObjectPools.Instance.Spawn<HintObject>(new GObjectBaseData(PackageName.Common, "ComHint", false));
            waitTime = new WaitTime();
            isSqawn = false;
        }

        public void Set(int id, int count)
        {
            var item = Tables.Instance.ItemTable.Get(id);
            var showItem = ReferencePool.Acquire<ItemInfo>();
            showItem.Item = item;
            showItem.Count = count;
            strList.Enqueue(showItem);
            if (isSqawn)
                return;
            SqawnItem().Forget();
        }

        private async UniTask SqawnItem()
        {
            while (strList.Count > 0)
            {
                isSqawn = true;
                objectPool.Spawn(strList.Dequeue());
                await waitTime.WaitSecs(0.3f);
            }

            isSqawn = false;
        }
    }
}