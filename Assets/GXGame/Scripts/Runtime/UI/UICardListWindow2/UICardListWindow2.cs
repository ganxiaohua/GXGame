using Cysharp.Threading.Tasks;
using GameFrame;
using System;


namespace GXGame
{
    public class UICardListWindow2 : UIEntity
    {
        protected override string PackName => "Card";
        
        protected override string WindowName => "CardListWindow2";
        
        protected override Type ViewType => typeof(UICardListWindow2View);
        
        public override async UniTask OnInitialize()
        {
             await base.OnInitialize();
        }     
    }
}