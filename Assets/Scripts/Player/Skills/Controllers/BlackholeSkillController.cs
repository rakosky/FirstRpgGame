using Assets.Scripts.Enemy;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    private bool isGrowing = true;
    private bool isShrinking = false;
    private float transformSpeed = .1f;
    private float maxSetupLifetime = 4f;
    private float lifetimeTimer;
    private float maxSize;
    private int maxTargets;
    private int numberOfAttacks;
    private float cloneAttackCooldown;
    private bool isAttacking;
    private bool canCreateTargets = true;
    private int currentTargetIdx;
    private float cloneAttackTimer;
    public bool playerCanExitState { get; private set; }


    private List<Transform> targets = new();
    public void AddTarget(Transform transform) => targets.Add(transform);

    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> validHotkeys = new();
    private List<KeyCode> usedHotKeys = new();
    private List<GameObject> createdHotkeys = new();

    public void Setup(float maxSize, float transformSpeed, float cloneAttackCooldown, int numberOfAttacks, int maxTargets, float maxSetupLifetime)
    {
        this.maxSize = maxSize;
        this.transformSpeed = transformSpeed;
        this.cloneAttackCooldown = cloneAttackCooldown;
        this.numberOfAttacks = numberOfAttacks;
        this.maxTargets = maxTargets;
        this.maxSetupLifetime = maxSetupLifetime;
    }

    private void Update()
    {
        lifetimeTimer += Time.deltaTime;
        if (canCreateTargets && (Input.GetKeyDown(KeyCode.R) || lifetimeTimer >= maxSetupLifetime))// trigger attacking phase
        {
            isAttacking = true;
            canCreateTargets = false;
            if (!SkillManager.instance.clone.crystalInsteadOfClone)
                PlayerManager.instance.player.SetVisible(false);
            DestroyHotkeys();
        }

        if (isAttacking)
        {
            if (targets.Count == 0 || numberOfAttacks <= 0)
            {
                isAttacking = false;
                isShrinking = true;
                playerCanExitState = true;
                PlayerManager.instance.player.SetVisible(true);
                return;
            }

            cloneAttackTimer -= Time.deltaTime;
            if (cloneAttackTimer > 0)
                return;

            var xOffset = Random.Range(0, 2) switch
            {
                0 => 2,
                1 or _ => -2,
            };
            // attack targets in the order they were registered
            var currentTarget = targets[currentTargetIdx];
            currentTargetIdx++;
            if(currentTargetIdx == targets.Count)
                currentTargetIdx = 0;

            numberOfAttacks--;

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(currentTarget.transform, new Vector3(xOffset, 0));
            }

            cloneAttackTimer = cloneAttackCooldown;
        }

        if(isShrinking)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), transformSpeed * 1.5f);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
        else if(isGrowing)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), transformSpeed);
        }   
    }

    private void DestroyHotkeys()
    {
        foreach (var hotkey in createdHotkeys)
            Destroy(hotkey);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() is Enemy enemy && !targets.Contains(enemy.transform)) // probably dont need to check if already in list since we 
                                                                                                  //freeze the time, and OnTriggerEnter only fires when entering the collision of this collider
        {
            if (!canCreateTargets || createdHotkeys.Count > maxTargets)
                return;

            if (validHotkeys.Count == 0)
            {
                if (usedHotKeys == null || usedHotKeys.Count == 0)
                {
                    Debug.Log("No valid quicktime hotkeys for black hole");
                    return;
                }
                validHotkeys.AddRange(usedHotKeys);
                usedHotKeys.Clear();
            }
            enemy.SetFreeze(true);

            // set hotkey prefab above enemy
            var hotkeyObject = Instantiate(hotkeyPrefab, enemy.transform.position + new Vector3(0,2), Quaternion.identity);
            var randomHotkey = validHotkeys[Random.Range(0, validHotkeys.Count-1)];
            validHotkeys.Remove(randomHotkey);
            usedHotKeys.Add(randomHotkey);

            hotkeyObject.GetComponent<BlackholeHotkeyController>().Setup(randomHotkey, enemy.transform, this);
            createdHotkeys.Add(hotkeyObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<Enemy>()?.SetFreeze(false);
    }

}
