using UnityEngine;

public class EnemyStats : CharacterStats
{
    private ItemDrop dropSystem;


    private void OnValidate()
    {

    }

    protected override void Start()
    {
        base.Start();

        dropSystem = GetComponent<ItemDrop>();
        stats.ApplyRandomSpread(level * 3); // each level an enemy has grants them 3 random stat points

    }

    protected override void Die()
    {
        base.Die();

        dropSystem.GenerateDrops();
    }
}
