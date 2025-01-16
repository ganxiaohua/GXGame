using System.Collections.Generic;

namespace GXGame.Data
{
    /// <summary>
    /// 模拟配表数据
    /// </summary>
    public class SkillData : Singleton<SkillData>
    {
        public Dictionary<int, string> skillIdWith;

        public Dictionary<int, float> skillCD;
        
        public SkillData()
        {
            skillIdWith = new Dictionary<int, string>();
            skillIdWith.Add(1,"Skill_1");
            skillIdWith.Add(2,"Skill_2");
            skillCD = new Dictionary<int, float>();
            skillCD.Add(1,5);
            skillCD.Add(1,7);
        }
    }
}