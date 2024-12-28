using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private bool skillUsed;
    private float flyTime = .4f;
    private float defaultPlayerGravity;

    public PlayerBlackholeState(
        Player player,
        PlayerStateMachine stateMachine,
        string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = flyTime;
        skillUsed = false;
        defaultPlayerGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultPlayerGravity;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            player.velocity = new Vector2(0, 15);
        }
        else
        {
            player.velocity = new Vector2(0, -.2f);

            if (!skillUsed)
            {
                if(player.skill.blackhole.UseSkill())
                    skillUsed = true;
            }
        }

        if (player.skill.blackhole.SkillFinished())
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
