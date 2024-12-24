using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack); 
    }

    public override bool UseSkill()
    {
        if (!base.UseSkill())
        {
            return false;
        }

        Debug.Log("Used clone skill");
        return true;
    }
}
