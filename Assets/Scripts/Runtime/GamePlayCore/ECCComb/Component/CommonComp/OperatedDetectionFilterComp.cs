using System.Collections.Generic;
using GameFrame.Runtime;
using GamePlay.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public struct DetectionFilter
    {
        public CampType Camp;
        public int EntityId;

        public DetectionFilter(CampType camp, int entityId = -1)
        {
            Camp = camp;
            EntityId = entityId;
        }
    }

    /// <summary>
    /// 使用碰撞过滤掉的类型
    /// </summary>
    public struct OperatedDetectionFilterComp : EffComponent
    {
        public DetectionFilter Value;

        public void Dispose()
        {
        }
    }
}