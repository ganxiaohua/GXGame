using FairyGUI;
using GameFrame;
namespace GXGame
{
    public partial class UIHomeMainPanelView : UIViewBase
    {
        
        private GButton m_n42;
        private GButton n42
        {
            get {
                if (m_n42 == null)
                {
                    m_n42 = contentPane.GetChildByPath("n42") as GButton;
                }
                return m_n42;
            }
        }
        private GImage m_n42_n0;
        private GImage n42_n0
        {
            get {
                if (m_n42_n0 == null)
                {
                    m_n42_n0 = contentPane.GetChildByPath("n42.n0") as GImage;
                }
                return m_n42_n0;
            }
        }
        private GButton m_n43;
        private GButton n43
        {
            get {
                if (m_n43 == null)
                {
                    m_n43 = contentPane.GetChildByPath("n43") as GButton;
                }
                return m_n43;
            }
        }
        private GImage m_n43_n0;
        private GImage n43_n0
        {
            get {
                if (m_n43_n0 == null)
                {
                    m_n43_n0 = contentPane.GetChildByPath("n43.n0") as GImage;
                }
                return m_n43_n0;
            }
        }
    }
}
