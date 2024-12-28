using Assets.Scripts.Enemy;
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
                enemy.stats.TakeDamage(player.stats.damage);
                enemy.Damage();
            }
        }
    }

    private void ThrowSwordTrigger()
    {
        SkillManager.instance.swordThrow.CreateSword();
    }
}
