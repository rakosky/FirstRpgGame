using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animationName;

    protected float xInput;
    protected float yInput;
    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animationName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animationName = animationName;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(animationName, true);
        triggerCalled = false;
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animationName, false);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            var dashSkill = player.skill.dash;
            if (dashSkill.CanUse())
            {
                dashSkill.UseSkill();
                player.dashDirection = Input.GetAxisRaw("Horizontal");
                if (player.dashDirection == 0)
                    player.dashDirection = player.facingDirection;

                stateMachine.ChangeState(player.dashState);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            player.skill.crystal.UseSkill();
        }

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
