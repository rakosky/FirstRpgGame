using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Enemy : Entity
    {
        [Header("Move info")]
        public float moveSpeed;
        public float idleTime;
        public float battleTime;
        public float defaultMoveSpeed;

        [Header("Attack info")]
        public float attackDistance;
        public float attackCooldown;
        [HideInInspector] public float lastTimeAttacked;

        [Header("Stunned info")]
        public Vector2 stunForce;
        public float stunDuration;
        [SerializeField] protected bool canBeStunned;
        public bool stunWindowOpen;
        [SerializeField] protected GameObject counterImage;

        protected LayerMask playerMask;
        public float playerDetectionDistance = 20f;

        public EnemyStateMachine stateMachine { get; private set; }

        public virtual void SetFreeze(bool isFrozen)
        {
            if (isFrozen)
            {
                animator.speed = 0;
                moveSpeed = 0;
            }
            else
            {
                animator.speed = 1;
                moveSpeed = defaultMoveSpeed;
            }
        }

        public virtual IEnumerator FreezeFor(float seconds)
        {
            SetFreeze(true);
            yield return new WaitForSeconds(seconds);
            SetFreeze(false);       
        }

        public override void SlowByPercentage(float slowPercentage, float slowDuration)
        {
            base.SlowByPercentage(slowPercentage, slowDuration);

            moveSpeed *= 1 - slowPercentage;
            animator.speed *= 1 - slowPercentage;

            Invoke(nameof(SetToDefaultSpeed), slowDuration);
        }

        public override void SetToDefaultSpeed()
        {
            base.SetToDefaultSpeed();

            moveSpeed = defaultMoveSpeed;
        }

        public virtual void SetCounterAttackWindow(bool value)
        {
            if (!canBeStunned)
                return;

            stunWindowOpen = value;
            counterImage.SetActive(value);
        }

        public virtual void Stun()
        {
            stunWindowOpen = false;
            counterImage.SetActive(false);
        }

        public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

        public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(transform.position, new Vector2(facingDirection, 0), playerDetectionDistance, playerMask);


        protected override void Awake()
        {
            base.Awake();
            stateMachine = new EnemyStateMachine();
            moveSpeed = defaultMoveSpeed;
        }

        protected override void Start()
        {
            base.Start();
            playerMask = LayerMask.GetMask("Player");
        }

        protected override void Update()
        {
            base.Update();

            stateMachine.currentState.Update();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
        }

    }
}
