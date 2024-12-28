using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    public int damage;

    void Start()
    {
        currentHealth = 0;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
