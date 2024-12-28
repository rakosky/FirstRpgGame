using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(
        Player player,
        PlayerStateMachine stateMachine,
        string animationName) : base(player, stateMachine, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.skill.clone.CreateCloneOnDashStart();

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.clone.CreateCloneOnDashEnd();
        player.velocity = new Vector2(0, player.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }

        player.velocity = new Vector2(player.dashDirection * player.dashSpeed, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}

