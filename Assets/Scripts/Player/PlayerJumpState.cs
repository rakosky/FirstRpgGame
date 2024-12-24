using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(
        Player player,
        PlayerStateMachine stateMachine,
        string animationName) : base(player, stateMachine, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //TODO there is a weird thing here that can cause you to turn around when jumping
        player.velocity = new Vector2(player.velocity.x * player.facingDirection, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.animator.SetFloat("yVelocity", player.velocity.y);

        if (xInput != 0)
        {
            player.velocity = new Vector2(
                player.airMoveSpeedFactor * player.moveSpeed * xInput,
                player.velocity.y);
        }

        if (player.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);// why is this called airState and not fallState?
        }
    }

}

