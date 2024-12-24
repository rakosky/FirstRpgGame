using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance { get; private set; }

    #region Skills
    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }
    public SwordThrowSkill swordThrow { get; private set; }
    #endregion

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        swordThrow = GetComponent<SwordThrowSkill>();
    }
}
