
using UnityEngine;


public class DashSkill : Skill
{
    public override bool UseSkill()
    {
        if (!base.UseSkill())
        {
            return false;
        }

        return true;
    }
}
