using System;
using Cysharp.Threading.Tasks;
using GameFrame;

namespace GXGame
{
    public class UICardListWindow : UIEntity
    {
        protected override string PackName => "Card";
        
        protected override string WindowName => "CardListWindow";

        protected override Type ViewType => typeof(UICardListWindowView);
        
        public override async UniTask Initialize()
        {
            await base.Initialize();
            // UIManager.Instance.AddChildUI(typeof(UICardListWindow2),UINode, UICardListWindowView.n43).Forget();
        }
    }
}