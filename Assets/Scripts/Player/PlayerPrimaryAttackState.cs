using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerPrimaryAttackState : PlayerState
    {
        private int comboCounter;

        private float lastTimeAttacked;
        private float comboWindow = 2f;


        public PlayerPrimaryAttackState(
            Player player,
            PlayerStateMachine stateMachine,
            string animationName) : base(player, stateMachine, animationName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            xInput = 0; // need this to fix weird bug where xInput has value from another state?

            if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)
            {
                comboCounter = 0;
            }
            player.animator.SetInteger("ComboCounter", comboCounter);

            var attackDir = player.facingDirection;
            if (xInput != 0)
                attackDir = xInput;

            // set a little bit of velocity on attack to give a better feel
            var curAttackMovement = player.attackMovement[comboCounter];
            player.velocity = new Vector2(curAttackMovement.x * attackDir, curAttackMovement.y);

            // time which the player can maintain their forward movement after starting an attack
            stateTimer = .1f;
        }

        public override void Exit()
        {
            base.Exit();

            player.BusyFor(.1f);
            comboCounter++;
            lastTimeAttacked = Time.time;
            triggerCalled = false;
        }

        public override void Update()
        {
            base.Update();

            if (stateTimer < 0)
                player.SetZeroVelocity();

            if (triggerCalled)
                stateMachine.ChangeState(player.idleState);
        }
    }
}
