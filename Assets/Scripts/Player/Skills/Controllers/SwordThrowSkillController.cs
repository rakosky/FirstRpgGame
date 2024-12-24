using Assets.Scripts.Enemy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwordThrowSkillController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool isReturning;

    private float bounceSpeed;
    private bool canBounce;
    private bool isBouncing;
    private int amountOfBounces;
    private List<Transform> bounceTargets = new List<Transform>();
    private int currentBounceTargetIdx = 0;

    private bool canRotate = true;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        // sword mid-air directional rotation
        if (canRotate)
        {
            transform.right = rb.linearVelocity;
        }

        if (isBouncing)
        {
            var currentTarget = bounceTargets[currentBounceTargetIdx];
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget.position) < .1)
            {
                currentBounceTargetIdx++;
                amountOfBounces--;
                if (amountOfBounces <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                    bounceTargets = new();
                }

                if (currentBounceTargetIdx >= bounceTargets.Count)
                {
                    currentBounceTargetIdx = 0;
                }
            }
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchSword();
                isReturning = false;
            }
        }
    }

    // when sword hits an object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (canBounce)
        {
            if (collision.GetComponent<Enemy>() is Enemy hitEnemy && bounceTargets.Count == 0)
            {
                var enemiesInRadius = Physics2D.OverlapCircleAll(transform.position, 10)
                    .Select(c => c.GetComponent<Enemy>())
                    .OfType<Enemy>()
                    .ToArray();

                for (int i = 0; i < enemiesInRadius.Length && bounceTargets.Count <= amountOfBounces; i++)
                {
                    var currentEnemy = enemiesInRadius[i];
                    if (currentEnemy != hitEnemy)
                    {
                        bounceTargets.Add(currentEnemy.transform);
                    }
                }

                if (bounceTargets.Count > 0)
                {
                    isBouncing = true;
                    currentBounceTargetIdx = 0;
                    bounceTargets.Add(hitEnemy.transform);
                }

            }
        }

        StickInto(collision);
    }

    private void StickInto(Collider2D collision)
    {
        canRotate = false;
        cd.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (!isBouncing)
        {
            anim.SetBool("Rotation", false);
            transform.parent = collision.transform;
        }
    }

    public void SetupBounceSword(int amountOfBounces, float bounceSpeed)
    {
        canBounce = true;
        this.amountOfBounces = amountOfBounces;
        this.bounceSpeed = bounceSpeed;
    }

    public void SetupSword(Vector2 velocity, float gravityScale)
    {
        player = PlayerManager.instance.player;

        rb.linearVelocity = velocity;
        rb.gravityScale = gravityScale;
        anim.SetBool("Rotation", true);
    }

    public void ReturnSword()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        isReturning = true;
    }
}
