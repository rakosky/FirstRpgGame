using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Enemy.Skeleton
{
    public class SkeletonStunnedState : EnemyState
    {
        private EnemySkeleton enemy;
        public SkeletonStunnedState(
            Enemy enemyBase,
            EnemyStateMachine stateMachine,
            string animationName) : base(enemyBase, stateMachine, animationName)
        {
            enemy = enemyBase as EnemySkeleton;
        }

        public override void Enter()
        {
            base.Enter();

            stateTimer = enemy.stunDuration;
            enemy.entityFX.StartCoroutine(
                enemy.entityFX.BlinkColorFx(
                    colors: new Color[] { Color.red },
                    duration: enemy.stunDuration,
                    blinkInterval: .1f));

            enemy.velocityNoFlip = new Vector2(-enemy.facingDirection * enemy.stunForce.x, enemy.stunForce.y);
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
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }
}
