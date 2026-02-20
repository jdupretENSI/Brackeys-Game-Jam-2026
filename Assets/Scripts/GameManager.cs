using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<string> collectedItems = new List<string>();
    [SerializeField] private BedInteraction bed;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CollectKeyItem(string itemID)
    {
        if (!collectedItems.Contains(itemID))
        {
            collectedItems.Add(itemID);
            Debug.Log($"🎵 items collected: {itemID} ({collectedItems.Count}/3)");

            if (collectedItems.Count >= 3 && bed != null)
            {
                bed.EnableInteraction();
            }
        }
    }

    public bool HasAllKeyItems() => collectedItems.Count >= 3;
}
