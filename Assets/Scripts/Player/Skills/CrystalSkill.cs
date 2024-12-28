using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CrystalSkill : Skill
{
    [SerializeField] private float maxCrystalLifetime = 5f;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;
    [SerializeField] private float growRateOnExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi crystal")]
    [SerializeField] private bool canUseMultiCrystal;
    [SerializeField] private int maxMultiCrystals;
    [SerializeField] private float multiCrystalExpirationTime;

    [Header("Mirage crystal")]
    [SerializeField] private bool createCloneOnTeleport;

    private int numMultiCrystalsUsed;
    private float multiCrystalExpiryTimer;

    [SerializeField] private GameObject crystalPrefab;
    private GameObject activeCrystal;
    public override bool UseSkill()
    {
        if (!base.UseSkill())
            return false;

        if (canUseMultiCrystal)
        {
            cooldownTimer = 0;
            UseMultiCrystal();
            return true;
        }

        if (activeCrystal != null)
        {
            if (canMoveToEnemy)
                return true;

            var playerPos = player.transform.position;
            player.transform.position = activeCrystal.transform.position;
            activeCrystal.transform.position = playerPos;
            
            if (createCloneOnTeleport)
            {
                SkillManager.instance.clone.CreateClone(activeCrystal.transform);
                Destroy(activeCrystal);
            }
            else
            {
                activeCrystal.GetComponent<CrystalSkillController>().ExpireCrystal();
            }

        }
        else
        {
            CreateCrystal();
        }

        return true;
    }

    public void CreateCrystal()
    {
        activeCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        activeCrystal.GetComponent<CrystalSkillController>()
            .Setup(maxCrystalLifetime, canExplode, growRateOnExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(activeCrystal.transform));
        cooldownTimer = 0; // dont set cooldown for initial crystal spawn
    }

    public void CurrentCrystalChooseRandomTarget()
    {
        activeCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    }

    private void UseMultiCrystal()
    {
        if (numMultiCrystalsUsed == 0)
        {
            multiCrystalExpiryTimer = multiCrystalExpirationTime;
        }
        if (multiCrystalExpiryTimer <= 0 || numMultiCrystalsUsed == maxMultiCrystals)
        {
            numMultiCrystalsUsed = 0;
            cooldownTimer = cooldown;
            return;
        }

        activeCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        activeCrystal.GetComponent<CrystalSkillController>()
            .Setup(maxCrystalLifetime, canExplode, growRateOnExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(activeCrystal.transform));

        numMultiCrystalsUsed++;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        multiCrystalExpiryTimer -= Time.deltaTime;
    }
}
