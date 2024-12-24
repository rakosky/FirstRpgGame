using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(
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

        if (player.IsWallDetected() && player.facingDirection == xInput)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        player.velocity = new Vector2(xInput * player.moveSpeed, player.velocity.y);

    }
}

