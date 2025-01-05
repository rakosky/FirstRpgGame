using UnityEngine;

public class ItemObject : MonoBehaviour
{    
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 velocity;

    public void SetupItem(ItemData itemData, Vector2 velocity)
    {
        this.itemData = itemData;
        rb.linearVelocity = velocity;
    }

    private void Start()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
    }

    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData, 1);
        Destroy(gameObject);
    }
}
