using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Lit Config")]
    [SerializeField] private GameObject lit;
    [SerializeField] private GameObject actionPrompt;  // Text "Appuyez E"
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private BoxCollider2D litTrigger; // Collider lit (IsTrigger)

    private int collectedCount = 0;
    private readonly string[] requiredItems = { "Pillow", "Nightcap", "Nightlight" };
    private bool playerNearby = false;
    private bool litActivated = false;

    void Start()
    {
        UpdateUI();
        HideUI();
        lit.SetActive(false); // LIT DÉSACTIVÉ au début
    }

    public void Interact()
    {

        Debug.Log($"🔍 DEBUG Interact: playerNearby={playerNearby} | litActivated={litActivated}");

        // Remplace le Update() Input.GetKeyDown
        if (playerNearby && litActivated)
        {
            LoadCreditsScene();
        }
        else
        {
            Debug.Log("❌ Conditions échouées - pas près du lit OU lit pas activé");
        }
    }

    public void CollectKeyItem(string itemName)
    {
        collectedCount++;
        Debug.Log($"🛏️ {itemName} collecté: {collectedCount}/3");
        UpdateUI();

        if (collectedCount >= 3)
        {
            ActivateLit();
        }
    }

    void UpdateUI()
    {
        if (counterText) counterText.text = $"{collectedCount}/3";
    }

    void ActivateLit()
    {
        litActivated = true;
        lit.SetActive(true); // LIT ACTIVÉ = visible !
        Debug.Log("🏆 LIT ACTIVÉ ! Approchez-vous et appuyez E");
    }

    void ShowPrompt()
    {
        if (actionPrompt) actionPrompt.SetActive(true);
    }

    public void CheckPlayerNearLit(bool isNear)
    {
        playerNearby = isNear;
        if (litActivated && isNear)
            ShowPrompt();
        else
            HideUI();
    }

    void HideUI()
    {
        if (actionPrompt) actionPrompt.SetActive(false);
    }

    void LoadCreditsScene()
    {
        Debug.Log("💤 DREAM SHEEP - BONNE NUIT ! Crédits...");
        SceneManager.LoadScene("Credits"); // ← TON NOM SCÈNE
    }
}
