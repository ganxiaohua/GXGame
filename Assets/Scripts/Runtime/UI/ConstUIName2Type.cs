using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using Gameplay.Runtime;

namespace GamePlay.Runtime
{
    public static class ConstEasyUI
    {
        public static Dictionary<string, Type> UIDictionary;

        public static void Init()
        {
            UIDictionary = new();
            UIDictionary.Add(nameof(MainPanel), typeof(MainPanel));
            UIDictionary.Add(nameof(WorkbenchPanel), typeof(WorkbenchPanel));
        }

        public static void OpenUI(string name)
        {
            UIDictionary.TryGetValue(name, out var type);
            Assert.IsNotNull(type, $"name not have");
            UISystem.Instance.ShowUniquePanelAsync(type).Forget();
        }
    }
}