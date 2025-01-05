using Assets.Scripts.Enemy;
using Assets.Scripts.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AilmentType
{
    None,
    Ignite, //applies dot to entity
    Chill,  //entities armor is reduced by 33%
    Shock   //entity does 33% less damage
}

public enum DamageType
{
    Physical,
    Magical
}

public class CharacterStats : MonoBehaviour
{
    private Entity entity;
    private EntityFX entityFx;

    public int level = 1;
    public Stats stats;
    
    [Space]
    [SerializeField] private float ailmentDuration = 6f;


    public int currentHealth;

    public AilmentType currentAilment;
    private float ailmentTimer;

    private float igniteDamageInterval = .8f;
    private float igniteDamageTimer;
    public int ignitedDamage;
    [SerializeField] private GameObject lightningStrikePrefab;
    public int shockedDamage;
    public event System.Action OnHealthChanged;

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        entityFx = GetComponent<EntityFX>();
        currentHealth = CalculateMaxHealth();

        Inventory.instance.onItemEquipped += ApplyEquipModifiers;
        Inventory.instance.onItemUnequipped += RemoveEquipModifiers;
    }

    private void OnDisable()
    {
        Inventory.instance.onItemEquipped -= ApplyEquipModifiers;
        Inventory.instance.onItemUnequipped -= RemoveEquipModifiers;
    }

    #region modifier management
    private void ApplyEquipModifiers(InventoryItem item)
    {
        var equipData = item.data as EquipmentItemData;
        stats.AddModifiers(equipData.stats);
    }
    private void RemoveEquipModifiers(InventoryItem item)
    {
        var equipData = item.data as EquipmentItemData;
        stats.RemoveModifiers(equipData.stats);
    }
    #endregion

    protected virtual void Update()
    {
        ailmentTimer -= Time.deltaTime;

        if (ailmentTimer <= 0)
            currentAilment = AilmentType.None;

        if (currentAilment == AilmentType.Ignite)
        {
            igniteDamageTimer -= Time.deltaTime;
            if (igniteDamageTimer <= 0)
            {
                igniteDamageTimer = igniteDamageInterval;
                TakeDamage(ignitedDamage, false); // TakeDamage should probably handle targets defense calculations
            }
        }
    }

    public virtual void TakeDamage(int damage, bool canPlayDamageFx = true)
    {
        Debug.Log($"{entity.GetType()} took {damage} damage!");

        UpdateHealth(-damage);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        entityFx.StartCoroutine(entityFx.FlashFx());

        if (damage > 0 && canPlayDamageFx) //TODO this would be knockback calculation
            entity.DamageImpact();
    }

    private void UpdateHealth(int amount)
    {
        OnHealthChanged?.Invoke();
        currentHealth += amount;
    }

    public virtual void DealDamage(CharacterStats target, DamageType damageType, float attackDamageFactor = 1)
    {
        bool isCrit = false;

        if (Random.Range(0, CalculateHitAccuracy()) < target.CalculateEvasion())
        {
            Debug.Log("Hit evaded!");
            return;
        }

        var totalDamage = attackDamageFactor * damageType switch
        {
            DamageType.Physical => CalculatePhysAttackPower(),
            DamageType.Magical or _ => CalculateMagicAttackPower()
        };
        
        var lightningDamage = CalculateLightningAttackPower();
        var iceDamage = CalculateIceAttackPower();
        var fireDamage = CalculateFireAttackPower();
        totalDamage += lightningDamage + iceDamage + fireDamage;
        var elementalDamages = new List<(AilmentType ailment, int value)>
        {
            (AilmentType.Shock, lightningDamage), (AilmentType.Chill, iceDamage), (AilmentType.Ignite, fireDamage) 
        };

        AilmentType prominentAilment = AilmentType.None;
        if(elementalDamages.Any(x => x.value > 0))
            prominentAilment = elementalDamages.MaxDefaultRandom(d => d.value).ailment;

        // check crit rate and apply crit damage
        if (Random.Range(0, 100) < CalculateCritChance())
        {
            Debug.Log("Critical hit!");
            isCrit = true;
            totalDamage += totalDamage * (CalculateCritDamage() / 100f);
        }

        var ignoreDefense = CalculateIgnoreDefense() / 100f;
        var finalDefense = target.CalculateDefense() * (1 - ignoreDefense);
        
        // apply defense damage reduction
        totalDamage = totalDamage * (1 - finalDefense / 100);

        totalDamage = Mathf.Max(totalDamage, 1); // final damage cant be under 1

        if(prominentAilment == AilmentType.Ignite)
            target.ignitedDamage = Mathf.RoundToInt(fireDamage * 2f);
        else if(prominentAilment == AilmentType.Shock)
            target.shockedDamage = Mathf.RoundToInt(lightningDamage * 8f);

        if(prominentAilment != AilmentType.None)
            target.ApplyAilment(prominentAilment);

        target.TakeDamage(Mathf.RoundToInt(totalDamage));
    }

    public void ApplyAilment(AilmentType ailment)
    {
        // refresh ailment timer regardless
        ailmentTimer = ailmentDuration;

        if (ailment == AilmentType.Ignite)
        {
            StartCoroutine(entityFx.BlinkColorFx(entityFx.ignitedColor, ailmentTimer, .5f));
            igniteDamageTimer = igniteDamageInterval;
        }
        else if (ailment == AilmentType.Chill)
        {
            entity.SlowByPercentage(.25f, ailmentDuration);
            if(this.currentAilment != AilmentType.Chill) 
                StartCoroutine(entityFx.BlinkColorFx(entityFx.chilledColor, ailmentTimer, .5f));
        }
        else if (ailment == AilmentType.Shock)
        {
            StartCoroutine(entityFx.BlinkColorFx(entityFx.shockedColor, ailmentTimer));
            if(this.currentAilment == AilmentType.Shock)// re-shock enemy applys lightning bolt that seeks nearest target 
            {
                var lightningStrike = Instantiate(lightningStrikePrefab, transform.position, Quaternion.identity);
                lightningStrike.GetComponent<LightningStrikeController>().Setup(GetComponent<Enemy>(), shockedDamage);
            }
        }
        this.currentAilment = ailment;
    }

    #region stat calcs
    public int CalculateMaxHealth()
    {
        return (int)(stats.maxHealth + (stats.vit * 4));
    }
      
    private int CalculateDefense()
    {
        var value = stats.defense + (stats.str * 2);
        if (currentAilment == AilmentType.Chill)
            value *= 1 - .33f;
        return Mathf.RoundToInt(value);
    }            

    private int CalculateEvasion()
    {
        return (int)(stats.evasion + (stats.dex * 3));
    }      

    private int CalculatePhysAttackPower()
    {
        return (int)(stats.damage * (stats.str / 2));
    }
    
    private int CalculateFireAttackPower()
    {
        return (int)(stats.fireDamage + (stats.@int * 2));
    }
    
    private int CalculateIceAttackPower()
    {
        return (int)(stats.iceDamage + (stats.@int * 2));
    }
    
    private int CalculateLightningAttackPower()
    {
        return (int)(stats.lightningDamage + (stats.@int * 2));
    }
        
    private int CalculateMagicAttackPower()
    {
        return (int)(stats.damage * (stats.@int * 2));
    }

    private int CalculateHitAccuracy()
    {
        var value = stats.accuracy + (stats.dex * 1.4f);
        if (currentAilment == AilmentType.Shock)
            value *= 1 - .33f;
        return Mathf.RoundToInt(value);
    }

    private int CalculateCritChance()
    {
        return (int)(stats.critRate + (stats.luk * 1.5));
    }

    private int CalculateCritDamage()
    {
        return (int)(stats.critDamage + stats.str * 1.2);
    }

    private int CalculateIgnoreDefense()
    {
        return (int)(stats.ignoreDefense);
    }
    #endregion


    protected virtual void Die()
    {
        entity.Die();
    }
}
