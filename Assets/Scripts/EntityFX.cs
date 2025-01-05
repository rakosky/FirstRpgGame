using System.Collections;
using System.Linq;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    
    [Header("FlashFx")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Ailment colors")]
    public Color[] chilledColor;
    public Color[] shockedColor;
    public Color[] ignitedColor;

    private Color defaultColor;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        defaultColor = sr.color;
    }

    public IEnumerator FlashFx(float remainingEffectTime = .4f, float flashInterval = .1f)
    {
        while (remainingEffectTime > 0)
        {
            sr.material = hitMat;
            yield return new WaitForSeconds(flashInterval/2);
            sr.material = originalMat;
            yield return new WaitForSeconds(flashInterval/2);
            remainingEffectTime -= flashInterval;
        }
    }

    public IEnumerator BlinkColorFx(Color[] colors, float duration = 5, float blinkInterval = .1f)
    {
        if (colors == null || colors.Length == 0)
            throw new System.Exception("Colors array invalid");

        if (colors.Length == 1)
            colors = new Color[2] { colors[0], defaultColor };

        for (float i = 0; i < duration; i += blinkInterval) // one cycle of colors array
        {
            foreach (var color in colors) // flash each color of array
            {
                sr.color = color;
                yield return new WaitForSeconds(blinkInterval / colors.Length);
            }
        }
        sr.color = defaultColor;
    }


    public void SetTransparent(bool isVisible)
    {
        if (isVisible)
        {
            sr.color = defaultColor;
        }
        else
        {
            sr.color = Color.clear;
        }
    }
}