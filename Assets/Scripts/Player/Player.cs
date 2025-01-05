using Assets.Scripts;
using System.Threading.Tasks;
using UnityEngine;

public class Player : Entity
{

    [Header("Movement")]
    [SerializeField] public float defaultMoveSpeed = 13f;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float airMoveSpeedFactor = 0.8f;
    [SerializeField] public float defaultJumpForce = 7f;
    [SerializeField] public float jumpForce;

    [Header("Attacking")]
    [SerializeField] public Vector2[] attackMovement;
    public float counterAttackDuration;
    public float swordReturnImpact = 4;

    [Header("Dash")]
    [SerializeField] public float defaultDashSpeed = 22f;
    [SerializeField] public float dashSpeed = 22f;
    [SerializeField] public float dashDuration = 0.3f;

    public float dashDirection { get; set; }
    public bool isBusy { get; private set; }

    public SkillManager skill;

    public GameObject sword { get; private set; }
    public void SetSword(GameObject sword)
    {
        if (this.sword != null)
            Destroy(this.sword);
        this.sword = sword;
    }

    #region State
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    #endregion

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
        sword = null;
    }

    public void BusyFor(float seconds)
    {
        isBusy = true;
        Task.Run(async () =>
        {
            await Task.Delay((int)(seconds * 1000));
            isBusy = false;
        });
    }



    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState (this, stateMachine, "Dead");

        skill = SkillManager.instance;
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public override void SlowByPercentage(float slowPercentage, float slowDuration)
    {
        base.SlowByPercentage(slowPercentage, slowDuration);

        moveSpeed *= 1 - slowPercentage;
        jumpForce *= 1 - slowPercentage;
        dashSpeed *= 1 - slowPercentage;
        animator.speed *= 1 - slowPercentage;

        Invoke(nameof(SetToDefaultSpeed), slowDuration);
    }

    public override void SetToDefaultSpeed()
    {
        base.SetToDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

}
