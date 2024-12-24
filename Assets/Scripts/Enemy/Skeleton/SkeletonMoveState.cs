using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Enemy.Skeleton
{
    public class SkeletonMoveState : SkeletonGroundedState
    {
        public SkeletonMoveState(
            Enemy enemyBase,
            EnemyStateMachine stateMachine,
            string animationName) : base(enemyBase, stateMachine, animationName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
            {
                enemy.Flip();
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
            enemy.velocity = new Vector2(enemy.moveSpeed * enemy.facingDirection, enemy.velocity.y);
        }
    }
}