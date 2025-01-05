using Assets.Scripts.Enemy;
using System.Linq;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
    
        foreach (var collider in colliders) 
        {
            var enemy = collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                player.stats.DealDamage(enemy.stats, DamageType.Physical);

                var equippedItemData = Inventory.instance.equippedItems.FirstOrDefault(e => (e.data as EquipmentItemData).equipmentType == EquipmentType.Weapon)?.data as EquipmentItemData;
                equippedItemData?.ExecuteItemEffects(enemy.transform);
            }
        }
    }

    private void ThrowSwordTrigger()
    {
        SkillManager.instance.swordThrow.CreateSword();
    }
}
