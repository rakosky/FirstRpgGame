using UnityEngine;

namespace Assets.Scripts.Enemy.Skeleton
{
    public class SkeletonBattleState : EnemyState
    {

        private EnemySkeleton enemy;

        private Transform playerTransform;
        private float moveDir;

        public SkeletonBattleState(
            Enemy enemyBase,
            EnemyStateMachine stateMachine,
            string animationName) : base(enemyBase, stateMachine, animationName)
        {
            this.enemy = enemyBase as EnemySkeleton;
        }

        public override void Enter()
        {
            base.Enter();
            playerTransform = PlayerManager.instance.player.transform;
            stateTimer = enemy.battleTime;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            var playerDetected = enemy.IsPlayerDetected();

            if (playerDetected && playerDetected.distance < enemy.attackDistance)
            {
                if(Time.time - enemy.lastTimeAttacked > enemy.attackCooldown)
                    stateMachine.ChangeState(enemy.attackState);

                return;
            }

            if (!playerDetected && stateTimer < 0)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }

            if (playerTransform.position.x > enemy.transform.position.x)
                moveDir = 1;
            else if (playerTransform.position.x < enemy.transform.position.x)
                moveDir = -1;

            enemy.velocity = new Vector2(enemy.moveSpeed * 1.5f * moveDir, enemy.velocity.y);


        }
    }
}
