using UnityEngine;

public class LitTrigger : MonoBehaviour // ← SUR LIT !
{
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager?.CheckPlayerNearLit(true);
            Debug.Log("👤 Joueur près du lit !");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager?.CheckPlayerNearLit(false);
        }
    }
}
