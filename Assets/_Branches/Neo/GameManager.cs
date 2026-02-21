using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject lit;
    [SerializeField] private SpriteRenderer litRenderer;
    [SerializeField] private TextMeshProUGUI counterText;

    private int collectedCount = 0;
    private readonly string[] requiredItems = { "Pillow", "Nightcap", "Nightlight" };

    void Start()
    {
        // Rouge par défaut
        if (litRenderer) litRenderer.color = Color.red;
        UpdateUI();
    }

    public void CollectKeyItem(string itemName)
    {
        collectedCount++;
        UpdateUI();

        if (collectedCount >= 3)
        {
            // VERT = VALIDE !
            litRenderer.color = Color.green;
            Debug.Log("🏆 LIT PRÊT ! 3/3 récupérés");
        }
    }

    void UpdateUI()
    {
        counterText.text = $"{collectedCount}/3";
    }
}
