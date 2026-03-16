using Cysharp.Threading.Tasks;
using FairyGUI;
using GamePlay.Runtime;

namespace Gameplay.Runtime
{
    public partial class MainPanel
    {
        private IntroduceItem introduceItem;

        private void ShowIntroduceItem(UnitItem UnitId, GComponent parent)
        {
            introduceItem ??= new IntroduceItem();
            introduceItem.ShowItem(UnitId, parent).Forget();
        }

        private void HideIntroduceItem()
        {
            introduceItem?.Hide();
        }

        private void DestroyIntroduceItem()
        {
            introduceItem?.Destroy();
            introduceItem = null;
        }

        private void DestroySingeItem()
        {
            DestroyIntroduceItem();
        }
    }
}