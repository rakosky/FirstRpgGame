using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(
        Player player,
        PlayerStateMachine stateMachine,
        string animationName) : base(player, stateMachine, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0 && !IsRunningIntoWall() && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    private bool IsRunningIntoWall() => player.IsWallDetected() && xInput == player.facingDirection;

}

