using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private Slider slider;
    void Start()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        entity.onFlipped += FlipUI;
        entity.stats.OnHealthChanged += UpdateHealthUI;

        slider.value = slider.maxValue;
    }

    private void FlipUI()
    {
        rectTransform.Rotate(0, 180, 0);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = entity.stats.CalculateMaxHealth();
        slider.value = entity.stats.currentHealth;
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        entity.stats.OnHealthChanged -= UpdateHealthUI;
    }
}
