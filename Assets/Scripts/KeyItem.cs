using UnityEngine;
public class KeyItem : MonoBehaviour
{
    public string itemID = "pillow";

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"🎁 {itemID} touché par {other.name}");
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectKeyItem(itemID);
            Destroy(gameObject);
        }
    }
}

