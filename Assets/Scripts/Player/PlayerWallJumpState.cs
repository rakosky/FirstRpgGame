using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerWallJumpState : PlayerState
    {
        public PlayerWallJumpState(
            Player player,
            PlayerStateMachine stateMachine,
            string animationName) : base(player, stateMachine, animationName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            stateTimer = 0.2f;
            //apply a force on the x that is half the players move speed in the opposite direction they are facing
            player.velocity = new Vector2(player.moveSpeed / 2 * -player.facingDirection, player.jumpForce);
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
                stateMachine.ChangeState(player.airState);
                return;
            }
            
            if (player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.idleState);
                return;
            }


        }
    }
}
