using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyState
    {
        protected Enemy enemyBase;

        protected EnemyStateMachine stateMachine;
        private string animationName;

        protected float xInput;
        protected float yInput;
        protected float stateTimer;
        protected bool triggerCalled;

        public EnemyState(
            Enemy enemyBase,
            EnemyStateMachine stateMachine,
            string animationName)
        {
            this.enemyBase = enemyBase;
            this.stateMachine = stateMachine;
            this.animationName = animationName;
        }

        public virtual void Enter()
        {
            enemyBase.animator.SetBool(animationName, true);
            triggerCalled = false;
        }

        public virtual void Exit()
        {
            enemyBase.animator.SetBool(animationName, false);
        }

        public virtual void Update()
        {
            stateTimer -= Time.deltaTime;

        }

        public virtual void AnimationFinishTrigger() => triggerCalled = true;
    }
}
