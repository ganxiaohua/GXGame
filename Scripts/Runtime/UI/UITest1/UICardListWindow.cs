using System;
using System.Collections.Generic;
using GameFrame;
namespace GXGame
{
    public class UICardListWindow : Entity, IStart,IPreShow,IShow,IHide,IUpdate,IClear
    {
          public UICardListWindowView UICardListWindowView;
          public string PackName = "Card";
          public string WindowName = "CardListWindow";
    }
}
