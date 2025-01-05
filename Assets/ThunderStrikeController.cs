using Assets.Scripts.Enemy;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var playerStats = PlayerManager.instance.player.stats as PlayerStats;
        if (collision.GetComponent<Enemy>() is Enemy enemy)
        {
            playerStats.DealDamage(enemy.stats, DamageType.Magical);
        }
    }
}
