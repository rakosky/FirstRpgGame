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
        var colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);

        foreach (var collider in colliders)
        {
            var player = collider.GetComponent<Player>();
            if (player != null)
            {
                
                player.Damage();
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
