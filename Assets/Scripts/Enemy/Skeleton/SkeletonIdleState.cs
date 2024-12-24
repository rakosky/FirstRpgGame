using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Enemy.Skeleton
{
    public class SkeletonIdleState : SkeletonGroundedState
    {
        public SkeletonIdleState(
            Enemy enemyBase,
            EnemyStateMachine stateMachine,
            string animationName) : base(enemyBase, stateMachine, animationName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateTimer = enemy.idleTime;
            enemy.SetZeroVelocity();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if (stateTimer < 0)
            {
                stateMachine.ChangeState(enemy.moveState);
            }

        }
    }
}