using TMPro.EditorUtilities;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
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
}
