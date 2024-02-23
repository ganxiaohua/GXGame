using FairyGUI;
using GameFrame;
namespace GXGame
{
    public partial class UICardListWindowView2 : UIViewBase
    {
           
        private GButton m_n43;
        public GButton n43
        {
            get {
                if (m_n43 == null)
                {
                    m_n43 = Root.GetChildByPath("n43") as GButton;
                }
                return m_n43;
            }
        }
        private GImage m_n43_n0;
        public GImage n43_n0
        {
            get {
                if (m_n43_n0 == null)
                {
                    m_n43_n0 = Root.GetChildByPath("n43.n0") as GImage;
                }
                return m_n43_n0;
            }
        }
        private GButton m_n44;
        public GButton n44
        {
            get {
                if (m_n44 == null)
                {
                    m_n44 = Root.GetChildByPath("n44") as GButton;
                }
                return m_n44;
            }
        }
        private GImage m_n44_n0;
        public GImage n44_n0
        {
            get {
                if (m_n44_n0 == null)
                {
                    m_n44_n0 = Root.GetChildByPath("n44.n0") as GImage;
                }
                return m_n44_n0;
            }
        }
        private GButton m_n45;
        public GButton n45
        {
            get {
                if (m_n45 == null)
                {
                    m_n45 = Root.GetChildByPath("n45") as GButton;
                }
                return m_n45;
            }
        }
        private GImage m_n45_n0;
        public GImage n45_n0 { get { return m_n45_n0 ??= Root.GetChildByPath("n45.n0") as GImage; } }     
    }
}
