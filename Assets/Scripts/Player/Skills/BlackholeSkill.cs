using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float transformSpeed;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private int numberOfAttacks;
    [SerializeField] private int maxTargets;
    [SerializeField] private int maxSetupLifetime;
    BlackholeSkillController blackholeController;

    public override bool CanUse()
    {
        return base.CanUse();
    }

    public override bool UseSkill()
    {
        if (!base.UseSkill())
            return false;

        var blackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        blackholeController = blackHole.GetComponent<BlackholeSkillController>();
        blackholeController.Setup(maxSize, transformSpeed, cloneAttackCooldown, numberOfAttacks, maxTargets, maxSetupLifetime);

        return true;

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillFinished()
    {
        if (blackholeController && blackholeController.playerCanExitState)
        {
            blackholeController = null;
            return true;
        }
        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
