using Assets.Scripts.Enemy;
using System.Linq;
using UnityEngine;

public class LightningStrikeController : MonoBehaviour
{

    private Enemy target;
    private Animator anim;

    private int damage;
    private float speed = 10f;
    private bool hasHit;

    public void Setup(Enemy source, int damage)
    {
        var enemies = Physics2D.OverlapCircleAll(source.transform.position, 20)
            .Select(x => x.GetComponent<Enemy>())
            .OfType<Enemy>()
            .ToList();

        var closestDistance = Mathf.Infinity;
        Enemy target = null;

        foreach (var enemy in enemies)
        {
            if (enemy == source)
                continue;

            var currentDistance = Vector2.Distance(transform.position, enemy.transform.position);

            if (closestDistance > currentDistance)
            {
                closestDistance = currentDistance;
                target = enemy;
            }
        }

        target ??= source;

        this.target = target;
        this.damage = damage;
    }
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (hasHit)
            return;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - target.transform.position;

        CheckForCollision();
    }

    private void CheckForCollision()
    {
        if (Vector2.Distance(transform.position, target.transform.position) < 1)
        {
            anim.transform.localPosition = new Vector3(0, .5f);
            anim.transform.localRotation = Quaternion.identity;
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke(nameof(DamageAndSelfDestroy), .2f);

            anim.SetBool("Hit", true);
            hasHit = true;
        }
    }

    private void DamageAndSelfDestroy()
    {
        target.stats.TakeDamage(20);
        Destroy(gameObject, .5f);
    }
}
