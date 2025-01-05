using System.Collections;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool canCreateCloneOnDashStart;
    [SerializeField] private bool canCreateCloneOnDashEnd;
    [SerializeField] private bool canCreateCloneOnCounterattack;
    [SerializeField] private bool canReproc;
    [SerializeField] private float chanceToReproc;

    public bool crystalInsteadOfClone;

    private float counterAttackCloneSpawnDelay = .4f;

    public void CreateClone(Transform clonePosition, Vector3? offset = null)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack, canReproc, chanceToReproc, FindClosestEnemy(newClone.transform, offset), offset); 
    }

    public override bool UseSkill()
    {
        if (!base.UseSkill())
        {
            return false;
        }

        return true;
    }

    public void CreateCloneOnDashStart()
    {
        if (canCreateCloneOnDashStart)
        {
            CreateClone(player.transform);
        }
    }
    public void CreateCloneOnDashEnd()
    {
        if (canCreateCloneOnDashStart)
        {
            CreateClone(player.transform);
        }
    }

    public void CreateCloneOnCounterattack(Transform enemyTransform)
    {
        if (canCreateCloneOnCounterattack)
        {
            StartCoroutine(CreateCloneWithDelay(counterAttackCloneSpawnDelay, enemyTransform, new Vector3(player.facingDirection * 2, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(float seconds, Transform clonePosition, Vector3? offset = null)
    {
        yield return new WaitForSeconds(seconds);
        CreateClone(clonePosition, offset);
    }
}
