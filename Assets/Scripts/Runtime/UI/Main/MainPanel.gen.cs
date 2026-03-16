using FairyGUI;

namespace Gameplay.Runtime
{
    public partial class MainPanel
    {
		private GRichTextField Time;
		private GComponent n3;
		private GProgressBar n3_n9;
		//private GImage n3_n9_bar;
		//private GImage n3_n9_n0;
		private GProgressBar n3_n8;
		//private GImage n3_n8_bar;
		//private GImage n3_n8_n0;
		//private GImage n3_n3;
		private GList n0;

        private void InitializeComponents(GComponent root)
        {
			Time = (GRichTextField)root.GetChildAt(2);
			n3 = (GComponent)root.GetChildAt(1);
			n3_n9 = (GProgressBar)n3.GetChildAt(2);
			//n3_n9_bar = (GImage)n3_n9.GetChildAt(1);
			//n3_n9_n0 = (GImage)n3_n9.GetChildAt(0);
			n3_n8 = (GProgressBar)n3.GetChildAt(1);
			//n3_n8_bar = (GImage)n3_n8.GetChildAt(1);
			//n3_n8_n0 = (GImage)n3_n8.GetChildAt(0);
			//n3_n3 = (GImage)n3.GetChildAt(0);
			n0 = (GList)root.GetChildAt(0);

        }
    }
}