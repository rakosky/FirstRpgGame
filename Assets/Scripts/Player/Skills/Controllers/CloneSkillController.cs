using Assets.Scripts.Enemy;
using System.Linq;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    private float cloneTimer;
    private bool canAttack;
    [SerializeField] private float colorLosingSpeed;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    private Enemy closestEnemy;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void SetupClone(Transform transform, float cloneDuration, bool canAttack)
    {
        this.transform.position = transform.position;
        this.FaceClosestTarget();
        cloneTimer = cloneDuration;
        this.canAttack = canAttack;

        if (canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));
            
            if (sr.color.a <= 0)
                Destroy(gameObject);

            return;
        }
    }

    private void FaceClosestTarget()
    {
        var nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 25)
            .Select(c => c.GetComponent<Enemy>())
            .OfType<Enemy>();

        var closestDistance = Mathf.Infinity;

        foreach (var enemy in nearbyEnemies)
        {
            var currentDistance = Vector2.Distance(this.transform.position, enemy.transform.position);

            if (closestDistance > currentDistance)
            {
                closestDistance = currentDistance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy is not null)
        {
            if (transform.position.x > closestEnemy.transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }

    private void AnimationTrigger()
    {
    }

    private void AttackTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage();
            }
        }
    }
}
