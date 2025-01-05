using Assets.Scripts.Enemy;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{

    float knockUpDuration = .1f;

    public SkeletonDeadState(
        Enemy enemyBase,
        EnemyStateMachine stateMachine,
        string animationName) : base(enemyBase, stateMachine, animationName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = knockUpDuration;
        enemyBase.animator.SetBool(enemyBase.lastAnimName, true);
        enemyBase.animator.speed = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;

        if (stateTimer > 0)
        {
            enemyBase.velocityNoFlip = new Vector2(1*-enemyBase.facingDirection, 10);
        }
        else
        {
            enemyBase.cd.enabled = false;
        }

    }
}

