using System;
using FairyGUI;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    //ToolItem
    //MaterialItem
    public class UIItem : IDisposable
    {
        public GComponent Root { get; private set; }
        protected GLoader GLoaderIcon;
        private FairyGUI.Controller c1;
        protected FairyGUI.GTextField TexCount;
        public BagData.BagType Type;
        protected int DataIndex;
        protected DefaultAssetReference AssetReference;

        public virtual void Init(GComponent item, DefaultAssetReference assetReference)
        {
            this.Root = item;
            GLoaderIcon = (GLoader) item.GetChild("Icon");
            c1 = item.GetController("c1");
            TexCount = (GTextField) item.GetChild("Number");
            this.AssetReference = assetReference;
            Type = BagData.BagType.Pocket;
            Visible(true);
        }

        public void Show(ItemInfo itemCount)
        {
            if (itemCount != null)
            {
                GLoaderIcon.Load(PackageName.Common, itemCount.Item.UIIconName, AssetReference);
                if (itemCount.Count == 0 || !itemCount.Item.IsStack)
                {
                    TexCount.text = string.Empty;
                }
                else
                {
                    TexCount.text = itemCount.Count.ToString();
                }
            }
            else
            {
                GLoaderIcon.url = null;
                TexCount.text = string.Empty;
            }
        }

        public void Show(ItemCount itemCount)
        {
            if (itemCount != null)
            {
                GLoaderIcon.Load(PackageName.Common, itemCount.Item_Ref.UIIconName, AssetReference);
                if (itemCount.Count == 0 || !itemCount.Item_Ref.IsStack)
                {
                    TexCount.text = string.Empty;
                }
                else
                {
                    TexCount.text = itemCount.Count.ToString();
                }
            }
            else
            {
                GLoaderIcon.url = null;
                TexCount.text = string.Empty;
            }
        }

        public void Show(Item item)
        {
            if (item != null)
            {
                GLoaderIcon.Load(PackageName.Common, item.UIIconName, AssetReference);
                TexCount.text = string.Empty;
            }
            else
            {
                GLoaderIcon.url = null;
                TexCount.text = string.Empty;
            }
        }

        public void SetIcon(string iconName)
        {
            GLoaderIcon.Load(PackageName.Common, iconName, AssetReference);
        }

        public void SetCount(string str)
        {
            TexCount.text = str;
        }

        public void SetColor(Color color)
        {
            TexCount.color = color;
        }

        public void SetDataIndex(int index)
        {
            DataIndex = index;
        }


        public void Selet(bool b)
        {
            c1.selectedIndex = b ? 1 : 0;
        }

        public void Visible(bool show)
        {
            GLoaderIcon.visible = show;
            TexCount.visible = show;
        }

        public void ClearData()
        {
            GLoaderIcon.url = null;
            TexCount.text = string.Empty;
        }

        public void Dispose()
        {
        }
    }
}