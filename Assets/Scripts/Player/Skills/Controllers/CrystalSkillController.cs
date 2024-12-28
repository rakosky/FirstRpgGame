using Assets.Scripts.Enemy;
using System.Linq;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalLifetimeTimer;

    private bool canExplode;
    private float growRateOnExplode;
    private bool canMove;
    private float moveSpeed;
    private Enemy targetedEnemy;

    private bool crystalExpired;

    public void Setup(float maxCrystalLifetime, bool canExplode, float growRateOnExplode, bool canMove, float moveSpeed, Enemy closestEnemy)
    {
        crystalLifetimeTimer = maxCrystalLifetime;
        this.canExplode = canExplode;
        this.growRateOnExplode = growRateOnExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
        this.targetedEnemy = closestEnemy;
    }

    private void Update()
    {
        crystalLifetimeTimer -= Time.deltaTime;
        if (crystalLifetimeTimer < 0)
        {
            crystalLifetimeTimer = Mathf.Infinity;
            ExpireCrystal();
        }

        if (canMove && targetedEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetedEnemy.transform.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetedEnemy.transform.position) < 1.5f)
            {
                canMove = false;
                ExpireCrystal();
            }
        }
    }

    public void ChooseRandomEnemy()
    {
        var radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        var hitEnemies = colliders
            .Select(c => c.GetComponent<Enemy>())
            .OfType<Enemy>()
            .ToList();

        if(hitEnemies.Count > 0)
            targetedEnemy = hitEnemies[Random.Range(0, hitEnemies.Count)];
    }

    public void ExpireCrystal()
    {
        if (crystalExpired)
            return;

        crystalExpired = true;
        if (canExplode)
        {
            if(growRateOnExplode > 0)
                transform.localScale = Vector2.Lerp(transform.localScale, transform.localScale * 3, growRateOnExplode);
            animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    private void AnimationExplodeEvent()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        var hitEnemies = colliders
            .Select(c => c.GetComponent<Enemy>())
            .OfType<Enemy>()
            .ToList();

        foreach (var enemy in hitEnemies)
        {
            enemy.Damage();
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
