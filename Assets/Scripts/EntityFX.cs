using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    
    [Header("FlashFx")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;

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

    public IEnumerator BlinkColorFx(Color color, int blinkTimes = 5, float blinkInterval = .1f)
    {
        var originalColor = sr.color;
        for (int i = 0; i < blinkTimes; i++)
        {
            sr.color = color;
            yield return new WaitForSeconds(blinkInterval / 2);
            sr.color = originalColor;
            yield return new WaitForSeconds(blinkInterval / 2);
        }
    }
}