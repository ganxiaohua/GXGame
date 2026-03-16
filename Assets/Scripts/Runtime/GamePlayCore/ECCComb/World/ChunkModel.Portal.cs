namespace GamePlay.Runtime
{
    public partial class ChunkModel
    {
        private void ChangePortal()
        {
            if (chunk == null)
                return;
            if (chunk.ProtalUnit != null)
            {
                foreach (var t in chunk.ProtalUnit)
                {
                    var entity = ConstCreateEntitys.CreateMapPortalUnit(mapArea.EccWorld, t, mapArea.Map.transform, RemoveUnit, ConstBeOperated.BeOperated_Transmit);
                    mapUnit.Set(t.Index, entity);
                }
            }
        }
    }
}