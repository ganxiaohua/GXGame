using Cysharp.Threading.Tasks;
using GameFrame;
using System;


namespace GXGame
{
    public class UICardListWindow : UIEntity,ITestEvent1
    {
        protected override string PackName => "Card";
        
        protected override string WindowName => "CardListWindow";
        
        protected override Type ViewType => typeof(UICardListWindowView);
        
        public override async UniTask OnInitialize()
        {
             await base.OnInitialize();
        }
        
        public void Test(string data)
        {
            Debugger.Log(data);
        }
    }
}