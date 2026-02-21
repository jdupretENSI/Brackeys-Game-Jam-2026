using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [SerializeField] private string keyItemName = "";
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.CollectKeyItem(keyItemName);
            gameObject.SetActive(false); // DISPARAIT !
            Debug.Log($"✓ {keyItemName} récupéré !");
        }
    }
}
