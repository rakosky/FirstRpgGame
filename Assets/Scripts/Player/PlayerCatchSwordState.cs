using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(
        Player player,
        PlayerStateMachine stateMachine,
        string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;

        if (player.facingDirection == 1 && sword.position.x < player.transform.position.x)
        {
            player.Flip();
        }
        else if (player.facingDirection == -1 && sword.position.x > player.transform.position.x)
        {
            player.Flip();
        }

        player.velocityNoFlip = new Vector2(player.swordReturnImpact * -player.facingDirection, player.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.BusyFor(.1f);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
