using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerAimSwordState : PlayerState  
{
    public PlayerAimSwordState(
        Player player,
        PlayerStateMachine stateMachine,
        string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
        player.skill.swordThrow.SetAimMarkerActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.skill.swordThrow.SetAimMarkerActive(false);
        player.BusyFor(.2f);
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (player.facingDirection == 1 && mousePosition.x < player.transform.position.x)
        {
            player.Flip();
        }
        else if (player.facingDirection == -1 && mousePosition.x > player.transform.position.x)
        {
            player.Flip();
        }


        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
