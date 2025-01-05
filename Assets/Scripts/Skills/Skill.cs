using Assets.Scripts.Enemy;
using System.Linq;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player => PlayerManager.instance.player;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUse()
    {
        if (cooldownTimer > 0)
        {
            Debug.Log($"{GetType()} is on cooldown!");
            return false;
        }


        return true;
    }

    public virtual bool UseSkill()
    {
        if (CanUse())
        {
            cooldownTimer = cooldown;
            Debug.Log($"Used {GetType()}!");
            return true;
        }

        return false;
    }

    public virtual Enemy FindClosestEnemy(Transform checkTransform, Vector3? offset = null)
    {
        var nearbyEnemies = Physics2D.OverlapCircleAll(checkTransform.position, 25)
            .Select(c => c.GetComponent<Enemy>())
            .OfType<Enemy>();

        var closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        var checkPosition = checkTransform.position;
        if (offset != null)
            checkPosition += offset.Value;

        foreach (var enemy in nearbyEnemies)
        {
            var currentDistance = Vector2.Distance(checkPosition, enemy.transform.position);

            if (closestDistance > currentDistance)
            {
                closestDistance = currentDistance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}
