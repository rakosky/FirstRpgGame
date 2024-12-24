using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }


    public Player player;

    private void Awake()
    {
        Debug.Log("Player manager awake");
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

}
