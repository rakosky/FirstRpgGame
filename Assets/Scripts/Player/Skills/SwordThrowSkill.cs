using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordThrowSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 throwVelocity;
    [SerializeField] private float swordGravity;

    [Header("Bounce info")]
    [SerializeField] private int amountOfBounces;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceGravity;

    [Header("Aim markers")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweemDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    private GameObject[] aimMarkers;

    private SwordThrowSkillController controller;

    private Vector2 finalThrowDirection;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
    }

    protected override void Update()
    {
        base.Update();

        // rmb held
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < aimMarkers.Length; i++)
            {
                aimMarkers[i].transform.position = AimMarkerPosition(i * spaceBetweemDots);
            }
        }

        // rmb released
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            var aimDir = AimDirection();
            finalThrowDirection = new Vector2(aimDir.normalized.x * throwVelocity.x, aimDir.normalized.y * throwVelocity.y);
        }
    }

    public void CreateSword()
    {
        GameObject sword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        controller = sword.GetComponent<SwordThrowSkillController>();

        if (swordType == SwordType.Bounce)
        {
            controller.SetupBounceSword(amountOfBounces, bounceSpeed);
        }

        player.SetSword(sword);
        controller.SetupSword(finalThrowDirection, swordGravity);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    public void SetAimMarkerActive(bool value)
    {
        foreach (var marker in aimMarkers)
        {
            marker.SetActive(value);
        }
    }

    private void GenerateDots()
    {
        aimMarkers = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            aimMarkers[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            aimMarkers[i].SetActive(false);
        }
    }

    private Vector2 AimMarkerPosition(float t)
    {
        var aimDir = AimDirection();

        Vector2 position = (Vector2)player.transform.position + new Vector2(
                aimDir.normalized.x * throwVelocity.x,
                aimDir.normalized.y * throwVelocity.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
}
