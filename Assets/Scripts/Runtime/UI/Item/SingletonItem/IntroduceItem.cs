using Cysharp.Threading.Tasks;
using FairyGUI;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class IntroduceItem : ISingletonItem, IVersions
    {
        private GComponent root;
        private GTextField titleText;
        private GTextField text;
        private GComponent parent;

        public int Versions { get; private set; }


        public async UniTask ShowItem(UnitItem unitid, GComponent parent)
        {
            var ver = ++Versions;
            root ??= await UISystem.Instance.CreateGObjectAsync(PackageName.Common, "IntroduceItem", false) as GComponent;
            if (root == null)
                return;
            if (ver != Versions)
            {
                root.Dispose();
                return;
            }

            titleText ??= (GTextField) root.GetChild("n3");
            text ??= (GTextField) root.GetChild("n5");
            parent.AddChild(root);
            root.SetXY(parent.width - root.width - 20, parent.height - 200 - root.height);
            ShowDoc(unitid);
        }


        public void Hide()
        {
            root.visible = false;
        }

        public void Destroy()
        {
            root?.Dispose();
            root = null;
            Versions++;
        }


        private void ShowDoc(UnitItem unitId)
        {
            titleText.text = unitId.Name;
            text.text = unitId.Doc;
            root.visible = true;
        }
    }
}