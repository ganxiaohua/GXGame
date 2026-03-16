using UnityEngine;

namespace GamePlay.Runtime
{
    public interface IGameData
    {
        public void Initialization();

        public void ShutDown();
    }
}