using FairyGUI;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class UITextItemLoop : LoopItem<string>
    {
        private FairyGUI.GTextField title;
        private FairyGUI.Controller c1;
        public override void OnAwake(GComponent com, ILoopList owner)
        {
            base.OnAwake(com,owner);
            title = (GTextField) com.GetChild("title");
            c1 = com.GetController("c1");
            com.onClick.Add(OnSelect);
        }

        public override void OnShow(int globalIndex, string data, bool selected)
        {
            base.OnShow(globalIndex, data, selected);
            title.text = data;
        }
        
        private void OnSelect()
        {
            Select();
        }
        
        protected override void OnSelectChanged()
        {
            c1.selectedIndex = Selected ? 1 : 0;
        }
    }
}