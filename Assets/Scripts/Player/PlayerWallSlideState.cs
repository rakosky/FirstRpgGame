using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(
        Player player,
        PlayerStateMachine stateMachine,
        string animationName) : base(player, stateMachine, animationName)
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

        if (xInput != 0 && xInput != player.facingDirection)
            stateMachine.ChangeState(player.idleState);

        if(player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        // fast fall
        if (yInput < 0)
            player.velocity = new Vector2(0, player.velocity.y);
        // normal fall
        else if (yInput == 0)
            player.velocity = new Vector2(0, player.velocity.y * .8f);
        // slow fall
        else if (yInput > 0)
            player.velocity = new Vector2(0, player.velocity.y * .5f);
    }
}

