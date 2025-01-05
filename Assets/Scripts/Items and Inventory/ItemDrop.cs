using System.Collections.Generic;
using UnityEngine;
public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;

    [SerializeField] private DropTableEntry[] dropTable;


    public void GenerateDrops()
    {
        var itemDrops = new List<ItemQuantity>();
        foreach (var drop in dropTable)
        {
            if (Random.Range(0f, 100f) < drop.dropRate)
            {
                var quant = Random.Range(drop.minQuantity, drop.maxQuantity);
                itemDrops.Add(new ItemQuantity(drop.itemData, quant));
            }
        }

        foreach (var drop in itemDrops)
        {
            for(int i = 0; i < drop.quantity; i++)
                DropItem(drop.itemData);
        }
    }

    public void DropItem(ItemData itemData)
    {
        var newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(12, 15));

        newDrop.GetComponent<ItemObject>().SetupItem(itemData, randomVelocity);
    }

}

[System.Serializable]
public class DropTableEntry
{
    public ItemData itemData;
    public float dropRate; // 100 is guaranteed drop
    public int minQuantity = 1;
    public int maxQuantity = 1;
}