using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision")]
    public Transform attackCheck;
    public float attackRadius;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform wallCheck;
    protected LayerMask groundMask;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockBackForce;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    public float facingDirection { get; set; } = 1;

    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX entityFX { get; private set; }

    #endregion

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        entityFX = GetComponent<EntityFX>();

        groundMask = LayerMask.GetMask("Ground");
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Damage()
    {
        entityFX.StartCoroutine(entityFX.FlashFx());
        StartCoroutine(HitKnockback());

        Debug.Log(gameObject.name + " was damaged!");
    }

    protected virtual IEnumerator HitKnockback()
    {
        velocityNoFlip = new Vector2(knockBackForce.x * -facingDirection, knockBackForce.y);
        isKnocked = true;

        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }


    #region Velocity
    public Vector2 velocity
    {
        get => rb.linearVelocity;
        set
        {
            if (isKnocked)
                return;

            rb.linearVelocity = value;
            FlipController();
        }
    }
    public Vector2 velocityNoFlip
    {
        get => rb.linearVelocity;
        set
        {
            if (isKnocked)
                return;

            rb.linearVelocity = value;
        }
    }
    public void SetZeroVelocity() => velocity = Vector2.zero;
    #endregion


    #region Flip
    public void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }
    protected void FlipController()
    {
        if ((velocity.x > 0 && facingDirection == -1)
          || velocity.x < 0 && facingDirection == 1)
        {
            Flip();
        }
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, new Vector2(facingDirection, 0), wallCheckDistance, groundMask);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + (wallCheckDistance * facingDirection), wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion
}
