using System;
using System.Collections.Generic;
using GameFrame;
namespace GXGame
{
    public class UICardListWindow2 : Entity, IStart,IPreShow,IShow,IHide,IUpdate,IClear
    {
          public UICardListWindowView2 UICardListWindowView;
          public string PackName = "Card";
          public string WindowName = "CardListWindow2";
    }
}
