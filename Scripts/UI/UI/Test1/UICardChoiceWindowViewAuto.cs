using FairyGUI;
using GameFrame;
namespace GXGame
{
    public partial class UICardChoiceWindowView : UIViewBase
    {
        
        private GLoader m_Bg;
        private GLoader Bg
        {
            get {
                if (m_Bg == null)
                {
                    m_Bg = contentPane.GetChildByPath("Bg") as GLoader;
                }
                return m_Bg;
            }
        }
        private GComponent m_EffectRoot01;
        private GComponent EffectRoot01
        {
            get {
                if (m_EffectRoot01 == null)
                {
                    m_EffectRoot01 = contentPane.GetChildByPath("EffectRoot01") as GComponent;
                }
                return m_EffectRoot01;
            }
        }
        private GImage m_n356;
        private GImage n356
        {
            get {
                if (m_n356 == null)
                {
                    m_n356 = contentPane.GetChildByPath("n356") as GImage;
                }
                return m_n356;
            }
        }
        private GComponent m_ModelLoader;
        private GComponent ModelLoader
        {
            get {
                if (m_ModelLoader == null)
                {
                    m_ModelLoader = contentPane.GetChildByPath("ModelLoader") as GComponent;
                }
                return m_ModelLoader;
            }
        }
        private GImage m_Mask;
        private GImage Mask
        {
            get {
                if (m_Mask == null)
                {
                    m_Mask = contentPane.GetChildByPath("Mask") as GImage;
                }
                return m_Mask;
            }
        }
        private GImage m_n369;
        private GImage n369
        {
            get {
                if (m_n369 == null)
                {
                    m_n369 = contentPane.GetChildByPath("n369") as GImage;
                }
                return m_n369;
            }
        }
        private GList m_n9;
        private GList n9
        {
            get {
                if (m_n9 == null)
                {
                    m_n9 = contentPane.GetChildByPath("n9") as GList;
                }
                return m_n9;
            }
        }
        private GComponent m_FilterBtn;
        private GComponent FilterBtn
        {
            get {
                if (m_FilterBtn == null)
                {
                    m_FilterBtn = contentPane.GetChildByPath("FilterBtn") as GComponent;
                }
                return m_FilterBtn;
            }
        }
        private GButton m_FullBtn;
        private GButton FullBtn
        {
            get {
                if (m_FullBtn == null)
                {
                    m_FullBtn = contentPane.GetChildByPath("FullBtn") as GButton;
                }
                return m_FullBtn;
            }
        }
        private GImage m_FullBtn_n1;
        private GImage FullBtn_n1
        {
            get {
                if (m_FullBtn_n1 == null)
                {
                    m_FullBtn_n1 = contentPane.GetChildByPath("FullBtn.n1") as GImage;
                }
                return m_FullBtn_n1;
            }
        }
        private GImage m_FullBtn_n0;
        private GImage FullBtn_n0
        {
            get {
                if (m_FullBtn_n0 == null)
                {
                    m_FullBtn_n0 = contentPane.GetChildByPath("FullBtn.n0") as GImage;
                }
                return m_FullBtn_n0;
            }
        }
        private GButton m_ShowBtn;
        private GButton ShowBtn
        {
            get {
                if (m_ShowBtn == null)
                {
                    m_ShowBtn = contentPane.GetChildByPath("ShowBtn") as GButton;
                }
                return m_ShowBtn;
            }
        }
        private GImage m_ShowBtn_n1;
        private GImage ShowBtn_n1
        {
            get {
                if (m_ShowBtn_n1 == null)
                {
                    m_ShowBtn_n1 = contentPane.GetChildByPath("ShowBtn.n1") as GImage;
                }
                return m_ShowBtn_n1;
            }
        }
        private GImage m_ShowBtn_n0;
        private GImage ShowBtn_n0
        {
            get {
                if (m_ShowBtn_n0 == null)
                {
                    m_ShowBtn_n0 = contentPane.GetChildByPath("ShowBtn.n0") as GImage;
                }
                return m_ShowBtn_n0;
            }
        }
        private GComponent m_TitleLine01;
        private GComponent TitleLine01
        {
            get {
                if (m_TitleLine01 == null)
                {
                    m_TitleLine01 = contentPane.GetChildByPath("TitleLine01") as GComponent;
                }
                return m_TitleLine01;
            }
        }
        private GImage m_Line02;
        private GImage Line02
        {
            get {
                if (m_Line02 == null)
                {
                    m_Line02 = contentPane.GetChildByPath("Line02") as GImage;
                }
                return m_Line02;
            }
        }
        private GComponent m_CardChoiceComp03;
        private GComponent CardChoiceComp03
        {
            get {
                if (m_CardChoiceComp03 == null)
                {
                    m_CardChoiceComp03 = contentPane.GetChildByPath("CardChoiceComp03") as GComponent;
                }
                return m_CardChoiceComp03;
            }
        }
        private GImage m_CardChoiceComp03_CardBg;
        private GImage CardChoiceComp03_CardBg
        {
            get {
                if (m_CardChoiceComp03_CardBg == null)
                {
                    m_CardChoiceComp03_CardBg = contentPane.GetChildByPath("CardChoiceComp03.CardBg") as GImage;
                }
                return m_CardChoiceComp03_CardBg;
            }
        }
        private GComponent m_CardChoiceComp03_CardChoicePic;
        private GComponent CardChoiceComp03_CardChoicePic
        {
            get {
                if (m_CardChoiceComp03_CardChoicePic == null)
                {
                    m_CardChoiceComp03_CardChoicePic = contentPane.GetChildByPath("CardChoiceComp03.CardChoicePic") as GComponent;
                }
                return m_CardChoiceComp03_CardChoicePic;
            }
        }
        private GLoader m_CardChoiceComp03_CardChoicePic_n347;
        private GLoader CardChoiceComp03_CardChoicePic_n347
        {
            get {
                if (m_CardChoiceComp03_CardChoicePic_n347 == null)
                {
                    m_CardChoiceComp03_CardChoicePic_n347 = contentPane.GetChildByPath("CardChoiceComp03.CardChoicePic.n347") as GLoader;
                }
                return m_CardChoiceComp03_CardChoicePic_n347;
            }
        }
        private GImage m_CardChoiceComp03_CardChoicePic_n348;
        private GImage CardChoiceComp03_CardChoicePic_n348
        {
            get {
                if (m_CardChoiceComp03_CardChoicePic_n348 == null)
                {
                    m_CardChoiceComp03_CardChoicePic_n348 = contentPane.GetChildByPath("CardChoiceComp03.CardChoicePic.n348") as GImage;
                }
                return m_CardChoiceComp03_CardChoicePic_n348;
            }
        }
        private GImage m_CardChoiceComp03_Choice;
        private GImage CardChoiceComp03_Choice
        {
            get {
                if (m_CardChoiceComp03_Choice == null)
                {
                    m_CardChoiceComp03_Choice = contentPane.GetChildByPath("CardChoiceComp03.Choice") as GImage;
                }
                return m_CardChoiceComp03_Choice;
            }
        }
        private GImage m_CardChoiceComp03_Line;
        private GImage CardChoiceComp03_Line
        {
            get {
                if (m_CardChoiceComp03_Line == null)
                {
                    m_CardChoiceComp03_Line = contentPane.GetChildByPath("CardChoiceComp03.Line") as GImage;
                }
                return m_CardChoiceComp03_Line;
            }
        }
        private GImage m_CardChoiceComp03_TxtBg;
        private GImage CardChoiceComp03_TxtBg
        {
            get {
                if (m_CardChoiceComp03_TxtBg == null)
                {
                    m_CardChoiceComp03_TxtBg = contentPane.GetChildByPath("CardChoiceComp03.TxtBg") as GImage;
                }
                return m_CardChoiceComp03_TxtBg;
            }
        }
        private GTextField m_CardChoiceComp03_UseTxt;
        private GTextField CardChoiceComp03_UseTxt
        {
            get {
                if (m_CardChoiceComp03_UseTxt == null)
                {
                    m_CardChoiceComp03_UseTxt = contentPane.GetChildByPath("CardChoiceComp03.UseTxt") as GTextField;
                }
                return m_CardChoiceComp03_UseTxt;
            }
        }
        private GImage m_CardChoiceComp03_Locked;
        private GImage CardChoiceComp03_Locked
        {
            get {
                if (m_CardChoiceComp03_Locked == null)
                {
                    m_CardChoiceComp03_Locked = contentPane.GetChildByPath("CardChoiceComp03.Locked") as GImage;
                }
                return m_CardChoiceComp03_Locked;
            }
        }
        private GTextField m_CardChoiceComp03_LockTxt;
        private GTextField CardChoiceComp03_LockTxt
        {
            get {
                if (m_CardChoiceComp03_LockTxt == null)
                {
                    m_CardChoiceComp03_LockTxt = contentPane.GetChildByPath("CardChoiceComp03.LockTxt") as GTextField;
                }
                return m_CardChoiceComp03_LockTxt;
            }
        }
        private GImage m_CardChoiceComp03_Line1;
        private GImage CardChoiceComp03_Line1
        {
            get {
                if (m_CardChoiceComp03_Line1 == null)
                {
                    m_CardChoiceComp03_Line1 = contentPane.GetChildByPath("CardChoiceComp03.Line") as GImage;
                }
                return m_CardChoiceComp03_Line1;
            }
        }
        private GComponent m_CardChoiceComp02;
        private GComponent CardChoiceComp02
        {
            get {
                if (m_CardChoiceComp02 == null)
                {
                    m_CardChoiceComp02 = contentPane.GetChildByPath("CardChoiceComp02") as GComponent;
                }
                return m_CardChoiceComp02;
            }
        }
        private GImage m_CardChoiceComp02_CardBg;
        private GImage CardChoiceComp02_CardBg
        {
            get {
                if (m_CardChoiceComp02_CardBg == null)
                {
                    m_CardChoiceComp02_CardBg = contentPane.GetChildByPath("CardChoiceComp02.CardBg") as GImage;
                }
                return m_CardChoiceComp02_CardBg;
            }
        }
        private GComponent m_CardChoiceComp02_CardChoicePic;
        private GComponent CardChoiceComp02_CardChoicePic
        {
            get {
                if (m_CardChoiceComp02_CardChoicePic == null)
                {
                    m_CardChoiceComp02_CardChoicePic = contentPane.GetChildByPath("CardChoiceComp02.CardChoicePic") as GComponent;
                }
                return m_CardChoiceComp02_CardChoicePic;
            }
        }
        private GLoader m_CardChoiceComp02_CardChoicePic_n347;
        private GLoader CardChoiceComp02_CardChoicePic_n347
        {
            get {
                if (m_CardChoiceComp02_CardChoicePic_n347 == null)
                {
                    m_CardChoiceComp02_CardChoicePic_n347 = contentPane.GetChildByPath("CardChoiceComp02.CardChoicePic.n347") as GLoader;
                }
                return m_CardChoiceComp02_CardChoicePic_n347;
            }
        }
        private GImage m_CardChoiceComp02_CardChoicePic_n348;
        private GImage CardChoiceComp02_CardChoicePic_n348
        {
            get {
                if (m_CardChoiceComp02_CardChoicePic_n348 == null)
                {
                    m_CardChoiceComp02_CardChoicePic_n348 = contentPane.GetChildByPath("CardChoiceComp02.CardChoicePic.n348") as GImage;
                }
                return m_CardChoiceComp02_CardChoicePic_n348;
            }
        }
        private GImage m_CardChoiceComp02_Choice;
        private GImage CardChoiceComp02_Choice
        {
            get {
                if (m_CardChoiceComp02_Choice == null)
                {
                    m_CardChoiceComp02_Choice = contentPane.GetChildByPath("CardChoiceComp02.Choice") as GImage;
                }
                return m_CardChoiceComp02_Choice;
            }
        }
        private GImage m_CardChoiceComp02_Line;
        private GImage CardChoiceComp02_Line
        {
            get {
                if (m_CardChoiceComp02_Line == null)
                {
                    m_CardChoiceComp02_Line = contentPane.GetChildByPath("CardChoiceComp02.Line") as GImage;
                }
                return m_CardChoiceComp02_Line;
            }
        }
        private GImage m_CardChoiceComp02_TxtBg;
        private GImage CardChoiceComp02_TxtBg
        {
            get {
                if (m_CardChoiceComp02_TxtBg == null)
                {
                    m_CardChoiceComp02_TxtBg = contentPane.GetChildByPath("CardChoiceComp02.TxtBg") as GImage;
                }
                return m_CardChoiceComp02_TxtBg;
            }
        }
        private GTextField m_CardChoiceComp02_UseTxt;
        private GTextField CardChoiceComp02_UseTxt
        {
            get {
                if (m_CardChoiceComp02_UseTxt == null)
                {
                    m_CardChoiceComp02_UseTxt = contentPane.GetChildByPath("CardChoiceComp02.UseTxt") as GTextField;
                }
                return m_CardChoiceComp02_UseTxt;
            }
        }
        private GImage m_CardChoiceComp02_Locked;
        private GImage CardChoiceComp02_Locked
        {
            get {
                if (m_CardChoiceComp02_Locked == null)
                {
                    m_CardChoiceComp02_Locked = contentPane.GetChildByPath("CardChoiceComp02.Locked") as GImage;
                }
                return m_CardChoiceComp02_Locked;
            }
        }
        private GTextField m_CardChoiceComp02_LockTxt;
        private GTextField CardChoiceComp02_LockTxt
        {
            get {
                if (m_CardChoiceComp02_LockTxt == null)
                {
                    m_CardChoiceComp02_LockTxt = contentPane.GetChildByPath("CardChoiceComp02.LockTxt") as GTextField;
                }
                return m_CardChoiceComp02_LockTxt;
            }
        }
        private GImage m_CardChoiceComp02_Line2;
        private GImage CardChoiceComp02_Line2
        {
            get {
                if (m_CardChoiceComp02_Line2 == null)
                {
                    m_CardChoiceComp02_Line2 = contentPane.GetChildByPath("CardChoiceComp02.Line") as GImage;
                }
                return m_CardChoiceComp02_Line2;
            }
        }
        private GComponent m_CardChoiceComp01;
        private GComponent CardChoiceComp01
        {
            get {
                if (m_CardChoiceComp01 == null)
                {
                    m_CardChoiceComp01 = contentPane.GetChildByPath("CardChoiceComp01") as GComponent;
                }
                return m_CardChoiceComp01;
            }
        }
        private GImage m_CardChoiceComp01_CardBg;
        private GImage CardChoiceComp01_CardBg
        {
            get {
                if (m_CardChoiceComp01_CardBg == null)
                {
                    m_CardChoiceComp01_CardBg = contentPane.GetChildByPath("CardChoiceComp01.CardBg") as GImage;
                }
                return m_CardChoiceComp01_CardBg;
            }
        }
        private GComponent m_CardChoiceComp01_CardChoicePic;
        private GComponent CardChoiceComp01_CardChoicePic
        {
            get {
                if (m_CardChoiceComp01_CardChoicePic == null)
                {
                    m_CardChoiceComp01_CardChoicePic = contentPane.GetChildByPath("CardChoiceComp01.CardChoicePic") as GComponent;
                }
                return m_CardChoiceComp01_CardChoicePic;
            }
        }
        private GLoader m_CardChoiceComp01_CardChoicePic_n347;
        private GLoader CardChoiceComp01_CardChoicePic_n347
        {
            get {
                if (m_CardChoiceComp01_CardChoicePic_n347 == null)
                {
                    m_CardChoiceComp01_CardChoicePic_n347 = contentPane.GetChildByPath("CardChoiceComp01.CardChoicePic.n347") as GLoader;
                }
                return m_CardChoiceComp01_CardChoicePic_n347;
            }
        }
        private GImage m_CardChoiceComp01_CardChoicePic_n348;
        private GImage CardChoiceComp01_CardChoicePic_n348
        {
            get {
                if (m_CardChoiceComp01_CardChoicePic_n348 == null)
                {
                    m_CardChoiceComp01_CardChoicePic_n348 = contentPane.GetChildByPath("CardChoiceComp01.CardChoicePic.n348") as GImage;
                }
                return m_CardChoiceComp01_CardChoicePic_n348;
            }
        }
        private GImage m_CardChoiceComp01_Choice;
        private GImage CardChoiceComp01_Choice
        {
            get {
                if (m_CardChoiceComp01_Choice == null)
                {
                    m_CardChoiceComp01_Choice = contentPane.GetChildByPath("CardChoiceComp01.Choice") as GImage;
                }
                return m_CardChoiceComp01_Choice;
            }
        }
        private GImage m_CardChoiceComp01_Line;
        private GImage CardChoiceComp01_Line
        {
            get {
                if (m_CardChoiceComp01_Line == null)
                {
                    m_CardChoiceComp01_Line = contentPane.GetChildByPath("CardChoiceComp01.Line") as GImage;
                }
                return m_CardChoiceComp01_Line;
            }
        }
        private GImage m_CardChoiceComp01_TxtBg;
        private GImage CardChoiceComp01_TxtBg
        {
            get {
                if (m_CardChoiceComp01_TxtBg == null)
                {
                    m_CardChoiceComp01_TxtBg = contentPane.GetChildByPath("CardChoiceComp01.TxtBg") as GImage;
                }
                return m_CardChoiceComp01_TxtBg;
            }
        }
        private GTextField m_CardChoiceComp01_UseTxt;
        private GTextField CardChoiceComp01_UseTxt
        {
            get {
                if (m_CardChoiceComp01_UseTxt == null)
                {
                    m_CardChoiceComp01_UseTxt = contentPane.GetChildByPath("CardChoiceComp01.UseTxt") as GTextField;
                }
                return m_CardChoiceComp01_UseTxt;
            }
        }
        private GImage m_CardChoiceComp01_Locked;
        private GImage CardChoiceComp01_Locked
        {
            get {
                if (m_CardChoiceComp01_Locked == null)
                {
                    m_CardChoiceComp01_Locked = contentPane.GetChildByPath("CardChoiceComp01.Locked") as GImage;
                }
                return m_CardChoiceComp01_Locked;
            }
        }
        private GTextField m_CardChoiceComp01_LockTxt;
        private GTextField CardChoiceComp01_LockTxt
        {
            get {
                if (m_CardChoiceComp01_LockTxt == null)
                {
                    m_CardChoiceComp01_LockTxt = contentPane.GetChildByPath("CardChoiceComp01.LockTxt") as GTextField;
                }
                return m_CardChoiceComp01_LockTxt;
            }
        }
        private GImage m_CardChoiceComp01_Line3;
        private GImage CardChoiceComp01_Line3
        {
            get {
                if (m_CardChoiceComp01_Line3 == null)
                {
                    m_CardChoiceComp01_Line3 = contentPane.GetChildByPath("CardChoiceComp01.Line") as GImage;
                }
                return m_CardChoiceComp01_Line3;
            }
        }
        private GComponent m_ComLimitTishi;
        private GComponent ComLimitTishi
        {
            get {
                if (m_ComLimitTishi == null)
                {
                    m_ComLimitTishi = contentPane.GetChildByPath("ComLimitTishi") as GComponent;
                }
                return m_ComLimitTishi;
            }
        }
        private GComponent m_UseHomeBtn;
        private GComponent UseHomeBtn
        {
            get {
                if (m_UseHomeBtn == null)
                {
                    m_UseHomeBtn = contentPane.GetChildByPath("UseHomeBtn") as GComponent;
                }
                return m_UseHomeBtn;
            }
        }
        private GComponent m_UseBtn;
        private GComponent UseBtn
        {
            get {
                if (m_UseBtn == null)
                {
                    m_UseBtn = contentPane.GetChildByPath("UseBtn") as GComponent;
                }
                return m_UseBtn;
            }
        }
        private GComponent m_AssetsTop;
        private GComponent AssetsTop
        {
            get {
                if (m_AssetsTop == null)
                {
                    m_AssetsTop = contentPane.GetChildByPath("AssetsTop") as GComponent;
                }
                return m_AssetsTop;
            }
        }
    }
}
