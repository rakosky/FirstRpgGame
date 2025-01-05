using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item effect", menuName = "Data/Item/Effect")]
[Serializable]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform effectPosition)
    {
    }
}
