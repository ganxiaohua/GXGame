using Cysharp.Threading.Tasks;
using GameFrame;
using System;


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
    }
}