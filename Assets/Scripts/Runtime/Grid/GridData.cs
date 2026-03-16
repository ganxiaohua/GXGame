using System;
using GamePlay.Runtime.MapData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Runtime
{
    [RequireComponent(typeof(Transform))]
    [Serializable]
    public partial class GridData : MonoBehaviour
    {
        [ShowInInspector]
        public CroplandDataBase CroplandData = new CroplandDataBase();

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                CroplandData.lPos = transform.position;
                CroplandData.lRot = transform.rotation;
                CroplandData.lScale = transform.localScale;
            }
        }
    }
}