namespace GXGame
{
    public interface IWolrdPosition
    {
        public void WolrdPosition(WorldPos worldPos);
    }

    public interface IWorldRotate
    {
        public void WorldRotate(WorldRotate worldRotate);
    }
    
    
    public interface ILocalPosition
    {
        public void LocalPosition(LocalPos localPos);
    }

    public interface ILocalRotate
    {
        public void LocalRotate(LocalRotate localRotate);
    }

    public interface ILocalScale
    {
        public void LocalScale(LocalScale localScale);
    }

    public interface IMeshRendererColor
    {
        public void MeshRendererColor(MeshRendererColor meshRendererColor); 
    }

}