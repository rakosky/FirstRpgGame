using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(
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

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.counterAttackState);
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttackState);
            return;
        }

        if(Input.GetKey(KeyCode.Mouse1)) 
        {
            if (player.sword is null)
            {
                stateMachine.ChangeState(player.aimSwordState);
                return;
            }
            else
            {
                player.sword.GetComponent<SwordThrowSkillController>().ReturnSword();
                return;
            }
        }

    }

}

