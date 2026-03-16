namespace InTerra
{
    public static class InTerra_Setting
    {
        static public bool DisableAllAutoUpdates = false; //This will cause that there will be no auto updates at all.

        static public bool DictionaryUpdate = true; //When false the update at various events will be sending the Terrains data just to Materials that are included in the dictionary. Dictionary update require check on all renderers and will be updated only via click on the "Update Terrain Data" in Object GUI or "Update Terrain Data for Objects" in Terrain GUI.

        static public bool ObjectGUICheckAndUpdateAtOpen = true; //At opening of GUI for Object shader there will be check if any render with this material is on wrong Terrain or outside of Terrain, if this bool is false this check will be done only if you open the Objects Info foldout.
    }
}
