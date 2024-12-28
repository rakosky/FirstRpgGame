using Assets.Scripts.Enemy;
using System.Linq;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    private float cloneTimer;
    private bool canAttack;
    private bool canReproc;
    private float chanceToReproc;
    private float facingDir = 1;
    [SerializeField] private float colorLosingSpeed;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void SetupClone(Transform transform, float cloneDuration, bool canAttack, bool canReproc, float chanceToReproc, Enemy closestEnemy, Vector3? offset = null)
    {
        this.transform.position = transform.position;
        if (offset != null)
            this.transform.position += offset.Value;

        this.FaceEnemy(closestEnemy);
        cloneTimer = cloneDuration;
        this.canAttack = canAttack;
        this.canReproc = canReproc;
        this.chanceToReproc = chanceToReproc;

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

    private void FaceEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            if (transform.position.x > enemy.transform.position.x)
            {
                transform.Rotate(0, 180, 0);
                facingDir = -1;
            }
        }
    }
     
    private void AnimationTrigger()
    {
    }

    private void AttackTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        bool hasReprocced = false;
        foreach (var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage();
                if (canReproc && !hasReprocced)
                {
                    hasReprocced = true;
                    if (Random.Range(0, 100) < chanceToReproc)
                    {
                        SkillManager.instance.clone.CreateClone(enemy.transform, new Vector3(1f * facingDir, 0));
                    }
                }
            }
        }

    }
}
