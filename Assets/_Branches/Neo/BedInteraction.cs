using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    [SerializeField] private bool canInteract = false;
    private BoxCollider2D bedCollider2D;
    [SerializeField] private GameObject interactionUI;

    void Start()
    {
        bedCollider2D = GetComponent<BoxCollider2D>();
        bedCollider2D.enabled = false;
        if (interactionUI) interactionUI.SetActive(false);
        Debug.Log("🛏️ Bed 2D START - OFF");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"🛏️ 2D TRIGGER: {other.name}");
        if (other.CompareTag("Player") && canInteract)
        {
            Debug.Log("✅ Player dans lit 2D → APPUIE E !");
            if (interactionUI) interactionUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && interactionUI)
            interactionUI.SetActive(false);
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("😴 DODO 2D !");
            SleepInBed();
        }
    }

    public void EnableInteraction()
    {
        canInteract = true;
        bedCollider2D.enabled = true;
        Debug.Log("✅🛏️ LIT 2D ACTIVÉ !");
    }

    void SleepInBed()
    {
        Debug.Log("🎉 GAME JAM WIN 2D !");
        Time.timeScale = 0;
    }
}
