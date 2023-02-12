using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public enum ESkillPassiveName
{
    NONE,
    GATHER_TOGETHER,
}
public class SkillPassiveFactory : Singleton<SkillPassiveFactory>
{
    public ASkillPassiveBase GetSkill(ESkillPassiveName skilName)
    {
        switch (skilName)
        {
            case ESkillPassiveName.GATHER_TOGETHER: return new SkillGatherTogether();
            default:  Assert.IsTrue(true,
                    string.Format(
                        "Get Skill instace of {0}'s case is undefined check SkillPassiveFactory class's function GetSkill",
                        skilName.ToString()));
                return null;
        }
    }
}
