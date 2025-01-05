using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item/Effect/Thunder Strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform effectPosition)
    {
        var thunderStrike = Instantiate(thunderStrikePrefab, effectPosition.position, Quaternion.identity);

        //setup

    }
}
