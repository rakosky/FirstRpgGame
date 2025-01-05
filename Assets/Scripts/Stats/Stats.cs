using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Stats
{
    [Serializable]

    public class Stats
    {
        [Header("Main stats")]
        public Stat str = new(); // physical attack damage, defense, and crit damage
        public Stat luk = new(); // crit rate and item find
        public Stat dex = new() ; // avoid and hit rate
        public Stat @int = new(); // spell damage and elemental effect magnitude and damage
        public Stat vit = new(); // max health and stun resist


        [Header("Defensive stats")]
        public Stat maxHealth = new();
        public Stat evasion = new();
        public Stat defense = new(); // whole number value. 100 defense would make the character immune to damage, assuming there is no incoming ignoreDefense stat on the damage source

        [Header("Offensive stats")]
        public Stat damage = new(); // damage from sources such as weapon
        public Stat critRate = new();
        public Stat critDamage = new(); // whole number value. 100 crit damage would make a characters critical attack deal 100% more damage.
        public Stat accuracy = new();
        public Stat ignoreDefense = new(); // percentage value between 0 and 1. ignore a ratio of the targets defense stat
        public Stat fireDamage = new();
        public Stat iceDamage = new();
        public Stat lightningDamage = new();


        private Stat[] allStats;
        public Stats()
        {
            allStats = new[] { str, luk, dex, @int, vit, maxHealth, evasion, defense, damage, critRate, critDamage, accuracy, ignoreDefense, fireDamage, iceDamage, lightningDamage };
        }

        public void AddModifiers(Stats incomingStats)
        {
            for(int i = 0; i < allStats.Length; i++)
            {
                var thisStat = allStats[i];
                var incomingStat = incomingStats.allStats[i];
                if(incomingStat != 0)
                    thisStat.modifiers.Add(incomingStat);
            }
        }

        public void RemoveModifiers(Stats incoming)
        {
            for (int i = 0; i < allStats.Length; i++)
            {
                var thisStat = allStats[i];
                var incomingStat = incoming.allStats[i];
                if (incomingStat != 0)
                    thisStat.modifiers.Remove(incomingStat);
            }
        }

        public void ApplyRandomSpread(int statPoints)
        {
            for (int x = 0; x < statPoints; x++)
            {
                var stat = allStats[UnityEngine.Random.Range(0, allStats.Length)];

                stat.ModifyBaseValue(1);
            }
        }
    }


}
