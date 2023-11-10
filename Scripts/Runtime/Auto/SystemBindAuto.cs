using System;
using System.Collections.Generic;
using GameFrame;
using GXGame;
public partial class AutoBindSystem
{
    public void AddSystem()
    {
       var systemBind = BindSystem.Instance.SystemBind;
        
        systemBind.Add(typeof(AssetInitComponent),typeof(IStartSystem),typeof(GameFrame.AssetInitComponentSystem.AssetInitComponentStartSystem));
        systemBind.Add(typeof(AssetInitComponent),typeof(IUpdateSystem),typeof(GameFrame.AssetInitComponentSystem.AssetInitComponentUpdateSystem));
        systemBind.Add(typeof(AssetInitComponent),typeof(IClearSystem),typeof(GameFrame.AssetInitComponentSystem.AssetInitComponentClearSystem));
        systemBind.Add(typeof(MainScene),typeof(IStartSystem<Type>),typeof(GameFrame.SceneEntitySystem.SceneEntityStartSystem));
        systemBind.Add(typeof(MainScene),typeof(IUpdateSystem),typeof(GameFrame.SceneEntitySystem.SceneEntityUpdateSystem));
        systemBind.Add(typeof(MainScene),typeof(IClearSystem),typeof(GameFrame.SceneEntitySystem.SceneEntityClearSystem));
        systemBind.Add(typeof(WaitComponent),typeof(IStartSystem<Type>),typeof(GameFrame.WaitComponentSystem.WaitComponentStartSystem));
        systemBind.Add(typeof(WaitComponent),typeof(IClearSystem),typeof(GameFrame.WaitComponentSystem.WaitComponentClearSystem));
        systemBind.Add(typeof(DependentResources),typeof(IStartSystem<String>),typeof(GameFrame.DependentResourcesSystem.DependentResourcesStartSystem));
        systemBind.Add(typeof(DependentResources),typeof(IClearSystem),typeof(GameFrame.DependentResourcesSystem.DependentResourcesClearSystem));
        systemBind.Add(typeof(DependentUIResources),typeof(IStartSystem<List<String>>),typeof(GameFrame.DependentUIResourcesSystem.DependentUIResourcesStartSystem));
        systemBind.Add(typeof(DependentUIResources),typeof(IClearSystem),typeof(GameFrame.DependentUIResourcesSystem.DependentUIResourcesClearSystem));
        systemBind.Add(typeof(GameObjectPoolComponent),typeof(IStartSystem),typeof(GameFrame.GameObjectPoolComponentSystem.GameObjectPoolComponentStartSystem));
        systemBind.Add(typeof(GameObjectPoolComponent),typeof(IClearSystem),typeof(GameFrame.GameObjectPoolComponentSystem.GameObjectPoolComponentClearSystem));
        systemBind.Add(typeof(CubeScene),typeof(IStartSystem),typeof(GXGame.CubeSceneSystem.CubeSceneStartSystem));
        systemBind.Add(typeof(CubeScene),typeof(IShowSystem),typeof(GXGame.CubeSceneSystem.CubeSceneShowSystem));
        systemBind.Add(typeof(CubeScene),typeof(IHideSystem),typeof(GXGame.CubeSceneSystem.CubeSceneHideSystem));
        systemBind.Add(typeof(CubeScene),typeof(IUpdateSystem),typeof(GXGame.CubeSceneSystem.CubeSceneUpdateSystem));
        systemBind.Add(typeof(CubeScene),typeof(IClearSystem),typeof(GXGame.CubeSceneSystem.CubeSceneClearSystem));
        systemBind.Add(typeof(UICardListWindowData),typeof(IStartSystem),typeof(GXGame.UICardListWindowDataSystem.UICardListWindowDataStartSystem));
        systemBind.Add(typeof(UICardListWindowData),typeof(IUpdateSystem),typeof(GXGame.UICardListWindowDataSystem.UICardListWindowDataUpdateSystem));
        systemBind.Add(typeof(UICardListWindow),typeof(IStartSystem),typeof(GXGame.UICardListWindowSystem.UICardListWindowStartSystem));
        systemBind.Add(typeof(UICardListWindow),typeof(IPreShowSystem),typeof(GXGame.UICardListWindowSystem.UICardListWindowPreShowSystem));
        systemBind.Add(typeof(UICardListWindow),typeof(IShowSystem),typeof(GXGame.UICardListWindowSystem.UICardListWindowShowSystem));
        systemBind.Add(typeof(UICardListWindow),typeof(IHideSystem),typeof(GXGame.UICardListWindowSystem.UICardListWindowHideSystem));
        systemBind.Add(typeof(UICardListWindow),typeof(IUpdateSystem),typeof(GXGame.UICardListWindowSystem.UICardListWindowUpdateSystem));
        systemBind.Add(typeof(UICardListWindow),typeof(IClearSystem),typeof(GXGame.UICardListWindowSystem.UICardListWindowClearSystem));
        systemBind.Add(typeof(UICardListWindow2Data),typeof(IStartSystem),typeof(GXGame.UICardListWindow2DataSystem.UICardListWindow2DataStartSystem));
        systemBind.Add(typeof(UICardListWindow2Data),typeof(IUpdateSystem),typeof(GXGame.UICardListWindow2DataSystem.UICardListWindow2DataUpdateSystem));
        systemBind.Add(typeof(UICardListWindow2),typeof(IStartSystem),typeof(GXGame.UICardListWindow2System.UICardListWindow2StartSystem));
        systemBind.Add(typeof(UICardListWindow2),typeof(IPreShowSystem),typeof(GXGame.UICardListWindow2System.UICardListWindow2PreShowSystem));
        systemBind.Add(typeof(UICardListWindow2),typeof(IShowSystem),typeof(GXGame.UICardListWindow2System.UICardListWindow2ShowSystem));
        systemBind.Add(typeof(UICardListWindow2),typeof(IHideSystem),typeof(GXGame.UICardListWindow2System.UICardListWindow2HideSystem));
        systemBind.Add(typeof(UICardListWindow2),typeof(IUpdateSystem),typeof(GXGame.UICardListWindow2System.UICardListWindow2UpdateSystem));
        systemBind.Add(typeof(UICardListWindow2),typeof(IClearSystem),typeof(GXGame.UICardListWindow2System.UICardListWindow2ClearSystem));
    }
}

