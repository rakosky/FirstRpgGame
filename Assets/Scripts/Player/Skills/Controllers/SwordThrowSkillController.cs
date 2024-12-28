using Assets.Scripts.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SwordThrowSkillController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool isReturning;
    private float autoReturnTimer;
    private float autoReturnTime = 7f;
    private float bounceSpeed;
    private bool canBounce;
    private bool isBouncing;
    private int amountOfBounces;
    private List<Transform> bounceTargets = new List<Transform>();
    private int currentBounceTargetIdx = 0;
    private int pierceAmount;
    private bool canRotate = true;
    private float spinTimer;
    private bool hasStopped;
    private bool isSpinningSword;
    private float spinningHitTimer;
    private float spinningHitTickDamageCooldown;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinDirection;
    private float freezeDuration;

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
                SwordSkillDamage(bounceTargets[currentBounceTargetIdx].GetComponent<Enemy>());
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

        if (isSpinningSword)
        {
            if (!hasStopped && Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance)
            {
                hasStopped = true;
                rb.angularVelocity = 0;
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
                spinTimer = spinDuration;
            }

            if (hasStopped)
            {
                spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), Time.deltaTime * 1.5f);

                if (spinTimer <= 0)
                {
                    isReturning = true;
                    isSpinningSword = false;
                    hasStopped = false;
                }

                spinningHitTimer -= Time.deltaTime;
                if (spinningHitTimer <= 0)
                {
                    spinningHitTimer = spinningHitTickDamageCooldown;
                    var hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1)
                        .Select(c => c.GetComponent<Enemy>())
                        .OfType<Enemy>()
                        .ToArray();

                    foreach (var enemy in hitEnemies)
                    {
                        SwordSkillDamage(enemy);
                    }
                }
            }
        }
        autoReturnTimer -= Time.deltaTime;
        if (autoReturnTimer <= 0)
        {
            isReturning = true;
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

        if (collision.gameObject.GetComponent<Enemy>() is Enemy hitEnemy)
        {

            SwordSkillDamage(hitEnemy);

            if (canBounce && bounceTargets.Count == 0)
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

    private void StickInto(Collider2D collider)
    {
        if (isSpinningSword)
        {
            hasStopped = true;
            rb.angularVelocity = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            spinTimer = spinDuration;
            return;
        }

        if (pierceAmount > 0 && collider.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        
        canRotate = false;
        cd.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (!isBouncing)
        {
            anim.SetBool("Rotation", false);
            transform.parent = collider.transform;
        }
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine(enemy.FreezeFor(freezeDuration));
    }

    public void SetupBounceSword(int amountOfBounces, float bounceSpeed)
    {
        canBounce = true;
        this.amountOfBounces = amountOfBounces;
        this.bounceSpeed = bounceSpeed;
    }

    public void SetupSpinSword(float maxTravelDistance, float spinDuration, float spinningHitTickDamageCooldown)
    {
        isSpinningSword = true;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.spinningHitTickDamageCooldown = spinningHitTickDamageCooldown;
    }

    public void SetupSword(Vector2 velocity, float gravityScale, float freezeDuration)
    {
        player = PlayerManager.instance.player;
        autoReturnTimer = autoReturnTime;
        rb.linearVelocity = velocity;
        rb.gravityScale = gravityScale;
        this.freezeDuration = freezeDuration;

        if(pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1);

    }

    public void ReturnSword()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        isReturning = true;
    }

    internal void SetupPierceSword(int pierceAmount)
    {
        this.pierceAmount = pierceAmount;
    }
}
