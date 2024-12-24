using System;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(
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

        if (xInput != 0)
        {
            player.velocity = new Vector2(player.airMoveSpeedFactor * player.moveSpeed * xInput, player.velocity.y);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }

}

