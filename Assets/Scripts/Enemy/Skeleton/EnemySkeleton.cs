using Assets.Scripts.Enemy;
using Assets.Scripts.Enemy.Skeleton;
using UnityEngine;

public class EnemySkeleton : Enemy
{

    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle");
        moveState = new SkeletonMoveState(this, stateMachine, "Move");
        battleState = new SkeletonBattleState(this, stateMachine, "Move");
        attackState = new SkeletonAttackState(this, stateMachine, "Attack");
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.InitializeState(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Stun()
    {
        base.Stun();

        stateMachine.ChangeState(stunnedState);
    }
}
