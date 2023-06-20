using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 技能开始释放
    /// </summary>
    [AssignBind(typeof(SkillManagerEntity))]
    public class OnSpellStartComponent : ECSComponent
    {
        public KeyCode KeyCode;
    }
}