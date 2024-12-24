using UnityEngine;

namespace Assets.Scripts.Enemy.Skeleton
{
    public class SkeletonGroundedState : EnemyState
    {
        protected EnemySkeleton enemy;

        private Transform player;

        public SkeletonGroundedState(
            Enemy enemyBase,
            EnemyStateMachine stateMachine,
            string animationName) : base(enemyBase, stateMachine, animationName)
        {

            player = PlayerManager.instance.player.transform;
            enemy = enemyBase as EnemySkeleton;
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

            if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
