using Cysharp.Threading.Tasks;
using GameFrame;
using System;
using GameFrame.Runtime.Timer;


namespace GXGame
{
    public class UICardListWindow : UIEntity
    {
        protected override string PackName => "Card";

        protected override string WindowName => "CardListWindow";

        protected override Type ViewType => typeof(UICardListWindowView);

        public override async UniTask OnInitialize()
        {
            await base.OnInitialize();
        }

        public override void OnPreShow(bool isFirstShow)
        {
            if (!isFirstShow) return;
            this.AddComponent<WaitComponent>();
            WaitServer().Forget();
        }

        /// <summary>
        /// 模拟和服务器通讯
        /// </summary>
        private async UniTaskVoid WaitServer()
        {
            //这个等待只是模拟服务器,其实是不用加的,相当于接受到服务器请求之后调用waitcomponent的waitover
            var waitTime = ReferencePool.Acquire<WaitTime>();
            await waitTime.WaitSec(2.0f);
            this.GetComponent<WaitComponent>().WaitOver();
            this.RemoveComponent<WaitComponent>();
            ReferencePool.Release(waitTime);
        }
    }
}