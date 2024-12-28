using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{
    private KeyCode hotkey;
    private TextMeshProUGUI textMeshPro;
    private Transform enemyTransform;
    BlackholeSkillController blackholeSkillController;
    private SpriteRenderer sr;

    public void Setup(KeyCode keyCode, Transform enemyTransform, BlackholeSkillController blackholeSkillController)
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        hotkey = keyCode;
        textMeshPro.text = keyCode.ToString();
        sr = GetComponent<SpriteRenderer>();
        this.enemyTransform = enemyTransform;
        this.blackholeSkillController = blackholeSkillController;
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotkey))
        {
            blackholeSkillController.AddTarget(enemyTransform);
            textMeshPro.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
