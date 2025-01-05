using Assets.Scripts.Enemy;
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        float attackDamageFactor = 1;

        var colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);

        foreach (var collider in colliders)
        {
            var player = collider.GetComponent<Player>();
            if (player != null)
            {
                enemy.stats.DealDamage(player.stats, DamageType.Physical, attackDamageFactor);
            }
        }
    }

    private void OpenCounterAttackWindow()
    {
        enemy.SetCounterAttackWindow(true);
    }
    private void CloseCounterAttackWindow()
    {
        enemy.SetCounterAttackWindow(false);
    }
}
