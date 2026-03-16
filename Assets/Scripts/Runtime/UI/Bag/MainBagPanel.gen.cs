using FairyGUI;

namespace Gameplay.Runtime
{
    public partial class MainBagPanel
    {
		private GLoader n10;
		private GButton n8;
		//private GImage n8_n3;
		//private GImage n8_n2;
		//private GImage n8_n1;
		//private GImage n8_n0;
		private GComponent n4;
		private GList n4_n7;
		private GList n4_n6;
		//private GImage n4_n4;

        private void InitializeComponents(GComponent root)
        {
			n10 = (GLoader)root.GetChildAt(2);
			n8 = (GButton)root.GetChildAt(1);
			//n8_n3 = (GImage)n8.GetChildAt(3);
			//n8_n2 = (GImage)n8.GetChildAt(2);
			//n8_n1 = (GImage)n8.GetChildAt(1);
			//n8_n0 = (GImage)n8.GetChildAt(0);
			n4 = (GComponent)root.GetChildAt(0);
			n4_n7 = (GList)n4.GetChildAt(2);
			n4_n6 = (GList)n4.GetChildAt(1);
			//n4_n4 = (GImage)n4.GetChildAt(0);

        }
    }
}