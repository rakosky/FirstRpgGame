
using Assets.Scripts.Enemy;
using System.Linq;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
    {
        public PlayerCounterAttackState(
            Player player,
            PlayerStateMachine stateMachine,
            string animationName) : base(player, stateMachine, animationName)
        {
        }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterAttackDuration;
        player.animator.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        var hitEnemies = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius)
                            .Select(c => c.GetComponent<Enemy>())
                            .OfType<Enemy>();

        bool cloneSpawned = false;
        foreach (var enemy in hitEnemies)
        {
            if (enemy.stunWindowOpen)
            {
                enemy.Stun();
                player.animator.SetBool("SuccessfulCounterAttack", true);
                if (!cloneSpawned)
                {
                    player.skill.clone.CreateCloneOnCounterattack(enemy.transform);
                    cloneSpawned = true;
                }
            }
        }
    }
}

