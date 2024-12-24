using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.Skeleton
{
    public class SkeletonAttackState : EnemyState
    {
        private EnemySkeleton enemy;

        public SkeletonAttackState(
            Enemy enemyBase,
            EnemyStateMachine stateMachine,
            string animationName) : base(enemyBase, stateMachine, animationName)
        {
            enemy = enemyBase as EnemySkeleton;
        }

        public override void Enter()
        {
            base.Enter();
            enemy.SetZeroVelocity();
        }

        public override void Exit()
        {
            base.Exit();
            enemy.lastTimeAttacked = Time.time;
        }

        public override void Update()
        {
            base.Update();


            if (triggerCalled)
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
